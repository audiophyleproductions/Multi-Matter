@echo off
setlocal EnableExtensions
cd /d "%~dp0"

echo ====================================================
echo   Multi-Matter V1.0.1 - Windows Installer Builder
echo ====================================================
echo.
where dotnet >nul 2>&1
if errorlevel 1 (
  echo ERROR: .NET SDK not found. Install the .NET SDK first.
  goto :failure
)

echo [1/2] Publishing the Windows x64 application...
dotnet publish "USBFormatter.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o "dist"
if errorlevel 1 goto :failure
if not exist "dist\MultiMatter.exe" (
  echo ERROR: dist\MultiMatter.exe was not produced.
  goto :failure
)

echo.
echo [2/2] Compiling the Inno Setup installer...
set "ISCC="
for %%P in ("%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" "%ProgramFiles%\Inno Setup 6\ISCC.exe" "%ProgramFiles(x86)%\Inno Setup 7\ISCC.exe" "%ProgramFiles%\Inno Setup 7\ISCC.exe") do (
  if exist "%%~P" set "ISCC=%%~P"
)
if not defined ISCC (
  for %%I in (ISCC.exe) do if not "%%~$PATH:I"=="" set "ISCC=%%~$PATH:I"
)
if not defined ISCC (
  echo ERROR: Inno Setup compiler ISCC.exe not found.
  echo Install Inno Setup from https://jrsoftware.org/isinfo.php
  goto :failure
)
"%ISCC%" "installer\MultiMatter.iss"
if errorlevel 1 goto :failure

echo.
echo SUCCESS: installer-output\Multi-Matter_Setup_1.0.1.exe
if exist "installer-output\Multi-Matter_Setup_1.0.1.exe" explorer.exe "installer-output"
pause
exit /b 0

:failure
echo.
echo BUILD FAILED. Read the error above. No installer was produced.
pause
exit /b 1
