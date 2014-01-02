function IndexChat() {

    var _this = this;

    this.ajaxToggleChatTranslate = "/admin/Chat/ToggleChatTranslate";

    this.init = function () {
        $(".toggle-translate").click(function () {
            _this.toggleChatTranslate($(this));
        });
    };

    this.toggleChatTranslate = function (item) {
        
        var id = item.data("id");

        $.ajax({
            type: "GET",
            url: _this.ajaxToggleChatTranslate,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    if (data.data) {
                        item.addClass("label-info");
                        item.text("On");
                    } else {
                        item.removeClass("label-info");
                        item.text("Off");
                    }
                } 
            }
        });
    };

}

var indexChat = null;
$().ready(function () {
    indexChat = new IndexChat();
    indexChat.init();
});