param(
  [string]$Slug,

  [string]$Title,

  [string]$Heading,

  [string]$DateText = (Get-Date -Format "yyyy-MM-dd"),

  [switch]$Interactive
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$maintenanceDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$siteRoot = Split-Path -Parent $maintenanceDir
$templatePath = Join-Path $maintenanceDir "page-template.html"

function Read-RequiredInput {
  param(
    [string]$PromptText,
    [string]$DefaultValue
  )

  while ($true) {
    if ([string]::IsNullOrWhiteSpace($DefaultValue)) {
      $value = Read-Host $PromptText
    }
    else {
      $value = Read-Host "$PromptText（未入力なら '$DefaultValue'）"
      if ([string]::IsNullOrWhiteSpace($value)) {
        $value = $DefaultValue
      }
    }

    if (-not [string]::IsNullOrWhiteSpace($value)) {
      return $value
    }

    Write-Host "入力が空です。もう一度入力してください。" -ForegroundColor Yellow
  }
}

if (-not (Test-Path $templatePath)) {
  throw "テンプレートが見つかりません: $templatePath"
}

$useInteractive = $Interactive -or [string]::IsNullOrWhiteSpace($Slug) -or [string]::IsNullOrWhiteSpace($Title)

if ($useInteractive) {
  Write-Host "=== 新規ページ作成（かんたんモード）===" -ForegroundColor Cyan
  Write-Host "URL名（Slug）とページタイトルを入力すると、ページを自動生成します。"
  Write-Host "例: Slug = news-2026-03, Title = お知らせ 2026年3月号"
  Write-Host ""

  $Slug = Read-RequiredInput -PromptText "1/4 URL名（Slug）" -DefaultValue $Slug
  $Title = Read-RequiredInput -PromptText "2/4 ページタイトル" -DefaultValue $Title

  if ([string]::IsNullOrWhiteSpace($Heading)) {
    $Heading = Read-RequiredInput -PromptText "3/4 ページ見出し" -DefaultValue $Title
  }

  if ([string]::IsNullOrWhiteSpace($DateText)) {
    $DateText = Get-Date -Format "yyyy-MM-dd"
  }
  $DateText = Read-RequiredInput -PromptText "4/4 最終更新日（YYYY-MM-DD）" -DefaultValue $DateText
  Write-Host ""
}

if ($Slug -match "^/|/$") {
  throw "Slug は先頭/末尾のスラッシュなしで指定してください（例: news-2026-03）。"
}

if ($Slug -match "[\\/:*?\""<>|]") {
  throw "Slug に使用できない文字が含まれています。"
}

if ([string]::IsNullOrWhiteSpace($Heading)) {
  $Heading = $Title
}

$targetDir = Join-Path $siteRoot $Slug
$targetFile = Join-Path $targetDir "index.html"

if (Test-Path $targetFile) {
  throw "既にページが存在します: $targetFile"
}

if (-not (Test-Path $targetDir)) {
  New-Item -ItemType Directory -Path $targetDir | Out-Null
}

$content = Get-Content -Path $templatePath -Raw
$content = $content.Replace("{{PAGE_TITLE}}", $Title)
$content = $content.Replace("{{PAGE_HEADING}}", $Heading)
$content = $content.Replace("{{UPDATED_AT}}", $DateText)

Set-Content -Path $targetFile -Value $content -Encoding UTF8

Write-Host "ページを作成しました: $targetFile"
Write-Host "ブラウザURLの例: ./$Slug/"

if ($useInteractive) {
  Write-Host ""
  Write-Host "次の作業:"
  Write-Host "1) 作成された index.html を開いて本文を編集"
  Write-Host "2) site-config.json の newsItems などにリンクを追加"
  Write-Host "3) ブラウザで ./$Slug/ を確認"
}
