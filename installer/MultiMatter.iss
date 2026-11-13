#define MyAppName "Multi-Matter"
#define MyAppVersion "1.0.1"
#define MyAppPublisher "Patrick Tiley"
#define MyAppExeName "MultiMatter.exe"

; Stable AppId: do not change this ID in later releases, or upgrades may install separately.
[Setup]
AppId={{87AD24D8-CE05-5292-830C-C9DFBD2AB95A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName=Multi-Matter - Removable Drive Batch Formatter V{#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppCopyright=Copyright (c) 2026 Patrick Tiley
DefaultDirName={autopf}\Multi-Matter
DefaultGroupName=Multi-Matter
DisableProgramGroupPage=yes
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
WizardStyle=modern
SetupIconFile=..\Assets\MultiMatter.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName=Multi-Matter - Removable Drive Batch Formatter
OutputDir=..\installer-output
OutputBaseFilename=Multi-Matter_Setup_{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
CloseApplications=yes
RestartApplications=no
DisableWelcomePage=no
LicenseFile=..\LICENSE

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
; Include all publish output (runtime files too, if present). Never ship debug symbols.
Source: "..\dist\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.pdb"

[Icons]
Name: "{group}\Multi-Matter"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{group}\Uninstall Multi-Matter"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Multi-Matter"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

