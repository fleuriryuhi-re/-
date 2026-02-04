"""
CuteButtonApp - Kivy版（Android/iOS対応）
"""

import os
import time
from pathlib import Path
from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.gridlayout import GridLayout
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.image import Image as KivyImage
from kivy.core.window import Window
from kivy.garden.matplotlib.backend_kivyagg import FigureCanvasKivyAgg
from kivy.core.audio import SoundLoader

# アセットディレクトリ
ASSETS_DIR = Path(__file__).parent / "assets"
ICON_PATH = str(ASSETS_DIR / "icon.png")
CHAR1_PATH = str(ASSETS_DIR / "char1.png")
CHAR2_PATH = str(ASSETS_DIR / "char2.png")
SOUND_PATH = str(ASSETS_DIR / "click.wav")

# ウィンドウサイズ
Window.size = (400, 600)


class CuteButtonApp(App):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.count = 0
        self.anim_state = 0
        self._last_click = 0

    def build(self):
        self.title = "可愛いボタン連打ソフト"

        # メインレイアウト
        main_layout = BoxLayout(orientation="vertical", spacing=10, padding=10)

        # キャラクター画像
        try:
            char_img = KivyImage(source=CHAR1_PATH, size_hint_y=0.3)
            main_layout.add_widget(char_img)
        except Exception:
            pass

        # ボタンレイアウト
        button_layout = BoxLayout(size_hint_y=0.4)
        self.button = Button(
            text="",
            background_normal=ICON_PATH,
            background_down=ICON_PATH,
            size_hint=(0.6, 1),
        )
        self.button.bind(on_press=self._on_button_click)
        button_layout.add_widget(Label(size_hint_x=0.2))
        button_layout.add_widget(self.button)
        button_layout.add_widget(Label(size_hint_x=0.2))
        main_layout.add_widget(button_layout)

        # カウント表示
        count_layout = BoxLayout(size_hint_y=0.15)
        self.count_label = Label(
            text=f"カウント: {self.count}",
            font_size="32sp",
            color=(1, 0.42, 0.7, 1),
        )
        count_layout.add_widget(self.count_label)
        main_layout.add_widget(count_layout)

        # 一時表示用ラベル（フラッシュ用）
        self.flash_label = Label(
            text="",
            font_size="24sp",
            size_hint_y=0.1,
            color=(1, 0.3, 0.5, 1),
        )
        main_layout.add_widget(self.flash_label)

        # リセットボタン
        reset_button = Button(text="リセット", size_hint_y=0.1)
        reset_button.bind(on_press=self._reset_count)
        main_layout.add_widget(reset_button)

        return main_layout

    def _on_button_click(self, instance=None):
        """ボタンをクリック時の処理"""
        now = time.time()
        if now - self._last_click < 0.05:
            return
        self._last_click = now

        self.count += 1
        self._update_ui()
        self._play_sound()
        self._show_flash()

    def _update_ui(self):
        """UI更新"""
        self.count_label.text = f"カウント: {self.count}"
        # キャラクター切り替え（アニメーション）
        self.anim_state = (self.anim_state + 1) % 2

    def _play_sound(self):
        """効果音を再生"""
        try:
            sound = SoundLoader.load(SOUND_PATH)
            if sound:
                sound.play()
        except Exception:
            # Kivy SoundLoader が使えない場合の fallback
            try:
                from playsound import playsound
                playsound(SOUND_PATH)
            except Exception:
                pass

    def _show_flash(self):
        """カワイイ！と表示"""
        self.flash_label.text = "カワイイ！"
        # 1秒後に消す
        self.root.after(1000, lambda: setattr(self.flash_label, "text", ""))

    def _reset_count(self, instance=None):
        """カウントをリセット"""
        self.count = 0
        self.count_label.text = f"カウント: {self.count}"
        self.flash_label.text = ""
        self.anim_state = 0


if __name__ == "__main__":
    CuteButtonApp().run()
