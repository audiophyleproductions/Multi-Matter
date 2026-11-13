# Multi-Matter

**Multi-Matter - Removable Drive Batch Formatter V1.0.1** — Windows removable-media batch formatter — a native C# / WPF desktop application with a charcoal-and-orange interface.

> \*\*Version 1.0.1 source package.\*\* Previous builds were tested on Windows; this specific revision has not been compiled or tested here. Verify the local build and removable-drive safeguards before public distribution.

## Features

* Discover eligible USB-connected volumes and native removable SD/MMC cards and show drive letters, labels and sizes.
* Select individual drives or use the **Select All / Deselect All** toggle.
* Format selected volumes to **exFAT, FAT32 or NTFS** with a shared volume label.
* Quick-format existing volumes; no partition creation or copying files.
* Revalidate selected device identity, exclude system/boot and protected volumes, and require final confirmation before erasure.
* Progress, per-drive outcome, refresh and diagnostic logging.
* Optional completion sound; preference persists. Uses an embedded locally supplied WAV when present, or a Windows notification fallback. Includes a Test Sound button.
* Runs as a native Windows GUI executable; requires administrator approval for disk formatting, but does not launch a PowerShell or Command Prompt window.

## System requirements

Windows 10/11 x64. Build requires the .NET 8 SDK or newer compatible SDK. Prebuilt **self-contained** builds do not require installing the .NET runtime.

## Build on Windows

In the folder containing `USBFormatter.csproj`:

```powershell
dotnet restore USBFormatter.csproj
dotnet publish USBFormatter.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o dist
```

Run `dist\\MultiMatter.exe`. Windows UAC approval is expected. `app.manifest` requests elevation. **Never accept a UAC request from an executable you did not obtain from a source you trust.**

To create a GitHub Actions build artifact, use **Actions → Build Windows executable → Run workflow**. The workflow builds, sanity-checks the output and uploads it; it does **not** automatically publish an untested release.

## How to use

1. Connect your USB drives or insert an SD/microSD card and launch Multi-Matter as administrator.
2. Confirm the drive **letters and capacities** before selecting anything.
3. Tick individual drives or press **Select All** (the button then changes to **Deselect All**).
4. Select file system and optional common label.
5. Click **FORMAT SELECTED**, read the erase confirmation, and choose Yes only if every drive is correct.
6. Check the per-drive results. Refresh to see updated labels.

**Warning:** Formatting permanently destroys existing files on selected volumes. External USB HDDs and SSDs may also qualify. This application only formats volumes, not entire physical disks. Backup anything important and test with spare drives first.

## Completion sound

In the main window, tick **Play sound when batch finishes** to enable the completion sound, or untick it to disable it. Click **Test Sound** to play it immediately, even if automatic playback is off. Your choice is saved automatically.



The private local build package can include a WAV converted from the audio file supplied by the project owner (`Assets/CompletionChime.wav`). The application embeds it using an MSBuild `EmbeddedResource` with the resource name `MultiMatter.CompletionChime.wav`. It plays on completion of the entire formatting batch and can be tested using **Test Sound** without formatting anything. The **Play sound when batch finishes** checkbox persists its setting to `%LOCALAPPDATA%\\Multi-Matter\\preferences.txt`; the test button works regardless of this preference.

If the WAV is absent, the application still builds and uses the standard Windows notification sound. **The WAV is deliberately gitignored** and must not be uploaded to a public GitHub repository or distributed as part of a public EXE until rights are confirmed. The file name suggests an audio production-library track; possession, editing, or a general music subscription does not automatically grant software redistribution rights. Verify the recording's origin and its licensing before publishing it. See [THIRD\_PARTY\_NOTICES.md](THIRD_PARTY_NOTICES.md).

To prepare public builds, remove `Assets/CompletionChime.wav` **before** building or committing; use a properly licensed sound if one is required for distribution.

## Logs and diagnostics

* `%LOCALAPPDATA%\\Multi-Matter\\drive-detection.log`
* `%LOCALAPPDATA%\\Multi-Matter\\startup-error.log`

When reporting a bug, review diagnostics before publishing them; they can contain disk identifiers.

## Contributing and releasing

* See [CONTRIBUTING.md](CONTRIBUTING.md), [SECURITY.md](SECURITY.md), [CHANGELOG.md](CHANGELOG.md) and [RELEASE\_CHECKLIST.md](RELEASE_CHECKLIST.md).
* The project uses the [MIT License](LICENSE) for **application code**. Bundled assets retain any applicable separate rights; the Sonilo audio is **not bundled**.
* Publisher and copyright metadata: **Patrick Tiley**.
* `.github/workflows/build.yml` provides a downloadable Windows build artifact. Do not call a CI artifact a verified release until hardware tests pass.

## Known limitations

* FAT32 formatting of volumes above approximately 32 GiB is normally unsupported by the Windows formatting backend.
* USB devices may be reported differently by some storage controllers; only devices confirmed to be USB are listed.
* Supports Windows x64 only. Not digitally signed. Windows SmartScreen may warn about unrecognised binaries.
* Drive-operation tests on other Windows systems, file-system combinations and unexpected device removal are pending.



### Licence



Copyright © 2026 Patrick Tiley. All rights reserved.



Multi-Matter is free for personal, educational and internal commercial use under the \*\*Multi-Matter Software Licence V1.0\*\*.



Resale, redistribution, advertising-supported modifications and unauthorised reuse of source code, branding or other protected assets require prior written permission from the copyright holder.



See \[LICENSE] for the complete terms.



\*\*Multi-Matter is source-available software, not open-source software under the OSI definition.\*\*

