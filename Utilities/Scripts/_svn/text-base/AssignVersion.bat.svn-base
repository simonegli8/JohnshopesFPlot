:: Store assembly version to temp file
..\AssemblyVersion ..\..\JohnsHope.FPlot\bin\Release\JohnsHope.FPlot.exe >%TEMP%\FPlotAssemblyVersion.txt 

:: Read assembly version into variable
set /p %1=<%TEMP%\FPlotAssemblyVersion.txt

:: Delete temp file
del %TEMP%\FPlotAssemblyVersion.txt