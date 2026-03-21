(self.webpackChunk_N_E = self.webpackChunk_N_E || []).push([
  [29],
  {
    748: function (i, a, t) {
      "use strict";
      t.d(a, {
        Z: function () {
          return ImageModal;
        }
      });
      t(7294);
      var styled = t(9163),
        breakpoint = t(9477),
        jsx = t(5893);

      function ImageModal(i) {
        var image = i.image,
          onClose = i.onClose;
        return null === image ? null : (0, jsx.jsxs)(ModalWrapper, {
          children: [(0, jsx.jsx)(BodyLock, {}), (0, jsx.jsx)(ModalBg, {
            onClick: onClose
          }), (0, jsx.jsx)("a", {
            href: image.url,
            target: "_blank",
            children: (0, jsx.jsx)(ModalImage, {
              src: image.url,
              title: image.title
            })
          })]
        });
      }

      var BodyLock = (0, styled.vJ)(["body{height:100% !important;overflow:hidden !important;}" ]),
        ModalWrapper = styled.ZP.div.withConfig({
          displayName: "ImageModal__Wrapper",
          componentId: "sc-hd9av5-0"
        })(["display:flex;justify-content:center;align-items:center;position:fixed;top:0;left:0;width:100%;height:100%;overflow:auto;-webkit-overflow-scrolling:touch;z-index:11;opacity:0;animation:fadeIn 0.4s ease-out 0s forwards;"]),
        ModalBg = styled.ZP.div.withConfig({
          displayName: "ImageModal__Bg",
          componentId: "sc-hd9av5-1"
        })(["position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.5);"]),
        ModalImage = styled.ZP.img.withConfig({
          displayName: "ImageModal__Image",
          componentId: "sc-hd9av5-2"
        })(["position:relative;display:block;width:auto;height:auto;max-width:calc(100vw - 2 * (30px + 16px));max-height:calc(100vh - 2 * 16px);transition:opacity 0.2s ease-out 0s;&:hover{opacity:0.8;}", "{max-width:calc(100vw - 2 * 16px);max-height:calc(100vh - 2 * (30px + 16px));}"], breakpoint.BC.smallDown);
    },
    2268: function (i, a, t) {
      "use strict";
      t.r(a);
      t.d(a, {
        default: function () {
          return CharacterPage;
        }
      });

      var e = t(7294),
        styled = t(9163),
        breakpoint = t(9477),
        seo = t(4559),
        footer = t(8540),
        menu = t(9866),
        jsx = t(5893),
        imageModal = t(748),
        floatingBanner = t(9313),
        divider = t(5866);

      function CharacterIcons(i) {
        var currentId = i.currentId,
          icons = i.icons,
          changeCharater = i.changeCharater;
        return (0, jsx.jsx)(IconsWrapper, {
          children: icons.map((function (i) {
            var icon = i.icon,
              id = i.id;
            return (0, jsx.jsx)(IconButton, {
              current: id === currentId,
              src: icon.url,
              onClick: function () {
                return changeCharater(id);
              }
            }, id);
          }))
        });
      }

      var IconsWrapper = styled.ZP.div.withConfig({
          displayName: "Icons__Wrapper",
          componentId: "sc-1a7dwec-0"
        })(["display:grid;grid-template-columns:repeat(5,auto);grid-gap:24px;justify-content:start;align-items:center;opacity:0;transform:translateY(12px);animation:0.6s cubic-bezier(0,0,0.21,1) 0s forwards;animation-name:slideIn,fadeIn;", "{grid-template-columns:repeat(3,auto);grid-gap:16px;}"], breakpoint.BC.smallDown),
        IconButton = styled.ZP.img.withConfig({
          displayName: "Icons__Icon",
          componentId: "sc-1a7dwec-1"
        })(["--icon-size:100px;display:block;width:var(--icon-size);height:var(--icon-size);background-color:var(--color-white);border-radius:50%;cursor:pointer;", "{--icon-size:72px;}"], breakpoint.BC.smallDown);

      function CharacterName(i) {
        var id = i.id,
          name = i.name,
          kana = name.kana,
          yomi = name.yomi,
          animateState = (0, e.useState)(!0),
          emit = animateState[0],
          setEmit = animateState[1];
        return (0, e.useEffect)((function () {
          setEmit(!0);
        }), [id]), (0, jsx.jsxs)(NameWrapper, {
          emit: emit,
          onAnimationEnd: function () {
            return setEmit(!1);
          },
          children: [(0, jsx.jsx)(NameKana, {
            children: kana
          }), (0, jsx.jsx)(NameYomi, {
            children: yomi
          })]
        });
      }

      var NameWrapper = styled.ZP.div.withConfig({
          displayName: "Name__Wrapper",
          componentId: "sc-10jwxr5-0"
        })(["position:relative;display:grid;grid-column-gap:24px;grid-template-columns:repeat(2,auto);justify-content:start;align-items:end;color:var(--color-black);text-shadow:0 0 2px #fff;", " ", "{grid-column-gap:8px;}"], (function (i) {
          return i.emit && (0, styled.iv)(["opacity:0;transform:translateY(12px);animation:0.6s cubic-bezier(0,0,0.21,1) 0.2s forwards;animation-name:slideIn,fadeIn;"]);
        }), breakpoint.BC.smallDown),
        NameKana = styled.ZP.div.withConfig({
          displayName: "Name__Kana",
          componentId: "sc-10jwxr5-1"
        })(["font-size:55px;font-weight:bold;line-height:71px;", "{font-size:38px;line-height:54px;}"], breakpoint.BC.smallDown),
        NameYomi = styled.ZP.div.withConfig({
          displayName: "Name__Yomi",
          componentId: "sc-10jwxr5-2"
        })(["font-size:30px;line-height:38px;", "{font-size:14px;line-height:28px;}"], breakpoint.BC.smallDown);

      function CharacterStatus(i) {
        var id = i.id,
          status = i.status,
          animateState = (0, e.useState)(!0),
          emit = animateState[0],
          setEmit = animateState[1];
        return (0, e.useEffect)((function () {
          setEmit(!0);
        }), [id]), (0, jsx.jsx)(StatusWrapper, {
          emit: emit,
          onAnimationEnd: function () {
            return setEmit(!1);
          },
          children: status.map((function (i) {
            var category = i.category,
              value = i.value;
            return (0, jsx.jsx)(StatusItem, {
              category: category,
              value: value
            }, value);
          }))
        });
      }

      function StatusItem(i) {
        var category = i.category,
          value = i.value;
        return (0, jsx.jsxs)(StatusItemWrapper, {
          children: [(0, jsx.jsx)(StatusCategory, {
            children: category
          }), (0, jsx.jsx)(StatusValue, {
            children: value
          })]
        });
      }

      var StatusWrapper = styled.ZP.div.withConfig({
          displayName: "Status__Wrapper",
          componentId: "sc-n4x7d-0"
        })(["", ""], (function (i) {
          return i.emit && (0, styled.iv)(["opacity:0;transform:translateY(12px);animation:0.6s cubic-bezier(0,0,0.21,1) 0.3s forwards;animation-name:slideIn,fadeIn;"]);
        })),
        StatusItemWrapper = styled.ZP.div.withConfig({
          displayName: "Status__ItemWrapper",
          componentId: "sc-n4x7d-1"
        })(["display:inline-grid;grid-column-gap:8px;grid-template-columns:repeat(2,auto);align-items:start;justify-content:start;font-size:18px;line-height:30px;margin:0 16px 16px 0;", "{font-size:14px;line-height:24px;}"], breakpoint.BC.smallDown),
        StatusCategory = styled.ZP.div.withConfig({
          displayName: "Status__Category",
          componentId: "sc-n4x7d-2"
        })(["color:var(--color-white);background:var(--color-pink);padding:0 16px;border-radius:30px;", "{padding:0 10px;}"], breakpoint.BC.smallDown),
        StatusValue = styled.ZP.div.withConfig({
          displayName: "Status__Value",
          componentId: "sc-n4x7d-3"
        })(["color:var(--color-black);"]);

      function CharacterSocialLinks(i) {
        var id = i.id,
          social = i.social || {},
          animateState = (0, e.useState)(!0),
          emit = animateState[0],
          setEmit = animateState[1],
          items = [{ label: "X", href: social.x }, { label: "YouTube", href: social.youtube }, { label: "TikTok", href: social.tiktok }];
        return (0, e.useEffect)((function () {
          setEmit(!0);
        }), [id]), (0, jsx.jsxs)("section", {
          style: { display: "grid", rowGap: "12px", width: "100%", padding: "24px", background: "rgba(255,255,255,0.72)", border: "1px solid rgba(0,0,0,0.08)", borderRadius: "24px", boxSizing: "border-box" },
          children: [(0, jsx.jsx)("div", { style: { fontSize: "14px", fontWeight: "700", letterSpacing: "0.08em", color: "var(--color-black)" }, children: "SNS" }), (0, jsx.jsx)(SocialWrapper, {
            emit: emit,
            onAnimationEnd: function () {
              return setEmit(!1);
            },
            children: items.map((function (i) {
              var label = i.label,
                href = i.href;
              return (0, jsx.jsxs)(SocialItem, {
                children: [(0, jsx.jsx)(SocialLabel, {
                  children: label
                }), href ? (0, jsx.jsx)(SocialLink, {
                  href: href,
                  target: "_blank",
                  rel: "noopener noreferrer",
                  children: "リンクを開く"
                }) : (0, jsx.jsx)(SocialPending, {
                  children: "準備中"
                })]
              }, label);
            }))
          })]
        });
      }

      var SocialWrapper = styled.ZP.div.withConfig({
          displayName: "Social__Wrapper",
          componentId: "sc-1kv8q1d-0"
        })(["display:grid;grid-row-gap:12px;", ""], (function (i) {
          return i.emit && (0, styled.iv)(["opacity:0;transform:translateY(12px);animation:0.6s cubic-bezier(0,0,0.21,1) 0.3s forwards;animation-name:slideIn,fadeIn;"]);
        })),
        SocialItem = styled.ZP.div.withConfig({
          displayName: "Social__Item",
          componentId: "sc-1kv8q1d-1"
        })(["display:grid;grid-template-columns:110px minmax(0,1fr);grid-column-gap:12px;align-items:start;", "{grid-template-columns:88px minmax(0,1fr);grid-column-gap:8px;}"], breakpoint.BC.smallDown),
        SocialLabel = styled.ZP.div.withConfig({
          displayName: "Social__Label",
          componentId: "sc-1kv8q1d-2"
        })(["color:var(--color-white);background:var(--color-pink);padding:0 16px;border-radius:30px;font-size:16px;line-height:30px;text-align:center;", "{padding:0 10px;font-size:13px;line-height:26px;}"], breakpoint.BC.smallDown),
        SocialLink = styled.ZP.a.withConfig({
          displayName: "Social__Link",
          componentId: "sc-1kv8q1d-3"
        })(["font-size:16px;line-height:30px;color:var(--color-black);text-decoration:underline;overflow-wrap:anywhere;&:hover{opacity:0.7;}", "{font-size:14px;line-height:24px;}"], breakpoint.BC.smallDown),
        SocialPending = styled.ZP.div.withConfig({
          displayName: "Social__Pending",
          componentId: "sc-1kv8q1d-4"
        })(["font-size:16px;line-height:30px;color:rgba(0,0,0,0.55);", "{font-size:14px;line-height:24px;}"], breakpoint.BC.smallDown);

      function CharacterPictures(i) {
        var picture = i.picture,
          imageState = (0, e.useState)(null),
          image = imageState[0],
          setImage = imageState[1];
        return (0, jsx.jsxs)(jsx.Fragment, {
          children: [(0, jsx.jsx)(PictureWrapper, {
            children: picture.map((function (i) {
              var thumbnail = i.thumbnail,
                url = i.url;
              return (0, jsx.jsx)(PictureItemWrapperWrapper, {
                onClick: function () {
                  return setImage({
                    url: url,
                    thumbUrl: thumbnail,
                    _originalUrl: url,
                    width: 0,
                    height: 0,
                    title: ""
                  });
                },
                children: (0, jsx.jsx)(PictureItemWrapper, {
                  src: thumbnail
                })
              }, url);
            }))
          }), (0, jsx.jsx)(imageModal.Z, {
            image: image,
            onClose: function () {
              setImage(null);
            }
          })]
        });
      }

      var CharacterId;

      var PictureWrapper = styled.ZP.div.withConfig({
          displayName: "Picture__Wrapper",
          componentId: "sc-fm6f50-0"
        })(["display:grid;grid-gap:16px;grid-template-columns:repeat(2,1fr);", "{width:400px;}"], breakpoint.BC.mediumUp),
        PictureItemWrapperWrapper = styled.ZP.div.withConfig({
          displayName: "Picture__ItemWrapperWrapper",
          componentId: "sc-fm6f50-1"
        })(["opacity:0;transform:translateY(12px);animation:0.6s cubic-bezier(0,0,0.21,1) forwards;animation-name:slideIn,fadeIn;&:nth-child(1){animation-delay:0.4s;}&:nth-child(2){animation-delay:0.45s;}&:nth-child(3){animation-delay:0.5s;}&:nth-child(4){animation-delay:0.55s;}"]),
        PictureItemWrapper = styled.ZP.div.withConfig({
          displayName: "Picture__ItemWrapper",
          componentId: "sc-fm6f50-2"
        })(["position:relative;border-radius:4px;background:var(--color-white) center / contain no-repeat;overflow:hidden;transition:opacity 0.2s ease-out 0s;cursor:pointer;&:hover{opacity:0.8;}", " &::before{content:'';display:block;padding-top:100%;}"], (function (i) {
          var src = i.src;
          return (0, styled.iv)(["background-image:url('", "');"], src);
        }));

      function CharacterIllust(i) {
        var id = i.id,
          url = i.url,
          animateState = (0, e.useState)(!0),
          emit = animateState[0],
          setEmit = animateState[1];
        return (0, e.useEffect)((function () {
          setEmit(!0);
        }), [id]), (0, jsx.jsx)(IllustWrapper, {
          characterId: id,
          emit: emit,
          onAnimationEnd: function () {
            return setEmit(!1);
          },
          children: (0, jsx.jsx)(IllustImage, {
            src: url,
            characterId: id
          })
        });
      }

      !function (i) {
        i.maika = "maika";
        i.koyuki = "koyuki";
        i.kanon = "kanon";
        i.kurono = "kurono";
        i.tenshi = "tenshi";
      }(CharacterId || (CharacterId = {}));

      var IllustWrapper = styled.ZP.div.withConfig({
          displayName: "Illust__Wrapper",
          componentId: "sc-1poftpg-0"
        })(["position:relative;width:420px;", " ", "{width:280px;", "}&::before{--width:590;--height:1200;content:'';display:block;padding-top:calc(var(--height) / var(--width) * 100%);}"], (function (i) {
          return i.emit && (0, styled.iv)(["opacity:0;transform:translateY(18px);animation:0.6s cubic-bezier(0,0,0.21,1) 0s forwards;animation-name:slideIn,fadeIn;"]);
        }), breakpoint.BC.smallDown, (function (i) {
          return i.characterId === CharacterId.koyuki && (0, styled.iv)(["margin-bottom:60px;"]);
        })),
        IllustImage = styled.ZP.img.withConfig({
          displayName: "Illust__Image",
          componentId: "sc-1poftpg-1"
        })(["display:block;position:absolute;top:0;left:50%;height:100%;transform-origin:top;transform:translateX(-50%);", ""], (function (i) {
          return i.characterId === CharacterId.koyuki && (0, styled.iv)(["transform:translateX(-50%) scale(1.15);"]);
        }));

      var CharacterList = [{
        id: CharacterId.maika,
        group: "talent",
        illust: { url: "https://karory.net/images/character/maika/illust.png", width: 590, height: 1200 },
        icon: { url: "https://karory.net/images/character/maika/icon.png" },
        name: { kana: "蒼海 舞香", yomi: "あおみ まいか" },
        social: { x: null, youtube: null, tiktok: null },
        description: "準備中",
        status: [{ category: "誕生日", value: "12月4日（いて座）" }, { category: "血液型", value: "O型" }, { category: "身長", value: "154cm" }, { category: "体重", value: "ヒミツ！" }, { category: "好きなスイーツ", value: "いちごタルト・マカロン" }],
        picture: [{ thumbnail: "https://karory.net/images/character/maika/picture/thumbnail/1.jpg", url: "https://karory.net/images/character/maika/picture/1.jpg" }, { thumbnail: "https://karory.net/images/character/maika/picture/thumbnail/2.jpg", url: "https://karory.net/images/character/maika/picture/2.jpg" }, { thumbnail: "https://karory.net/images/character/maika/picture/thumbnail/3.jpeg", url: "https://karory.net/images/character/maika/picture/3.png" }, { thumbnail: "https://karory.net/images/character/maika/picture/thumbnail/4.png", url: "https://karory.net/images/character/maika/picture/4.png" }]
      }, {
        id: CharacterId.koyuki,
        group: "talent",
        illust: { url: "https://karory.net/images/character/koyuki/illust.png", width: 443, height: 1200 },
        icon: { url: "https://karory.net/images/character/koyuki/icon.png" },
        name: { kana: "月石 来雪", yomi: "つきいし こゆき" },
        social: { x: null, youtube: null, tiktok: null },
        description: "準備中",
        status: [{ category: "誕生日", value: "4月17日（おひつじ座）" }, { category: "血液型", value: "AB型" }, { category: "身長", value: "152cm" }, { category: "体重", value: "聞かないでください！" }, { category: "好きなスイーツ", value: "抹茶金時マカロン・チョコミント" }],
        picture: [{ thumbnail: "https://karory.net/images/character/koyuki/picture/thumbnail/1.jpg", url: "https://karory.net/images/character/koyuki/picture/1.jpg" }, { thumbnail: "https://karory.net/images/character/koyuki/picture/thumbnail/2.jpg", url: "https://karory.net/images/character/koyuki/picture/2.jpg" }, { thumbnail: "https://karory.net/images/character/koyuki/picture/thumbnail/3.jpg", url: "https://karory.net/images/character/koyuki/picture/3.jpg" }, { thumbnail: "https://karory.net/images/character/koyuki/picture/thumbnail/4.png", url: "https://karory.net/images/character/koyuki/picture/4.png" }]
      }, {
        id: CharacterId.kanon,
        group: "talent",
        illust: { url: "https://karory.net/images/character/kanon/illust.png", width: 523, height: 1200 },
        icon: { url: "https://karory.net/images/character/kanon/icon.png" },
        name: { kana: "蒼海 夏音", yomi: "あおみ かのん" },
        social: { x: null, youtube: null, tiktok: null },
        description: "準備中",
        status: [{ category: "誕生日", value: "7月20日" }, { category: "血液型", value: "Ａ型" }, { category: "身長", value: "154cm" }, { category: "体重", value: "ナイショだよ！" }, { category: "好きなスイーツ", value: "オレンジショートケーキ・ふっわふわなスフレチーズケーキ" }],
        picture: [{ thumbnail: "https://karory.net/images/character/kanon/picture/thumbnail/1.png", url: "https://karory.net/images/character/kanon/picture/1.png" }, { thumbnail: "https://karory.net/images/character/kanon/picture/thumbnail/2.jpg", url: "https://karory.net/images/character/kanon/picture/2.jpg" }, { thumbnail: "https://karory.net/images/character/kanon/picture/thumbnail/3.jpg", url: "https://karory.net/images/character/kanon/picture/3.jpg" }, { thumbnail: "https://karory.net/images/character/kanon/picture/thumbnail/4.png", url: "https://karory.net/images/character/kanon/picture/4.png" }]
      }, {
        id: CharacterId.kurono,
        group: "creator",
        illust: { url: "https://karory.net/images/character/kurono/illust.png", width: 523, height: 1200 },
        icon: { url: "https://karory.net/images/character/kurono/icon.png" },
        name: { kana: "クロノ", yomi: "くろの" },
        social: { x: null, youtube: null, tiktok: null },
        description: "時空に留まる謎の多い少女・・・\n    最近ネコミミメイド服が密かにお気に入り",
        status: [{ category: "役割", value: "時空を見守るミステリアスクリエイター" }, { category: "特徴", value: "ネコミミメイド服がお気に入り" }],
        picture: [{ thumbnail: "https://karory.net/images/character/kurono/picture/thumbnail/1.jpg", url: "https://karory.net/images/character/kurono/picture/1.jpg" }, { thumbnail: "https://karory.net/images/character/kurono/picture/thumbnail/2.jpg", url: "https://karory.net/images/character/kurono/picture/2.jpg" }, { thumbnail: "https://karory.net/images/character/kurono/picture/thumbnail/3.png", url: "https://karory.net/images/character/kurono/picture/3.png" }]
      }, {
        id: CharacterId.tenshi,
        group: "creator",
        illust: { url: "https://karory.net/images/character/tenshi/illust.png", width: 523, height: 1200 },
        icon: { url: "https://karory.net/images/character/tenshi/icon.png" },
        name: { kana: "天使ちゃん", yomi: "てんしちゃん" },
        social: { x: null, youtube: null, tiktok: null },
        description: "天界からやってきた女神の使い\n    家事が得意でとっても甘やかしてくれる",
        status: [{ category: "役割", value: "天界から来た女神の使い" }, { category: "特徴", value: "家事が得意で甘やかし上手" }],
        picture: [{ thumbnail: "https://karory.net/images/character/tenshi/picture/thumbnail/1.jpg", url: "https://karory.net/images/character/tenshi/picture/1.jpg" }, { thumbnail: "https://karory.net/images/character/tenshi/picture/thumbnail/2.jpg", url: "https://karory.net/images/character/tenshi/picture/2.jpg" }]
      }];

      function CharacterDescription(i) {
        var id = i.id,
          description = i.description || "準備中",
          animateState = (0, e.useState)(!0),
          emit = animateState[0],
          setEmit = animateState[1];
        return (0, e.useEffect)((function () { setEmit(!0); }), [id]), (0, jsx.jsxs)("section", {
          style: { display: "grid", rowGap: "12px", width: "100%", padding: "24px", background: "rgba(255,255,255,0.72)", border: "1px solid rgba(0,0,0,0.08)", borderRadius: "24px", boxSizing: "border-box" },
          children: [(0, jsx.jsx)("div", { style: { fontSize: "14px", fontWeight: "700", letterSpacing: "0.08em", color: "var(--color-black)" }, children: "自己紹介" }), (0, jsx.jsx)(DescriptionWrapper, { emit: emit, onAnimationEnd: function () { return setEmit(!1); }, children: description })]
        });
      }

      var DescriptionWrapper = styled.ZP.div.withConfig({ displayName: "Description__Wrapper", componentId: "sc-1p9v8rb-0" })(["font-size:18px;line-height:32px;color:var(--color-black);white-space:pre-line;", "{font-size:16px;line-height:28px;}", ""], breakpoint.BC.smallDown, (function (i) { return i.emit && (0, styled.iv)(["opacity:0;transform:translateY(12px);animation:0.6s cubic-bezier(0,0,0.21,1) 0.3s forwards;animation-name:slideIn,fadeIn;"]); }));

      function CharacterGroupSection(i) {
        var title = i.title,
          description = i.description,
          items = i.items,
          currentId = i.currentId,
          changeCharater = i.changeCharater,
          icons = items.map((function (i) { return { id: i.id, icon: i.icon }; })),
          current = items.find((function (i) { return i.id === currentId; }));
        return current ? (0, jsx.jsxs)("section", {
          style: { display: "grid", rowGap: "28px", width: "100%", maxWidth: "1120px", padding: "clamp(20px,4vw,32px)", background: "rgba(255,255,255,0.84)", border: "1px solid rgba(0,0,0,0.08)", borderRadius: "32px", boxSizing: "border-box", boxShadow: "0 18px 48px rgba(0,0,0,0.06)" },
          children: [(0, jsx.jsxs)("div", { style: { display: "grid", rowGap: "8px" }, children: [(0, jsx.jsx)("div", { style: { fontSize: "28px", fontWeight: "700", lineHeight: "1.4", color: "var(--color-black)" }, children: title }), description && (0, jsx.jsx)("div", { style: { fontSize: "14px", lineHeight: "1.9", color: "rgba(0,0,0,0.68)" }, children: description })] }), (0, jsx.jsx)(CharacterIcons, { currentId: currentId, icons: icons, changeCharater: changeCharater }), (0, jsx.jsxs)(CharacterInner, { children: [(0, jsx.jsx)(CharacterIllust, { id: current.id, url: current.illust.url }), (0, jsx.jsxs)(CharacterInfo, { children: [(0, jsx.jsx)(CharacterName, { id: current.id, name: current.name }), current.status && current.status.length > 0 && (0, jsx.jsx)(CharacterStatus, { id: current.id, status: current.status }), (0, jsx.jsx)(CharacterDescription, { id: current.id, description: current.description }), (0, jsx.jsx)(CharacterSocialLinks, { id: current.id, social: current.social }), (0, jsx.jsx)(CharacterPictures, { picture: current.picture })] })] })]
        }) : null;
      }

      function CharacterContent() {
        var talentItems = CharacterList.filter((function (i) { return "talent" === i.group; })),
          creatorItems = CharacterList.filter((function (i) { return "creator" === i.group; })),
          talentState = (0, e.useState)(talentItems[0].id),
          currentTalentId = talentState[0],
          setCurrentTalentId = talentState[1],
          creatorState = (0, e.useState)(creatorItems[0].id),
          currentCreatorId = creatorState[0],
          setCurrentCreatorId = creatorState[1];

        return (0, jsx.jsxs)(CharacterWrapper, {
          children: [(0, jsx.jsxs)("div", { style: { display: "grid", rowGap: "10px", justifyItems: "center", textAlign: "center" }, children: [(0, jsx.jsx)("div", { style: { fontSize: "22px", fontWeight: "700", lineHeight: "1.8", color: "var(--color-black)" }, children: "所属メンバー一覧" }), (0, jsx.jsx)("div", { style: { fontSize: "14px", lineHeight: "1.9", color: "rgba(0,0,0,0.7)" }, children: "所属タレントと所属クリエイターを上下に分けてご覧いただけます。" })] }), (0, jsx.jsx)(CharacterGroupSection, { title: "所属タレント", description: "配信・出演を中心に活動するメンバーです。", items: talentItems, currentId: currentTalentId, changeCharater: setCurrentTalentId }), (0, jsx.jsx)(CharacterGroupSection, { title: "所属クリエイター", description: "制作・サポートを中心に活動するメンバーです。", items: creatorItems, currentId: currentCreatorId, changeCharater: setCurrentCreatorId })]
        });
      }

      var CharacterWrapper = styled.ZP.div.withConfig({ displayName: "Character__Wrapper", componentId: "sc-9iwzmg-0" })(["display:grid;grid-row-gap:48px;justify-items:center;", "{grid-row-gap:32px;}"], breakpoint.BC.smallDown),
        CharacterInner = styled.ZP.div.withConfig({ displayName: "Character__Inner", componentId: "sc-9iwzmg-1" })(["display:grid;", "{grid-column-gap:48px;grid-template-columns:repeat(2,auto);align-items:start;}", "{grid-row-gap:32px;justify-items:center;}"], breakpoint.BC.mediumUp, breakpoint.BC.smallDown),
        CharacterInfo = styled.ZP.div.withConfig({ displayName: "Character__Info", componentId: "sc-9iwzmg-2" })(["display:grid;grid-row-gap:40px;max-width:480px;"]);

      function CharacterPage() {
        return (0, jsx.jsxs)(jsx.Fragment, {
          children: [(0, jsx.jsx)(seo.Z, { title: "CHARACTER | mashiromix.com｜Comic&illustration web site by mashiro" }), (0, jsx.jsxs)(PageWrapper, { children: [(0, jsx.jsxs)(PageInner, { children: [(0, jsx.jsx)(menu.bK, {}), (0, jsx.jsx)(CharacterContent, {})] }), (0, jsx.jsx)(divider.Z, { reverse: !0 }), (0, jsx.jsx)(footer.Z, {})] }), (0, jsx.jsx)(floatingBanner.dw, {})]
        });
      }

      var PageWrapper = styled.ZP.div.withConfig({ displayName: "character__Wrapper", componentId: "sc-jb7pnf-0" })(["background:repeating-linear-gradient( -45deg,#eee,#eee 1px,transparent 0,transparent 14px ) top left;overflow:hidden;"]),
        PageInner = styled.ZP.div.withConfig({ displayName: "character__Inner", componentId: "sc-jb7pnf-1" })(["display:grid;grid-row-gap:80px;justify-items:center;padding:160px 0;", "{grid-row-gap:40px;padding:80px 16px;}"], breakpoint.BC.smallDown);
    },
    4712: function (i, a, t) {
      (window.__NEXT_P = window.__NEXT_P || []).push(["/character", function () {
        return t(2268);
      }]);
    }
  },
  function (i) {
    i.O(0, [493, 856, 774, 888, 179], (function () {
      return a = 4712, i(i.s = a);
      var a;
    }));
    var a = i.O();
    _N_E = a;
  }
]);