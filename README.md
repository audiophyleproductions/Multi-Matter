<div align="center">

# Multi-Matter
### Removable Drive Batch Formatter

**Because formatting 40 USB drives one at a time is nobody's idea of a good time.**

[**Download Multi-Matter V1.0.1**](https://github.com/audiophyleproductions/Multi-Matter/releases/tag/v1.0.1) · [Report a bug](https://github.com/audiophyleproductions/Multi-Matter/issues) · [Licence](LICENSE)

</div>

---

## Meet Multi-Matter

A pile of USB sticks? A handful of SD cards? Multi-Matter makes repetitive removable-media formatting a much less repetitive job. Choose your drives, choose the file system, give them a common label, and format in a batch—using one straightforward Windows interface.

No command-line gymnastics, subscriptions or accounts. Just a purpose-built tool that does the job.

## Download and install

1. Open the [V1.0.1 release page](https://github.com/audiophyleproductions/Multi-Matter/releases/tag/v1.0.1).
2. Under **Assets**, download `Multi-Matter_Setup_1.0.1.exe` (if listed).
3. Run the installer and follow the setup wizard.
4. Start **Multi-Matter** from the Start Menu and approve the Windows administrator prompt when required.

> **Important:** Formatting erases existing files on selected volumes. Always check drive letters and capacities and back up anything important first. Test unfamiliar devices with expendable media. The installer is currently unsigned and Windows SmartScreen may warn about an unrecognised application.

## Features

- **Batch formatting:** Select multiple removable volumes in one go.
- **USB, SD and microSD:** Detects eligible volumes that Windows classifies as removable, including cards connected through compatible internal readers.
- **Select All / Deselect All:** A single toggle for selecting or clearing the drive list.
- **exFAT, FAT32 and NTFS:** Choose the file system supported by Windows for the selected volume.
- **Shared volume label:** Apply one optional name across selected drives.
- **Drive safeguards:** Rechecks identities, excludes protected boot/system volumes and asks for confirmation before formatting.
- **Progress and results:** Per-drive status, refresh and diagnostics.
- **Optional completion sound:** Enable or disable it, save the preference and use **Test Sound** without formatting anything.
- **Simple desktop UI:** Charcoal-and-orange native C# WPF application.

## How to use

1. Insert your removable USB drives or SD/microSD cards.
2. Launch Multi-Matter and check each displayed letter, label and capacity.
3. Select individual volumes or click **Select All** (which changes to **Deselect All** when everything is selected).
4. Choose exFAT, FAT32 or NTFS, and optionally set a common volume label.
5. Click **Format Selected**, review the erase confirmation, and proceed only if the selections are correct.
6. Review the results when the batch finishes.

## Compatibility and limitations

- Windows 10/11, x64; administrator approval required for formatting.
- Formats existing volumes, not partition layouts or whole physical disks.
- Eligibility is based on Windows removable-drive classification plus protection checks; some hardware may be classified differently and not appear.
- Windows' FAT32 formatting tools may not support larger volumes or every device/file-system combination.
- Storage operations are destructive; disconnects, faulty media and controller-specific behaviour can still cause errors.

## Completion sound

Use **Play sound when batch finishes** to enable or disable the notification. Click **Test Sound** to check playback. Your preference is saved.

The public source repository does not include the privately supplied completion audio. Redistribution of an embedded sound requires the appropriate rights. See [third-party notices](THIRD_PARTY_NOTICES.md).

## Build from source

Install a compatible .NET SDK (the project targets .NET 8), then run in the repository root:

```powershell
dotnet publish USBFormatter.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o dist
```

The self-contained build is produced at `dist\MultiMatter.exe`. A Windows installer can be built using the included `Build-Installer.cmd` and Inno Setup project; see [installer instructions](INSTALLER_README.md).

## Support and security

- [Report an issue](https://github.com/audiophyleproductions/Multi-Matter/issues) — remove sensitive disk identifiers from logs before sharing.
- [Security policy](SECURITY.md)
- [Changelog](CHANGELOG.md)
- [Third-party notices](THIRD_PARTY_NOTICES.md)

## Licence

**Copyright © 2026 Patrick Tiley. All rights reserved.**

Multi-Matter is **source-available**, not OSI-approved open-source software. The [Multi-Matter Software Licence V1.0](LICENSE) permits personal, educational and authorised commercial use, subject to its conditions. Reselling the software, charging for access, unauthorised redistribution, advertising-monetised derivatives and reuse of protected source or branding require separate written permission.

**Free to use. Not free to exploit.**
