using System.IO;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;

namespace USBFormatter;

internal sealed record UsbVolume(string Letter, string Label, ulong Bytes, string Model,
    string DiskPnpId, int DiskIndex, ulong PartitionOffset, ulong PartitionBytes,
    string VolumeDeviceId)
{
    public string Description => $"{Letter}:   {(string.IsNullOrWhiteSpace(Label) ? "Unlabelled" : Label)}   ·   {Bytes / 1_000_000_000d:0.0} GB   ·   {Model}";
    public string Identity => $"{Letter}|{DiskPnpId}|{DiskIndex}|{PartitionOffset}|{PartitionBytes}|{VolumeDeviceId}";
}

internal static class Storage
{
    private static readonly string StorageNamespace = @"\\.\root\Microsoft\Windows\Storage";
    private static string Str(ManagementBaseObject obj, string name) => Convert.ToString(obj[name]) ?? "";
    private static ulong Num(ManagementBaseObject obj, string name) => Convert.ToUInt64(obj[name]);
    private static bool Flag(ManagementBaseObject obj, string name) => Convert.ToBoolean(obj[name] ?? false);

    private static List<ManagementObject> Query(string ns, string wql)
    {
        using var searcher = new ManagementObjectSearcher(ns, wql);
        using var result = searcher.Get();
        return result.Cast<ManagementObject>().ToList();
    }

    // MSFT_* DriveLetter is a CIM Char16: some Windows/WMI providers return a char,
    // others a one-character string, and some return its numeric UTF-16 code point.
    // DO NOT use WQL comparisons against quoted 'D' here: the type mismatch caused
    // valid FAT32 USB volumes to disappear in the previous C# implementation.
    private static string DriveLetter(object? value)
    {
        if (value is null) return "";
        if (value is char c) return c.ToString().ToUpperInvariant();
        if (value is byte b && b >= 65 && b <= 90) return ((char)b).ToString();
        if (value is ushort s && s >= 65 && s <= 90) return ((char)s).ToString();
        var text = Convert.ToString(value)?.Trim().TrimEnd(':').ToUpperInvariant() ?? "";
        if (text.Length == 1 && text[0] is >= 'A' and <= 'Z') return text;
        if (int.TryParse(text, out var code) && code >= 65 && code <= 90) return ((char)code).ToString();
        return "";
    }

    private static string? ReadDeviceId(string letter)
    {
        try
        {
            foreach (var volume in Query(@"root\cimv2", "SELECT DriveLetter, DeviceID FROM Win32_Volume"))
            {
                using (volume)
                {
                    if (Str(volume, "DriveLetter").Equals(letter + ":", StringComparison.OrdinalIgnoreCase))
                        return Str(volume, "DeviceID");
                }
            }
        }
        catch { /* Win32_Volume is optional for discovery; still bind to disk/partition identity. */ }
        return null;
    }

    private static string DiagnosticsFile => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Multi-Matter", "drive-detection.log");

