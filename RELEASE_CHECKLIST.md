# Release checklist — do not tag stable v1.0 until complete

## Build and metadata
- [ ] Build from a clean clone on Windows x64 using .NET 8 or 10 SDK.
- [ ] Launch the published EXE from File Explorer: no console window; expected UAC prompt.
- [ ] Validate icon, window title, displayed version, executable file/product version.
- [ ] Inspect GitHub Actions log, download build artifact, and verify its SHA-256.
- [ ] Choose real publisher/maintainer identity. Decide whether to purchase code signing.

## Destructive-operation safety
- [ ] No internal boot/system disk is selectable.
- [ ] Scan correctly includes representative removable USB sticks.
- [ ] Confirm selected drive letter + capacity correspond to physical test drives.
- [ ] Drive unplug/reinsert during refresh or formatting is rejected or fails safely.
- [ ] Revalidation blocks attempts after a selected disk has been changed.
- [ ] Explicit confirmation shows every target drive, file system and label.
- [ ] One spare drive formats and receives label; verify with Windows Explorer.
- [ ] Multi-drive batch formats and reports accurate per-drive results.
- [ ] Validate FAT32, exFAT, NTFS and invalid-label errors on expendable drives.

## UI and audio
- [ ] Select All / Deselect All reflects manual checkbox changes.
- [ ] Sound checkbox persists between launches.
- [ ] With no custom WAV, Windows notification is played (when enabled).
- [ ] User-installed local WAV is played (when enabled) after restart.
- [ ] No Sonilo audio is embedded in the public binary/source distribution.
- [ ] About / Credits does not display incorrect CC licensing claims.

## GitHub release
- [ ] Check ownership of brand, icon, source code and other assets.
- [ ] Confirm MIT is the intended code licence; copyright attribution is Patrick Tiley.
- [ ] Check security, README, screenshots, project URLs and changelog.
- [ ] Scan EXE and verify file hashes, then publish a **pre-release** first.
- [ ] Collect test feedback before declaring stable v1.0.

- [ ] Verify redistribution rights for private custom completion audio before including it in a public EXE.
- [ ] Verify the Test Sound button and completed-batch sound independently on Windows.
