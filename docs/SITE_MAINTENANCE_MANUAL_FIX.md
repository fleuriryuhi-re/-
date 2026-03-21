# サイト保守メンテナンスマニュアル FIX版

このマニュアルは、このワークスペース内のサイトを初心者でも安全に更新できるようにまとめた実務向けの手順書です。

---

## 0. まずここだけ見ればOK（クイックFIX）

### 0-1. どこを触るか（結論）

1. トップバナーを増減する: `htmllatest/index.html` の `__NEXT_DATA__` 内 `res.banners`
2. WORKS「詳細」遷移を調整する: `htmllatest/_next/static/chunks/pages/index-c083d6e47c74352d52ac.js`
3. WORKS遷移先カード（`work-1` など）を調整する: `htmllatest/works/index.html`
4. 修正後は必ず `mashiromix.com-10` 側へ同内容を反映

### 0-2. 反映順（毎回固定）

1. `htmllatest` で編集
2. 表示確認
3. `mashiromix.com-10` へ同内容を反映

### 0-3. 現在のFIX状態（2026-03時点）

1. トップバナー件数: 3件（`sort: 1,2,3`）
2. WORKS詳細遷移: `/works/#work-x` 形式
3. WORKS側: `id="work-1"` から `id="work-8"` を保持
4. ハッシュ遷移時: 該当カードをハイライトし、画像モーダルを自動表示
5. 追加していたメニューアイコン: 廃止済み

対象:

1. 画像差し替え
2. リンク差し替え
3. 画像追加
4. circleページの更新と追加
5. 所属タレントページの編集とメンバー追加
6. 更新時の注意点

---

## 1. 最初に理解すること（karory.net準拠）

このサイトは、主に次の2系統で管理されています。

1. htmllatest
2. mashiromix.com-10

基本ルール:

1. まず `htmllatest` を直す（一次更新先）
2. 表示確認する
3. 問題がなければ `mashiromix.com-10` に同じ内容を反映する（二次反映先）

この2つはミラー扱いです。片方だけ更新すると、環境によって表示差分が出る可能性があります。

---

## 2. どのページをどこで直すか

### 2-1. トップページ

編集対象:

1. `htmllatest/index.html`
2. `mashiromix.com-10/index.html`

主に編集する内容:

1. バナー画像
2. バナーリンク
3. メニューリンク
4. 各種外部リンク
5. お知らせ更新はmaintenance/page-builder.htmlから行う

注意:

トップバナーの増減は、**下部の `__NEXT_DATA__` 内 `res.banners` を編集するのが基準**です。

`slick-slide` などの見えているスライダーHTMLは、初期描画断片が混在して見えるため、通常はここを手編集しません。
`_originalUrl`（ctfassets.net）は参照履歴として残っている項目なので、通常は `url` / `thumbUrl` / `link` / `sort` の整合を優先します。

---

### 2-2. circleページ

編集対象:

1. `htmllatest/circle/index.html`
2. `mashiromix.com-10/circle/index.html`

主に編集する内容:

1. サークル説明
2. リンクカード
3. circle用画像
4. 下部JSON内のバナーや画像情報

---

### 2-3. 所属メンバーページ

編集対象:

1. `htmllatest/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`
2. `mashiromix.com-10/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`

主に編集する内容:

1. メンバー名
2. イラスト
3. アイコン
4. 自己紹介文
5. ステータス
6. ギャラリー画像
7. 所属区分
8. メンバー追加

注意:

このページはHTMLではなくJavaScriptの配列データ（`CharacterList`）で管理しています。
各メンバーは1人ずつ `description` と `group` を持てるため、所属タレント・所属クリエイターのどちらでも自己紹介を個別編集できます。
所属区分は `group: "talent"` または `group: "creator"` で判定します。
波かっこ、角かっこ、カンマの位置を崩すとページが壊れます。

---

## 3. 更新前の基本ルール

更新前は必ず次を守ってください。

1. いきなり大量に変えない
2. 1ページずつ直す
3. 1回の修正で1テーマだけ触る
4. 先に `htmllatest` を直す
5. 問題なければ `mashiromix.com-10` を同じ内容にする

初心者向けのオススメ順:

1. 画像差し替え
2. リンク差し替え
3. 文言変更
4. 画像追加
5. メンバー追加

