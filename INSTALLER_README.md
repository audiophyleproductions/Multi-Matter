# Multi-Matter V1.0.1 - Inno Setup Installer Project

This package includes the full Multi-Matter V1.0.1 C# WPF source project and a ready-to-compile **Inno Setup installer**. It does **not** contain a compiled EXE or installer; build both on Windows.

## Prerequisites (Windows x64)

1. Install the .NET SDK (the existing project targets .NET 8; a compatible later SDK can build it).
2. Install [Inno Setup](https://jrsoftware.org/isinfo.php) (Inno Setup 6.3+).

## Fast build

1. Extract this ZIP and open the `MultiMatter` folder.
2. Double-click **`Build-Installer.cmd`**.
3. If both build steps succeed, collect **`installer-output\Multi-Matter_Setup_1.0.1.exe`**.

The helper publishes `dist\MultiMatter.exe` as a self-contained Windows x64 program, then compiles `installer\MultiMatter.iss` with Inno Setup. It does not silently install anything.

## Manual build

Open PowerShell in the `MultiMatter` directory:

```powershell
dotnet publish USBFormatter.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o dist
```

Then open `installer\MultiMatter.iss` in **Inno Setup Compiler** and click **Build > Compile** (or run `ISCC.exe installer\MultiMatter.iss`).

## Installer features

- Installs for all users to `C:\Program Files\Multi-Matter` by default.
- Creates a **Multi-Matter** Start Menu shortcut and an **Uninstall Multi-Matter** shortcut.
- Optionally creates a desktop shortcut (unchecked by default).
- Registers with Windows' Installed Apps list and provides a standard uninstaller.
- Uses the existing branded icon, product version 1.0.1, and publisher **Patrick Tiley**.
- Installer requires administrator approval; app launches request UAC separately because drive formatting is privileged.
- Supports installation over later versions with the **same AppId**; do not change the AppId for subsequent releases.
- Inno Setup installer is x64 only (matching the published executable).

## Safety / publication

- **Formatting erases data.** Test installer, uninstaller, upgrades and formatting with a spare USB or SD card before distributing.
- Uninstalling removes installed program files; normal per-user application settings/logs under `%LOCALAPPDATA%\Multi-Matter` are deliberately not deleted.
- Windows SmartScreen may warn about an unsigned installer. Code signing is strongly recommended before a public release.
- The bundled completion audio may require a separate redistribution licence; confirm permission for public redistribution before publishing this installer. This package contains the same audio as your V1.0.1 source.
- The installer is **not** yet configured or certified for Microsoft Store submission.

## Source of installer settings

Edit `installer\MultiMatter.iss` to change the default install folder, shortcut labels, wizard wording, version and icons.
