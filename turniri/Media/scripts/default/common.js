$.fn.centerInClient = function (options) {
    /// <summary>Centers the selected items in the browser window. Takes into account scroll position.
    /// Ideally the selected set should only match a single element.
    /// </summary>    
    /// <param name="fn" type="Function">Optional function called when centering is complete. Passed DOM element as parameter</param>    
    /// <param name="forceAbsolute" type="Boolean">if true forces the element to be removed from the document flow 
    ///  and attached to the body element to ensure proper absolute positioning. 
    /// Be aware that this may cause ID hierachy for CSS styles to be affected.
    /// </param>
    /// <returns type="jQuery" />
    var opt = {
        forceAbsolute: false,
        container: window,    // selector of element to center in
        completeHandler: null
    };
    $.extend(opt, options);

    return this.each(function (i) {
        var el = $(this);
        var jWin = $(opt.container);
        var isWin = opt.container == window;

        // force to the top of document to ENSURE that 
        // document absolute positioning is available
        if (opt.forceAbsolute) {
            if (isWin)
                el.remove().appendTo("body");
            else
                el.remove().appendTo(jWin.get(0));
        }

        // have to make absolute
        el.css("position", "absolute");

        // height is off a bit so fudge it
        var heightFudge = isWin ? 2.0 : 1.8;

        var y = (isWin ? jWin.height() : jWin.outerHeight()) / heightFudge - el.outerHeight() / 2;
        var x = (isWin ? jWin.width() : jWin.outerWidth()) / 2 - el.outerWidth() / 2;

        if (y < 10) {
            y = 10;
        }

        el.css("top", y + jWin.scrollTop());
        el.css("left", x + jWin.scrollLeft());

        // if specified make callback and pass element
        if (opt.completeHandler)
            opt.completeHandler(this);
    });
}

function LoadCssDynamically(fileName) {
    var fileref = $('<link>');
    fileref.attr("rel", "stylesheet");
    fileref.attr("type", "text/css");
    fileref.attr("href", fileName);
    $("head").append(fileref);
}

function LoadJsDynamically(fileName) {
    var fileref = $('<script>');
    fileref.attr("type", "text/javascript");
    fileref.attr("src", fileName);

    $("head").append(fileref);
}


function InitUpload(item, multiple, url, oncomplete, extensions, title) {
    if (extensions == null) {
        extensions = [];
    }
    if (typeof (qq) == 'undefined') {
        LoadCssDynamically("/Media/css/fineuploader.css");
        LoadJsDynamically("/Media/scripts/jquery.fineuploader-3.0.js");
    }

    var obj = new qq.FineUploader({
        element: item,
        multiple: multiple,
        request: {
            endpoint: url
        },
        text: {
            uploadButton: title
        },
        callbacks: {
            onComplete: oncomplete
        },
        validation: {
            allowedExtensions: extensions
        }
    });
}


function InitBbCodeEditor(item) {
    var _this = this;

    if ($.fn.queryBBCodeEditor == null) {
        LoadCssDynamically("/Media/css/bbCodeEditor.css");
        LoadJsDynamically("/Media/scripts/jquery.bbCodeEditor.js");
    }
    item.queryBBCodeEditor();
}