    public static List<UsbVolume> Find()
    {
        var result = new List<UsbVolume>();
        var lines = new List<string> { $"[{DateTimeOffset.Now:O}] Windows removable-media scan" };
        var systemLetter = Path.GetPathRoot(Environment.SystemDirectory)!.Substring(0, 1).ToUpperInvariant();
        var parts = Query(StorageNamespace, "SELECT * FROM MSFT_Partition");
        var disks = Query(StorageNamespace, "SELECT * FROM MSFT_Disk");
        var physical = Query(@"root\cimv2", "SELECT Index, Model, PNPDeviceID, InterfaceType FROM Win32_DiskDrive");
        var logicals = Query(@"root\cimv2", "SELECT DeviceID, DriveType, VolumeName, Size FROM Win32_LogicalDisk");
        try
        {
            foreach (var logical in logicals)
            {
                var device = Str(logical, "DeviceID").ToUpperInvariant();
                if (!Regex.IsMatch(device, "^[A-Z]:$")) continue;
                var letter = device.Substring(0, 1);
                var type = Convert.ToInt32(logical["DriveType"]);
                // Windows DRIVE_REMOVABLE (2) is the sole inclusion criterion for media type.
                // Fixed disks (3), including USB HDDs/SSDs, are intentionally excluded.
                if (type != 2) { lines.Add($"{letter}: skipped: Windows DriveType={type} (not removable)"); continue; }
                if (letter is "A" or "B" || letter == systemLetter) continue;
                if (logical["Size"] == null) continue;
                var bytes = Num(logical, "Size");
                if (bytes == 0) continue;

                // Drive-letter-first detection: this is what worked on the user's six USB sticks.
                var matches = parts.Where(p => DriveLetter(p["DriveLetter"]) == letter).ToList();
                if (matches.Count != 1) { lines.Add($"{letter}: skipped: {matches.Count} storage partition mappings"); continue; }
                var part = matches[0];
                if (Flag(part, "IsBoot") || Flag(part, "IsSystem") || Flag(part, "IsHidden") || Flag(part, "IsReadOnly"))
                { lines.Add($"{letter}: protected partition"); continue; }
                var gpt = Str(part, "GptType").Trim('{', '}');
                if (gpt.Length > 0 && gpt != "00000000-0000-0000-0000-000000000000" &&
                    !gpt.Equals("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7", StringComparison.OrdinalIgnoreCase))
                { lines.Add($"{letter}: special GPT partition"); continue; }
                // FAT32 LBA MBR type 0x0C / decimal 12 is VALID and deliberately permitted.
                var mbr = Str(part, "MbrType").ToUpperInvariant();
                if (mbr is "05" or "0X05" or "0F" or "0X0F" or "27" or "0X27" or "EE" or "0XEE" or "EF" or "0XEF")
                { lines.Add($"{letter}: special MBR partition"); continue; }

                var diskNumber = Convert.ToInt32(part["DiskNumber"]);
                var matchDisk = disks.Where(d => Convert.ToInt32(d["Number"]) == diskNumber).ToList();
                if (matchDisk.Count != 1) { lines.Add($"{letter}: disk mapping unavailable"); continue; }
                var disk = matchDisk[0];
                if (Flag(disk, "IsBoot") || Flag(disk, "IsSystem") || Flag(disk, "IsOffline") || Flag(disk, "IsReadOnly"))
                { lines.Add($"{letter}: protected disk"); continue; }
                var physicalDisk = physical.FirstOrDefault(d => Convert.ToInt32(d["Index"]) == diskNumber);
                var pnp = physicalDisk == null ? "" : Str(physicalDisk, "PNPDeviceID");
                // Do not filter by BusType, InterfaceType or PNP prefix. Built-in
                // Realtek PCIe SD readers can expose removable cards as SCSI (bus 1).
                // The Win32_LogicalDisk DriveType=2 check above is authoritative for
                // removable-media selection. Disk/partition safety rules still apply.
                var busType = Convert.ToUInt16(disk["BusType"]);
                var model = physicalDisk == null ? "Removable media" : Str(physicalDisk, "Model");
                var offset = Num(part, "Offset");
                var partBytes = Num(part, "Size");
                var volumeId = ReadDeviceId(letter) ?? "";
                result.Add(new UsbVolume(letter, Str(logical, "VolumeName"), bytes, model,
                    pnp, diskNumber, offset, partBytes, volumeId));
                lines.Add($"{letter}: included, disk {diskNumber}, removable={type == 2}, bus={busType}, media=Windows-removable");
            }
        }
        finally
        {
            foreach (var obj in logicals.Concat(parts).Concat(disks).Concat(physical)) obj.Dispose();
            try { Directory.CreateDirectory(Path.GetDirectoryName(DiagnosticsFile)!); File.WriteAllLines(DiagnosticsFile, lines); }
            catch { /* Diagnostics must never interfere with drive listing. */ }
        }
        return result.GroupBy(x => x.Letter).Where(g => g.Count() == 1)
            .Select(g => g.Single()).OrderBy(x => x.Letter).ToList();
    }

    public static void Format(UsbVolume expected, string fileSystem, string label)
    {
        var now = Find().SingleOrDefault(x => x.Letter == expected.Letter);
        if (now is null || now.Identity != expected.Identity || now.Bytes != expected.Bytes)
            throw new InvalidOperationException($"{expected.Letter}: changed or disappeared. Refresh before retrying.");

        // Match by the CIM Char16 property IN CODE, not by a quoted WQL filter.
        var volumes = Query(StorageNamespace, "SELECT * FROM MSFT_Volume");
        try
        {
            var matches = volumes.Where(v => DriveLetter(v["DriveLetter"]) == expected.Letter).ToList();
            if (matches.Count != 1) throw new InvalidOperationException("Cannot uniquely resolve removable volume in Windows Storage.");
            var volume = matches[0];
            using var args = volume.GetMethodParameters("Format");
            args["FileSystem"] = fileSystem;
            args["FileSystemLabel"] = label;
            args["Full"] = false;
            args["Force"] = true;
            using var output = volume.InvokeMethod("Format", args, null);
            if (output is null) throw new InvalidOperationException("Windows storage provider did not return a result.");
            var code = Convert.ToUInt32(output["ReturnValue"]);
            if (code != 0) throw new InvalidOperationException($"Windows returned storage format error {code}.");
        }
        finally { foreach (var v in volumes) v.Dispose(); }
    }
}
