Multi-Matter V1.0.1 SD/microSD card support

Native SD/MMC buses are now accepted only for removable logical volumes.
USB drive filtering, partition protection and identity revalidation are unchanged.

Build in the MultiMatter directory:
  dotnet publish USBFormatter.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o dist

Insert a microSD card, then use Refresh. Verify the correct letter/size.
Do not format important cards until tested with a blank/spare one.
If it is not shown, send the log at:
  %LOCALAPPDATA%\Multi-Matter\drive-detection.log

Some Dell card readers present SD media as SCSI rather than native SD/MMC.
Those ambiguous classifications remain excluded for safety.
