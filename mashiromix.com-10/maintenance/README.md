# Maintenance Manual（PC初心者向け）

このフォルダーは、サイトの更新を安全に行うための運用フォルダーです。
現在は **Webツール一元管理** に切り替え済みです。

## 運用方針（2026-03-08以降）

- 運用の中心は `maintenance/page-builder.html` です。
- `site-maintenance.js` と `site-config.json` に依存しない運用へ移行済みです。
- 「Webへ反映（公開）」時のお知らせ自動追加は、`index.html` へ直接追記されます。
- 旧手順（site-config編集ベース）は互換情報として残していますが、新規運用では使用しません。

## 0. 最初に読む（超重要）

- 編集するのは `site-config.json` だけです。
- `index.html` は原則編集しません。
- 反映ロジックは `site-maintenance.js` が自動で行います。
- 迷ったら、まずこのREADMEの「3. よく使う記載例」を見てください。

## 1. 3分でできる更新手順

1. `site-config.json` を開く
2. 変更したい項目だけ書き換える
3. ファイル保存（Ctrl + S）
4. ブラウザを再読み込み（Ctrl+F5推奨）

反映が弱いときは、ブラウザのコンソールで以下を実行します。

```js
window.dispatchEvent(new Event('site-config:refresh'));
```

## 2. 初回移行の進め方（推奨順あり）

初回移行は、一気に全部ではなく段階的に進めるのが安全です。
推奨順は次の通りです。

1. banner
2. news
3. works
4. about
5. contact
6. footer

この順番は `site-config.json` の
`migration.recommendedPhasedRollout` に実データとして同梱済みです。

### 2-1. Step 1（bannerだけ有効）記載例

```json
"migration": {
  "enabledSections": {
    "mainVisual": false,
    "banner": true,
    "news": false,
    "works": false,
    "about": false,
    "contact": false,
    "footer": false
  }
}
```

### 2-2. Step 4（banner/news/works/about）記載例

```json
"migration": {
  "enabledSections": {
    "mainVisual": false,
    "banner": true,
    "news": true,
    "works": true,
    "about": true,
    "contact": false,
    "footer": false
  }
}
```

### 2-3. 全面移行（footerまで有効）記載例

```json
"migration": {
  "enabledSections": {
    "mainVisual": false,
    "banner": true,
    "news": true,
    "works": true,
    "about": true,
    "contact": true,
    "footer": true
  }
}
```

## 3. よく使う記載例（そのまま流用OK）

以下は実際に使えるサンプルです。必要な値だけ書き換えてください。

### 3-1. バナーを1件追加

```json
"bannerItems": [
  {
    "href": "https://example.com",
    "target": "_blank",
    "image": "./images/banner/sample/thumb/banner.jpg",
    "title": "サンプルバナー"
  }
]
```

### 3-2. お知らせを1件追加

```json
"newsItems": [
  {
    "date": "2026-03-08",
    "text": "公式サイトを更新しました。"
  }
]
```

### 3-3. お知らせ本文をリンク化

```json
"newsItems": [
  {
    "date": "2026-03-08",
    "text": "新しいお知らせ詳細はこちら",
    "href": "https://example.com/news/1",
    "target": "_blank"
  }
]
```

### 3-4. 作品セクションのリンク更新

```json
"works": {
  "moreHref": "./circle/",
  "moreTarget": "_self",
  "movieHref": "https://www.youtube.com/watch?v=xxxxxxxxxxx",
  "movieTarget": "_blank"
}
```

### 3-5. プロフィールの自己紹介を更新

```json
"about": {
  "job": "CreaterVTuber.",
  "name": "mashiro",
  "nameJa": "ましろ",
  "twitterHref": "https://twitter.com/mashiro6425",
  "descriptions": [
    "Live2Dと映像制作を中心に活動しています。",
    "ご依頼はお問い合わせからお願いします。"
  ],
  "masterpieceLabel": "代表作",
  "masterpiece": "実績テキストをここに記載"
}
```

### 3-6. お問い合わせボタンを差し替え

