@echo off
setlocal

set HTML=%~dp0page-builder.html

if not exist "%HTML%" (
  echo [ERROR] page-builder.html が見つかりません。
  pause
  exit /b 1
)

start "" "%HTML%"