---

## 4. 画像差し替えのやり方

### 4-1. 一番安全な方法

一番安全なのは、同じファイル名のまま画像ファイルだけ差し替える方法です。

例:

1. すでに使われている画像ファイルを探す
2. 同じ名前で新しい画像に置き換える
3. パスが変わらないので、HTMLやJSの修正量を減らせる

### 4-2. ファイル名も変える場合

その場合は、画像ファイルを置くだけでなく、参照先も変更します。

変更対象の例:

1. `<img src="...">`
2. `background-image:url(...)`
3. JSON内の `url`
4. JSON内の `thumbUrl`

### 4-3. トップページで画像差し替えする場所

例:

1. メインビジュアル画像: `htmllatest/index.html` 内の `/images/main-visual/...`
2. バナー画像表示: `htmllatest/index.html` 内の `/images/banner/...`
3. バナーデータ本体: `htmllatest/index.html` 下部 `__NEXT_DATA__` 内の `banners`

初心者向け注意:

1. トップページのバナーは見えている箇所だけでなく、下のデータ側も合わせる
2. `url` と `thumbUrl` が別れているときは両方確認する
3. 画像サイズや縦横比は元画像にできるだけ近づける

---

## 5. リンク差し替えのやり方

リンク差し替えは基本的に `href` を変更するだけです。

例:

1. `<a href="旧URL">` を探す
2. `href` の値を新URLへ変更する

注意:

1. 外部サイトなら `target="_blank"` の有無を確認する
2. サイト内ページなら相対パスか絶対パスかを既存に合わせる
3. バナーのリンクは表示HTML側とJSON側の両方を確認する

---

## 6. 画像追加のやり方

画像追加は次の2段階です。

1. 画像ファイルを置く
2. その画像を参照する記述を追加する

初心者向けに一番失敗しにくい方法は、既存の1ブロックをコピーして中だけ変える方法です。

---

## 7. トップページ更新の実務手順

### 7-1. バナー差し替え

編集対象:

1. `htmllatest/index.html`
2. `mashiromix.com-10/index.html`

見るポイント:

1. 下部の `__NEXT_DATA__` 内の `banners`（ここを編集）

更新手順:

1. 新画像を `images/banner/...` に配置する
2. `__NEXT_DATA__` 内 `banners` の対象要素で`url` / `thumbUrl` を変更する
3. `link` がある場合は一緒に変更する
4. `sort` の順番が崩れていないか確認する
5. 動作確認後、`mashiromix.com-10/index.html` へ同内容を反映する

注意:

1. `title` が `empty` のままでも動くが、管理上は分かりにくい
2. できれば後で見てわかる名前にしておく
3. 空の `href` や古いリンクが残っていないか確認する

### 7-1-1. バナー件数を増減する（今回のFIXで使った手順）

件数変更の対象は `res.banners` 配列です。

1. 1件減らす: 配列から1要素削除
2. 1件増やす: 配列へ1要素追加
3. 順番調整: `sort` を連番や意図した順に調整

例（実施済み）:

1. `sort:6` を削除
2. `sort:7` を1件追加
3. その後、試験のため最終的に3件（`sort:1,2,3`）へ縮小

ポイント:

1. 追加要素は `sort` / `image` / `title` / `link` を揃える
2. `image` には `url` / `thumbUrl` / `_originalUrl` / `height` / `width` / `title` を入れる
3. 追加・削除後は必ず表示確認する

### 7-1-2. よくある質問

Q.「現在準備中」が並ぶ部分はどこを直す？

1. `index.html` の `__NEXT_DATA__` 内 `res.banners` を編集

Q.`slick-slide` を直接編集していい？

1. 通常は非推奨（データ源の `banners` を編集する）

### 7-2. お知らせ更新

お知らせ更新は、直接 `index.html` や `__NEXT_DATA__` を編集せず、`maintenance/page-builder.html` の「お知らせのみ追加」から行ってください。

このマニュアルでは、お知らせ配列の手編集手順は扱いません。

### 7-3. メインビジュアルの増減手順（初心者向け）

メインビジュアルは増減できます。
ただし、次の2か所の枚数を必ずそろえてください。

