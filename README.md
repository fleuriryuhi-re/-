# discordpy-startup

[![Deploy](https://www.herokucdn.com/deploy/button.svg)](https://heroku.com/deploy)

- Herokuでdiscord.pyを始めるテンプレートです。
- Use Template からご利用ください。
- 使い方はこちら： [Discord Bot 最速チュートリアル【Python&Heroku&GitHub】 - Qiita](https://qiita.com/1ntegrale9/items/aa4b373e8895273875a8)

## 各種ファイル情報

### discordbot.py
PythonによるDiscordBotのアプリケーションファイルです。

### requirements.txt
使用しているPythonのライブラリ情報の設定ファイルです。

### Procfile
Herokuでのプロセス実行コマンドの設定ファイルです。

### runtime.txt
Herokuでの実行環境の設定ファイルです。

### app.json
Herokuデプロイボタンの設定ファイルです。

### AudioSetupUI.cs (本番環境)
Windows オーディオデバイスの一括設定ツール（本番用）です。Plantronics DA80 ヘッドセットを使用します。  
`setup-audio.bat` を実行するとコンパイルして起動します。

### AudioSetupUI_dev.cs (開発環境)
Windows オーディオデバイスの一括設定ツール（開発用）です。Realtek ヘッドホン／マイクを使用します。  
`setup-audio-dev.bat` を実行するとコンパイルして起動します。

### setup-audio.bat
本番環境ビルド＆起動スクリプトです。`AudioSetupUI.cs` をコンパイルして `AudioSetupUIApp10.exe` を生成・起動します。

### setup-audio-dev.bat
開発環境ビルド＆起動スクリプトです。`AudioSetupUI_dev.cs` をコンパイルして `AudioSetupUIApp_dev.exe` を生成・起動します。

### .github/workflows/flake8.yaml
GitHub Actions による Python 構文チェック（flake8）の設定ファイルです。

### .github/workflows/csharp-build.yaml
GitHub Actions による C# ビルド検証ワークフローです。  
プッシュのたびに本番環境（`AudioSetupUI.cs`）と開発環境（`AudioSetupUI_dev.cs`）の両方をWindows上でコンパイルして、安定してビルドできることを自動検証します。

### .gitignore
Git管理が不要なファイル/ディレクトリの設定ファイルです。

### LICENSE
このリポジトリのコードの権利情報です。MITライセンスの範囲でご自由にご利用ください。

### README.md
このドキュメントです。
