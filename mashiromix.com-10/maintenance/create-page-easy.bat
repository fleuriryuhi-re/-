@echo off
setlocal

set SCRIPT_DIR=%~dp0
set PS_SCRIPT=%SCRIPT_DIR%create-page.ps1

if not exist "%PS_SCRIPT%" (
  echo [ERROR] create-page.ps1 が見つかりません。
  pause
  exit /b 1
)

echo =============================================
echo   新規ページ作成（かんたんモード）
echo =============================================
echo この画面のあと、入力案内に従ってください。
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_SCRIPT%" -Interactive

echo.
echo 完了しました。ウィンドウを閉じるには何かキーを押してください。
pause >nul