function Common() {
    var _this = this;
    
    this.ajaxClickBanner = "/Home/ClickBanner";

    this.ajaxLikeNews = "/New/ToggleLike";
    this.ajaxLikeBlog = "/Blog/ToggleLike";
    this.ajaxLikePhoto = "/Photo/ToggleLike";



    this.init = function () {
        if ($('.scroll-pane').length > 0) {
            $('.scroll-pane').jScrollPane({ autoReinitialise: true });
        }
        if ($(".gamers-list .header-wrapper, .gamers-list .top-panel-wrapper").length > 0) {
            _this.updateScrollBars();
        }

        textboxHint("search");
        textboxHint("login");
        textboxHint("gamer_search");
        textboxHint("blog_title");
        textboxHint("password");

        /* --- login --- */
        $('.enter-link').live('click', function () {
            $(this).parent().find('.login-wrapper').toggle();
            $('.login-toggle').toggle();
        });

        $('.login-toggle').live('click', function () {
            $('.login-wrapper, .login-toggle').toggle();
        });

        /* --- top menu dropdown --- */
        $('li.menu-item').live('hover', function () {
            var $this = $(this);
            var left = $this.width() / 2 - 15;
            $this.find('.dropdown-arrow').css('left', left);
            $this.find('.dropdown-wrapper').toggle();
        });

        /* --- switcher --- */
        $('.switcher li').live('click', function () {
            var $this = $(this);
            var tabId = $this.attr('id');
            var contentId = '#' + tabId + '_content';
            $this.parent().parent().parent().find('.switcher-content').hide();
            $this.parent().find('li').removeClass('current');
            $this.addClass('current');
            $this.parent().parent().parent().find(contentId).show();
        });

        /* --- switcher --- */
        $('.psiswitcher li').live('click', function () {
            var $this = $(this);
            $this.parent().find('li').removeClass('current');
            $this.addClass('current');
        });

        /* --- textarea inner text --- */
        /* $('.comment-input textarea, .hide-inner-text').live('click', function () {
             $(this).text('');
         });
         */
        $('.popup-image-wrapper .icon-close-popup, .popup-video-wrapper .icon-close-popup, .gray-background').live('click', function () {
            $('#gallery_popup').hide();
            $('#video_popup').hide();
            $("#video_popup .video").empty();
            $('.gray-background').hide();
        });

        /* --- popup --- */
        $('.popup-open').live('click', function () {
            $('.popup-window').show();
            $('.gray-background').show();
        });

        /* --- popup big --- */
        $('.popup-big-open').live('click', function () {
            var topHeight = $(window).scrollTop();
            var topValue = topHeight + 10;
            $('.popup-window-big').css('top', topValue).show();
            $('.gray-background').show();
        });

        $('.popup-window .icon-close-popup9, .gray-background, .popup-window .gray-24-button-wrapper span.popup-close, .popup-close').live('click', function () {
            _this.closePopup();
            return false;
        });
        $('.popup-window .icon-close-error-popup9, .error-gray-background').live("click", function () {
            _this.closeErrorPopup();
            return false;
        });
        _this.replaceQuote();
        _this.replaceSpoiler();

        $(".spoiler-wrapper .title").live("click", function () {
            $(this).closest(".spoiler-wrapper").find(".text").toggle();
        });

        $(".chat-panel").hover(function () {
            $(".chat-panel").css({ left: "1px" });
        },
        function () {
            $(".chat-panel").css({ left: "0px" });
        });

        $(".chat-panel").click(function () {
            commonChat.showCommonChat();
        });

        $("a.banner").click(function () {
            var id = $(this).data("id");
            var href = $(this).attr("href");
            $.ajax({
                type: "GET",
                url: _this.ajaxClickBanner,
                data: { id: id },
                success: function () {
                    window.location = href;
                },
                error: function () {
                    window.location = href;
                }
            });
            return false;
        });

        $(".like.news .icon.active").live("click", function () {
            _this.toggleLike($(this), _this.ajaxLikeNews);
        });

        $(".like.blog .icon.active").live("click", function () {
            _this.toggleLike($(this), _this.ajaxLikeBlog);
        });

        $(".like.photo .icon.active").live("click", function () {
            _this.toggleLike($(this), _this.ajaxLikePhoto);
        });

        $(" .top .enter .money").click(function () {
            window.location = "/user/money";
        });
    }

    this.updateScrollBars = function ()
    {
        console.log("updateScrollBars");
        $('.scroll-bars').scrollbars();
        $('.viewport .scrollcontent, .header-wrapper').scrollsync({ targetSelector: '.viewport .scrollcontent', axis: 'x' });
        $('.viewport .scrollcontent').dragscrollable({
            dragSelector: '.scrollwrap *'
        });

        $('.tournament .scrollcontent').dragscrollable({ dragSelector: '.scrollwrap *' });
        $('.tournament .scrollcontent, .top-panel-wrapper').scrollsync({ targetSelector: '.tournament .scrollcontent', axis: 'x' });
        $('.tournament .scrollcontent, .left-panel-wrapper').scrollsync({ targetSelector: '.tournament .scrollcontent', axis: 'y' });
    }

    this.closePopup = function ()
    {
        $('.popup-window').hide();
        $('.gray-background').hide();
        var url = window.location.href;
        var indexOf = url.indexOf("?");
        url = url.substring(0, indexOf);
        _this.rewriteUrl(url);
    }

    this.closeErrorPopup = function () {
        $('.popup-window.error').hide();
        $('.error-gray-background').hide();
    }

    this.rewriteUrl = function (url)
    {
        try {
            window.history.pushState("", "", url);
        } catch (exception_var) {
            //window.location.href = url + "#dot";
        }
    }

    this.replaceQuote = function () {
        $.each($("q"), function (i, item) {
            var title = $(this).attr("cite") + " сказал(а):";
            var html = $(this).html();

            var quote = $("<div>").addClass("quote-wrapper").insertBefore($(this));
            if ($(this).attr("cite")) {
                $("<div>").addClass("quote-title").text(title).appendTo(quote);
            }
            $("<div>").addClass("quote-text").html(html).appendTo(quote);

            $(this).remove();
        });
    }

    this.replaceSpoiler = function () {
        $.each($(".spoiler-wrapper"), function (i, item) {
            var title = $(this).attr("title");
            var text = $(this).text();
            $(this).text("");
            $(this).append($("<span>").addClass("title").text(title));
            $(this).append($("<div>").addClass("text").text(text).hide());
        });
    }

    this.toggleLike = function (item, url)
    {
        var main = item.closest(".like");
        var id = main.data("id");

        $.ajax({
            type: "POST",
            url: url,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok")
                {
                    $(".count", main).text(data.count);
                    $(".icon", main).toggleClass("selected");
                }
            }
        });
    }

    
}