1. `index` 用JSチャンク内のメインビジュアル配列
2. `index.html` 上部の初期表示HTML（画像ブロックとドット）

編集対象:

1. `htmllatest/_next/static/chunks/pages/index-c083d6e47c74352d52ac.js`
2. `htmllatest/index.html`
3. `mashiromix.com-10/_next/static/chunks/pages/index-c083d6e47c74352d52ac.js`
4. `mashiromix.com-10/index.html`

手順:

1. まず `htmllatest` 側を編集する
2. JSチャンクのメインビジュアル配列で、増やすなら1件追加、減らすなら対象件数を削除する
3. `index.html` のメインビジュアル画像ブロック数を、JS配列の件数と同じにする
4. `index.html` のドット数も、画像ブロック数と同じにする
5. 表示確認して問題なければ `mashiromix.com-10` 側へ同じ変更を反映する

増減時の重要ポイント:

1. 画像ブロック数とドット数がずれると見た目や挙動が崩れる
2. JS配列の件数とHTML初期表示件数がずれると、初期描画で違和感が出る
3. 画像を新規追加する場合は、対応する画像ファイルを事前に配置してからパスを設定する
4. 「準備中」枠を試験追加する場合は、既存画像を流用して先に動作確認してよい

最小テンプレート（1件追加する時の考え方）:

1. JS配列に `num` を1つ増やして `illust` と `illustDark` を追加する
2. `index.html` のメインビジュアル画像ブロックを1つコピーして画像パスを変更する
3. `index.html` のドットを1つ追加する

---

## 8. circleページ更新マニュアル

---

## 8.5 WORKS遷移FIX（詳細リンクと自動表示）

このFIXで、トップのWORKS画像をクリックしてモーダル内の「詳細」を押すと、
WORKSページの該当カードへ遷移し、対象画像が自動表示されるようになっています。

### 対象ファイル

1. `htmllatest/_next/static/chunks/pages/index-c083d6e47c74352d52ac.js`
2. `mashiromix.com-10/_next/static/chunks/pages/index-c083d6e47c74352d52ac.js`
3. `htmllatest/works/index.html`
4. `mashiromix.com-10/works/index.html`

### 仕組み

1. トップ側で`workIndex` を持たせる
2. 「詳細」リンクを `/works/#work-x` にする
3. WORKSページ側カードに `id="work-1"` 形式のアンカーを付与
4. 遷移後、該当カードをハイライト
5. 該当カードの画像をモーダルで自動表示

### 増減時の注意

1. WORKSカード件数を増減したら `work-番号` を欠番なく更新
2. トップ側で参照する件数と、WORKSページカード件数を大きくずらさない
3. 画像URLが同一だと、遷移先が違っても同じ画像に見える

---

### 8-1. circleページでよく触る場所

編集対象:

1. `htmllatest/circle/index.html`
2. `mashiromix.com-10/circle/index.html`

よく触る箇所:

1. リンクカード一覧
2. 下部 `__NEXT_DATA__` 内の `banners`
3. `circle` オブジェクトの画像情報

### 8-2. リンクカードの編集方法

circleページのリンクカードは、同じ形の `a` ブロックが並んでいます。

構造:

1. `href` がリンク先
2. `background-image:url(...)` がカード画像
3. `LinkCard__ServiceName...` が表示名

更新手順:

1. 変更したいリンクカード1つを見つける
2. `href` を新しいURLに変更する
3. `background-image:url(...)` を新しい画像に変更する
4. 表示名を変更する

### 8-3. リンクカードの追加方法

一番安全な方法:

1. 既存の `a` ブロックを丸ごとコピーする
2. コピー先を直後に貼る
3. `href` を変更する
4. 画像パスを変更する
5. サービス名を変更する

注意:

1. クラス名はそのままでよい
2. 画像が存在しないと見た目が崩れる
3. 外部リンクなら `target="_blank"` を残す

### 8-4. circleページ更新時の重要注意点

1. 上側の表示HTMLだけでなく、下部の `__NEXT_DATA__` にも画像情報がある
2. 画像を差し替えたら `url` と `thumbUrl` を両方確認する
3. 画像のサムネイルが別管理なら `thumb` フォルダーも忘れない
4. 既存ブロックをコピーする時、閉じタグの位置を崩さない
5. circleの説明文が空欄になっている箇所を触る時は、改行の扱いに注意する