```json
"contact": {
  "requestHtml": "ご依頼いただく際は<br />・<span>内容</span><br />・<span>稿料</span><br />・<span>納期</span><br />を必ず書いていただけると幸いです。",
  "buttons": [
    {
      "href": "mailto:info@example.com",
      "target": "_blank",
      "text": "メールでお問い合わせ"
    },
    {
      "href": "https://docs.google.com/forms/d/e/xxxxxxxx/viewform",
      "target": "_blank",
      "text": "Googleフォームでお問い合わせ"
    }
  ],
  "smallTextHtml": "※無償のご依頼は受け付けておりません。"
}
```

### 3-7. フッター文言を変更

```json
"footer": {
  "copyright": "© 2026 水咲ましろ/MashiroMIX."
}
```

## 4. 入力ルール（失敗しやすい所）

- JSONでは、行の最後のカンマ漏れに注意
- URLは `https://` から書く
- 画像パスは `./images/...` の相対パスで書く
- 改行したい文章は `<br />` を使う
- 内部リンクは `./circle/` のように相対パス推奨

## 5. 作業前チェックリスト

- 変更前に `site-config.json` をコピーしてバックアップ
- 1回の更新は1セクションだけ
- 変更後は必ず表示確認（PC表示）
- 問題があれば `enabledSections` で切り戻す

## 6. トラブル時の戻し方（切り戻し手順）

1. `site-config.json` を開く
2. 問題があるセクションを `false` にする
3. 保存して再読み込みする

例: aboutを止める

```json
"enabledSections": {
  "mainVisual": false,
  "banner": true,
  "news": true,
  "works": true,
  "about": false,
  "contact": true,
  "footer": true
}
```

## 7. よくある質問

### Q1. 何を編集すればいいかわかりません

A. まず `site-config.json` を開き、
このREADMEの「3. よく使う記載例」から近い例を探してください。

### Q2. 反映されません

A. Ctrl+F5で再読み込みしてください。
それでも反映されない場合は、コンソールで再読込イベントを実行してください。

### Q3. 見た目が崩れました

A. 直前に編集したセクションを `false` にして切り戻し、
そのセクションだけ値を見直してください。

## 8. 新しいページをかんたん作成する方法

この機能は、PC初心者でも使えるように2通りあります。

- 方法A（オススメ）: ダブルクリックで作成
- 方法B: PowerShellコマンドで作成

### 8-1. 方法A（オススメ）ダブルクリックで作成

1. `maintenance` フォルダーを開く
1. `create-page-easy.bat` をダブルクリック
1. 画面の質問に順番に入力

  入力例:
  1/4 URL名（Slug）例: `news-2026-03`
  2/4ページタイトル例: `お知らせ2026年3月号`
  3/4ページ見出し（空ならタイトルと同じ）
  4/4最終更新日（空なら今日の日付）

1. 完了メッセージが出たら作成成功

作成される場所:

- `作成したSlug/index.html`
- 例: `news-2026-03/index.html`

### 8-2. 方法B（コマンドで作成）

PowerShellで以下を実行します。

```powershell
cd .\mashiromix.com-10
.\maintenance\create-page.ps1 -Slug "news-2026-03" -Title "お知らせ 2026年3月号"
```

### 8-3. 入力値の意味（初心者向け）

- Slug（スラッグ）
  - URLに使う英数字名です
  - 例: `news-2026-03`, `event`, `release-note`
  - 使えない文字: `\\ / : * ? " < > |`
- Title
  - ブラウザタイトルに表示される文字
- Heading
  - ページ内の見出し（未入力ならTitleと同じ）
- DateText
  - 最終更新日（`YYYY-MM-DD`）

### 8-4. 作成後に必ずやること

1. 作成された `index.html` を開いて本文を編集
2. `site-config.json` の `newsItems` などにリンクを追加
3. ブラウザで `./作成したSlug/` にアクセスして確認

### 8-5. 記載例（コピペ用）

お知らせページを作る:

