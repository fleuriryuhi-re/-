@echo off
setlocal enabledelayedexpansion
chcp 65001 > nul
cd /d "%~dp0"
set "ICON=%~dp0assets\headset.ico"

REM Production build script
REM Output: オーディオ一括設定.exe

echo Building Production version...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" ^
  /nologo ^
  /target:winexe ^
  /platform:anycpu ^
  /optimize+ ^
  /win32icon:"%ICON%" ^
  /out:"オーディオ一括設定.exe" ^
  /reference:System.dll ^
  /reference:System.Windows.Forms.dll ^
  /reference:System.Drawing.dll ^
  "AudioSetupUI.cs"

if %ERRORLEVEL% equ 0 (
  echo.
  echo Build successful!
  echo Output: %cd%\オーディオ一括設定.exe
) else (
  echo Build failed with exit code %ERRORLEVEL%
  exit /b %ERRORLEVEL%
)

endlocal
