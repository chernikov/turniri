function CommonChat() {

    var _this = this;

    this.isShow = false;
    this.ajaxCommonChat = "/Chat/Index";
    this.ajaxWriteMessage = "/Chat/Write";
    this.ajaxRemoveMessage = "/Chat/Remove";
    this.ajaxUpdate = "/Chat/Update";
    this.ajaxBanUser = "/Chat/CommonBanUser";

    this.init = function () {
        $("#CommonSubmitChatMessageButton").live("click", function () {
            _this.writeMessage();
            return false;
        });

        $(".close-chat").live("click", function () {
            _this.showCommonChat();
        });

        $(".common-chat .remove").live("click", function () {
            var item = $(this).closest(".item");
            _this.removeMessage(item);
        });

        $(".common-chat .ban").live("click", function () {
            var user = $(this).closest(".user");
            _this.showBanUser(user);
        });

        $(document).bind("keydown", 'ctrl+à ctrl+À ctrl+~ ctrl+` ctrl+ё ctrl+Ё', function ()
        {
            _this.showCommonChat();
        });
    };

    this.showCommonChat = function () {
        if ($(".common-chat").length > 0) {
            $("#ChatWrapper").empty();
            _this.isShow = false;
        } else {
            $.ajax({
                type: "GET",
                url: _this.ajaxCommonChat,
                success: function (data) {
                    _this.isShow = true;
                    $("#ChatWrapper").html(data);
                    $('#ChatWrapper .scroll-pane').jScrollPane({ autoReinitialise: true });
                    $('#ChatWrapper .scroll-pane').data('jsp').scrollToBottom();
                    $("#CommonChatMessage").bind("keydown", 'ctrl+à ctrl+À ctrl+~ ctrl+` ctrl+ё ctrl+Ё', function () {
                        _this.showCommonChat();
                    });
                }
            });
        }
    }

    this.writeMessage = function ()
    {
        var ajaxData = {
            id : $("#CommonChatRoomID").val(),
            message : $("#CommonChatMessage").val()
        };
        if ($("#CommonChatMessage").val().length > 0) {
            $.ajax({
                type: "POST",
                url: _this.ajaxWriteMessage,
                data: ajaxData,
                beforeSend: function ()
                {
                    $("#CommonSubmitChatMessageButton").attr("disabled", true);
                },
                success: function (data) {
                    if (data.result == "ok") {
                        $("#CommonChatMessage").val("");
                        _this.updateMessage();
                        $("#CommonSubmitChatMessageButton").removeAttr("disabled");
                    }
                }
            });
        }
    }

    this.removeMessage = function (item)
    {
        var ajaxData = {
            id: item.data("id")
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxRemoveMessage,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok")
                {
                    item.remove();
                    _this.updateMessage();
                }
            }
        });
    }

    this.updateMessage = function ()
    {
        if (_this.isShow) {
            var ajaxData = {
                id: $("#CommonChatRoomID").val()
            };
            if ($("#CommonChatMessages").find("li.item").length > 0) {
                ajaxData.chatMessageId = $("#CommonLastUpdateID").val();
            } else {
                ajaxData.chatMessageId = 0;
            };
            console.log(ajaxData);

            $.ajax({
                type: "POST",
                url: _this.ajaxUpdate,
                data: ajaxData,
                success: function (data) {
                    if (data) {
                        var wrapper = $("<div>").append(data);
                        if ($("#CanAdd", wrapper).length > 0) {
                            if ($("#CanAdd", wrapper).val() == "True") {
                                $("#CommonInputWrapper").show();
                            } else {
                                $("#CommonInputWrapper").hide();
                            }
                        }

                        if ($("#LastUpdateIDNew", wrapper).length > 0) {
                            $("#CommonLastUpdateID").remove();
                            var maxID = $("li.item", wrapper).first().data("id");
                            var minID = $("li.item", wrapper).last().data("id");
                            $.each($("#CommonChatMessages li.item"), function (i, item) {
                                if ($(this).data("id") >= minID || $(this).data("id") == $("#LastUpdateID", wrapper).val()) {
                                    $(this).remove();
                                }
                            });

                            var lastUpdateID = $("#LastUpdateIDNew", wrapper).detach();
                            lastUpdateID.attr("id", "CommonLastUpdateID");
                            $("#CommonChatMessages").prepend(lastUpdateID);
                            $.each($("li.item", wrapper).get().reverse(), function (i, item) {
                                $("#CommonChatMessages").append($(this));
                            });
                            $('#ChatWrapper .scroll-pane').jScrollPane({ autoReinitialise: true });
                            $('#ChatWrapper .scroll-pane').data('jsp').scrollToBottom();
                            delete wrapper;
                        }
                    }
                }
            });
        }
    }

    this.showBanUser = function (user)
    {
        var ajaxData = {
            id: user.data("id")
        };
        $.ajax({
            type: "GET",
            url: _this.ajaxBanUser,
            data: ajaxData,
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                $("#BanUserButton").click(function () {
                    _this.banUserSubmit();
                    return false;
                });

            }
        });
    }

    this.banUserSubmit = function ()
    {
        var ajaxData = {
            UserID: $("#UserID").val(),
            ChatRoomID: $("#CommonChatRoomID").val(),
            Reason: $("#Reason").val(),
            ChoisePeriod: $("#ChoisePeriod:checked").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxBanUser,
            data: ajaxData,
            success: function (data) {
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                $("#BanUserButton").click(function () {
                    _this.banUserSubmit();
                    return false;
                });
            }
        });
    }

}

var commonChat = null;
$().ready(function () {
    commonChat = new CommonChat();
    commonChat.init();
    setInterval(commonChat.updateMessage, 5000);
});