@echo off

call AssignVersion Version

pushd ..\..

if not exist Backup\%Version% mkdir Backup\%Version%

copy Setup\JohnsHope.FPlot.%Version%.exe Backup\%Version%
copy Backup\JohnsHope.FPlot.Source.%Version%.7z Backup\%Version%

echo binary >>%TEMP%\Upload.FTP.Commands.txt
echo cd incoming >>%TEMP%\Upload.FTP.Commands.txt
echo lcd Setup >>%TEMP%\Upload.FTP.Commands.txt
echo put JohnsHope.FPlot.%Version%.exe >>%TEMP%\Upload.FTP.Commands.txt
echo lcd ..\Backup >>%TEMP%\Upload.FTP.Commands.txt
echo put JohnsHope.FPlot.Source.%Version%.7z >>%TEMP%\Upload.FTP.Commands.txt
echo quit >>%TEMP%\Upload.FTP.Commands.txt

ftp -s:%TEMP%\Upload.FTP.Commands.txt -A upload.sourceforge.net

popd


del %TEMP%\Upload.FTP.Commands.txt

SET Verison=