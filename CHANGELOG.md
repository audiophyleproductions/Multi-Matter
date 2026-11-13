## 1.0.1 — 2026-11-13

- Updated application name, displayed version, Windows executable metadata and copyright holder to Patrick Tiley.
- Simplified About dialog sound instructions and removed its audio source / rights paragraph.
- Preserved existing removable-drive detection, formatting and audio implementation.

## v0.9.0-rc.3 — Removable SD/MMC support

- Added native SD/MMC bus types (12/13) for built-in card readers, requiring Windows removable-volume classification (DriveType=2).
- Left the working USB drive detection and formatting path intact.
- Expanded drive-list wording to USB and SD/microSD media.
- For reader configurations that still do not appear, inspect `%LOCALAPPDATA%\Multi-Matter\drive-detection.log`; no broad fixed-disk fallback.

# Changelog

## 0.9.0-rc.1 — 2026-11-11
- Prepared public repository, Windows CI build artifact and pre-release documentation.
- Preserved v0.4.0's working USB discovery, safety and format implementation without changes.
- Public release no longer embeds Sonilo audio: optional locally supplied WAV, Windows notification fallback.
- Corrected prior README/version/license placeholder inconsistencies.

## 0.4.0-beta.1 — 2026-11-11
- Select All / Deselect All one-button toggle.
- Optional sound setting and About / Credits.
- Verified working by original Windows tester (including drive discovery); further compatibility testing needed.

## 0.3.0 beta — 2026-11-11
- First standalone C# WPF implementation, branded icon and charcoal/orange UI.
- Iterative drive-discovery repair for Win32 logical and MSFT storage mappings.

### 0.9.0-rc.2 (private sound-integration build)
- Add embedded local completion WAV support and a Test Sound button.
- Preserve completion sound enable/disable preference and Windows fallback.
- Exclude audio from Git; redistribution rights must be verified.