```powershell
.\maintenance\create-page.ps1 -Slug "news-2026-03" -Title "お知らせ 2026年3月号"
```

イベントページを作る（見出し別名）:

```powershell
.\maintenance\create-page.ps1 -Slug "event" -Title "イベント情報" -Heading "イベントのお知らせ"
```

更新履歴ページを作る（日付指定）:

```powershell
.\maintenance\create-page.ps1 -Slug "release-note" -Title "更新履歴" -DateText "2026-03-08"
```

### 8-6. よくあるエラーと対処

- 「すでにページが存在します」
  - 同じSlugがすでにあります。別のSlugに変更してください。
- 「Slugに使用できない文字が含まれています」
  - Slugを英数字とハイフン中心で作り直してください。
- 実行できない（権限エラー）
  - `create-page-easy.bat` を使うか、次を実行してから再実行してください。

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

## 9. Web上でページを作る（WordPress風の新ツール）

コマンドが苦手な方向けに、ブラウザで入力して新規ページを作るツールを追加しました。
既存配置は変更せず、`maintenance` 配下に独立して動作します。

- ツール本体: `maintenance/page-builder.html`
- かんたん起動: `maintenance/open-page-builder.bat` をダブルクリック
- 特徴:
  - `index.html` と同じフォント（Adobe Fonts / Typekit: `mtj3xoq`）を読み込み
  - フォント選択（index準拠 / ゴシック / 明朝 / 丸ゴシック）
  - テンプレート選択（お知らせ / News簡易更新 / プロフィール / イベント / CIRCLE / 自由入力）
  - テンプレごとに最終HTMLの準拠レイアウトを固定化（NEWS / ABOUT / WORKS / CIRCLE / CONTACT系）
  - テンプレごとに見出し初期文を固定プリセット化
  - テンプレごとに推奨ブロック順を固定プリセット化
  - 画像アップロード（ブロック単位）
  - 文章ブロック追加
  - ボタンリンク追加
  - ブロックごとの位置微調整（横/縦スライダー）
  - 文字配列の微調整（左寄せ / 中央寄せ / 右寄せ）
  - 全リンク編集（ボタン / 文章 / 画像 / 戻るリンク / ヘッダーホームリンク）
  - カスタムHTML/JSブロック追加（ブロック単位で自由編集）
  - ページ全体カスタムCSS/カスタムJSの埋め込み
  - プレビュー上でドラッグ＆ドロップ操作
  - リアルタイムプレビュー（`index.html` と同じサイトCSSを参照）
  - ページ作成ツールからサイトへ直接反映（公開）
  - HTML保存（ダウンロード / 直接保存）

固定プリセット例:

- お知らせ: 文章 → ボタン
- News簡易更新: お知らせ本文 + 日付 + リンク（任意）のみ
- プロフィール: 画像 → 文章
- イベント: 画像 → 概要（文章）→ ボタン
- CIRCLE: 紹介文（文章）→ メイン画像 → リンクカード（ボタン）
- 自由入力: 文章

### 9-1. 使い方（最短手順）

1. `maintenance/open-page-builder.bat` をダブルクリック
1. （または）`maintenance/page-builder.html` をブラウザで開く
1. テンプレートを選んで「テンプレ反映」を押す
1. タイトル・見出し・本文・画像を入力する
1. 右側プレビューで確認する
1. 「サイトフォルダー接続」で `mashiromix.com-10` を選択
1. 「Webへ反映（公開）」を押す

### 9-1-1. お知らせだけ更新する（リンク任意）

ページを新規作成せず、`index.html` のNEWS一覧だけ更新できます。

1. 最終更新日を入力
1. 「ページタイトル」にお知らせ本文を入力
1. 必要なら「お知らせリンク（任意）」を入力（空欄なら文字のみ）
1. 「サイトフォルダー接続」で `mashiromix.com-10` を選択
1. 「お知らせのみ追加」を押す

補足:

- リンクを空欄にすると、クリック無しのテキストお知らせとして追加されます。
- リンクありの場合は同一リンクの重複追加を自動でスキップします。

