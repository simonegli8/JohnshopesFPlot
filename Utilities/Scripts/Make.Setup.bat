@echo off

call AssignVersion Version

pushd ..

SET UDIR=%CD%

cd ..\Setup\Release

"%UDIR%\7z" a -tzip "..\JohnsHope.FPlot.%Version%.zip" *.*

cd ..

::del "JohnsHope.FPlot*.exe"
wzipse32 "JohnsHope.FPlot.%Version%.zip" @FPlot.se.config.txt
del "JohnsHope.FPlot.%Version%.zip"

popd

SET UDIR=
SET Version=