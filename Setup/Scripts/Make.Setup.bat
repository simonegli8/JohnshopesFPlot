@echo off

pushd ..\..\Utilities

call .\Scripts\AssingVersion "..\JohnsHope.FPlot\bin\Release\JohnsHope.FPlot.exe" 2.20

SET UDIR=%CD%

cd ..\Setup\Release

"%UDIR%\7z" a -t7z "..\JohnsHope.FPlot.2.20.7z" *.*

cd ..

::del "JohnsHope.FPlot*.exe"
REM wzipse32 "JohnsHope.FPlot.%Version%.7z" @FPlot.se.config.txt
REM del "JohnsHope.FPlot.%Version%.7z"

popd

SET UDIR=
SET Version=