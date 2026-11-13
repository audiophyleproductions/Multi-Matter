# Contributing to Multi-Matter

**Multi-Matter — Removable Drive Batch Formatter**

Copyright © 2026 Patrick Tiley. All rights reserved.

Thanks for your interest in Multi-Matter!

Multi-Matter started with a straightforward goal: make formatting multiple removable storage devices faster, easier and considerably less tedious.

Although Multi-Matter is independently developed and distributed under a proprietary software licence, community feedback, bug reports, feature suggestions and testing are welcome.

This document explains how you can contribute and how we manage contributions while protecting the project's intellectual property.

---

## 1. Ways to Contribute

You don't need to be a developer to help improve Multi-Matter.

### Report Bugs

Found something that isn't working properly? We'd love to know about it.

Before opening a new issue, please check whether someone has already reported the same problem.

When reporting a bug, include:

- Your Windows version (Windows 10 or 11).
- The Multi-Matter version you're using.
- The affected removable device type (USB, SD or microSD).
- What you expected to happen.
- What actually happened.
- Steps to reproduce the issue.
- Screenshots, error messages or diagnostic logs where relevant.

**Important:** Remove personal information, device serial numbers and other sensitive details from screenshots or logs before posting them publicly.

[Report a Bug](https://github.com/audiophyleproductions/Multi-Matter/issues/new)

### Suggest Features

Have an idea that would make Multi-Matter more useful?

Open a GitHub Issue with a clear description of:

- The feature you're proposing.
- The problem it would solve.
- How you think it should work.
- Why it would benefit other users.

We're particularly interested in ideas that improve usability, removable-media compatibility, reliability, accessibility and formatting efficiency.

[Suggest a Feature](https://github.com/audiophyleproductions/Multi-Matter/issues/new)

### Test and Provide Feedback

Testing is one of the most valuable ways to contribute.

Useful feedback includes:

- Removable-device compatibility.
- Behaviour across different Windows versions.
- USB, SD and microSD card reader detection.
- File-system formatting reliability.
- Interface accessibility and usability.
- Installation and uninstallation behaviour.
- Error handling and diagnostic accuracy.

When testing formatting operations, always use expendable storage media and ensure that any important data has been backed up.

**Formatting permanently erases data on selected volumes.**

---

## 2. Software Licensing and Intellectual Property

Multi-Matter is proprietary, source-available software.

Its source code is publicly accessible for transparency, evaluation and security review, but public availability does not grant unrestricted rights to modify, redistribute or commercially exploit the software.

Multi-Matter is governed by the **Multi-Matter Software Licence V1.0**, available in the repository's [LICENSE](LICENSE) file.

### Permitted Use

Subject to the applicable licence, users may:

- Download and use Multi-Matter for personal purposes.
- Use Multi-Matter for educational purposes.
- Use Multi-Matter within businesses and organisations.
- Use Multi-Matter while performing paid services, including preparing removable storage devices for customers.
- Review the published source code within the scope permitted by the licence and applicable law.

### Restricted Activities

Unless expressly authorised in writing by Patrick Tiley, contributors and users must not:

- Sell, sublicense or commercially redistribute Multi-Matter.
- Charge users to download or access Multi-Matter.
- Create or distribute advertising-supported or otherwise monetised derivatives.
- Redistribute modified versions or unauthorised forks.
- Incorporate protected Multi-Matter code into other software products.
- Reuse Multi-Matter logos, icons, interface designs or other protected assets outside the permissions granted by the applicable licence.
- Misrepresent the ownership, authorship or official status of the software.

These restrictions are subject to rights that cannot lawfully be excluded and to any valid permissions granted under earlier applicable licences.

**Submitting feedback or contributing to the project does not grant anyone additional rights to redistribute, sublicense or commercialise Multi-Matter.**

---

## 3. Source Code Contributions

We welcome expressions of interest from developers who would like to help improve Multi-Matter.

However, code contributions are managed differently from those of conventional open-source projects.

### Before Submitting Code

Please open a GitHub Issue describing the proposed change before creating a pull request.

This allows us to discuss its suitability, scope, safety implications and potential inclusion in a future version.

Do not assume that opening a pull request automatically authorises you to modify, redistribute or publish derivative versions of Multi-Matter.

### Contribution Licensing

To protect both contributors and the project owner:

1. Contributors retain ownership of original code they independently create, unless otherwise agreed in writing.
2. Submission of a pull request does not, by itself, transfer copyright ownership to Patrick Tiley.
3. Before a substantive code contribution is accepted and incorporated into Multi-Matter, the contributor may be required to sign a separate written Contributor Licence Agreement (CLA).
4. The CLA will identify the rights granted to Patrick Tiley to use, reproduce, modify, distribute, maintain and license the contribution as part of Multi-Matter.
5. Any proposed transfer of copyright ownership must be expressly agreed in a separate written assignment.
6. Contributors must have the necessary rights to submit their contributions and must disclose third-party code, assets or licence requirements.

Contributors are not required to assign copyright merely to report bugs or suggest features.

The project owner may decline or defer code submissions until the necessary permissions have been established.

**Please do not submit substantial code contributions until the required licensing arrangements have been confirmed.**

---

## 4. Development Guidelines

Multi-Matter is built using C# and Windows Presentation Foundation (WPF).

When proposing changes, please follow these principles.

### Keep It Simple

Multi-Matter is intended to do one job exceptionally well: batch-format removable storage devices.

New features should improve that experience rather than introduce unnecessary complexity.

### Prioritise Safety

Storage formatting is a destructive operation.

Any change involving storage detection, drive selection or formatting must:

- Preserve protections against formatting system and boot volumes.
- Respect the removable-media eligibility rules.
- Revalidate selected volumes immediately before formatting.
- Avoid relying solely on drive letters when confirming device identity.
- Present clear confirmation before destructive operations.
- Handle unavailable, disconnected or unexpectedly changed devices safely.
- Report errors clearly rather than silently ignoring failures.

Changes that unnecessarily increase the risk of accidental data loss will not be accepted.

### Preserve Existing Functionality

New features should not unnecessarily disrupt working capabilities, including:

- USB, SD and microSD detection.
- Batch formatting.
- exFAT, FAT32 and NTFS selection.
- Custom volume labels.
- Select All / Deselect All behaviour.
- Completion notifications and user preferences.
- Application startup and administrator elevation.
- Existing safety checks.

### Maintain the User Interface

Multi-Matter uses a charcoal-and-orange visual identity.

Changes should preserve a consistent, accessible, uncluttered interface unless a redesign has been expressly approved.

### Document Your Changes

Proposed code contributions should explain:

- What was changed.
- Why the change was necessary.
- Any potential risks or compatibility considerations.
- How the changes were tested.
- Whether new dependencies or third-party components are introduced.

---

## 5. Third-Party Software and Assets

Do not submit third-party code, audio, images, icons, fonts or other materials unless their licence terms have been identified and they are suitable for the project's intended distribution.

All external components must be disclosed, including:

- Original source or creator.
- Applicable licence.
- Required attribution.
- Redistribution conditions.
- Commercial-use restrictions, if any.

Materials subject to incompatible licensing conditions may be rejected.

Multi-Matter's proprietary licence does not override valid third-party intellectual-property rights.

---

## 6. Security Vulnerabilities

Please do not publicly disclose a vulnerability that could result in unintended storage formatting, data loss, privilege escalation or another serious security issue before the project maintainer has had a reasonable opportunity to investigate.

For sensitive vulnerabilities, use GitHub's private vulnerability reporting feature if enabled.

See [SECURITY.md](SECURITY.md) for the project's security reporting guidance.

Avoid posting exploit details, private device information or sensitive diagnostic information in public issues.

---

## 7. Pull Request Process

For authorised code contributors:

1. Open a GitHub Issue to discuss the proposed change.
2. Obtain any required written development