### 9-1-1-1. JSを使った自由編集（上級者向け）

- 「カスタムHTML/JS追加」で、任意のHTMLとJSをブロックとして追加できます。
- 「ページ全体カスタムCSS / カスタムJS」で、ページ全体に対する見た目・動作を調整できます。
- 入力したコードは公開HTMLへそのまま埋め込まれます。
- News簡易更新テンプレでは安全運用のため、これらの自由編集UIは自動で無効化されます。

### 9-1-1. 直接反映（公開）の仕様

- 出力先: `作成したSlug/index.html`
- 例: Slugが `news-2026-03` の場合は `news-2026-03/index.html` が上書き/作成されます
- 「公開時にお知らせへ自動追加」がONなら、`index.html` のNEWS一覧へ公開情報を自動追記します
- 「公開設定をindex.htmlと各ページへ即時反映」がONなら、公開時にサイト内の各`index.html`へリンク設定を一括反映します
- 「反映対象フォルダー選択」を使うと、選択フォルダー配下の`index.html`のみに一括反映できます
- 「カスタムCSS/JSも全ページへ反映」がONなら、ページ全体カスタムCSS/JSも各ページへ一括反映します
- 同じ `href`（`./slug/`）がすでにある場合は重複追加しません

### 9-1-2. プレビューのドラッグ＆ドロップ

- ブロック並べ替え:
  - （位置調整モードOFF時）
  - 右側プレビュー内のブロックをドラッグして、移動したい位置でドロップ
  - 左側の編集ブロック順にも自動反映されます
- 画像追加:
  - 画像ファイルを右側プレビューへドロップすると、画像ブロックを自動追加
  - 複数画像を同時ドロップした場合は複数ブロックとして追加されます

### 9-1-2-1. プレビュー上で位置を直接微調整

- 「プレビュー位置調整モード」をONにすると、
  プレビュー上のブロックを直接ドラッグして位置調整できます
- 位置調整モードON時は「並べ替え」ではなく「位置移動」になります
- 調整結果は左側の横/縦スライダーにも反映されます

### 9-1-3. 位置と文字配列の微調整

- 左側ブロック編集欄で、各ブロックの次を調整できます
  - 横位置（微調整）
  - 縦位置（微調整）
  - 文字配列（左寄せ / 中央寄せ / 右寄せ）
- スライダーはマウス操作で1px単位調整できます
- 「位置を初期化」でそのブロックの位置だけ元に戻せます

### 9-2. 画像を入れる方法

1. 「画像ブロック追加」を押す
1. 画像ファイルを選ぶ
1. 必要なら代替テキスト（alt）とキャプションを入力する

補足:

- 画像はページ内に埋め込み形式で保存されるため、
  画像パスの設定ミスが起きにくく、初心者向けです。

### 9-3. 保存方法の違い

- `HTMLをダウンロード`
  - どのブラウザでも使いやすい保存方法
  - 保存されたファイル名は `slug-index.html`
- `フォルダーへ直接保存`
  - 対応ブラウザで `slug/index.html` を直接作成
  - ブラウザ仕様により使えない環境ではエラー表示されます

### 9-4. レイアウトが崩れないための設計

このツールは、出力HTMLに固定スタイルを同梱します。

- 画像は `width: 100%` / `height: auto` でレスポンシブ固定
- テキストは `overflow-wrap: anywhere` で長文折返し
- セクションごとに余白と境界線を固定
- スマホ幅でも見出しサイズを自動調整

そのため、通常入力の範囲で大きく崩れにくい設計です。

### 9-5. まず試す入力例

- Slug: `news-2026-03`
- ページタイトル: `お知らせ2026年3月号`
- ページ見出し: `お知らせ`
- 本文: `公式サイトを更新しました。`

### 9-6. 注意点

- Slugは英数字とハイフン推奨（例: `event-2026`）
- Slugに `\\ / : * ? " < > |` は使えません
- 既存サイトにリンクする場合は `../` や `./circle/` など相対リンクを使う

