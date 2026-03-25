@echo off
REM ビルドスクリプト - Production版
REM 出力: オーディオ一括設定.exe

setlocal enabledelayedexpansion
cd /d "%~dp0"

echo Building Production version...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" ^
  /nologo ^
  /target:winexe ^
  /platform:anycpu ^
  /optimize+ ^
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
