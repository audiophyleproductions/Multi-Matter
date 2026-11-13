# Contributing

Thank you for helping improve Multi-Matter.

1. Create a GitHub issue describing the operating system, drive hardware, expected and observed behaviour.
2. Make changes in a feature branch; keep storage/formatting changes separate from purely visual changes.
3. Build on Windows using the command in README and test with **empty, expendable** USB media.
4. Do not submit logs containing full PnP identifiers or other private device information.
5. Do not add third-party audio or images without redistribution rights verified for source and compiled releases.
6. Submit a pull request with code changes and tests performed. **Never test destructive operations on important data**.

Suggested test order: scan with no removable USB attached; scan with one and many USB drives; verify internal-drive exclusion; check selection toggle; test one empty USB; test batch format; file systems; volume labels; sound toggle and custom local WAV; format failure/device removal. See RELEASE_CHECKLIST.md.