---

## 9. 所属メンバーページ更新マニュアル

### 9-1. 所属メンバーページの仕組み

所属メンバーページは、`CharacterList` という配列でメンバー情報を管理しています。

さらに、現在は次の条件で表示先が分かれています。

1. `group: "talent"` のメンバー → 所属タレント
2. `group: "creator"` のメンバー → 所属クリエイター

つまり、メンバーを追加する時は `status` の有無ではなく `group` の値で表示セクションが決まります。

### 9-2. 編集対象

1. `htmllatest/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`
2. `mashiromix.com-10/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`

### 9-3. 既存メンバーの編集方法

編集できる主な項目:

1. `group`
1. `name.kana`
1. `name.yomi`
1. `illust.url`
1. `illust.width`
1. `illust.height`
1. `icon.url`
1. `logo.url`
1. `social.x`
1. `social.youtube`
1. `social.tiktok`
1. `description`
1. `status`
1. `picture`

`description` の考え方:

1. 自己紹介文は各メンバーのブロック内にある `description` を編集する
2. 所属タレントでも所属クリエイターでも同じ項目を使う
3. まだ文面が未確定の人は一時的に `"準備中"` と入れておける

`group` の考え方:

1. `group: "talent"` なら所属タレント側に表示される
2. `group: "creator"` なら所属クリエイター側に表示される
3. 編集時にその人がどちらの所属か迷ったら、まず `group` を確認する

手順:

1. 対象メンバーのブロックを探す
2. 自己紹介を変えたい時は、その人の `description` だけを直す
3. 変更したい項目だけを直す
4. カンマや波かっこを崩さない
5. 反映後に表示確認する

### 9-3-1. 丸アイコンとイラストの差し替え早見表（どこを修正するか）

編集対象ファイル（必ず両方）:

1. `htmllatest/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`
2. `mashiromix.com-10/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`

修正キー:

1. 丸アイコンを差し替える: 各メンバーの `icon.url`
2. 立ち絵を差し替える: 各メンバーの `illust.url`
3. 立ち絵表示サイズを合わせる: 各メンバーの `illust.width` と `illust.height`

例（1人分）:

```js
{
  id: CharacterId.maika,
  illust: { url: "https://.../illust.png", width: 523, height: 1200 },
  icon: { url: "https://.../icon.png" }
}
```

表示差異の注意（今回のFIX）:

1. 画像URLが同じでも、スタイルで`scale(...)` が当たると見た目サイズが変わる
2. 2番目だけ大きく見える問題は、`CharacterId.koyuki` 専用スタイルが原因だった
3. 現在は専用拡大指定を無効化済み

### 9-4. メンバー追加方法

メンバー追加は必ず2か所です。

#### 手順1: CharacterId に追加

既存のならびに合わせてIDを追加します。

例の考え方:

1. `i.newmember = "newmember";` を追加する

#### 手順2: CharacterList に1人分追加

もっとも安全な方法:

1. 既存メンバー1人分を丸ごとコピー
2. 新しいメンバー情報に置き換える

必要項目:

1. `id`
2. `group`
3. `illust`
4. `icon`
5. `name`
6. `picture`

任意項目:

1. `status`
2. `description`
3. `social`
4. `logo`（名前の上に表示するロゴ画像）

既存メンバーの自己紹介だけを更新する場合は、新規追加ではなくその人の `description` だけを変更します。

### 9-5. タレントとクリエイターの分岐ルール

現在のルール:

1. `group: "talent"` → 所属タレント
2. `group: "creator"` → 所属クリエイター

したがって:

1. タレントとして追加したいなら `group: "talent"` を入れる
2. クリエイターとして追加したいなら `group: "creator"` を入れる
3. `status` は所属タレント・所属クリエイターのどちらにも設定できる

### 9-6. status の書き方

`status` は配列です。

例:

1. 誕生日
2. 血液型
3. 身長
4. 体重
5. 好きなスイーツ

必要に応じて項目数は変えてもよいですが、見た目のバランスは確認してください。

初心者向けテンプレート:

タレント用テンプレート:

```js
group: "talent",
status: [
  { category: "誕生日", value: "1月1日" },
  { category: "血液型", value: "A型" },
  { category: "身長", value: "160cm" }
],
```

クリエイター用テンプレート:

```js
group: "creator",
status: [
  { category: "役割", value: "イラスト・デザイン担当" },
  { category: "特徴", value: "やさしい色使いが得意" }
],
```

編集の見分け方:

1. 所属タレントとして表示したい人は `group: "talent"`
2. 所属クリエイターとして表示したい人は `group: "creator"`
3. `status` はどちらでも使えるが、中身の項目名は役割に合わせて変える
4. タレントはプロフィール項目型、クリエイターは役割紹介型で書くと整理しやすい

そのままコピペして使う追加テンプレート:

```js
{
  id: CharacterId.newmember,
  group: "creator",
  illust: { url: "https://example.com/illust.png", width: 523, height: 1200 },
  icon: { url: "https://example.com/icon.png" },
  logo: { url: "/images/character/AdobeStock_526338514.png" },
  name: { kana: "表示名", yomi: "よみ" },
  social: { x: null, youtube: null, tiktok: null },
  description: "ここに自己紹介を書く",
  status: [
    { category: "役割", value: "ここに担当を書く" },
    { category: "特徴", value: "ここに特徴を書く" }
  ],
  picture: [
    { thumbnail: "https://example.com/thumb-1.jpg", url: "https://example.com/1.jpg" }
  ]
}
```

### 9-7. SNSリンクの書き方

各メンバーに `social` オブジェクトを持たせると、所属メンバーページにSNS欄が表示されます。

項目:

1. `social.x`
2. `social.youtube`
3. `social.tiktok`

例:

1. `social: { x: "https://x.com/example", youtube: "https://www.youtube.com/@example", tiktok: "https://www.tiktok.com/@example" }`

表示ルール:

1. URLが入っている項目は「リンクを開く」と表示される
2. URLが空、または `null` の項目は「準備中」と表示される
3. X、YouTube、TikTokの3項目は常に表示される

### 9-8. picture の書き方

`picture` はギャラリー画像です。

1. `thumbnail`
2. `url`

の2つを1セットで並べます。

注意:

1. サムネイルだけあって元画像がないと開けない
2. 元画像だけあってサムネイルがないと一覧表示で崩れやすい
3. 画像数を増やしすぎる前に表示確認する

### 9-9. 所属メンバーページ更新時の重要注意点

1. 1人追加するたびに `CharacterId` と `CharacterList` の両方を直す
2. `id` の文字列不一致に注意する
3. `group` の値で表示先が変わることを忘れない
4. 画像URLのスペルミスに注意する
5. `description` は改行がそのまま表示されるため、余計な空白を入れすぎない
6. `social` を追加したら `x` `youtube` `tiktok` のキー名を変えない
7. 既存の `componentId`やstyled-componentsの定義は触らない
8. 構文エラーが出たら、まずカンマ抜けを疑う
9. 所属クリエイターにも `status` を設定できるため、分岐判定に `status` を使わない

---

### 9-10. ロゴ画像の設定・差し替え方法（名前上部 アニメーション表示）

#### 仕組みの概要

各メンバーの名前の直上に、所属ロゴ・チャンネルロゴなどの画像を表示できます。
ロゴはキャラ切り替え時に立ち絵と同じアニメーション（スライドイン + フェードイン）で表示されます。

表示コンポーネント: `CharacterLogo`（`character-18bfe4354cceb393588a.js` 内に定義済み）

#### ロゴ画像ファイルの配置場所

現在のファイルパス（両ミラー共通）:

```text
htmllatest/images/character/AdobeStock_526338514.png
mashiromix.com-10/images/character/AdobeStock_526338514.png
```

新しいロゴ画像を使う場合は、**両ミラーの同じパスに同じファイルを置いてください**。

ファイルのコピーコマンド（PowerShell）:

```powershell
Copy-Item -Path "mashiromix.com-10\images\character\新ファイル名.png" `
          -Destination "htmllatest\images\character\新ファイル名.png" -Force
