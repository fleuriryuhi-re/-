# ビルドスクリプト - Production版
# 出力: オーディオ一括設定.exe

$scriptPath = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Set-Location $scriptPath

Write-Host "Building Production version..." -ForegroundColor Cyan

$compiler = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
$output = "オーディオ一括設定.exe"
$sourceFile = "AudioSetupUI.cs"

& $compiler /nologo `
  /target:winexe `
  /platform:anycpu `
  /optimize+ `
  "/out:$output" `
  /reference:System.dll `
  /reference:System.Windows.Forms.dll `
  /reference:System.Drawing.dll `
  $sourceFile

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Build successful!" -ForegroundColor Green
    Write-Host "Output: $(Get-Item $output | Select-Object -ExpandProperty FullName)"
} else {
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}
