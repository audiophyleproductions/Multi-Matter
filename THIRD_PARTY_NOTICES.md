# Third-party notices

## Sonilo audio — **not included**

The v0.4.0 development prototype embedded a modified-format copy of **Bright Ascending Logo Chime** by Sonilo. The project credits this as:

> Sound effects by Sonilo — https://sonilo.com/royalty-free-sound-effects/ui/bright-ascending-logo-chime?ref=13e3B7Y4

At the time of this release-candidate preparation, the official library page states use is subject to its library-specific permissions, including restrictions on distributing files on their own and on types of use covered by plan. It does **not** clearly identify this particular download as CC BY with unrestricted redistribution in an open-source desktop app. Therefore **no Sonilo audio is shipped in this repository or proposed public executable**. Users may supply a WAV file locally where their own licensing permits. This is not legal advice.

Source: https://sonilo.com/royalty-free-sound-effects/ui/bright-ascending-logo-chime?ref=13e3B7Y4
Library licensing: https://sonilo.com/licensing

## System.Management

This project references Microsoft's `System.Management` NuGet package; see the NuGet package's own licensing information when distributing dependencies. NuGet package restore occurs during build.

## Locally supplied completion audio (private build only)

The file `ES_Magical Twinkle 1 - SFX Producer.mp3` was supplied by the project owner, converted to an embedded WAV in the private local-build package. Its provenance and software-redistribution licence have **not** been independently verified. Do not publish the WAV or a compiled executable embedding it until the applicable licence explicitly permits that distribution. The file is excluded from public source control by `.gitignore`.
