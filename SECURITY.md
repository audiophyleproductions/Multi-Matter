# Security and safety

Multi-Matter performs destructive disk-volume formatting. Use it only with removable devices you own or are authorised to erase.

- Confirm the full drive list and capacities before clicking Format.
- The app excludes known system/boot and protected partitions and revalidates device identity before invoking Windows formatting. These checks are not a guarantee against OS/provider bugs or hardware changes.
- Windows UAC elevation is required; there is no supported no-elevation formatting mode.
- The app does not intentionally make network requests or automatically update itself.
- Diagnostics are written under the user's LocalAppData directory and can contain storage identifiers. Redact sensitive information from reports.

Report vulnerabilities privately to the repository owner through GitHub's private vulnerability reporting if enabled. Do not publish a proof of concept that puts users' data at risk.
