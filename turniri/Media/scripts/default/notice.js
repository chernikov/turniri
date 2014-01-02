function Notice() {

    var _this = this;

    this.ajaxShowNotice = "/Notice/Index";
    this.ajaxShowCount = "/Notice/Count";
    this.ajaxRemoveNotice = "/Notice/Remove";
    this.ajaxReadAllNotice = "/Notice/ReadAll";
    this.ajaxUpdateUnreadMessage = "/Home/UnreadMessages";


    this.init = function () {
        $("#ShowNotice").live("click", function () {
            $.ajax({
                type: "GET",
                url: _this.ajaxShowNotice,
                success: function (data) {
                    $("#PopupWrapper").html(data);
                    _this.initPopup();
                    $(".popup-window", $("#PopupWrapper")).centerInClient();
                    $(".gray-background").show();
                }
            });
            return false;
        });

        $(".remove-notice").live("click", function () {
            var item = $(this).closest(".item");
            _this.removeNotice(item);
            return false;
        });

        $(".notice-action").live("click", function () {
            _this.makeAction($(this));
            return false;
        });

    };

    this.initPopup = function () {
        _this.readAll();
    }

    this.readAll = function () {
        $.ajax({
            type: "POST",
            url: _this.ajaxReadAllNotice,
            success: function (data) {
                _this.updateCount();
                _this.updateUnreadMessage();
            }
        });
    }

    this.updateCount = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxShowCount,
            success: function (data) {
                $("#NoticeWrapper").html(data);
            }
        });
    }

    this.updateUnreadMessage = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxUpdateUnreadMessage,
            success: function (data) {
                $("#UnreadMessagesWrapper").html(data);
            }
        });
    }

    this.removeNotice = function (item) {
        var id = item.data("id");
        $.ajax({
            type: "POST",
            url: _this.ajaxRemoveNotice,
            data: { id: id },
            success: function (data) {
                item.fadeOut();
                _this.updateCount();
                _this.updateUnreadMessage();
            }
        });
    }

    this.makeAction = function (item)
    {
        var resolve = item.data("resolve");
        var url = item.data("url");
        var run = item.data("run");
        $.ajax({
            type: "POST",
            url: url,
            success: function (data)
            {
                if (run == "True")
                {
                    var obj = $("<div>");
                    obj.append(data);
                    $("body").append(obj);
                }
                if (resolve == "True")
                {
                    _this.removeNotice(item.closest(".item"));
                } 
                _this.updateUnreadMessage();
            }
        });
    }
}

var notice = null;
$().ready(function () {
    notice = new Notice();
    notice.init();
});