```

#### CharacterList での `logo` キーの書き方

各メンバーオブジェクトに `logo: { url: "..." }` を追加します。

例（1人分）:

```js
{
  id: CharacterId.maika,
  logo: { url: "/images/character/AdobeStock_526338514.png" },
  illust: { url: "...", width: 523, height: 1200 },
  ...
}
```

ルール:

1. `logo.url` にパスを設定すると名前の上にロゴが表示される
2. `logo` キー自体がなければロゴは表示されない（エラーにはならない）
3. `logo.url` が空文字や `null` でもロゴは非表示になる
4. 全メンバーで同じロゴ画像を使う場合は、全員に同じパスを設定する
5. メンバーごとに別ロゴを使いたい場合は、それぞれ異なる `url` を設定する

#### ロゴ画像を差し替える手順

1. 新しいロゴ画像ファイルを用意する
2. `mashiromix.com-10/images/character/` に配置する
3. PowerShell で`htmllatest/images/character/` にもコピーする
4. `CharacterList` 内の各メンバーの `logo.url` を新ファイルのパスに変更する
5. 両ミラーのJSファイルを同じ内容にする
6. 表示確認する

変更対象ファイル（必ず両方）:

1. `htmllatest/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`
2. `mashiromix.com-10/_next/static/chunks/pages/character-18bfe4354cceb393588a.js`

#### アニメーションの仕組み（参考情報）

`CharacterLogo` コンポーネントは `CharacterIllust` と同じ制御パターンで動いています。

1. キャラクターが選択されると `emit = true` になる
2. `slideIn` + `fadeIn` の2種アニメーションが `0.6s` で再生される（イージング: `cubic-bezier(0,0,0.21,1)`）
3. アニメーション終了後に `emit = false` になり静止する
4. 別のキャラクターを選択するとリセットされて再度アニメーションが走る

調整できる値（JSチャンク内 `CharacterLogo` 関数）:

1. アニメーション時間: `0.6s` の数値部分
2. アニメーション遅延: `0s` の部分（例: `0.15s` にすると立ち絵より少し遅れて表示）
3. 表示サイズ: `width: "min(100%,340px)"` と `maxHeight: "120px"`

---

## 10. 画像ファイル管理のコツ

### 10-1. 画像解像度一覧（FIX基準: 2026-03）

この章の数値は、`htmllatest` 側の実ファイルと `__NEXT_DATA__` / `CharacterList` の定義値を基準にしています。
`mashiromix.com-10` 側はミラーのため、同じ解像度で合わせて運用してください。

#### トップページ（index）

1. バナー本画像（`res.banners[*].image`）: `1200 x 580`
2. バナーthumb画像（`images/banner/**/thumb/*`）: 主に `500 x 242`（一部 `500 x 250` が混在）
3. 代替プレースホルダー（`images/banner/1.png` 〜 `6.png`）: `646 x 256`
4. WORKS画像（`res.works[*].image`）: `580 x 280`

#### circleページ（circle/index）

1. circleメイン画像（`res.circle.image`）: `1479 x 1109`
2. circleメインthumb（`images/circle/**/thumb/*`）: `500 x 375`
3. circleページのバナー（`res.banners[*].image`）: `1200 x 580`
4. リンクカード画像（`/images/about/links/*`）: `646 x 256`

#### 所属タレントページ（character）

`CharacterList` の `illust` に定義されている解像度は次の通りです。

1. `maika`: `523 x 1200`
2. `koyuki`: `523 x 1200`
3. `kanon`: `523 x 1200`

補足（同ファイル内の所属クリエイター）:

1. `kurono`: `523 x 1200`
2. `tenshi`: `523 x 1200`

注意:

1. `icon.url` と `picture.url` は、現在のデータ定義に `width` / `height` を持っていません
2. 表示側はアイコンを円形で`100 x 100`（モバイル時 `72 x 72`）に描画します
3. このリポジトリには `images/character` 実ファイルが同梱されていないため、`icon` / `picture` の実寸はURL元で管理されています
4. 立ち絵の見た目サイズは `illust.width` / `illust.height` に加えてCSSの `transform` でも変わるため、個別 `scale` 指定の有無を必ず確認する

### 10-2. 更新時の推奨解像度（崩れにくい基準）

1. トップバナー: `1200 x 580` に統一（比率を揃える）
2. バナーthumb: `500 x 242` を推奨（既存UIとの整合が取りやすい）
3. circleリンクカード: `646 x 256` を維持（差し替え時も同じ比率）
4. 所属タレント `illust`: 高さ `1200px` 基準で作成し、FIX運用では `523 x 1200` を第一推奨にする
5. アイコン画像: 正方形素材（最低 `400 x 400` 推奨）を用意し、円形切り抜きで破綻しない構図にする
6. 立ち絵を横並びで同じ見え方にしたい場合は、`illust` を `523 x 1200` で統一し、個別 `scale` を入れない

### 10-3. 画像運用のコツ

初心者向けのオススメ:

1. 既存フォルダー構成を真似する
2. できるだけ英数字のファイル名を使う
3. サムネイルがある仕組みでは `thumb` 側も用意する
4. 元画像とサムネイルの対応がわかるようにする

画像差し替え時の確認項目:

1. パスが間違っていないか
2. 大文字小文字の違いがないか
3. jpgとpngの拡張子違いがないか
4. サムネイルの参照先も合っているか

---

## 11. 更新後の確認チェックリスト

更新後は必ず確認してください。

1. 画像が表示されているか
2. リンク先が正しいか
3. 新しく追加した項目が崩れていないか
4. `htmllatest` と `mashiromix.com-10` の両方に反映したか
5. タレント追加時、所属タレント側に出ているか
6. クリエイター追加時、所属クリエイター側に出ているか
7. 画像クリックやギャラリー遷移が動くか
8. スマホ幅でも大きく崩れていないか

---

## 12. よくある失敗

### 12-1. 片側のミラーだけ直してしまう

対策:

1. まず `htmllatest`
2. 確認後に `mashiromix.com-10`

### 12-2. バナー画像だけ変えてリンクを変え忘れる

対策:

1. 画像
2. リンク
3. JSONデータ

をセットで確認する

### 12-3. 所属区分を `group` で入れ忘れる

対策:

1. タレントなら `group: "talent"`
2. クリエイターなら `group: "creator"`
3. `status` はどちらにも設定可能

### 12-4. メンバー追加時に CharacterId を増やしていない

対策:

1. 追加は `CharacterId` と `CharacterList` の2か所セット

### 12-5. カンマ抜けでJSが壊れる

対策:

1. 1人分をコピーして中身だけ変える
2. 配列末尾やオブジェクト末尾のカンマ位置を確認する

---

## 13. 初心者向けの安全な更新手順テンプレート

### 13-1. 画像差し替えだけしたい時

1. 使われている画像パスを探す
2. 同名で画像を差し替える
3. 表示確認する
4. 必要ならミラー側にも反映する

### 13-2. リンクだけ変えたい時

1. `href` を探す
2. 新URLへ変更する
3. クリック確認する
4. ミラー側へ反映する

### 13-3. circleのリンクカードを1件追加したい時

1. 既存カードを1個コピー
2. `href` を変更
3. 画像を変更
4. タイトルを変更
5. 表示確認
6. ミラー側へ反映

### 13-4. 所属メンバーを1人追加したい時

1. `CharacterId` にID追加
2. `CharacterList` に1人分追加
3. `group` に `"talent"` か `"creator"` を入れる
4. 必要なら `status` を入れる
5. `social`にX/YouTube/TikTokを入れるか、未設定なら `null` にする
6. 画像パスを設定
7. 表示確認
8. ミラー側へ反映

---

## 14. 迷った時の判断基準

次の判断で進めると失敗しにくいです。

1. 画像だけ変えたい → まず同名差し替えを検討する
2. カードやバナーを増やしたい → 既存ブロックをコピーする
3. メンバーを増やしたい → 既存メンバーをコピーして必要項目だけ変える
4. 表示先が分からない → `group` の値を見る
5. どこを直すか迷う → 先に `htmllatest` を見る

---

## 15. 最終結論

このサイトの保守で一番大事なのは次の4点です。

1. `htmllatest` を先に直す
2. 確認後に `mashiromix.com-10` に反映する
3. トップとcircleはHTML中心、所属メンバーはJS配列中心で管理する
4. 追加作業は必ず既存データをコピーして編集する

初心者のうちは、新規で一から書かず、必ず似た既存データを複製して中だけ変更してください。
それがもっとも安全です。
