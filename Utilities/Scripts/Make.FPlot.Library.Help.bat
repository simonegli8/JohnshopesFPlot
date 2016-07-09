@echo off

pushd ..\..\JohnsHope.FPlot.Library\doc

copy ..\bin\JohnsHope.FPlot.Library.dll .
copy ..\bin\JohnsHope.FPlot.Library.xml .

..\..\Utilities\Scripts\Build.Sandcastle prototype JohnsHope.FPlot.Library

popd

pause