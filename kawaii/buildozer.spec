[app]

# アプリケーション情報
title = Cute Button App
package.name = cutebutton
package.domain = org.kawaii

# ソースとバージョン
source.dir = .
source.include_exts = py,png,jpg,kv,atlas,wav
source.main = main_kivy.py
version = 1.0.0

# 必要な依存関係
requirements = python3,kivy,pillow,playsound

# 画面設定
orientation = portrait
fullscreen = 0

# Android設定
android.permissions = INTERNET
android.api = 31
android.minapi = 21
android.ndk = 25.1.8937393
android.archs = arm64-v8a,armeabi-v7a

# アイコン設定
icon.filename = %(source.dir)s/assets/icon.png

[buildozer]

# ビルド設定
log_level = 2
warn_on_root = 1
