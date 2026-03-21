(self.webpackChunk_N_E = self.webpackChunk_N_E || []).push([
    [405], {
      8436: function (n, i, t) {
        "use strict";
        t.d(i, {
          Z: function () {
            return c
          }
        });
        var e = t(8152),
          o = t(7294),
          a = t(9163),
          r = t(131),
          s = t(5893);
  
        function c(n) {
          var i = n.className,
            t = n.originalSize,
            a = n.image,
            c = a.width,
            d = a.height,
            p = a.thumbUrl,
            m = a.url,
            g = a.title,
            h = (0, o.useState)(!1),
            f = h[0],
            u = h[1],
            x = (0, r.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            w = (0, e.Z)(x, 2),
            v = w[0],
            b = w[1],
            y = (0, o.useCallback)((function () {
              u(!0)
            }), []);
          return (0, s.jsx)(l, {
            ref: v,
            aspectRatio: d / c * 100,
            className: i,
            isIntersected: f,
            children: b && (0, s.jsx)("img", {
              src: t ? m : p,
              title: g,
              onLoad: y
            })
          })
        }
        var l = a.ZP.div.withConfig({
          displayName: "LazyImage__Wrapper",
          componentId: "sc-xnobnf-0"
        })(["position:relative;border-radius:6px;background:rgba(0,0,0,0.05);overflow:hidden;cursor:pointer;transition:opacity 0.2s ease-out 0s;&:hover{opacity:0.8;}&::before{content:'';display:block;", "}& > img{display:block;position:absolute;top:0;left:0;width:100%;height:100%;opacity:0;transition:opacity 0.2s ease-out 0s;", "}"], (function (n) {
          var i = n.aspectRatio;
          return (0, a.iv)(["padding-top:", "%;"], i)
        }), (function (n) {
          return n.isIntersected && (0, a.iv)(["opacity:1;"])
        }))
      },
      8211: function (n, i, t) {
        "use strict";
        t.d(i, {
          Z: function () {
            return c
          }
        });
        var e = t(8152),
          o = (t(7294), t(9163)),
          a = t(9477),
          r = t(131),
          s = t(5893);
  
        function c(n) {
          var i = n.serviceName,
            t = n.url,
            o = n.imageUrl,
            a = (0, r.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            c = (0, e.Z)(a, 2),
            h = c[0],
            f = c[1];
          return (0, s.jsxs)(l, {
            href: t,
            target: "_blank",
            ref: h,
            isIntersected: f,
            children: [(0, s.jsx)(d, {
              style: {
                backgroundImage: "url(".concat(o, ")")
              }
            }), (0, s.jsxs)(p, {
              children: [(0, s.jsx)(m, {
                children: i
              }), (0, s.jsx)(g, {})]
            })]
          })
        }
        var l = o.ZP.a.withConfig({
            displayName: "LinkCard__Wrapper",
            componentId: "sc-1p9qe2f-0"
          })(["box-sizing:border-box;display:grid;grid-row-gap:4px;padding:8px;border:solid 1px var(--color-black);border-radius:12px;background-color:var(--color-white);transition:background-color 0.2s ease-out 0s;opacity:0;transform:translateY(12px);", " &:hover{background-color:rgba(0,0,0,0.04);}"], (function (n) {
            return n.isIntersected && (0, o.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          })),
          d = o.ZP.div.withConfig({
            displayName: "LinkCard__Thumb",
            componentId: "sc-1p9qe2f-1"
          })(["position:relative;width:100%;border-radius:8px;background:center / cover no-repeat;overflow:hidden;&::before{--width:430;--height:170;content:'';display:block;padding-top:calc(var(--height) / var(--width) * 100%);}"]),
          p = o.ZP.div.withConfig({
            displayName: "LinkCard__ServiceNameWrapper",
            componentId: "sc-1p9qe2f-2"
          })(["display:grid;grid-column-gap:10px;grid-template-columns:repeat(2,auto);justify-content:end;align-items:center;margin-right:8px;"]),
          m = o.ZP.div.withConfig({
            displayName: "LinkCard__ServiceName",
            componentId: "sc-1p9qe2f-3"
          })(["font-size:18px;font-weight:bold;line-height:26px;color:var(--color-black);transform:translateY(-2%);", "{font-size:16px;line-height:24px;}"], a.BC.smallDown),
          g = o.ZP.div.withConfig({
            displayName: "LinkCard__ArrowIcon",
            componentId: "sc-1p9qe2f-4"
          })(["width:6.5px;background:center / contain no-repeat;background-image:url('data:image/svg+xml;charset=utf8,%3Csvg%20width%3D%227%22%20height%3D%2213%22%20viewBox%3D%220%200%207%2013%22%20fill%3D%22none%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%0A%3Cpath%20d%3D%22M1.38348%2012.3474C0.974156%2012.3474%200.633054%2012.2%200.360173%2011.8316C-0.117369%2011.2421%200.0190714%2010.2842%200.564834%209.76842L3.08898%207.33684C3.43009%207.0421%203.43009%206.45263%203.08898%206.15789L0.496613%203.57894C-0.0491489%203.06315%20-0.185589%202.17894%200.291953%201.58947C0.769495%200.852626%201.72458%200.778942%202.27034%201.36842L5.74958%204.75789C6.22712%205.27368%206.5%205.93684%206.5%206.67368C6.5%207.41052%206.22712%208.07368%205.68136%208.58947L2.27034%2011.9053C1.99746%2012.2%201.65636%2012.3474%201.38348%2012.3474Z%22%20fill%3D%22%23111111%22%2F%3E%0A%3C%2Fsvg%3E%0A');", "{width:5px;}&::before{--width:6.5;--height:11.37;content:'';display:block;padding-top:calc(var(--height) / var(--width) * 100%);}"], a.BC.smallDown)
      },
      7276: function (n, i, t) {
        "use strict";
        t.r(i), t.d(i, {
          __N_SSG: function () {
            return Xt
          },
          default: function () {
            return Gt
          }
        });
        var e = t(7294),
          o = t(4559),
          a = t(9163),
          r = t(8152),
          s = t(9477),
          c = t(7484),
          l = t.n(c),
          d = t(5893);
  
        function p(n) {
          var i = n.num,
            t = n.isActive,
            o = n.illust,
            a = n.illustDark,
            u = n.link,
            r = n.onClick,
            s = (0, e.useState)(!1),
            c = s[0],
            p = s[1];
          return (0, d.jsx)(m, {
            isActive: t,
            onClick: function (n) {
              function i() {
                return n.apply(this, arguments)
              }
              return i.toString = function () {
                return n.toString()
              }, i
            }((function () {
              return r(i)
            })),
            ref: function () {
              p(l()().hour() >= 19 || l()().hour() <= 5)
            },
              children: (0, d.jsxs)(d.Fragment, {
                children: [(0, d.jsx)(g, {
                  src: c && a ? a : o
                }), (0, d.jsx)(linkAnchor, {
                  href: u || "#",
                  target: "_blank",
                  rel: "noopener noreferrer",
                  onClick: function (n) {
                    n.stopPropagation()
                  }
                })]
            })
          })
        }
        var m = a.ZP.div.withConfig({
            displayName: "Illust__Wrapper",
            componentId: "sc-19znc2x-0"
          })(["position:relative;overflow:hidden;background:#eee;cursor:pointer;&::before{content:'';display:block;}", "{--side-illust-width:9%;width:var(--side-illust-width);height:100%;border-radius:10px;transition:width 0.7s cubic-bezier(0.7,0,0.25,0.99) 0s;", " &::before{position:absolute;top:0;left:0;width:100%;height:100%;}}", "{width:100%;border-radius:5px;&::before{padding-top:14%;transition:padding-top 0.7s cubic-bezier(0.7,0,0.25,0.99) 0s;", "}}"], s.BC.mediumUp, (function (n) {
            return n.isActive && (0, a.iv)(["width:calc(100% - var(--side-illust-width) * 3 - var(--gap) * 3);"])
          }), s.BC.smallDown, (function (n) {
            return n.isActive && (0, a.iv)(["padding-top:100%;"])
          })),
          g = a.ZP.img.withConfig({
            displayName: "Illust__Image",
            componentId: "sc-19znc2x-1"
          })(["display:block;position:absolute;top:0;left:0;width:100%;height:100%;object-fit:cover;"]),
          linkAnchor = a.ZP.a.withConfig({
            displayName: "Illust__LinkAnchor",
            componentId: "sc-19znc2x-2"
          })(["position:absolute;top:0;left:0;width:100%;height:100%;z-index:1;"]),
          h = "draw-text1__mask-id";
        var f = a.ZP.svg.attrs({
            viewBox: "0 0 1163.6 203.1"
          }).withConfig({
            displayName: "DrawText1__Svg",
            componentId: "sc-5ijh38-0"
          })(["pointer-events:none;"]),
          u = a.ZP.polyline.withConfig({
            displayName: "DrawText1__Polyline",
            componentId: "sc-5ijh38-1"
          })(["fill:none;stroke:#fff;stroke-width:8;stroke-linecap:round;stroke-linejoin:round;opacity:0;", ""], (function (n) {
            return n.emit && (0, a.iv)(["&:nth-child(1){--stroke-length:625.359;animation:fadeIn 0.05s ease-out calc(0.4s + 3.4s) forwards,drawLine 0.15s ease-out calc(0.4s + 3.4s) forwards;}&:nth-child(2){--stroke-length:886.533;animation:fadeIn 0.05s ease-out calc(0.55s + 3.4s) forwards,drawLine 0.2s ease-out calc(0.55s + 3.4s) forwards;}&:nth-child(3){--stroke-length:424.6;animation:fadeIn 0.05s ease-out calc(0.75s + 3.4s) forwards,drawLine 0.1s ease-out calc(0.75s + 3.4s) forwards;}&:nth-child(4){--stroke-length:72.6;animation:fadeIn 0.05s ease-out calc(0.85s + 3.4s) forwards,drawLine 0.05s ease-out calc(0.85s + 3.4s) forwards;}&:nth-child(5){--stroke-length:432;animation:fadeIn 0.05s ease-out calc(0.9s + 3.4s) forwards,drawLine 0.1s ease-out calc(0.9s + 3.4s) forwards;}&:nth-child(6){--stroke-length:182.5;animation:fadeIn 0.05s ease-out calc(1s + 3.4s) forwards,drawLine 0.1s ease-out calc(1s + 3.4s) forwards;}&:nth-child(7){--stroke-length:73.5;animation:fadeIn 0.05s ease-out calc(1s + 3.4s) forwards,drawLine 0.05s ease-out calc(1s + 3.4s) forwards;}&:nth-child(8){--stroke-length:579.293;animation:fadeIn 0.05s ease-out calc(1.05s + 3.4s) forwards,drawLine 0.15s ease-out calc(1.05s + 3.4s) forwards;}"])
          })),
          x = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line1",
            componentId: "sc-5ijh38-2"
          })([""]),
          w = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line2",
            componentId: "sc-5ijh38-3"
          })([""]),
          v = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line3",
            componentId: "sc-5ijh38-4"
          })([""]),
          b = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line4",
            componentId: "sc-5ijh38-5"
          })([""]),
          y = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line5",
            componentId: "sc-5ijh38-6"
          })([""]),
          C = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line6",
            componentId: "sc-5ijh38-7"
          })([""]),
          _ = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line7",
            componentId: "sc-5ijh38-8"
          })([""]),
          k = (0, a.ZP)(u).attrs({
            points: ""
          }).withConfig({
            displayName: "DrawText1__Line8",
            componentId: "sc-5ijh38-9"
          })([""]),
          j = a.ZP.path.attrs({
            d: ""
          }).withConfig({
            displayName: "DrawText1__Text",
            componentId: "sc-5ijh38-10"
          })(["fill:var(--color-pink);"]),
          I = "draw-text2__mask-id";
        var N = a.ZP.svg.attrs({
            viewBox: "0 0 712.7 237.3"
          }).withConfig({
            displayName: "DrawText2__Svg",
            componentId: "sc-tryrvg-0"
          })(["pointer-events:none;"]),
          Z = a.ZP.polyline.withConfig({
            displayName: "DrawText2__Polyline",
            componentId: "sc-tryrvg-1"
          })(["fill:none;stroke:#fff;stroke-width:8;stroke-linecap:round;stroke-linejoin:round;opacity:0;", ""], (function (n) {
            return n.emit && (0, a.iv)(["&:nth-child(1){--stroke-length:280;animation:fadeIn 0.05s ease-out calc(0.5s + 3.4s) forwards,drawLine 0.15s ease-out calc(0.5s + 3.4s) forwards;}&:nth-child(2){--stroke-length:697;animation:fadeIn 0.05s ease-out calc(0.65s + 3.4s) forwards,drawLine 0.25s ease-out calc(0.65s + 3.4s) forwards;}&:nth-child(3){--stroke-length:1229.5;animation:fadeIn 0.05s ease-out calc(0.9s + 3.4s) forwards,drawLine 0.4s ease-out calc(0.9s + 3.4s) forwards;}"])
          })),
          z = (0, a.ZP)(Z).attrs({
            points: "162.6,34.6 155.4,37.2 148.6,41.3 142.8,46 136.7,53.3 130.7,62.6 123.5,77.3 118.9,87.5\n  110.1,112.7 104.1,131.5 97.3,150 91.3,164.8 85.7,179.5 80,188.9 72.7,200.9 67.1,208.5 59.9,215.7 49.4,223.8 39.1,228.7\n  31.2,231.7 23,233.3 17.3,232.6 12.2,230.8 7.7,227.6 5.4,223.8 4,220.3"
          }).withConfig({
            displayName: "DrawText2__Line1",
            componentId: "sc-tryrvg-2"
          })([""]),
          P = (0, a.ZP)(Z).attrs({
            points: "290.1,4 284.7,5.7 279.7,7.7 272.9,12.2 262.2,20.3 251,31.7 238.3,46.5 218,71.1 204.9,84.7\n  183.3,105.2 167.7,118 153.3,128.9 140.3,136.9 131.4,141.1 123.3,143.1 117.3,143.8 107,141.6 117.5,149.3 128.2,156.2\n  139.1,163.5 154.3,172.3 168.3,179.5 178.9,185.4 195,193.3 214.8,202.3 235.4,209.7 248.3,214.4 258.5,216.9 274.5,220.6\n  293.8,222.4 304.4,222.3 318.8,221 328.6,219 338.5,216.9 345.3,213.4 352.4,209.6 358.1,203.3 361.3,197.2 360.9,190.3\n  357.9,185.4 351.8,181.2 344.3,179.2 334.7,177.9 323.9,177.5 310.8,178.5 298.4,179.9 284.4,182.6 270.5,186.5 251.6,193.9\n  233.6,202 221,209.4 207.9,216.7 203.2,219.9"
          }).withConfig({
            displayName: "DrawText2__Line2",
            componentId: "sc-tryrvg-3"
          })([""]),
          D = (0, a.ZP)(Z).attrs({
            points: "274.4,141.5 271.9,139.5 268.1,138.8 265.2,139 262.1,140 259.8,141.8 257.1,144.8 254.2,148.8\n  252.2,152.5 251.5,155.4 251.5,158.3 251.9,161.6 252.9,163.8 254.5,165.2 256.8,165 260.4,162.4 266.6,157.2 272,150.8\n  274.8,146.3 281.7,135.7 279.9,142.2 277.6,150.5 277.4,155 278.1,158.6 279.4,160.8 282.3,162.7 286.8,162.9 294.9,160.6\n  305.2,154.9 318,147.3 330.9,139.1 343.2,130.8 351.6,123.4 360.7,115.9 363.9,113.7 365.5,113.7 366.9,114.4 366.6,116.5\n  364.6,125.4 354.8,150.2 369.7,122.7 372,118.5 375.2,114.3 379.4,110.3 383.2,109.1 386.2,109 389.6,109.3 390.6,110\n  390.8,112.7 382.7,105.1 381.6,112.8 380.5,117.8 381,121.7 383,125.2 387.5,128.2 393.3,129.5 399.8,129.9 406.8,129\n  413.5,127.7 420.4,125 426.2,122.1 431.2,119.2 433.4,116.9 441.9,108.3 438,116.8 436.7,122.7 437.4,125.7 438.8,127.5\n  440.7,128 443,127.3 449.4,123.8 453.8,120 458.7,114 462.3,108.3 463.6,104.9 464.5,101.5 464.1,99 463,96.9 461.1,95.7\n  459,95.3 456.4,95.5 454.1,96.4 452.3,98.1 450.7,100.5 449.9,103.3 449.5,107.8 449.8,112.4 451.2,115.7 458.4,120.9 462.4,122\n  469.7,121.7 478.5,120.4 491.6,116.7 509.8,108.6 517.9,103.2 528.3,95.3 536.6,86.8 540.2,80.9 542.3,75 542.2,71.6 541.4,69.3\n  539.7,67.9 537.3,67.1 534.9,66.9 532.5,68.1 532.5,70.5 534.3,74 537.3,77.1 542.9,78.2 547.8,79.2 552.3,79.4 553.2,79.9\n  550.2,84.4 547.2,89.8 545.9,93.7 545.9,98 547.2,101.7 549.5,104.9 552.6,106.8 555.9,107.4 560,107.4 566.7,106.2 574.8,103.2\n  589.4,98.2 604.2,89.7 620.3,79.3 632.4,70.1 637.7,66.8 641.7,64.5 645.2,63.2 647.2,63.2 648.4,64.3 648.1,66.5 646.4,73.2\n  643.1,82 640.1,88.5 639.9,91.3 640.3,93.3 641.8,94.7 643.6,94.9 647.6,92.3 669.9,59 649,101.3 635.5,127.9 628.7,140.7\n  621.6,152.3 615.1,163.1 608.1,173 602.6,180.2 595.8,186.5 591.6,189 586.9,190.4 583.3,189.8 580.4,187.7 578.8,184.1\n  578.4,179.8 579.4,174.4 582.4,167.2 587.6,159.2 593.9,151.1 602.1,142.1 611.6,133.9 621.9,125.5 634.4,116.9 661.7,97.4\n  678.2,85.4 690.9,76.7 708.7,64.2"
          }).withConfig({
            displayName: "DrawText2__Line3",
            componentId: "sc-tryrvg-4"
          })([""]),
          B = a.ZP.path.attrs({
            d: ""
          }).withConfig({
            displayName: "DrawText2__Text",
            componentId: "sc-tryrvg-5"
          })(["fill:var(--color-pink);"]),
          M = t(131);
        var L = (0, a.F4)(["from,to{transform:scale(1);}50%{transform:scale(0.6);}"]),
          A = a.ZP.svg.attrs({
            viewBox: "0 0 29.4 47.3"
          }).withConfig({
            displayName: "KiraKira__Svg",
            componentId: "sc-1iilsk-0"
          })(["transform-origin:center;width:29px;", ""], (function (n) {
            return n.emit && (0, a.iv)(["animation:", " 2s ease-in-out 0s infinite;"], L)
          })),
          S = a.ZP.path.attrs({
            d: "M29.4,23.7c-8,0-14.7,10.9-14.7,23.7C14.7,34.6,8,23.7,0,23.7c8,0,14.7-10.9,14.7-23.7\n\tC14.7,12.8,21.5,23.7,29.4,23.7z"
          }).withConfig({
            displayName: "KiraKira__Path",
            componentId: "sc-1iilsk-1"
          })(["fill:var(--color-yellow);"]),
          T = [{
            num: 0,
            illust: "/images/main-visual/1.jpg",
            illustDark: "/images/main-visual/1--dark.jpg",
            link: "/character/"
          }, {
            num: 1,
            illust: "/images/main-visual/2.jpg",
            illustDark: "/images/main-visual/2--dark.jpg",
            link: "/circle/"
          }];
  
        function W(n) {
          var i = n.emit,
            t = (0, e.useState)(!1),
            o = t[0],
            a = t[1],
            r = (0, e.useState)(0),
            s = r[0],
            c = r[1],
            l = function () {
              return c((function (n) {
                return n >= T.length - 1 ? 0 : n + 1
              }))
            };
          return (0, e.useEffect)((function () {
            if (i) {
              var n = setTimeout((function () {
                a(!0)
              }), 4e3);
              return function () {
                return clearTimeout(n)
              }
            }
          }), [i]), (0, e.useEffect)((function () {
            if (o) {
              var n = setTimeout(l, 3500);
              return function () {
                return clearTimeout(n)
              }
            }
          }), [s, o]), (0, d.jsxs)(O, {
            children: [T.map((function (n) {
              var i = n.num,
                t = n.illust,
                e = n.illustDark,
                o = n.link;
              return (0, d.jsx)(p, {
                num: i,
                isActive: i === s,
                illust: t,
                illustDark: e,
                link: o,
                onClick: c
              }, i)
            })), (0, d.jsx)(E, {
              emit: i
            }), (0, d.jsx)(K, {
              emit: i
            }), (0, d.jsx)(Y, {
              children: T.map((function (n) {
                var i = n.num;
                return (0, d.jsx)(F, {
                  isActive: i === s,
                  onClick: function () {
                    return c(i)
                  }
                }, i)
              }))
            }), (0, d.jsxs)(U, {
              children: [(0, d.jsx)(V, {}), (0, d.jsx)(R, {}), (0, d.jsx)(X, {}), (0, d.jsx)(G, {}), (0, d.jsx)(H, {})]
            })]
          })
        }
        var O = a.ZP.div.withConfig({
            displayName: "Accordion__Wrapper",
            componentId: "sc-19fenr7-0"
          })(["--gap:5px;", "{--menu-width:250px;--dots-width:30px;position:absolute;bottom:var(--gap);right:var(--dots-width);display:flex;width:calc(100% - (var(--menu-width) + var(--dots-width)));height:calc(100% - var(--gap) * 2);& > *:not(:first-child){margin-left:var(--gap);}}", "{--menu-width:5px;height:calc(100% - var(--gap) * 2 - 70px);}", "{position:relative;max-width:580px;width:calc(100% - var(--gap) * 2);margin:80px auto 0;& > *:not(:first-child){margin-top:var(--gap);}}"], s.BC.mediumUp, s.BC.tablet, s.BC.smallDown),
          Y = a.ZP.div.withConfig({
            displayName: "Accordion__Dots",
            componentId: "sc-19fenr7-1"
          })(["", "{display:grid;grid-row-gap:16px;justify-content:center;align-content:center;position:absolute;top:0;right:0;width:var(--dots-width);height:100%;transform:translateX(97%);margin:0 !important;}", "{position:absolute;bottom:0;left:0;display:grid;grid-column-gap:12px;grid-template-columns:repeat(4,auto);justify-content:start;align-items:start;padding:16px 8px;transform:translateY(100%);}"], s.BC.mediumUp, s.BC.smallDown),
          F = a.ZP.div.withConfig({
            displayName: "Accordion__Dot",
            componentId: "sc-19fenr7-2"
          })(["--dot-size:8px;position:relative;width:var(--dot-size);height:var(--dot-size);border-radius:50%;background:#ccc;cursor:pointer;", "{--dot-size:6px;}&::before{--active-dot-size:10px;box-sizing:border-box;content:'';display:block;position:absolute;top:-1px;left:-1px;width:var(--active-dot-size);height:var(--active-dot-size);border-radius:50%;border:solid 3px var(--color-black);background:var(--color-white);opacity:0;transition:opacity 0.16s ease-out 0s;", "{--active-dot-size:8px;}", "}"], s.BC.smallDown, s.BC.smallDown, (function (n) {
            return n.isActive && (0, a.iv)(["opacity:1;"])
          })),
          E = (0, a.ZP)((function (n) {
            var i = n.className,
              t = n.emit;
            return (0, d.jsxs)(f, {
              className: i,
              children: [(0, d.jsxs)("mask", {
                id: h,
                children: [(0, d.jsx)(x, {
                  emit: t
                }), (0, d.jsx)(w, {
                  emit: t
                }), (0, d.jsx)(v, {
                  emit: t
                }), (0, d.jsx)(b, {
                  emit: t
                }), (0, d.jsx)(y, {
                  emit: t
                }), (0, d.jsx)(C, {
                  emit: t
                }), (0, d.jsx)(_, {
                  emit: t
                }), (0, d.jsx)(k, {
                  emit: t
                })]
              }), (0, d.jsx)(j, {
                mask: "url(#".concat(h, ")")
              })]
            })
          })).withConfig({
            displayName: "Accordion__StyledDrawText1",
            componentId: "sc-19fenr7-3"
          })(["position:absolute;top:0;left:50%;width:102%;transform:translate(-50%,-31%);"]),
          K = (0, a.ZP)((function (n) {
            var i = n.className,
              t = n.emit;
            return (0, d.jsxs)(N, {
              className: i,
              children: [(0, d.jsxs)("mask", {
                id: I,
                children: [(0, d.jsx)(z, {
                  emit: t
                }), (0, d.jsx)(P, {
                  emit: t
                }), (0, d.jsx)(D, {
                  emit: t
                })]
              }), (0, d.jsx)(B, {
                mask: "url(#".concat(I, ")")
              })]
            })
          })).withConfig({
            displayName: "Accordion__StyledDrawText2",
            componentId: "sc-19fenr7-4"
          })(["position:absolute;bottom:0;right:0;width:60%;transform:translate(0,30%);", "{transform:translate(0,60%);}"], s.BC.smallDown),
          U = a.ZP.div.withConfig({
            displayName: "Accordion__KiraKiraWrapper",
            componentId: "sc-19fenr7-5"
          })(["position:absolute;top:0;left:0;width:100%;height:100%;pointer-events:none;"]),
          q = (0, a.ZP)((function (n) {
            var i = n.className,
              t = (0, M.YD)({
                rootMargin: "0px"
              }),
              e = (0, r.Z)(t, 2),
              o = e[0],
              a = e[1];
            return (0, d.jsx)(A, {
              ref: o,
              emit: a,
              className: i,
              children: (0, d.jsx)(S, {})
            })
          })).withConfig({
            displayName: "Accordion__StyledKiraKira",
            componentId: "sc-19fenr7-6"
          })(["position:absolute;width:2.5%;", "{width:4%;}"], s.BC.smallDown),
          V = (0, a.ZP)(q).withConfig({
            displayName: "Accordion__KiraKira1",
            componentId: "sc-19fenr7-7"
          })(["top:8%;left:3%;", "{top:2%;left:3%;}"], s.BC.smallDown),
          R = (0, a.ZP)(q).withConfig({
            displayName: "Accordion__KiraKira2",
            componentId: "sc-19fenr7-8"
          })(["top:5%;left:57%;", "{top:1%;}"], s.BC.smallDown),
          X = (0, a.ZP)(q).withConfig({
            displayName: "Accordion__KiraKira3",
            componentId: "sc-19fenr7-9"
          })(["top:2%;left:83%;", "{top:0%;left:90%;}"], s.BC.smallDown),
          G = (0, a.ZP)(q).withConfig({
            displayName: "Accordion__KiraKira4",
            componentId: "sc-19fenr7-10"
          })(["bottom:-1%;right:41%;", "{bottom:-5%;}"], s.BC.smallDown),
          H = (0, a.ZP)(q).withConfig({
            displayName: "Accordion__KiraKira5",
            componentId: "sc-19fenr7-11"
          })(["bottom:0%;right:15%;", "{bottom:-5%;}"], s.BC.smallDown),
          J = t(9313);
  
        function Q(n) {
          var i = n.emit,
            t = (0, M.YD)({
              rootMargin: "0px"
            }),
            o = (0, r.Z)(t, 2),
            a = o[0],
            s = o[1],
            c = (0, e.useState)(!1),
            l = c[0],
            p = c[1],
            m = (0, e.useCallback)((function () {
              p(!0)
            }), [p]),
            g = (0, e.useCallback)((function () {
              p(!1)
            }), [p]);
          return (0, d.jsxs)(d.Fragment, {
            children: [(0, d.jsxs)($, {
              ref: a,
              emit: i,
              children: [(0, d.jsx)(nn, {}), (0, d.jsx)(W, {
                emit: i
              })]
            }), (0, d.jsx)(tn, {
              isOpen: l,
              onOpen: m,
              onClose: g,
              isIntersected: s
            }), l && (0, d.jsx)(J.XN, {
              onClick: g
            }), l && (0, d.jsxs)(en, {
              children: [(0, d.jsx)(on, {
                onClick: g
              }), (0, d.jsx)(an, {
                hiddenLogo: !0,
                onClick: g
              })]
            })]
          })
        }
        var $ = a.ZP.div.withConfig({
            displayName: "MainVisual__Wrapper",
            componentId: "sc-10rtb2s-0"
          })(["position:relative;", "{width:100%;height:100vh;}"], s.BC.mediumUp),
          nn = (0, a.ZP)(J.WA).withConfig({
            displayName: "MainVisual__StyledMenuDesktop",
            componentId: "sc-10rtb2s-1"
          })(["position:absolute;top:0;left:0;", "{display:none;}", "{display:none;}"], s.BC.tablet, s.BC.smallDown),
          tn = (0, a.ZP)(J.ti).withConfig({
            displayName: "MainVisual__StyledMenuHeader",
            componentId: "sc-10rtb2s-2"
          })(["", "{transform:translateY(-100px);transition:0.5s ease-in-out 0s;", "}"], s.BC.desktop, (function (n) {
            return !n.isIntersected && (0, a.iv)(["transform:translateY(0);"])
          })),
          en = a.ZP.div.withConfig({
            displayName: "MainVisual__MenuDesktopWrapper",
            componentId: "sc-10rtb2s-3"
          })(["position:fixed;top:0;left:0;width:100%;height:100%;z-index:9;", "{display:none;}"], s.BC.smallDown),
          on = a.ZP.div.withConfig({
            displayName: "MainVisual__MenuDesktopBg",
            componentId: "sc-10rtb2s-4"
          })(["position:absolute;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.5);opacity:0;animation:fadeIn 0.45s ease-out 0s forwards;"]),
          an = (0, a.ZP)(J.WA).withConfig({
            displayName: "MainVisual__SideMenuDesktop",
            componentId: "sc-10rtb2s-5"
          })(["position:absolute;top:0;right:0;box-shadow:-4px 0 4px rgba(0,0,0,0.05);transform:translateX(100%);animation:slideIn 0.45s cubic-bezier(0,0,0.14,1.01) 0s forwards;"]),
          rn = t(8540);
        var sn = a.ZP.svg.attrs({
            viewBox: "0 0 65 51.1"
          }).withConfig({
            displayName: "Ribbon__Svg",
            componentId: "sc-o2ks21-0"
          })([""]),
          cn = a.ZP.path.attrs({
            d: "M63.4,3.6l-0.1-0.4c-0.5-1.6-1.8-2.8-3.5-3.1C59-0.1,58.1,0,57.3,0.3c-8,3.3-15.3,7.8-21.8,13.4\n\tl-2.5,2.1l-3.8-2.7C22.6,8.2,15.3,4,7.7,0.5C6.5,0,5.1,0,3.9,0.5C2.7,1,1.8,2.1,1.4,3.3c-2.6,8.6-1.7,18,2.6,25.9l0.5,0.8\n\tc1,2,3.4,2.9,5.5,2.1l8.4-3.3c1.3-0.5,2.5-1.1,3.7-1.7l5.7-2.7c-4.6,8.1-10.3,15.4-17.2,21.7l-0.6,0.6c-1,0.9-1.1,2.5-0.1,3.5\n\tc0.5,0.5,1.1,0.8,1.8,0.8c0.6,0,1.2-0.2,1.7-0.7l0.6-0.6c7.7-7.1,14.1-15.4,19-24.6c3.3,7.8,9,17.9,18.2,24.8\n\tc0.5,0.4,1.2,0.6,1.8,0.5c0.7-0.1,1.2-0.4,1.6-1c0.4-0.5,0.6-1.2,0.5-1.8c-0.1-0.7-0.4-1.2-1-1.6c-7.7-5.8-12.8-14.4-15.9-21.2\n\tc5.5,2.9,11.3,5.2,17.2,7l0.1,0c2.3,0.7,4.7-0.5,5.6-2.7c1.3-3.2,2.4-6.5,3.1-9.9c0.4-1.8,0.6-3.6,0.6-5.4\n\tC65,10.4,64.4,6.9,63.4,3.6z M56.8,27c-6.8-2-13.3-4.8-19.3-8.3l1.3-1.1c5.9-5.2,12.6-9.3,19.9-12.4C59.5,8,60,10.9,60,13.8\n\tc0,1.5-0.2,2.9-0.5,4.3c0,0,0,0,0,0C58.9,21.2,58,24.2,56.8,27z M8.7,27.4l-0.3-0.5C4.9,20.3,4,12.4,6.1,5.2\n\tc7.1,3.3,14,7.3,20.3,11.9l2.1,1.5l-8.5,4c0,0-0.1,0-0.1,0c-1,0.6-2.1,1.1-3.3,1.5L8.7,27.4z"
          }).withConfig({
            displayName: "Ribbon__Path",
            componentId: "sc-o2ks21-1"
          })(["fill:currentColor;"]);
  
        function ln(n) {
          var i = n.news,
            t = (0, M.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            e = (0, r.Z)(t, 2),
            o = e[0],
            a = e[1];
          return (0, d.jsxs)(dn, {
            ref: o,
            isIntersected: a,
            children: [(0, d.jsx)(gn, {}), (0, d.jsx)(hn, {}), (0, d.jsx)(fn, {}), (0, d.jsx)(un, {}), (0, d.jsx)(xn, {
              children: (0, d.jsx)(wn, {
                children: i.sort((function (n, i) {
                  return l()(i.date, "YYYY-MM-DD").unix() - l()(n.date, "YYYY-MM-DD").unix()
                })).map((function (n) {
                  var i = n.text,
                    t = n.date;
                  return (0, d.jsxs)(vn, {
                    children: [(0, d.jsx)(bn, {
                      children: t
                    }), (0, d.jsx)(yn, {
                      children: i
                    })]
                  }, "".concat(t, "-").concat(i))
                }))
              })
            }), (0, d.jsx)(pn, {})]
          })
        }
        var dn = a.ZP.div.withConfig({
            displayName: "NewsBox__Wrapper",
            componentId: "sc-1gifetl-0"
          })(["box-sizing:border-box;position:relative;color:var(--color-black);border:solid currentColor;opacity:0;transform:translateY(12px);&::after{content:'';display:block;position:absolute;bottom:0;left:0;width:100%;height:64px;background:linear-gradient( to top,rgba(255,255,255,1),rgba(255,255,255,0) );pointer-events:none;", "{height:32px;}}", " ", "{max-width:620px;width:calc(100% - 48px);height:375px;border-width:4px;}", "{max-width:580px;width:calc(100% - 48px);height:270px;border-width:3px;}"], s.BC.smallDown, (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          }), s.BC.mediumUp, s.BC.smallDown),
          pn = (0, a.ZP)((function (n) {
            var i = n.className;
            return (0, d.jsx)(sn, {
              className: i,
              children: (0, d.jsx)(cn, {})
            })
          })).withConfig({
            displayName: "NewsBox__StyledRibbon",
            componentId: "sc-1gifetl-1"
          })(["position:absolute;bottom:0;left:50%;width:50px;transform:translate(-50%,65%);z-index:1;", "{width:34px;}"], s.BC.smallDown),
          mn = a.ZP.div.withConfig({
            displayName: "NewsBox__Deco",
            componentId: "sc-1gifetl-2"
          })(["--icon-size:18px;box-sizing:border-box;position:absolute;width:var(--icon-size);height:var(--icon-size);border:inherit;border-radius:50% 50% 0 50%;", "{--icon-size:12px;}"], s.BC.smallDown),
          gn = (0, a.ZP)(mn).withConfig({
            displayName: "NewsBox__Deco1",
            componentId: "sc-1gifetl-3"
          })(["top:0;left:0;transform:translate(-100%,-100%);"]),
          hn = (0, a.ZP)(mn).withConfig({
            displayName: "NewsBox__Deco2",
            componentId: "sc-1gifetl-4"
          })(["top:0;right:0;transform:translate(100%,-100%) rotate(90deg);"]),
          fn = (0, a.ZP)(mn).withConfig({
            displayName: "NewsBox__Deco3",
            componentId: "sc-1gifetl-5"
          })(["bottom:0;right:0;transform:translate(100%,100%) rotate(180deg);"]),
          un = (0, a.ZP)(mn).withConfig({
            displayName: "NewsBox__Deco4",
            componentId: "sc-1gifetl-6"
          })(["bottom:0;left:0;transform:translate(-100%,100%) rotate(270deg);"]),
          xn = a.ZP.div.withConfig({
            displayName: "NewsBox__Inner",
            componentId: "sc-1gifetl-7"
          })(["--padding:32px;box-sizing:border-box;position:absolute;top:0;left:0;width:100%;height:100%;padding:var(--padding);overflow:auto;-webkit-overflow-scrolling:touch;", "{--padding:16px;}"], s.BC.smallDown),
          wn = a.ZP.div.withConfig({
            displayName: "NewsBox__List",
            componentId: "sc-1gifetl-8"
          })(["display:grid;grid-row-gap:16px;"]),
          vn = a.ZP.div.withConfig({
            displayName: "NewsBox__ListItem",
            componentId: "sc-1gifetl-9"
          })(["display:grid;grid-row-gap:4px;", "{grid-row-gap:2px;}"], s.BC.smallDown),
          bn = a.ZP.div.withConfig({
            displayName: "NewsBox__PublishDate",
            componentId: "sc-1gifetl-10"
          })(["font-size:12px;line-height:20px;letter-spacing:0.1em;color:var(--color-pink);", "{font-size:10px;line-height:18px;}"], s.BC.smallDown),
          yn = a.ZP.div.withConfig({
            displayName: "NewsBox__Text",
            componentId: "sc-1gifetl-11"
          })(["font-size:16px;line-height:24px;color:currentColor;word-break:break-all;", "{font-size:12px;line-height:20px;}"], s.BC.smallDown),
          Cn = t(5866),
          _n = t(9866);
  
        function kn(n) {
          var i = n.news;
          return (0, d.jsxs)(jn, {
            id: "news",
            children: [(0, d.jsx)(Cn.Z, {
              reverse: !0
            }), (0, d.jsxs)(In, {
              children: [(0, d.jsx)(_n.KP, {}), (0, d.jsx)(ln, {
                news: i
              })]
            })]
          })
        }
        var jn = a.ZP.div.withConfig({
            displayName: "News__Wrapper",
            componentId: "sc-1txvu8-0"
          })([""]),
          In = a.ZP.div.withConfig({
            displayName: "News__Inner",
            componentId: "sc-1txvu8-1"
          })(["display:grid;grid-row-gap:80px;justify-items:center;padding:160px 0;", "{grid-row-gap:48px;padding:64px 0;}"], s.BC.smallDown),
          Nn = t(8211),
          Zn = [{
            serviceName: "FANBOX",
            url: "https://mashiro6425.fanbox.cc/",
            imageUrl: "/images/about/links/fanbox.png"
          }, {
            serviceName: "Lit.link",
            imageUrl: "/images/about/links/Lit.link.png"
          }, {
            serviceName: "YouTube",
            url: "https://www.youtube.com/channel/UC4vAYHKWl4gUkprCOOOCt4g",
            imageUrl: "/images/about/links/youtube.png"
          }, {
             serviceName: "nizima",
            imageUrl: "/images/about/links/nizima.png"
          }];
  
        function zn(n) {
          var i = n.status,
            t = n.onClick,
            e = (0, M.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            o = (0, r.Z)(e, 2),
            a = o[0],
            s = o[1];
          return (0, d.jsxs)(Dn, {
            status: i,
            ref: a,
            isIntersected: s,
            children: [(0, d.jsx)(Pn, {
              onClick: function (n) {
                function i() {
                  return n.apply(this, arguments)
                }
                return i.toString = function () {
                  return n.toString()
                }, i
              }((function () {
                return t(!1)
              })),
              children: "CreaterVTuber."
            }), (0, d.jsx)(Pn, {
              onClick: function (n) {
                function i() {
                  return n.apply(this, arguments)
                }
                return i.toString = function () {
                  return n.toString()
                }, i
              }((function () {
                return t(!0)
              })),
              children: "VTuber."
            })]
          })
        }
        var Pn = a.ZP.div.withConfig({
            displayName: "Tab__Item",
            componentId: "sc-121kqbq-0"
          })(["position:relative;display:grid;justify-content:center;align-items:center;width:172px;height:40px;font-size:22px;font-weight:bold;line-height:1;color:var(--color-white);cursor:pointer;transition:color 0.24s ease-out 0s;", "{font-size:16px;width:120px;height:32px;}"], s.BC.smallDown),
          Dn = a.ZP.div.withConfig({
            displayName: "Tab__Wrapper",
            componentId: "sc-121kqbq-1"
          })(["box-sizing:border-box;position:relative;display:grid;grid-template-columns:repeat(2,auto);justify-content:center;background:var(--color-white);border:solid 2px var(--color-black);opacity:0;transform:translateY(12px);", " &::before{content:'';display:block;position:absolute;top:0;left:0;width:50%;height:100%;background:var(--color-black);transition:0.24s cubic-bezier(0.67,-0.01,0.33,0.97) 0s;}", ""], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          }), (function (n) {
            return n.status ? (0, a.iv)(["&::before{transform:translateX(100%);}", "{&:first-child{color:var(--color-black);}}"], Pn) : (0, a.iv)(["", "{&:last-child{color:var(--color-black);}}"], Pn)
          }));
  
        function Bn() {
          return (0, d.jsxs)(Mn, {
            children: [(0, d.jsx)(Ln, {
              children: (0, d.jsx)("span", {
                children: "VTuber."
              })
            }), (0, d.jsx)(An, {
              children: (0, d.jsx)("span", {
                children: "水咲ましろ"
              })
            }), (0, d.jsx)(Sn, {
              children: (0, d.jsx)("span", {
                children: "MIZUSAKI MASHIRO"
              })
            })]
          })
        }
        var Mn = a.ZP.div.withConfig({
            displayName: "Name__Wrapper",
            componentId: "sc-1cyid6i-0"
          })(["position:absolute;top:30%;left:50%;display:grid;grid-row-gap:12px;justify-items:start;width:max-content;transform:translateX(-50%);", "{grid-row-gap:8px;}"], s.BC.smallDown),
          Ln = a.ZP.div.withConfig({
            displayName: "Name__Job",
            componentId: "sc-1cyid6i-1"
          })(["--skew:0px;position:relative;background:var(--color-white);clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );animation:maskIn 0.7s cubic-bezier(0,0,0.24,1.01) 0.1s forwards;span{display:block;position:relative;font-size:22px;font-weight:bold;line-height:1;padding:0 12px;color:var(--color-black);", "{font-size:18px;padding:0 8px;}}"], s.BC.smallDown),
          An = a.ZP.div.withConfig({
            displayName: "Name__FullName",
            componentId: "sc-1cyid6i-2"
          })(["--skew:0px;position:relative;clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );animation:maskIn 0.7s cubic-bezier(0,0,0.24,1.01) 0.2s forwards;&::before{content:'';display:block;position:absolute;top:0;left:0;width:100%;height:100%;background:var(--color-black);}span{display:block;position:relative;font-size:60px;font-weight:bold;line-height:1;color:var(--color-white);padding:0 24px;z-index:1;", "{font-size:48px;}}"], s.BC.smallDown),
          Sn = a.ZP.div.withConfig({
            displayName: "Name__NameEn",
            componentId: "sc-1cyid6i-3"
          })(["--skew:0px;position:relative;clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );animation:maskIn 0.7s cubic-bezier(0,0,0.24,1.01) 0.3s forwards;&::before{content:'';display:block;box-sizing:border-box;position:absolute;top:0;left:0;width:100%;height:100%;background:var(--color-white);border:solid 1px #ddd;}span{display:block;position:relative;font-size:22px;font-weight:bold;letter-spacing:0.1em;line-height:1;color:var(--color-black);padding:6px 20px;z-index:1;", "{font-size:18px;padding:4px 14px;}}"], s.BC.smallDown);
  
        function Tn() {
          return (0, d.jsxs)(Wn, {
            children: [(0, d.jsx)(Yn, {}), (0, d.jsx)(Fn, {}), (0, d.jsx)(Bn, {}), (0, d.jsx)(En, {})]
          })
        }
        var Wn = a.ZP.div.withConfig({
            displayName: "Character__Wrapper",
            componentId: "sc-tiy317-0"
          })(["position:relative;margin-top:-50px;", "{margin-top:-25px;}"], s.BC.smallDown),
          On = a.ZP.div.withConfig({
            displayName: "Character__VTuberBase",
            componentId: "sc-tiy317-1"
          })(["--skew:400px;position:relative;width:390px;clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );&::before{--width:780;--height:2055;content:'';display:block;padding-top:calc(var(--height) / var(--width) * 100%);}", "{width:280px;}"], s.BC.smallDown),
          Yn = (0, a.ZP)(On).withConfig({
            displayName: "Character__VTuberBg",
            componentId: "sc-tiy317-2"
          })(["position:absolute;top:0;left:0;background:url('/images/about/vtuber-bg.png') center / contain no-repeat;animation:maskIn 0.7s ease-in-out 0s forwards;"]),
          Fn = (0, a.ZP)(On).withConfig({
            displayName: "Character__VTuber",
            componentId: "sc-tiy317-3"
          })(["background:url('/images/about/vtuber.png') center / contain no-repeat;animation:maskIn 0.7s ease-in-out 0.1s forwards;"]),
          En = a.ZP.div.withConfig({
            displayName: "Character__SD",
            componentId: "sc-tiy317-4"
          })(["--size:250px;box-sizing:border-box;position:absolute;top:45%;right:calc(var(--size) * 0.2 * -1);width:var(--size);height:var(--size);border-radius:50%;border:solid 2px var(--color-black);background:var(--color-white) url('/images/about/vtuber-sd.png') center / auto 90% no-repeat;transform:translateX(6%);opacity:0;animation:0.5s cubic-bezier(0,0,0.2,1) 0.8s forwards;animation-name:slideIn,fadeIn;", "{--size:200px;}"], s.BC.smallDown);
  
        function Kn() {
          var n = (0, M.YD)({
              rootMargin: "0px",
              triggerOnce: !0
            }),
            i = (0, r.Z)(n, 2),
            t = i[0],
            e = i[1];
          return (0, d.jsxs)(Un, {
            ref: t,
            children: [(0, d.jsx)(qn, {
              isIntersected: e,
              children: (0, d.jsx)("span", {
                children: "CreaterVTuber."
              })
            }), (0, d.jsx)(Vn, {
              isIntersected: e,
              children: (0, d.jsx)("span", {
                children: "ましろ"
              })
            }), (0, d.jsx)(Rn, {
              isIntersected: e,
              children: (0, d.jsx)("span", {
                children: "Mashiro"
              })
            })]
          })
        }
        var Un = a.ZP.div.withConfig({
            displayName: "Name__Wrapper",
            componentId: "sc-1v78cr4-0"
          })(["display:grid;grid-row-gap:12px;justify-items:start;width:max-content;", "{grid-row-gap:8px;}"], s.BC.smallDown),
          qn = a.ZP.div.withConfig({
            displayName: "Name__Job",
            componentId: "sc-1v78cr4-1"
          })(["--skew:0px;position:relative;background:var(--color-white);clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );", " span{display:block;position:relative;font-size:22px;font-weight:bold;line-height:1;padding:0 12px;color:var(--color-black);", "{font-size:18px;padding:0 8px;}}"], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:maskIn 0.7s cubic-bezier(0,0,0.24,1.01) 0s forwards;"])
          }), s.BC.smallDown),
          Vn = a.ZP.div.withConfig({
            displayName: "Name__FullName",
            componentId: "sc-1v78cr4-2"
          })(["--skew:0px;position:relative;clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );", " &::before{content:'';display:block;position:absolute;top:0;left:0;width:100%;height:100%;background:var(--color-pink);}span{display:block;position:relative;font-size:60px;font-weight:bold;line-height:1.4;color:var(--color-white);padding:0 24px;z-index:1;", "{font-size:48px;}}"], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:maskIn 0.7s cubic-bezier(0,0,0.24,1.01) 0.1s forwards;"])
          }), s.BC.smallDown),
          Rn = a.ZP.div.withConfig({
            displayName: "Name__NameEn",
            componentId: "sc-1v78cr4-3"
          })(["--skew:0px;position:relative;clip-path:polygon( 0 0,0 0,calc(var(--skew) * -1) 100%,calc(var(--skew) * -1) 100% );", " &::before{content:'';display:block;box-sizing:border-box;position:absolute;top:0;left:0;width:100%;height:100%;background:var(--color-white);border:solid 1px #ddd;}span{display:block;position:relative;font-size:22px;font-weight:bold;letter-spacing:0.1em;line-height:1;color:var(--color-black);padding:6px 20px;z-index:1;", "{font-size:18px;padding:4px 14px;}}"], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:maskIn 0.7s cubic-bezier(0,0,0.24,1.01) 0.2s forwards;"])
          }), s.BC.smallDown);
  
        function Xn(n) {
          var i = n.about,
            t = i.profile,
            e = i.history,
            o = i.masterpiece,
            a = (0, M.YD)({
              rootMargin: "0px",
              triggerOnce: !0
            }),
            s = (0, r.Z)(a, 2),
            c = s[0],
            l = s[1],
            p = (0, M.YD)({
              rootMargin: "0px",
              triggerOnce: !0
            }),
            m = (0, r.Z)(p, 2),
            g = m[0],
            h = m[1],
            f = (0, M.YD)({
              rootMargin: "0px",
              triggerOnce: !0
            }),
            u = (0, r.Z)(f, 2),
            x = u[0],
            w = u[1];
          return (0, d.jsxs)(Gn, {
            children: [(0, d.jsx)(Hn, {
              ref: c,
              isIntersected: l,
              children: (0, d.jsx)(Jn, {
                children: t
              })
            }), (0, d.jsx)(Hn, {
              ref: g,
              isIntersected: h,
              children: (0, d.jsx)(Jn, {
                children: e
              })
            }), (0, d.jsxs)(Hn, {
              ref: x,
              isIntersected: w,
              children: [(0, d.jsx)(Qn, {
                children: "代表作"
              }), (0, d.jsx)(Jn, {
                children: o
              })]
            })]
          })
        }
        var Gn = a.ZP.div.withConfig({
            displayName: "Text__Wrapper",
            componentId: "sc-430h82-0"
          })(["display:grid;grid-row-gap:32px;justify-items:start;color:var(--color-black);", "{padding:0 24px;}"], s.BC.smallDown),
          Hn = a.ZP.div.withConfig({
            displayName: "Text__DescriptionWrapper",
            componentId: "sc-430h82-1"
          })(["display:grid;grid-row-gap:8px;justify-items:start;opacity:0;transform:translateY(12px);", ""], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.45s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          })),
          Jn = a.ZP.div.withConfig({
            displayName: "Text__Description",
            componentId: "sc-430h82-2"
          })(["font-size:18px;line-height:40px;letter-spacing:0.08em;white-space:pre-line;", "{font-size:14px;line-height:32px;}"], s.BC.smallDown),
          Qn = a.ZP.div.withConfig({
            displayName: "Text__Label",
            componentId: "sc-430h82-3"
          })(["box-sizing:border-box;font-size:18px;line-height:30px;padding:0 16px;border:solid 1px #ddd;background:var(--color-white);", "{font-size:14px;line-height:26px;}"], s.BC.smallDown);
  
        function $n() {
          var n = (0, M.YD)({
              rootMargin: "0px",
              triggerOnce: !0
            }),
            i = (0, r.Z)(n, 2),
            t = i[0],
            e = i[1];
          return (0, d.jsxs)(ni, {
            ref: t,
            children: [(0, d.jsx)(ii, {
              isIntersected: e
            }), (0, d.jsx)(ti, {
              isIntersected: e,
              href: "https://twitter.com/mashiro6425",
              target: "_blank",
              rel: "noopener noreferrer"
            })]
          })
        }
        var ni = a.ZP.div.withConfig({
            displayName: "Icon__Wrapper",
            componentId: "sc-146d2sg-0"
          })(["--icon-size:200px;position:relative;width:var(--icon-size);height:var(--icon-size);", "{--icon-size:180px;}"], s.BC.smallDown),
          ii = a.ZP.div.withConfig({
            displayName: "Icon__IconImg",
            componentId: "sc-146d2sg-1"
          })(["position:absolute;top:0;left:0;width:100%;height:100%;border-radius:50%;background:url('/images/about/icon.jpg') center / contain no-repeat;opacity:0;transform:translateY(12px);", ""], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.45s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          })),
          ti = a.ZP.a.withConfig({
            displayName: "Icon__Twitter",
            componentId: "sc-146d2sg-2"
          })(["display:block;position:absolute;bottom:0;right:0;width:28%;height:28%;background:url('/images/menu/twitter.svg') center / contain no-repeat;border-radius:50%;cursor:pointer;opacity:0;transform:translateY(12px);", ""], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.45s ease-out 0.15s forwards;animation-name:slideIn,fadeIn;"])
          }));
  
        function ei(n) {
          var i = n.about;
          return (0, d.jsxs)(oi, {
            children: [(0, d.jsx)($n, {}), (0, d.jsxs)(ai, {
              children: [(0, d.jsx)(Kn, {}), (0, d.jsx)(Xn, {
                about: i
              })]
            })]
          })
        }
        var oi = a.ZP.div.withConfig({
            displayName: "Profile__Wrapper",
            componentId: "sc-3292jv-0"
          })(["display:grid;", "{grid-column-gap:70px;grid-template-columns:repeat(2,auto);justify-content:center;max-width:900px;width:calc(100% - 48px);align-items:start;}", "{grid-row-gap:32px;justify-items:center;}"], s.BC.mediumUp, s.BC.smallDown),
          ai = a.ZP.div.withConfig({
            displayName: "Profile__TextWrapper",
            componentId: "sc-3292jv-1"
          })(["display:grid;grid-row-gap:32px;justify-items:start;", "{justify-items:center;}"], s.BC.smallDown);
  
        function ri(n) {
          var i = n.about,
            t = (0, e.useState)(!1),
            o = t[0],
            a = t[1];
          return (0, d.jsxs)(si, {
            id: "about",
            children: [(0, d.jsx)(Cn.Z, {}), (0, d.jsxs)(ci, {
              children: [(0, d.jsx)(_n.Gm, {}), (0, d.jsx)(zn, {
                status: o,
                onClick: a
              }), o ? (0, d.jsx)(Tn, {}) : (0, d.jsx)(ei, {
                about: i
              }), (0, d.jsx)(li, {
                children: Zn.map((function (n) {
                  var i = n.serviceName,
                    t = n.url,
                    e = n.imageUrl;
                  return (0, d.jsx)(Nn.Z, {
                    serviceName: i,
                    url: t,
                    imageUrl: e
                  }, i)
                }))
              })]
            }), (0, d.jsx)(Cn.Z, {
              reverse: !0
            })]
          })
        }
        var si = a.ZP.div.withConfig({
            displayName: "About__Wrapper",
            componentId: "sc-p7tifz-0"
          })(["background:repeating-linear-gradient( -45deg,#eee,#eee 1px,transparent 0,transparent 14px ) top left;"]),
          ci = a.ZP.div.withConfig({
            displayName: "About__Inner",
            componentId: "sc-p7tifz-1"
          })(["display:grid;grid-row-gap:80px;justify-items:center;padding:160px 0;color:var(--color-black);", "{grid-row-gap:48px;padding:64px 0;}"], s.BC.smallDown),
          li = a.ZP.div.withConfig({
            displayName: "About__Links",
            componentId: "sc-p7tifz-2"
          })(["display:grid;", "{grid-gap:24px;grid-template-columns:repeat(2,1fr);max-width:820px;width:calc(100% - 48px);}", "{grid-gap:16px;max-width:320px;width:calc(100% - 32px);}"], s.BC.mediumUp, s.BC.smallDown),
          di = t(5406),
          pi = "draw-text__mask-id";
  
        function mi() {
          var n = (0, M.YD)({
              rootMargin: "0px 0px -100px",
              triggerOnce: !0
            }),
            i = (0, r.Z)(n, 2),
            t = i[0],
            e = i[1];
          return (0, d.jsxs)(gi, {
            ref: t,
            children: [(0, d.jsxs)("mask", {
              id: pi,
              children: [(0, d.jsx)(fi, {
                emit: e
              }), (0, d.jsx)(ui, {})]
            }), (0, d.jsx)(xi, {
              mask: "url(#".concat(pi, ")")
            })]
          })
        }
        var gi = a.ZP.svg.attrs({
            viewBox: "0 0 873.4 132.3"
          }).withConfig({
            displayName: "DrawText__Svg",
            componentId: "sc-l8mnz4-0"
          })(["position:absolute;bottom:0;right:0;width:64%;", "{width:78%;}"], s.BC.smallDown),
          hi = a.ZP.polyline.withConfig({
            displayName: "DrawText__Polyline",
            componentId: "sc-l8mnz4-1"
          })(["fill:none;stroke:#fff;stroke-width:8;stroke-linecap:round;stroke-linejoin:round;opacity:0;"]),
          fi = (0, a.ZP)(hi).attrs({
            points: "4.1,110.9 22.4,100.6 36.1,93.5 45,89 53.9,84.7 60.7,81.9 67.4,79.7 71.3,78.8 75.2,78.6\n  77.3,79.3 78.8,80.6 78.5,83.6 77.9,87.7 76.7,93.6 73.7,102.5 68.3,115.1 60.5,128.3 73,118.4 85.2,109.5 96.4,100.7 109.7,92.1\n  115.8,88.8 118.8,87.9 120.9,87.9 122.5,88.7 123.1,90.5 123.5,93.7 123.2,97.4 121,104.7 110.5,122.9 121.4,114.2 129.2,106.5\n  139.9,98.1 146.8,92.7 152.2,89.6 159,86.1 162.7,84.7 164.3,84.7 166.1,85.4 165.9,87.3 164.4,91.7 161.3,99 157.5,106.4\n  154.8,112 153.4,116 153.3,118.5 154,121.1 156.5,122.3 160.5,122.7 166.3,122.4 175.2,121 188.7,117.2 207.2,111.1 227,103.2\n  245.5,93.1 256.4,86 271.6,72 263.4,83.2 260.2,90 258.8,94.8 259.1,98.9 260,102.3 261.7,104.8 264.9,106.3 269.3,105.7\n  278,102.5 284.1,98.4 293.6,90.4 300.2,83.1 304.6,76.9 307.4,71.3 309,66.9 309.3,62.2 308.2,57.8 305.7,54.6 302.2,52.4\n  298.1,52.5 293.8,53.7 290.5,56.4 286.7,60.3 284.2,66 282.9,72.4 282.6,77.8 283.2,82.8 284.6,87.1 289.4,95.3 294.1,98.5\n  299.5,100.5 305.7,101.4 312.3,101.3 323.6,98.1 333.1,93.3 345.1,85.8 361.6,73.7 375.8,63.1 386.5,56.7 397,50.4 402.3,48\n  406.9,47 409.7,46.7 411.8,47.1 412.6,48.4 412.2,51 409.7,57.9 402.8,69.9 397.4,78 394.2,85.2 392.3,90.4 391.7,93.4\n  391.9,94.7 392.9,97.2 394.8,98 397.2,98.2 401.5,96.7 412.4,89.8 423.9,80.1 435.7,68.3 445.1,56 448.8,48.5 450.1,42.9\n  450.2,38.3 449.1,34.7 447.2,31.7 445,31 442.6,31.1 440.3,32.2 437.9,35.8 436.1,40.1 435.3,45.3 435.3,49.8 436.5,54.3\n  438.4,57.9 446.4,65.9 452.4,69.3 459.4,71.9 466.8,73.4 479.8,74.5 494.9,74.3 506.1,73.5 520.3,70.4 535.7,66.6 547.2,61.6\n  560.8,54 570.6,48.7 580.5,43.3 586.2,39 596.7,31 588.4,41.6 586.3,46 584.6,50.4 584.1,54.9 584.1,59 585,62.2 587.3,65.3\n  590.3,68.9 594.3,71 600.1,72.9 605.2,73.7 611.3,74.3 618.4,74.4 626.2,73.8 636.4,72.8 646.5,71.9 658.7,68.9 672.9,64.7\n  686.1,60.7 704.2,52.8 719.5,45.9 728.2,40.5 734,36.3 736.9,40.8 740.5,44.5 745.4,46 752.8,44.8 763.4,39.1 768.5,34\n  772.6,28.8 774.2,23.4 774.1,17.5 772.3,12.7 768.1,10 762.7,9.3 756.1,10.6 748.6,13.6 742.9,18.3 738.4,23.1 736.6,27.7\n  735.2,32.5 734.5,43.2 738.7,50.8 743.9,56.2 749.3,60.3 757.1,63.3 764,64.3 773.2,64.3 781.1,63 792.7,60.4 809.3,55.5\n  823.3,49 839.3,42 859.4,29.8 869.4,24"
          }).withConfig({
            displayName: "DrawText__Mask1",
            componentId: "sc-l8mnz4-2"
          })(["--stroke-length:4378;", ""], (function (n) {
            return n.emit && (0, a.iv)(["animation:fadeIn 0.05s ease-out 0s forwards,drawLine 1.4s ease-in-out 0s forwards;"])
          })),
          ui = (0, a.ZP)(hi).attrs({
            points: "628.2,4 617.8,12.3"
          }).withConfig({
            displayName: "DrawText__Mask2",
            componentId: "sc-l8mnz4-3"
          })([""]),
          xi = a.ZP.path.attrs({
            d: "M259.6,89.1c-38.4,23.5-75.1,34.9-94.8,37c-16.7,1.7-18.9-6.2-8.3-25.3c3.5-6.1,5-9.5,5.6-11.7\n  c-5.6,1.4-14.5,6.9-23.8,14.7L112.8,126l-2.2-2.8l-2.7-2.2c8.5-10.4,14.9-29.8,11.1-29.5c-5.1,0.5-16.8,8.8-56.4,39.6l-2.2-2.8\n  l-2.4-2.5l0.2-0.3c2.2-2.7,7.8-13.9,11.5-23.5c3.5-8.5,5.6-16.6,5.3-19.9c0,0-0.6-0.2-2.5,0c-13.2,1.4-47.4,21.2-69.3,33.3L0,109.2\n  c21.9-12.1,57.1-32.5,71.9-34.1c5.7-0.6,9.7,1.5,10.2,6.3c0.5,4.8-1.6,13.5-5.5,23.2c-1.3,3.4-2.8,6.8-4.4,10.3\n  c30-23.2,40-29.7,46.2-30.3c9.7-1,10.5,9.8,5.8,22.3l9.7-8.6c11.5-9.6,22.6-15.9,29.6-16.7c3-0.3,5.6,1.6,5.9,4.3\n  c0.5,5.1-3.4,12.3-6.8,18.4c-8,14.4-7.9,15.8,1.5,14.8c19.1-2,54.2-12.7,91.8-36.1L259.6,89.1z M561,58.1\n  c-16.2,9.6-33.2,15.7-52.3,18.2c-36.5,4.8-57-1.2-67-10.5c-15.7,19.3-41,37.7-48.1,35.4c-9.9-3.6-5.1-15,6.3-33.9\n  c9.1-15.1,10.2-17.1,8.1-16.9c-10.5,1.1-26.7,13.1-43.9,25.7c-17.7,13-36.4,26.9-49.9,28.3c-10.8,1.1-19.4-0.7-25.3-5.3\n  c-7.3,5.7-15.1,9.7-21.8,10.4c-6.5,0.7-10.9-3-11.6-9.7c-1-9.7,6-23.8,15.5-31.8l4.4,5.5c-7.8,6.5-13.7,18-12.9,25.6\n  c0.3,3,1.2,3.7,3.9,3.4c5.1-0.5,11.6-3.9,17.6-8.6c-2.5-3.6-4.1-8-4.6-13.4c-1.6-15.9,6.4-30.1,18.3-31.3\n  c8.1-0.8,14.3,4.5,15.2,12.9c0.9,8.9-7.5,22-18.5,32.4c4.6,2.8,11,3.8,19.1,2.9c11.6-1.2,28.8-13.8,46.5-26.8\n  c17.2-12.9,34.9-25.9,47.3-27.2c12.9-1.3,9.4,9.6-1.5,27.4c-10,17.1-11.8,23-9.6,23.6c4.5,1.7,28-16,41.5-33.1\n  c-14-19.8,6.3-47.5,15.2-27.2c3.2,7.3-0.7,17.2-8.1,27.2c9.2,7.8,27.7,12.7,62.9,8c18.3-2.4,34.6-8.2,49.7-17.4L561,58.1z\n   M289.5,89.3c9.5-8.6,17-19.7,16.3-26.5c-0.5-4.6-3.2-7-7.5-6.6c-7.5,0.8-13.3,11.2-12,23.6C286.8,83.6,287.9,86.7,289.5,89.3z\n   M440.8,56.9c5.2-7.6,7.7-14.7,5.6-19.9C442.8,28.6,432.2,44.1,440.8,56.9z M736.4,40.1c-26.4,16.3-70.1,33.6-102.9,37\n  c-33.4,3.4-51.2-2.6-52.9-19.1c-0.4-4,0.2-8.2,2-12.5c-5.9,3.6-13.1,7.6-21.8,12.6l-3.4-6.2c21.9-12.1,33.4-19.5,37.6-23.7l0.2-0.3\n  l5.1,4.9c-10.1,10.6-13.5,17.2-12.7,24.4c1.2,11.3,15.5,15.8,45.1,12.8c31.8-3.3,73.9-19.8,99.7-36.1L736.4,40.1z M618.3,16.6\n  l-4.4-5.5L627,0.4l4.4,5.5L618.3,16.6z M872.5,26.1C842.1,45.6,794,70.9,762.4,67.7c-20.4-2-30.5-15.7-31.1-29.7l-0.1-0.5\n  c-0.1-14.4,10.1-29.1,30.8-31.2c8.1-0.8,14.8,3.9,15.6,11.2c2,19.7-21.5,34.1-35.7,31.4c3.9,6.1,10.9,10.9,21.1,11.7\n  c29.4,3,75.4-21.1,105.8-40.5L872.5,26.1z M738.3,37.2c2.1,12.3,34.3,0,32.3-19.1c-0.3-3.2-3.5-5.4-7.9-4.9\n  C746.3,14.9,738.2,26,738.3,37.2z"
          }).withConfig({
            displayName: "DrawText__Text",
            componentId: "sc-l8mnz4-4"
          })(["fill:var(--color-yellow);"]),
          wi = "0S1NT5FNrxM";
  
        function vi() {
          var n = (0, e.useCallback)((function (n) {
            n.target.mute(), n.target.playVideo()
          }), []);
          return (0, d.jsxs)(yi, {
            href: "https://www.youtube.com/watch?v=".concat(wi),
            target: "_blank",
            children: [(0, d.jsx)(Ci, {
              children: (0, d.jsx)(di.Z, {
                videoId: wi,
                opts: {
                  height: "100%",
                  width: "100%",
                  playerVars: {
                    mute: 1,
                    autoplay: 1,
                    controls: 0,
                    disablekb: 1,
                    playsinline: 1,
                    rel: 0,
                    playlist: wi,
                    loop: 1
                  }
                },
                onReady: n
              })
            }), (0, d.jsx)(mi, {}), (0, d.jsx)(bi, {})]
          })
        }
        var bi = a.ZP.div.withConfig({
            displayName: "Movie__PlayButton",
            componentId: "sc-1gx03gr-0"
          })(["--icon-size:140px;position:absolute;top:calc(50% - var(--icon-size) / 2);left:calc(50% - var(--icon-size) / 2);width:var(--icon-size);height:var(--icon-size);background:center / contain no-repeat;background-image:url('data:image/svg+xml;charset=utf8,%3Csvg%20version%3D%221.1%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20xmlns%3Axlink%3D%22http%3A%2F%2Fwww.w3.org%2F1999%2Fxlink%22%20x%3D%220px%22%20y%3D%220px%22%20width%3D%22200px%22%0A%09%20height%3D%22200px%22%20viewBox%3D%220%200%20200%20200%22%20style%3D%22overflow%3Avisible%3Benable-background%3Anew%200%200%20200%20200%3B%22%20xml%3Aspace%3D%22preserve%22%3E%0A%3Cstyle%20type%3D%22text%2Fcss%22%3E%0A%09.st0%7Bfill%3A%23FFFFFF%3B%7D%0A%3C%2Fstyle%3E%0A%3Cdefs%3E%0A%3C%2Fdefs%3E%0A%3Cpath%20class%3D%22st0%22%20d%3D%22M100%2C0C44.8%2C0%2C0%2C44.8%2C0%2C100s44.8%2C100%2C100%2C100s100-44.8%2C100-100S155.2%2C0%2C100%2C0z%20M130%2C113.4L91%2C136%0A%09c-8%2C4.6-18-1.2-18-10.4v-45c0-9.2%2C10-15%2C18-10.4l39%2C22.5C138%2C97.3%2C138%2C108.8%2C130%2C113.4z%22%2F%3E%0A%3C%2Fsvg%3E%0A');transition:0.16s ease-out 0s;transition-property:transform,opacity;z-index:1;", "{--icon-size:70px;}"], s.BC.smallDown),
          yi = a.ZP.a.withConfig({
            displayName: "Movie__Wrapper",
            componentId: "sc-1gx03gr-1"
          })(["display:block;position:relative;width:100%;overflow:hidden;&::before,&::after{content:'';display:block;}&::before{--width:1440;--height:520;padding-top:calc(var(--height) / var(--width) * 100%);", "{--height:680;}}&::after{position:absolute;top:0;left:0;width:100%;height:100%;background-image:radial-gradient( rgba(255,255,255,0.35) 10%,transparent 10% ),radial-gradient(rgba(255,255,255,0.35) 10%,transparent 10%);background-size:20px 20px;background-position:0 0,10px 10px;", "{background-size:16px 16px;background-position:0 0,8px 8px;}}& iframe{position:absolute;top:0;left:0;width:100%;height:100%;}&:hover ", "{transform:scale(1.2);opacity:0.7;}"], s.BC.smallDown, s.BC.smallDown, bi),
          Ci = a.ZP.div.withConfig({
            displayName: "Movie__Inner",
            componentId: "sc-1gx03gr-2"
          })(["position:absolute;top:50%;left:50%;width:100%;transform:translate(-50%,-50%);&::before,&::after{content:'';display:block;}&::before{--width:1280;--height:720;padding-top:calc(var(--height) / var(--width) * 100%);}&::after{position:absolute;top:0;left:0;width:100%;height:100%;background:var(--color-pink);opacity:0.6;}"]);
  
        function _i(n) {
          var i = n.className;
          return (0, d.jsx)(ki, {
            className: i,
            children: (0, d.jsx)(ji, {})
          })
        }
        var ki = a.ZP.svg.attrs({
            viewBox: "0 0 9.5 15.4"
          }).withConfig({
            displayName: "ArrowIcon__Svg",
            componentId: "sc-ufj53r-0"
          })([""]),
          ji = a.ZP.path.attrs({
            d: "M2,15.4c-0.6,0-1.1-0.2-1.5-0.7c-0.7-0.8-0.5-2.1,0.3-2.8l3.7-3.3c0.5-0.4,0.5-1.2,0-1.6L0.7,3.5\n\tc-0.8-0.7-1-1.9-0.3-2.7c0.7-1,2.1-1.1,2.9-0.3l5.1,4.6c0.7,0.7,1.1,1.6,1.1,2.6c0,1-0.4,1.9-1.2,2.6l-5,4.5\n\tC2.9,15.2,2.4,15.4,2,15.4z M5.7,8L5.7,8L5.7,8z"
          }).withConfig({
            displayName: "ArrowIcon__Path",
            componentId: "sc-ufj53r-1"
          })(["fill:currentColor;"]);
  
        function Ii(n) {
          var i = n.className,
            t = n.children;
          return (0, d.jsxs)(Di, {
            className: i,
            children: [(0, d.jsx)(Ni, {
              children: t
            }), (0, d.jsx)(Zi, {
              children: (0, d.jsx)(zi, {})
            })]
          })
        }
        var Ni = a.ZP.div.withConfig({
            displayName: "Button__Text",
            componentId: "sc-1wtijey-0"
          })(["font-size:18px;font-weight:bold;letter-spacing:0.1em;"]),
          Zi = a.ZP.div.withConfig({
            displayName: "Button__IconWrapper",
            componentId: "sc-1wtijey-1"
          })([""]),
          zi = (0, a.ZP)(_i).withConfig({
            displayName: "Button__StyledArrowIcon",
            componentId: "sc-1wtijey-2"
          })(["height:10px;"]),
          Pi = (0, a.F4)(["from,50%,to{transform:translateX(0);}25%{transform:translateX(4px);}"]),
          Di = a.ZP.div.withConfig({
            displayName: "Button__Wrapper",
            componentId: "sc-1wtijey-3"
          })(["box-sizing:border-box;position:relative;display:inline-grid;grid-column-gap:12px;grid-template-columns:repeat(2,auto);align-items:center;color:var(--color-black);height:46px;padding-left:40px;padding-right:36px;border:solid 3px currentColor;border-radius:0 10px 0 10px;cursor:pointer;user-select:none;transition:transform 0.16s ease-out 0s;", "{&:hover{transform:translateX(3px);", "{animation:", " 1.4s ease-in-out 0.16s infinite;}}}&::before,&::after{--icon-size:11px;content:'';display:block;box-sizing:border-box;position:absolute;width:var(--icon-size);height:var(--icon-size);border:solid 3px currentColor;}&::before{top:0;left:0;transform:translate(-100%,-100%);border-radius:50% 50% 0 50%;}&::after{bottom:0;right:0;transform:translate(100%,100%);border-radius:0 50% 50% 50%;}"], s.BC.mediumUp, zi, Pi),
          Bi = t(827),
          Mi = t(8436);
  
        function Li(n) {
          var i = n.works,
            t = n.onClick;
          return (0, d.jsx)(Ti, {
            id: "works",
            children: (0, d.jsxs)(Wi, {
              children: [(0, d.jsx)(_n.n8, {}), (0, d.jsx)(Oi, {
                className: "works__masonry",
                columnClassName: "works__masonry-column",
                children: i.sort((function (n, i) {
                  return l()(i.publishedAt).unix() - l()(n.publishedAt).unix()
                })).slice(0, Math.max(0, i.length - 1)).map((function (n, i) {
                  return (0, d.jsx)(Ai, {
                    work: n,
                    workIndex: i + 1,
                    onClick: t
                  }, "".concat(i, "-").concat(n.image.title))
                }))
              }), (0, d.jsx)("a", {
                href: "/works/",
                target: "_blank",
                children: (0, d.jsx)(Ii, {
                  children: "MORE"
                })
              }), (0, d.jsx)(vi, {})]
            })
          })
        }
  
        function Ai(n) {
          var i = n.work,
            t = n.onClick,
            e = n.workIndex,
            o = (0, M.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            a = (0, r.Z)(o, 2),
            s = a[0],
            c = a[1];
          return (0, d.jsx)(Si, {
            ref: s,
            isIntersected: c,
            onClick: function (n) {
              function i() {
                return n.apply(this, arguments)
              }
              return i.toString = function () {
                return n.toString()
              }, i
            }((function () {
              return t(Object.assign({}, i, {
                worksAnchor: "work-".concat(e)
              }))
            })),
            children: (0, d.jsx)(Yi, {
              image: i.image
            })
          })
        }
        var Si = a.ZP.div.withConfig({
            displayName: "Works__ImageWrapper",
            componentId: "sc-7ep0hw-0"
          })(["opacity:0;transform:translateY(12px);", ""], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          })),
          Ti = a.ZP.div.withConfig({
            displayName: "Works__Wrapper",
            componentId: "sc-7ep0hw-1"
          })([""]),
          Wi = a.ZP.div.withConfig({
            displayName: "Works__Inner",
            componentId: "sc-7ep0hw-2"
          })(["display:grid;grid-row-gap:80px;justify-items:center;padding:160px 0;", "{grid-row-gap:48px;padding:64px 0;}"], s.BC.smallDown),
          Oi = (0, a.ZP)(Bi.Z).attrs({
            breakpointCols: {
              default: 3,
              767: 2,
              560: 1
            }
          }).withConfig({
            displayName: "Works__StyledMasonry",
            componentId: "sc-7ep0hw-3"
          })(["display:flex;max-width:1080px;width:calc(100% - 16px);margin-left:-16px;.works__masonry-column{padding-left:16px;background-clip:padding-box;}"]),
          Yi = (0, a.ZP)(Mi.Z).withConfig({
            displayName: "Works__StyledLazyImage",
            componentId: "sc-7ep0hw-4"
          })(["margin-bottom:16px;"]),
          Fi = t(4942),
          Ei = t(6066);
        t(3043), t(6326);
  
        function Ki(n, i) {
          var t = Object.keys(n);
          if (Object.getOwnPropertySymbols) {
            var e = Object.getOwnPropertySymbols(n);
            i && (e = e.filter((function (i) {
              return Object.getOwnPropertyDescriptor(n, i).enumerable
            }))), t.push.apply(t, e)
          }
          return t
        }
  
        function Ui(n) {
          for (var i = 1; i < arguments.length; i++) {
            var t = null != arguments[i] ? arguments[i] : {};
            i % 2 ? Ki(Object(t), !0).forEach((function (i) {
              (0, Fi.Z)(n, i, t[i])
            })) : Object.getOwnPropertyDescriptors ? Object.defineProperties(n, Object.getOwnPropertyDescriptors(t)) : Ki(Object(t)).forEach((function (i) {
              Object.defineProperty(n, i, Object.getOwnPropertyDescriptor(t, i))
            }))
          }
          return n
        }
  
        function qi(n) {
          var i = n.banners;
          return (0, d.jsx)(Vi, {
            children: (0, d.jsx)(Ei.Z, Ui(Ui({}, {
              autoplay: !0,
              arrows: !1,
              centerMode: !0,
              variableWidth: !0,
              dots: !0,
              infinite: !0,
              speed: 600,
              easing: "easeOutExpo",
              responsive: [{
                breakpoint: 768,
                settings: {
                  variableWidth: !1
                }
              }]
            }), {}, {
              children: i.map((function (n) {
                var i = n.image,
                  t = n.title,
                  e = n.link;
                return (0, d.jsx)(Ri, {
                  style: {
                    width: 480
                  },
                  href: e,
                  target: "_blank",
                  children: (0, d.jsx)(Xi, {
                    src: i.thumbUrl,
                    title: t
                  })
                }, t)
              }))
            }))
          })
        }
        var Vi = a.ZP.div.withConfig({
            displayName: "Slider__Wrapper",
            componentId: "sc-e3sqqr-0"
          })(["padding:160px 0;", "{padding:64px 0;}"], s.BC.smallDown),
          Ri = a.ZP.a.withConfig({
            displayName: "Slider__Item",
            componentId: "sc-e3sqqr-1"
          })(["&:focus{outline:none;}"]),
          Xi = a.ZP.img.withConfig({
            displayName: "Slider__Image",
            componentId: "sc-e3sqqr-2"
          })(["width:100%;height:auto;"]);
  
        function Gi(n) {
          var i = n.banners;
          return (0, d.jsx)(Hi, {
            children: (0, d.jsx)(qi, {
              banners: i
            })
          })
        }
        var Hi = a.ZP.div.withConfig({
          displayName: "Banner__Wrapper",
          componentId: "sc-130zxj4-0"
        })([""]);
        var Ji = a.ZP.div.withConfig({
            displayName: "ShadowButton__Wrapper",
            componentId: "sc-kl8ptp-0"
          })(["display:inline-grid;grid-column-gap:16px;grid-template-columns:repeat(2,auto);justify-content:start;align-items:center;padding:0 36px;height:64px;background:var(--color-white);border-radius:64px;box-shadow:0 0 6px rgba(0,0,0,0.2);transition:0.2s ease-out 0s;transition-property:box-shadow;opacity:0;transform:translateY(12px);", "{grid-column-gap:8px;padding:0 28px;height:50px;border-radius:50px;}", " &:hover{box-shadow:0 0 16px rgba(0,0,0,0.2);}"], s.BC.smallDown, (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          })),
          Qi = a.ZP.div.withConfig({
            displayName: "ShadowButton__Text",
            componentId: "sc-kl8ptp-1"
          })(["font-size:20px;font-weight:bold;line-height:28px;color:var(--color-black);", "{font-size:14px;line-height:22px;}"], s.BC.smallDown),
          $i = (0, a.ZP)(_i).withConfig({
            displayName: "ShadowButton__StyledArrowIcon",
            componentId: "sc-kl8ptp-2"
          })(["color:var(--color-black);width:7px;", "{width:5px;}"], s.BC.smallDown);
  
        function nt() {
          var n = (0, M.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            i = (0, r.Z)(n, 2),
            t = i[0],
            e = i[1],
            o = (0, M.YD)({
              rootMargin: "0px 0px -200px",
              triggerOnce: !0
            }),
            a = (0, r.Z)(o, 2),
            s = a[0],
            c = a[1];
          return (0, d.jsxs)(it, {
            id: "contact",
            children: [(0, d.jsx)(Cn.Z, {}), (0, d.jsxs)(tt, {
              children: [(0, d.jsx)(_n.nL, {}), (0, d.jsxs)(et, {
                children: [(0, d.jsxs)(rt, {
                  ref: t,
                  isIntersected: e,
                  children: ["ご依頼いただく際は", (0, d.jsx)("br", {}), "・", (0, d.jsx)("span", {
                    children: "内容"
                  }), (0, d.jsx)("br", {}), "・", (0, d.jsx)("span", {
                    children: "ご希望金額"
                  }), (0, d.jsx)("br", {}), "・", (0, d.jsx)("span", {
                    children: "納期"
                  }), (0, d.jsx)("br", {}), "・", (0, d.jsx)("span", {
                    children: "期間"
                  }), (0, d.jsx)("br", {}), "を必ず書いていただけると幸いです。"]
                }), (0, d.jsxs)(ot, {
                  children: [(0, d.jsx)("a", {
                    href: "mailto:info@mashiromix.com",
                    target: "_blank",
                    children: (0, d.jsx)(at, {
                      children: "メールでお問い合わせ"
                    })
                  }), (0, d.jsx)("a", {
                    href: "https://docs.google.com/forms/d/e/1FAIpQLScc9Bq2UJ7mhG5iioud9P-Tg024ZhqAlbEYpZ8qcHzT_oJf6A/viewform?usp=sf_link",
                    target: "_blank",
                    children: (0, d.jsx)(at, {
                      children: "Googleフォームでお問い合わせ"
                    })
                  })]
                }), (0, d.jsxs)(st, {
                  ref: s,
                  isIntersected: c,
                  children: ["※なお、無償のご依頼は受け付けておりません。", (0, d.jsx)("br", {}), "あらかじめご了承ください。"]
                })]
              })]
            }), (0, d.jsx)(Cn.Z, {
              reverse: !0
            })]
          })
        }
        var it = a.ZP.div.withConfig({
            displayName: "Contact__Wrapper",
            componentId: "sc-f2005g-0"
          })(["background:repeating-linear-gradient( -45deg,#eee,#eee 1px,transparent 0,transparent 14px ) top left;"]),
          tt = a.ZP.div.withConfig({
            displayName: "Contact__Inner",
            componentId: "sc-f2005g-1"
          })(["display:grid;grid-row-gap:80px;justify-items:center;padding:160px 0;", "{grid-row-gap:48px;padding:64px 0;}"], s.BC.smallDown),
          et = a.ZP.div.withConfig({
            displayName: "Contact__Content",
            componentId: "sc-f2005g-2"
          })(["display:grid;grid-row-gap:64px;justify-items:center;", "{grid-row-gap:32px;}"], s.BC.smallDown),
          ot = a.ZP.div.withConfig({
            displayName: "Contact__ButtonWrapper",
            componentId: "sc-f2005g-3"
          })(["display:grid;grid-gap:24px;", "{grid-template-columns:repeat(2,auto);justify-content:center;align-items:center;}", "{justify-items:center;}"], s.BC.mediumUp, s.BC.smallDown),
          at = (0, a.ZP)((function (n) {
            var i = n.className,
              t = n.children,
              e = (0, M.YD)({
                rootMargin: "0px 0px -200px",
                triggerOnce: !0
              }),
              o = (0, r.Z)(e, 2),
              a = o[0],
              s = o[1];
            return (0, d.jsxs)(Ji, {
              className: i,
              ref: a,
              isIntersected: s,
              children: [(0, d.jsx)(Qi, {
                children: t
              }), (0, d.jsx)($i, {})]
            })
          })).withConfig({
            displayName: "Contact__StyledButton",
            componentId: "sc-f2005g-4"
          })([""]),
          rt = a.ZP.div.withConfig({
            displayName: "Contact__Text",
            componentId: "sc-f2005g-5"
          })(["font-size:20px;line-height:32px;color:var(--color-black);text-align:center;opacity:0;transform:translateY(12px);", " span{border-bottom:solid 3px var(--color-pink);}", "{font-size:16px;line-height:28px;}"], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          }), s.BC.smallDown),
          st = a.ZP.div.withConfig({
            displayName: "Contact__SmallText",
            componentId: "sc-f2005g-6"
          })(["font-size:16px;line-height:24px;color:var(--color-black);text-align:center;padding:0 24px;opacity:0;transform:translateY(12px);", " ", "{font-size:12px;line-height:22px;}"], (function (n) {
            return n.isIntersected && (0, a.iv)(["animation:0.5s ease-out 0s forwards;animation-name:slideIn,fadeIn;"])
          }), s.BC.smallDown);
  
        function ct(n) {
          var i = n.work,
            t = n.onClose;
          if (null === i) return null;
          var e = i.image,
            o = i.detail,
            a = i.worksAnchor ? "/works/#".concat(i.worksAnchor) : "/works/",
            r = i.publishedAt;
          return (0, d.jsxs)(dt, {
            children: [(0, d.jsx)(lt, {}), (0, d.jsx)(pt, {
              onClick: t
            }), (0, d.jsxs)(mt, {
              children: [(0, d.jsx)("a", {
                href: e.url,
                target: "_blank",
                children: (0, d.jsx)(gt, {
                  originalSize: !0,
                  image: e
                })
              }), (0, d.jsx)(ht, {
                children: r
              }), o && (0, d.jsx)(ft, {
                children: o
              }), a && (0, d.jsx)(ut, {
                href: a,
                target: "_blank",
                children: "詳細"
              })]
            })]
          })
        }
        var lt = (0, a.vJ)(["body{height:100% !important;overflow:hidden !important;}"]),
          dt = a.ZP.div.withConfig({
            displayName: "Modal__Wrapper",
            componentId: "sc-roz4s4-0"
          })(["display:grid;justify-items:center;align-items:center;position:fixed;top:0;left:0;width:100%;height:100%;overflow:auto;-webkit-overflow-scrolling:touch;z-index:11;opacity:0;animation:fadeIn 0.4s ease-out 0s forwards;"]),
          pt = a.ZP.div.withConfig({
            displayName: "Modal__Bg",
            componentId: "sc-roz4s4-1"
          })(["position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.5);"]),
          mt = a.ZP.div.withConfig({
            displayName: "Modal__Content",
            componentId: "sc-roz4s4-2"
          })(["box-sizing:border-box;position:relative;display:grid;width:calc(100% - 32px);max-width:980px;padding:16px 16px 64px;background:var(--color-white);margin:48px 0;", "{width:calc(100% - 16px);padding:8px 8px 32px;}"], s.BC.smallDown),
          gt = (0, a.ZP)(Mi.Z).withConfig({
            displayName: "Modal__StyledLazyImage",
            componentId: "sc-roz4s4-3"
          })(["border-radius:0;"]),
          ht = a.ZP.div.withConfig({
            displayName: "Modal__PublishDate",
            componentId: "sc-roz4s4-4"
          })(["font-size:14px;line-height:2.2;color:#666;margin-top:8px;", "{font-size:12px;margin-top:4px;}"], s.BC.smallDown),
          ft = a.ZP.div.withConfig({
            displayName: "Modal__Description",
            componentId: "sc-roz4s4-5"
          })(["font-size:18px;line-height:2.2;color:var(--color-black);margin-top:8px;", "{font-size:14px;margin-top:2px;}"], s.BC.smallDown),
          ut = a.ZP.a.withConfig({
            displayName: "Modal__Link",
            componentId: "sc-roz4s4-6"
          })(["justify-self:center;font-size:18px;line-height:40px;letter-spacing:0.1em;color:var(--color-black);padding:0 64px;border-radius:32px;background-color:#eee;margin-top:24px;"]),
          xt = t(8944),
          wt = "loading-icon__mask-id";
        var vt = a.ZP.svg.attrs({
            viewBox: "0 0 73 69.2"
          }).withConfig({
            displayName: "LoadingIcon__Svg",
            componentId: "sc-1cvog04-0"
          })(["width:100px;", " ", "{width:80px;}"], (function (n) {
            return !n.isLoading && (0, a.iv)(["& > *{animation:none !important;}"])
          }), s.BC.smallDown),
          bt = (0, a.F4)(["from,40%,to{transform:translateY(0);opacity:1;}20%{transform:translateY(-3px);opacity:0.4;}"]),
          yt = a.ZP.path.withConfig({
            displayName: "LoadingIcon__Word",
            componentId: "sc-1cvog04-1"
          })(["fill:var(--color-black);animation:", " 1.4s ease-out infinite;&:nth-child(1){animation-delay:0s;}&:nth-child(2){animation-delay:0.1s;}&:nth-child(3){animation-delay:0.2s;}&:nth-child(4){animation-delay:0.3s;}&:nth-child(5){animation-delay:0.4s;}&:nth-child(6){animation-delay:0.5s;}&:nth-child(7){animation-delay:0.6s;}"], bt),
          Ct = (0, a.ZP)(yt).attrs({
            d: "M6.9,68.9c-0.7,0-1-0.3-1-1v-6.1c0-0.3,0.3-0.5,0.7-0.5c0.3,0,0.7,0.2,0.7,0.5v5.1c0,0.1,0.1,0.2,0.2,0.2h3\n  c0.4,0,0.6,0.5,0.6,0.9s-0.2,0.9-0.6,0.9H6.9z"
          }).withConfig({
            displayName: "LoadingIcon__Word1",
            componentId: "sc-1cvog04-2"
          })([""]),
          _t = (0, a.ZP)(yt).attrs({
            d: "M14,65.2c0-2.3,1.7-4,3.9-4s3.9,1.7,3.9,4c0,2.3-1.7,4-3.9,4S14,67.5,14,65.2z M17.9,62.5c-1.4,0-2.4,1.1-2.4,2.4\n  c0,1.3,1,2.4,2.4,2.4s2.4-1,2.4-2.4C20.2,63.6,19.2,62.5,17.9,62.5z"
          }).withConfig({
            displayName: "LoadingIcon__Word2",
            componentId: "sc-1cvog04-3"
          })([""]),
          kt = (0, a.ZP)(yt).attrs({
            d: "M27,66.6c-0.2,0.6-0.4,1.3-0.6,1.9C26.4,68.9,26,69,25.7,69c-0.5,0-1.1-0.3-1.1-0.7c0-0.1,0-0.2,0.1-0.2l2.8-6.2\n  c0.2-0.4,0.6-0.6,1-0.6c0.4,0,0.8,0.2,1,0.6l2.8,6.2c0,0.1,0.1,0.2,0.1,0.3c0,0.4-0.6,0.7-1.1,0.7c-0.4,0-0.7-0.1-0.8-0.5\n  c-0.2-0.7-0.4-1.3-0.6-1.9H27z M29.4,65.3c-0.3-0.9-0.6-1.7-1-2.4c-0.3,0.7-0.6,1.5-1,2.4H29.4z"
          }).withConfig({
            displayName: "LoadingIcon__Word3",
            componentId: "sc-1cvog04-4"
          })([""]),
          jt = (0, a.ZP)(yt).attrs({
            d: "M36.4,68.9c-0.7,0-1-0.3-1-1v-5.6c0-0.6,0.3-0.9,1-0.9h1.9c2.4,0,4,1.7,4,3.8c0,2.3-1.5,3.8-3.9,3.8H36.4z M38.4,67.1\n  c1.5,0,2.4-1,2.4-2.2c0-1.3-1-2.2-2.5-2.2h-1.5c0,0,0,0,0,0V67c0,0.1,0.1,0.1,0.1,0.1H38.4z"
          }).withConfig({
            displayName: "LoadingIcon__Word4",
            componentId: "sc-1cvog04-5"
          })([""]),
          It = (0, a.ZP)(yt).attrs({
            d: "M45.8,68.3c0.3-2.4,0.3-4.4,0.3-5.8v-0.7c0-0.3,0.3-0.5,0.7-0.5c0.3,0,0.7,0.2,0.7,0.5v0.7c0,1.4,0,3.3,0.3,5.8\n  c0,0,0,0.1,0,0.1c0,0.5-0.5,0.7-1,0.7S45.8,68.8,45.8,68.3L45.8,68.3z"
          }).withConfig({
            displayName: "LoadingIcon__Word5",
            componentId: "sc-1cvog04-6"
          })([""]),
          Nt = (0, a.ZP)(yt).attrs({
            d: "M58,68.2c0,0.6-0.6,0.9-1.1,0.9c-0.4,0-0.8-0.1-1-0.4c-1.1-1.6-2.2-3.8-2.8-5.1c0,0,0.1,2.8,0.3,4.7c0,0,0,0.1,0,0.1\n  c0,0.5-0.5,0.8-0.9,0.8c-0.5,0-0.9-0.3-0.9-0.8v0c0.1-2.2,0.1-3.6,0.1-6.1c0-0.6,0.5-0.9,1-0.9c0.3,0,0.6,0.1,0.8,0.4\n  c1.1,1.9,2.1,3.3,3.1,4.5v-4.4c0-0.3,0.4-0.5,0.7-0.5c0.3,0,0.7,0.2,0.7,0.5C57.9,64.3,57.9,65.7,58,68.2L58,68.2z"
          }).withConfig({
            displayName: "LoadingIcon__Word6",
            componentId: "sc-1cvog04-7"
          })([""]),
          Zt = (0, a.ZP)(yt).attrs({
            d: "M67.6,65.8C67.6,65.8,67.5,65.7,67.6,65.8l-2.1-0.1c-0.4,0-0.6-0.3-0.6-0.6c0-0.3,0.2-0.6,0.6-0.6h2.3\n  c0.9,0,1.2,0.5,1.2,1.4c0,1.7-1.1,3.3-3.5,3.3c-2.3,0-3.8-1.7-3.8-4c0-2.2,1.5-4,3.8-4c1.3,0,2.7,0.7,3.2,1.9\n  c0,0.1,0.1,0.2,0.1,0.3c0,0.4-0.4,0.6-0.8,0.6c-0.2,0-0.4-0.1-0.5-0.3c-0.4-0.8-1.2-1.2-2-1.2c-1.3,0-2.3,1.1-2.3,2.4\n  c0,1.4,0.9,2.5,2.3,2.5C66.7,67.4,67.5,66.8,67.6,65.8L67.6,65.8z"
          }).withConfig({
            displayName: "LoadingIcon__Word7",
            componentId: "sc-1cvog04-8"
          })([""]),
          zt = (0, a.F4)(["from{stroke-dasharray:0 var(--stroke-length);}50%,to{stroke-dasharray:var(--stroke-length) var(--stroke-length);}from,20%{stroke-dashoffset:0;}70%,to{stroke-dashoffset:calc(-1px * var(--stroke-length));}"]),
          Pt = a.ZP.path.attrs({
            d: "M7,54.5c0,0-5-7.1-5-20.8S15.6,2,36.8,2S71,22.3,71,31.6s-1.6,19.9-4.8,22.9"
          }).withConfig({
            displayName: "LoadingIcon__MaskLine",
            componentId: "sc-1cvog04-9"
          })(["--stroke-length:146.5;fill:none;stroke:#fff;stroke-width:4;stroke-linecap:round;animation:", " 1.4s ease-in-out 0s infinite;"], zt),
          Dt = a.ZP.path.attrs({
            d: ""
          }).withConfig({
            displayName: "LoadingIcon__Line",
            componentId: "sc-1cvog04-10"
          })(["fill:var(--color-black);"]),
          Bt = a.ZP.g.withConfig({
            displayName: "LoadingIcon__Rabbit",
            componentId: "sc-1cvog04-11"
          })([""]),
          Mt = a.ZP.path.withConfig({
            displayName: "LoadingIcon__RabbitBase",
            componentId: "sc-1cvog04-12"
          })(["fill:var(--color-black);"]),
          Lt = (0, a.ZP)(Mt).attrs({
            d: ""
          }).withConfig({
            displayName: "LoadingIcon__RabbitFace",
            componentId: "sc-1cvog04-13"
          })([""]),
          At = (0, a.F4)(["from{transform:rotate(0);}10%{transform:rotate(var(--motion-range));}16%{transform:rotate(0);}22%{transform:rotate(var(--motion-range));}28%,to{transform:rotate(0);}"]),
          St = (0, a.ZP)(Mt).attrs({
            d: "M34.4,30.1l-4.5,0.2l-0.2-0.6c-0.7-2.5-0.8-5.1-0.3-7.6l0,0c0.3-1.1,0.9-2.8,2.1-2.9h0\n  c1.2-0.1,2,1.2,2.4,2.6v0C34.8,24.5,35,27.4,34.4,30.1L34.4,30.1z"
          }).withConfig({
            displayName: "LoadingIcon__RabbitEar1",
            componentId: "sc-1cvog04-14"
          })(["--motion-range:-10deg;transform-origin:33px 33px;animation:", " 1.4s ease-out 0s infinite;"], At),
          Tt = (0, a.ZP)(Mt).attrs({
            d: "M43.3,30.4l-4.5-0.2l-0.1-0.6c-0.5-2.5-0.3-5.2,0.5-7.6l0,0c0.4-1.1,1.1-2.7,2.4-2.6h0\n  c1.2,0.1,1.8,1.4,2.1,2.9v0C44.2,24.8,44.2,27.7,43.3,30.4L43.3,30.4z"
          }).withConfig({
            displayName: "LoadingIcon__RabbitEar2",
            componentId: "sc-1cvog04-15"
          })(["--motion-range:10deg;transform-origin:41px 33px;animation:", " 1.4s ease-out 0s infinite;"], At);
  
        function Wt(n) {
          var i = n.isLoading;
          return (0, d.jsx)(Et, {
            emit: !i,
            children: (0, d.jsxs)(Kt, {
              emit: !i,
              children: [(0, d.jsx)(Ot, {
                isLoading: i
              }), (0, d.jsx)(qt, {
                emit: !i
              }), (0, d.jsx)(Vt, {
                emit: !i
              })]
            })
          })
        }
        var Ot = (0, a.ZP)((function (n) {
            var i = n.className,
              t = n.isLoading;
            return (0, d.jsxs)(vt, {
              className: i,
              isLoading: t,
              children: [(0, d.jsx)("mask", {
                id: wt,
                children: (0, d.jsx)(Pt, {})
              }), (0, d.jsx)(Dt, {
                mask: "url(#".concat(wt, ")")
              }), (0, d.jsxs)(Bt, {
                children: [(0, d.jsx)(Lt, {}), (0, d.jsx)(St, {}), (0, d.jsx)(Tt, {})]
              }), (0, d.jsxs)("g", {
                children: [(0, d.jsx)(Ct, {}), (0, d.jsx)(_t, {}), (0, d.jsx)(kt, {}), (0, d.jsx)(jt, {}), (0, d.jsx)(It, {}), (0, d.jsx)(Nt, {}), (0, d.jsx)(Zt, {})]
              })]
            })
          })).withConfig({
            displayName: "OpeningAnimation__StyledLoadingIcon",
            componentId: "sc-1kxtadf-0"
          })(["position:absolute;top:50%;left:50%;transform:translate(-50%,-80%);"]),
          Yt = (0, a.F4)(["from{clip-path:polygon(0 0,calc(100% + var(--skew)) 0,100% 100%,0 100%);}to{clip-path:polygon(calc(100% + var(--skew)) 0,calc(100% + var(--skew)) 0,100% 100%,100% 100%);}"]),
          Ft = (0, a.F4)(["to{transform:translateX(100%);}"]),
          Et = a.ZP.div.withConfig({
            displayName: "OpeningAnimation__Wrapper",
            componentId: "sc-1kxtadf-1"
          })(["--skew:1000px;position:fixed;top:0;left:0;width:100%;height:100%;overflow:hidden;background:var(--color-black);z-index:11;clip-path:polygon(0 0,calc(100% + var(--skew)) 0,100% 100%,0 100%);", " ", "{--skew:1000px;}"], (function (n) {
            return n.emit && (0, a.iv)(["animation:", " 0.7s cubic-bezier(0.73,0.01,0.24,1.01) 3.3s forwards,", " 0.05s ease-out 4s forwards;"], Yt, Ft)
          }), s.BC.smallDown),
          Kt = a.ZP.div.withConfig({
            displayName: "OpeningAnimation__Inner",
            componentId: "sc-1kxtadf-2"
          })(["position:absolute;top:0;left:0;width:100%;height:100%;background:#eee;clip-path:polygon(0 0,calc(100% + var(--skew)) 0,100% 100%,0 100%);", ""], (function (n) {
            return n.emit && (0, a.iv)(["animation:", " 0.7s cubic-bezier(0.73,0.01,0.24,1.01) 3.2s forwards;"], Yt)
          })),
          Ut = (0, a.F4)(["from{transform:translate(-50%,-50%) scale(0);}to{transform:translate(-50%,-50%) scale(1);}"]),
          qt = a.ZP.div.withConfig({
            displayName: "OpeningAnimation__Overlay",
            componentId: "sc-1kxtadf-3"
          })(["position:absolute;top:0;left:0;width:100%;height:100%;&::before,&::after{content:'';display:block;position:absolute;top:50%;left:50%;width:128vmax;height:128vmax;border-radius:50%;transform:translate(-50%,-50%) scale(0);}&::before{background:var(--color-black);}&::after{background:var(--color-white);animation-delay:0.35s;}", ""], (function (n) {
            return n.emit && (0, a.iv)(["&::before,&::after{animation:", " 0.75s cubic-bezier(0.02,0.51,0.26,1.02) forwards;}&::before{animation-delay:0.2s;}&::after{animation-delay:0.35s;}"], Ut)
          })),
          Vt = (0, a.ZP)(xt.I).withConfig({
            displayName: "OpeningAnimation__StyledMotionLogo",
            componentId: "sc-1kxtadf-4"
          })(["--delay:0.3s;position:absolute;top:50%;left:50%;width:50vmin;transform:translate(-50%,-60%);", "{width:80vmin;}"], s.BC.smallDown),
          Rt = function (n) {
            return new Promise((function (i, t) {
              var e = new Image;
              e.onload = function () {
                return i(e)
              }, e.onerror = function (n) {
                return t(n)
              }, e.src = n
            }))
          },
          Xt = !0;
  
        function Gt(n) {
          var i = n.res,
            t = i.news,
            a = i.about,
            r = i.works,
            s = i.banners,
            c = (0, e.useState)(!0),
            l = c[0],
            p = c[1],
            m = (0, e.useState)(null),
            g = m[0],
            h = m[1];
          return (0, e.useEffect)((function () {
            var n = function () {
              setTimeout((function () {
                p(!1)
              }), 1200)
            };
            Promise.all([Rt("/images/main-visual/1.jpg"), Rt("/images/main-visual/1--dark.jpg"), Rt("/images/main-visual/2.jpg"), Rt("/images/main-visual/2--dark.jpg"), Rt("/images/about/vtuber.png"), Rt("/images/about/vtuber-bg.png"), Rt("/images/about/vtuber-sd.png"), Rt("/images/menu/about.svg"), Rt("/images/menu/character.svg"), Rt("/images/menu/circle.svg"), Rt("/images/menu/contact.svg"), Rt("/images/menu/news.svg"), Rt("/images/menu/works.svg"), Rt("/images/menu/share.svg"), Rt("/images/menu/facebook.svg"), Rt("/images/menu/twitter.svg")]).then(n, n)
          }), []), (0, d.jsxs)(d.Fragment, {
            children: [(0, d.jsx)(o.Z, {
              title: "Re:ALive for TailWind"
            }), (0, d.jsxs)(Ht, {
              children: [(0, d.jsx)(Q, {
                emit: !l
              }), (0, d.jsx)(Wt, {
                isLoading: l
              }), (0, d.jsx)(Gi, {
                banners: s
              }), (0, d.jsx)(kn, {
                news: t
              }), (0, d.jsx)(ri, {
                about: a
              }), (0, d.jsx)(Li, {
                works: r,
                onClick: function (n) {
                  h(n)
                }
              }), (0, d.jsx)(nt, {}), (0, d.jsx)(rn.Z, {})]
            }), (0, d.jsx)(ct, {
              work: g,
              onClose: function () {
                h(null)
              }
            })]
          })
        }
        var Ht = a.ZP.div.withConfig({
          displayName: "pages__Wrapper",
          componentId: "sc-13eskut-0"
        })(["overflow:hidden;"])
      },
      5301: function (n, i, t) {
        (window.__NEXT_P = window.__NEXT_P || []).push(["/", function () {
          return t(7276)
        }])
      }
    },
    function (n) {
      n.O(0, [774, 493, 552, 856, 888, 179], (function () {
        return i = 5301, n(n.s = i);
        var i
      }));
      var i = n.O();
      _N_E = i
    }
  ]);