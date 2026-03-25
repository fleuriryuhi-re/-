@echo off
setlocal
chcp 65001 > nul

set "SCRIPT_DIR=%~dp0"
set "SRC=%SCRIPT_DIR%AudioSetupUI_dev.cs"
set "EXE=%SCRIPT_DIR%オーディオ一括設定dev.exe"
set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%CSC%" (
	echo [ERROR] csc.exe が見つかりません。
	pause
	exit /b 1
)

if not exist "%SRC%" (
	echo [ERROR] AudioSetupUI_dev.cs が見つかりません。
	echo パス: %SRC%
	pause
	exit /b 1
)

echo ============================================================
echo  Windows サウンド設定 UI [開発用] をビルドしています...
echo ============================================================
echo.

"%CSC%" /nologo /target:winexe /platform:anycpu /optimize+ /out:"%EXE%" /reference:System.dll /reference:System.Windows.Forms.dll /reference:System.Drawing.dll "%SRC%"
set "EXIT_CODE=%ERRORLEVEL%"

echo.
if not "%EXIT_CODE%"=="0" (
	echo [WARN] ビルドに失敗しました。終了コード: %EXIT_CODE%
	pause
	endlocal
	exit /b %EXIT_CODE%
)

start "" "%EXE%"
endlocal
exit /b 0
