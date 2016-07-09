@echo off

if {%1} == {/?} (

  echo.
  echo Options for backup.bat:
  echo.
  echo backup [/svn] [path]
  echo.
  echo If the /svn option is set, the _svn folders will be included in the backup.
  echo.
  echo If a path is passed to the backup script, the 
  echo backup files will be copied to that path.
  pause
  exit /b

)

:: assing assembly version to the variable Version
call AssignVersion Version
if {%Version%} == {} SET Version="Latest"

pushd ..\..

echo.
echo Cleanup...

del /S /Q JohnsHope.FPlot\obj\*.*
del /S /Q JohnsHope.FPlot.Library\bin\Debug\*.*
del /S /Q JohnsHope.FPlot.Library\obj\*.*
del /S /Q JohnsHope.FPlot.Library\doc\Output\html\*.*
del /S /Q JohnsHope.FPlot.Library\doc\chm\html\*.*
del /S /Q JohnsHope.FPlot.Library\doc\*.xml
del /S /Q JohnsHope.FPlot.Excel\obj\*.*
del /S /Q WebDemo\temp\*.*
del /S /Q "WinForms Demo\bin\*.*"
del /S /Q "WinForms Demo\obj\*.*"

echo.
echo Creating backup archive...

if not exist Backup mkdir Backup

:: Create backup archive
del "Backup\JohnsHope.FPlot.Source.%Version%.7z"

if {%1} == {/svn} (
  Utilities\7zG a -r -xr!Backup\*.* "Backup\JohnsHope.FPlot.Source.%Version%.svn.7z" *.*
  shift
) else (
  Utilities\7zG a -r -xr!Backup\*.* -xr!_svn "Backup\JohnsHope.FPlot.Source.%Version%.7z" *.*
)
:: Copy backup archive and setup executable to destination
if not {%1} == {} (

  echo.
  echo Copy backup archive and setup executable to %1

  copy "Backup\JohnsHope.FPlot.Source.%Version%.7z" %1
  copy "Setup\JohnsHope.FPlot*.exe" %1

) ELSE (

  echo.
  echo You can pass a destination path to this script,
  echo so the archive files will be copied to that path.

)

popd

:: Delete variable
set Version=

:End