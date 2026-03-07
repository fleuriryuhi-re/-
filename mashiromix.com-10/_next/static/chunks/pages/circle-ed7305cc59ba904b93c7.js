(self.webpackChunk_N_E = self.webpackChunk_N_E || []).push([
    [701], {
      748: function (i, e, t) {
        "use strict";
        t.d(e, {
          Z: function () {
            return o
          }
        });
        t(7294);
        var a = t(9163),
          n = t(9477),
          r = t(5893);
  
        function o(i) {
          var e = i.image,
            t = i.onClose;
          return null === e ? null : (0, r.jsxs)(s, {
            children: [(0, r.jsx)(c, {}), (0, r.jsx)(l, {
              onClick: t
            }), (0, r.jsx)("a", {
              href: e.url,
              target: "_blank",
              children: (0, r.jsx)(d, {
                src: e.url,
                title: e.title
              })
            })]
          })
        }
        var c = (0, a.vJ)(["body{height:100% !important;overflow:hidden !important;}"]),
          s = a.ZP.div.withConfig({
            displayName: "ImageModal__Wrapper",
            componentId: "sc-hd9av5-0"
          })(["display:flex;justify-content:center;align-items:center;position:fixed;top:0;left:0;width:100%;height:100%;overflow:auto;-webkit-overflow-scrolling:touch;z-index:11;opacity:0;animation:fadeIn 0.4s ease-out 0s forwards;"]),
          l = a.ZP.div.withConfig({
            displayName: "ImageModal__Bg",
            componentId: "sc-hd9av5-1"
          })(["position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.5);"]),
          d = a.ZP.img.withConfig({
            displayName: "ImageModal__Image",
            componentId: "sc-hd9av5-2"
          })(["position:relative;display:block;width:auto;height:auto;max-width:calc(100vw - 2 * (30px + 16px));max-height:calc(100vh - 2 * 16px);transition:opacity 0.2s ease-out 0s;&:hover{opacity:0.8;}", "{max-width:calc(100vw - 2 * 16px);max-height:calc(100vh - 2 * (30px + 16px));}"], n.BC.smallDown)
      },
      8436: function (i, e, t) {
        "use strict";
        t.d(e, {
          Z: function () {
            return s
          }
        });
        var a = t(8152),
          n = t(7294),
          r = t(9163),
          o = t(131),
          c = t(5893);
  
        function s(i) {
          var e = i.className,
            t = i.originalSize,
            r = i.image,
            s = r.width,
            d = r.height,
            p = r.thumbUrl,
            g = r.url,
            m = r.title,
            h = (0, n.useState)(!1),
            u = h[0],
            x = h[1],
            f = (0, o.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            w = (0, a.Z)(f, 2),
            v = w[0],
            b = w[1],
            k = (0, n.useCallback)((function () {
              x(!0)
            }), []);
          return (0, c.jsx)(l, {
            ref: v,
            aspectRatio: d / s * 100,
            className: e,
            isIntersected: u,
            children: b && (0, c.jsx)("img", {
              src: t ? g : p,
              title: m,
              onLoad: k
            })
          })
        }
        var l = r.ZP.div.withConfig({
          displayName: "LazyImage__Wrapper",
          componentId: "sc-xnobnf-0"
        })(["position:relative;border-radius:6px;background:rgba(0,0,0,0.05);overflow:hidden;cursor:pointer;transition:opacity 0.2s ease-out 0s;&:hover{opacity:0.8;}&::before{content:'';display:block;", "}& > img{display:block;position:absolute;top:0;left:0;width:100%;height:100%;opacity:0;transition:opacity 0.2s ease-out 0s;", "}"], (function (i) {
          var e = i.aspectRatio;
          return (0, r.iv)(["padding-top:", "%;"], e)
        }), (function (i) {
          return i.isIntersected && (0, r.iv)(["opacity:1;"])
        }))
      },
      8211: function (i, e, t) {
        "use strict";
        t.d(e, {
          Z: function () {
            return s
          }
        });
        var a = t(8152),
          n = (t(7294), t(9163)),
          r = t(9477),
          o = t(131),
          c = t(5893);
  
        function s(i) {
          var e = i.serviceName,
            t = i.url,
            n = i.imageUrl,
            r = (0, o.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            s = (0, a.Z)(r, 2),
            h = s[0],
            u = s[1];
          return (0, c.jsxs)(l, {
            href: t,
            target: "_blank",
            ref: h,
            isIntersected: u,
            children: [(0, c.jsx)(d, {
              style: {
                backgroundImage: "url(".concat(n, ")")
              }
            }), (0, c.jsxs)(p, {
              children: [(0, c.jsx)(g, {
                children: e
              }), (0, c.jsx)(m, {})]
            })]
          })
        }
        var l = n.ZP.a.withConfig({
            displayName: "LinkCard__Wrapper",
            componentId: "sc-1p9qe2f-0"
          })(["box-sizing:border-box;display:grid;grid-row-gap:4px;padding:8px;border:solid 1px var(--color-black);border-radius:12px;background-color:var(--color-white);transition:background-color 0.2s ease-out 0s;opacity:0;transform:translateY(12px);", " &:hover{background-color:rgba(0,0,0,0.04);}"], (function (i) {
            return i.isIntersected && (0, n.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          })),
          d = n.ZP.div.withConfig({
            displayName: "LinkCard__Thumb",
            componentId: "sc-1p9qe2f-1"
          })(["position:relative;width:100%;border-radius:8px;background:center / cover no-repeat;overflow:hidden;&::before{--width:430;--height:170;content:'';display:block;padding-top:calc(var(--height) / var(--width) * 100%);}"]),
          p = n.ZP.div.withConfig({
            displayName: "LinkCard__ServiceNameWrapper",
            componentId: "sc-1p9qe2f-2"
          })(["display:grid;grid-column-gap:10px;grid-template-columns:repeat(2,auto);justify-content:end;align-items:center;margin-right:8px;"]),
          g = n.ZP.div.withConfig({
            displayName: "LinkCard__ServiceName",
            componentId: "sc-1p9qe2f-3"
          })(["font-size:18px;font-weight:bold;line-height:26px;color:var(--color-black);transform:translateY(-2%);", "{font-size:16px;line-height:24px;}"], r.BC.smallDown),
          m = n.ZP.div.withConfig({
            displayName: "LinkCard__ArrowIcon",
            componentId: "sc-1p9qe2f-4"
          })(["width:6.5px;background:center / contain no-repeat;background-image:url('data:image/svg+xml;charset=utf8,%3Csvg%20width%3D%227%22%20height%3D%2213%22%20viewBox%3D%220%200%207%2013%22%20fill%3D%22none%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%0A%3Cpath%20d%3D%22M1.38348%2012.3474C0.974156%2012.3474%200.633054%2012.2%200.360173%2011.8316C-0.117369%2011.2421%200.0190714%2010.2842%200.564834%209.76842L3.08898%207.33684C3.43009%207.0421%203.43009%206.45263%203.08898%206.15789L0.496613%203.57894C-0.0491489%203.06315%20-0.185589%202.17894%200.291953%201.58947C0.769495%200.852626%201.72458%200.778942%202.27034%201.36842L5.74958%204.75789C6.22712%205.27368%206.5%205.93684%206.5%206.67368C6.5%207.41052%206.22712%208.07368%205.68136%208.58947L2.27034%2011.9053C1.99746%2012.2%201.65636%2012.3474%201.38348%2012.3474Z%22%20fill%3D%22%23111111%22%2F%3E%0A%3C%2Fsvg%3E%0A');", "{width:5px;}&::before{--width:6.5;--height:11.37;content:'';display:block;padding-top:calc(var(--height) / var(--width) * 100%);}"], r.BC.smallDown)
      },
      4547: function (i, e, t) {
        "use strict";
        t.r(e), t.d(e, {
          __N_SSG: function () {
            return x
          },
          default: function () {
            return f
          }
        });
        var a = t(7294),
          n = t(9163),
          r = t(9477),
          o = t(4559),
          c = t(8540),
          s = t(9866),
          l = t(8436),
          d = t(9313),
          p = t(8211),
          g = t(748),
          m = t(5866),
          h = t(5893),
          u = [{
            serviceName: "株式会社NAIBUS",
            url: "https://naibus-nft.com",
            imageUrl: "/images/about/links/melon.jpg"
          }, {
            serviceName: "TSUKUMO",
            url: "https://shop.tsukumo.co.jp/pc?gad=1&gclid=CjwKCAjwvdajBhBEEiwAeMh1UxPvSgDo811wmQCwoJL1jFeJ2b5AdzyP456KszjV-Qr-s2QuMaaFjRoCNaQQAvD_BwE",
            imageUrl: "/images/about/links/tora.jpg"
          }, {
            serviceName: "Live2D株式会社",
            url: "https://www.live2d.com",
            imageUrl: "/images/about/links/fanza.jpg"
          }, {
            serviceName: "nizima",
            url: "https://nizima.com",
            imageUrl: "/images/about/links/dlsite.jpg"
          }],
          x = !0;
  
        function f(i) {
          var e = i.res.circle,
            t = (0, a.useState)(null),
            n = t[0],
            r = t[1];
          return (0, h.jsxs)(h.Fragment, {
            children: [(0, h.jsx)(o.Z, {
              title: "CIRCLE mashiromix.com｜web site by 水咲ましろ"
            }), (0, h.jsxs)(w, {
              children: [(0, h.jsxs)(v, {
                children: [(0, h.jsx)(s.aR, {}), (0, h.jsx)(b, {
                  children: e.description
                }), (0, h.jsx)(k, {
                  onClick: function () {
                    return function (i) {
                      r(i)
                    }(e.image)
                  },
                  children: (0, h.jsx)(l.Z, {
                    originalSize: !0,
                    image: e.image
                  })
                }), (0, h.jsx)(_, {
                  children: u.map((function (i) {
                    var e = i.serviceName,
                      t = i.url,
                      a = i.imageUrl;
                    return (0, h.jsx)(p.Z, {
                      serviceName: e,
                      url: t,
                      imageUrl: a
                    }, e)
                  }))
                })]
              }), (0, h.jsx)(m.Z, {
                reverse: !0
              }), (0, h.jsx)(c.Z, {})]
            }), (0, h.jsx)(g.Z, {
              image: n,
              onClose: function () {
                r(null)
              }
            }), (0, h.jsx)(d.dw, {})]
          })
        }
        var w = n.ZP.div.withConfig({
            displayName: "circle__Wrapper",
            componentId: "sc-n5b2kj-0"
          })(["background:repeating-linear-gradient( -45deg,#eee,#eee 1px,transparent 0,transparent 14px );overflow:hidden;"]),
          v = n.ZP.div.withConfig({
            displayName: "circle__Inner",
            componentId: "sc-n5b2kj-1"
          })(["display:grid;grid-row-gap:80px;justify-items:center;padding:160px 0;", "{grid-row-gap:40px;padding:80px 0;}"], r.BC.smallDown),
          b = n.ZP.div.withConfig({
            displayName: "circle__Description",
            componentId: "sc-n5b2kj-2"
          })(["font-size:20px;line-height:2.2;color:var(--color-black);text-align:center;white-space:pre-line;word-break:break-all;max-width:820px;width:calc(100% - 32px);", "{font-size:16px;}"], r.BC.smallDown),
          k = n.ZP.div.withConfig({
            displayName: "circle__LazyImageWrapper",
            componentId: "sc-n5b2kj-3"
          })(["max-width:820px;width:calc(100% - 32px);"]),
          _ = n.ZP.div.withConfig({
            displayName: "circle__Links",
            componentId: "sc-n5b2kj-4"
          })(["display:grid;", "{grid-gap:24px;grid-template-columns:repeat(2,1fr);max-width:820px;width:calc(100% - 48px);}", "{grid-gap:16px;max-width:320px;width:calc(100% - 32px);}"], r.BC.mediumUp, r.BC.smallDown)
      },
      1829: function (i, e, t) {
        (window.__NEXT_P = window.__NEXT_P || []).push(["/circle", function () {
          return t(4547)
        }])
      }
    },
    function (i) {
      i.O(0, [493, 856, 774, 888, 179], (function () {
        return e = 1829, i(i.s = e);
        var e
      }));
      var e = i.O();
      _N_E = e
    }
  ]);