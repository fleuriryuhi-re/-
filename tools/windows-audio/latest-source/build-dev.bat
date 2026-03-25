@echo off
REM ビルドスクリプト - Dev版
REM 出力: オーディオ一括設定dev.exe

setlocal enabledelayedexpansion
chcp 65001 > nul
cd /d "%~dp0"

echo Building Dev version...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" ^
  /nologo ^
  /target:winexe ^
  /platform:anycpu ^
  /optimize+ ^
  /out:"オーディオ一括設定dev.exe" ^
  /reference:System.dll ^
  /reference:System.Windows.Forms.dll ^
  /reference:System.Drawing.dll ^
  "AudioSetupUI_dev.cs"

if %ERRORLEVEL% equ 0 (
  echo.
  echo Build successful!
  echo Output: %cd%\オーディオ一括設定dev.exe
) else (
  echo Build failed with exit code %ERRORLEVEL%
  exit /b %ERRORLEVEL%
)

endlocal