function textboxHint(id, options) {
    var o = { selector: 'input:text[title]', blurClass: 'blur' };
    $e = $('#' + id);
    $.extend(true, o, options || {});

    if ($e.is(':text')) {
        if (!$e.attr('title')) $e = null;
    } else {
        $e = $e.find(o.selector);
    }
    if ($e) {
        $e.each(function () {
            var $t = $(this);
            if ($.trim($t.val()).length == 0) { $t.val($t.attr('title')); }
            if ($t.val() == $t.attr('title')) {
                $t.addClass(o.blurClass);
            } else {
                $t.removeClass(o.blurClass);
            }

            $t.focus(function () {
                if ($.trim($t.val()) == $t.attr('title')) {
                    $t.val('');
                    $t.removeClass(o.blurClass);
                }
            }).blur(function () {
                var val = $.trim($t.val());
                if (val.length == 0 || val == $t.attr('title')) {
                    $t.val($t.attr('title'));
                    $t.addClass(o.blurClass);
                }
            });

            // empty the text box on form submit
            $(this.form).submit(function () {
                if ($.trim($t.val()) == $t.attr('title')) $t.val('');
            });
        });
    }
}

function UpdateOnline()
{
    SendUpdateOnline();
}

function SendUpdateOnline() {
    $.getJSON("/home/online/", function (data) { });
    if (notice != null) {
        notice.updateCount();
        notice.updateUnreadMessage();
    }
    setTimeout(UpdateOnline, 60000);
}

function ShowError(message)
{
    $.ajax({
        type: "GET",
        url: "/Home/ShowError",
        data: { message: message },
        success: function (data) {
            $(".error-gray-background").show();
            $("#ErrorPopupWrapper").html(data);
            $(".popup-window", $("#ErrorPopupWrapper")).centerInClient();
        }
    });
}

function ShowInfoMessage(message) {
    $.ajax({
        type: "GET",
        url: "/Home/ShowError",
        data: { message: message },
        success: function (data) {
            $(".error-gray-background").show();
            $("#ErrorPopupWrapper").html(data);
            $(".popup-window", $("#ErrorPopupWrapper")).centerInClient();
        }
    });
}


var common;
$().ready(function () {
    common = new Common();
    common.init();
    SendUpdateOnline();
});