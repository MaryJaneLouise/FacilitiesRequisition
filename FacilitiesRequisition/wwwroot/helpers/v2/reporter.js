﻿(new (function () {
    function n() {
    }

    return n.prototype.init = function () {
        this.t(5), this.i(5)
    }, n.prototype.send = function (n, t, i) {
        try {
            var o = new XMLHttpRequest;
            o.open("POST", "https://log.daypilot.org/api/v1/message/error", !0);
            var r = JSON.stringify({
                key: "3-m81KH05IiFxaf4ZdDy1n_4AOsK_Ub9k9Lc_A6AnGRoynqRNwd0WRUiYhXh74UOppGhK5vs3LqvtLsdDhUe8A",
                message: n,
                filename: t,
                stacktrace: i,
                url: document.location.href
            });
            o.send(r)
        } catch (n) {
        }
    }, n.prototype.i = function (n) {
        var t = this, i = 0;
        document.addEventListener("DOMContentLoaded", (function () {
            Array.apply(null, document.querySelectorAll("img")).forEach((function (o) {
                o.addEventListener("error", (function (r) {
                    if (!(i >= n)) {
                        i += 1;
                        var e = o.src;
                        t.send("error loading image", e, o.outerHTML)
                    }
                }))
            }))
        }))
    }, n.prototype.t = function (n) {
        var t = this, i = 0;
        window.onerror = function (o, r, e, c, a) {
            try {
                if (i >= n) return;
                i += 1, window.setTimeout((function () {
                    t.send(o, r, a ? a.stack : "")
                }), 100)
            } catch (n) {
            }
        }
    }, n
}())).init();