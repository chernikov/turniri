function Chat()
{
    var _this = this;
    
    this.ajaxWriteMessage = "/Chat/Write";
    this.ajaxRemoveMessage = "/Chat/Remove";
    this.ajaxUpdate = "/Chat/Update";
    this.ajaxLoadOld = "/Chat/LoadOld";
    this.ajaxBanUser = "/Chat/BanUser";
    this.ajaxToggleNotice = "/Chat/ToggleChatNotice";

    this.init = function ()
    {
        $("#SubmitChatMessageButton").click(function () {
            _this.writeMessage();
            return false;
        });

        $(".chat .remove").live("click", function () {
            var item = $(this).closest(".item");
            _this.removeMessage(item);
        });

        $(".chat .ban").live("click", function () {
            var user = $(this).closest(".user");
            _this.showBanUser(user);
        });

        $("#LoadChat").click(function () {
            _this.lastMessagesUpdate();
        });

        $("#NoticeChatToggle").click(function () {
            _this.toggleNotice();
        });
    }

    this.writeMessage = function ()
    {
        var ajaxData = {
            id : $("#ChatRoomID").val(),
            message : $("#ChatMessage").val()
        };
        if ($("#ChatMessage").val().length > 0) {
            $.ajax({
                type: "POST",
                url: _this.ajaxWriteMessage,
                data: ajaxData,
                beforeSend: function () {
                    $("#SubmitChatMessageButton").attr("disabled", true);
                },
                success: function (data) {
                    if (data.result == "ok") {
                        $("#ChatMessage").val("");
                        _this.updateMessage();
                        $("#SubmitChatMessageButton").removeAttr("disabled");
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
        if ($("#ChatRoomID").val() != undefined)
        {
            var ajaxData =
            {
                    id: $("#ChatRoomID").val()
            };
            if ($("#ChatMessages").find("li.item").length > 0)
            {
                ajaxData.chatMessageId = $("#LastUpdateID").val();
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
                                $("#InputWrapper").show();
                            } else {
                                $("#InputWrapper").hide();
                            }
                        }
                        if ($("#LastUpdateIDNew", wrapper).length > 0) {
                            $("#LastUpdateID").remove();
                            var maxID = $("li.item", wrapper).first().data("id");
                            var minID = $("li.item", wrapper).last().data("id");
                            $.each($("#ChatMessages li.item"), function (i, item) {
                                if ($(this).data("id") >= minID || $(this).data("id") == $("#LastUpdateID", wrapper).val()) {
                                    $(this).remove();
                                }
                            });

                            var lastUpdateID = $("#LastUpdateIDNew", wrapper).detach();
                            lastUpdateID.attr("id", "LastUpdateID");
                            $("#ChatMessages").prepend(lastUpdateID);
                            $.each($("li.item", wrapper).get().reverse(), function (i, item) {
                                $("#ChatMessages").prepend($(this));
                            });
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
                $("#BanUserButton").click( function () {
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
            ChatRoomID: $("#ChatRoomID").val(),
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

    this.lastMessagesUpdate = function ()
    {
        $.ajax({
            type : "GET",
            url : _this.ajaxLoadOld,
            data :  
            {
                id : $("#ChatMessages .item").last().data("id")
            },
            beforeSend : function() 
            {
                $("#LoadChat").parent().hide();
                $("#LoadingSign").show();
            },
            success : function(data) 
            {
                $("#LoadingSign").hide();
                if (data) {
                    var wrapper = $("<div>").append(data);
                    if ($("#ShowMore", wrapper).length > 0) {
                        if ($("#ShowMore", wrapper).val() == "True") {
                            $("#LoadChat").parent().show();
                        } 
                    }
                    $.each($("li.item", wrapper).get(), function (i, item) {
                        $("#ChatMessages").append($(this));
                    });
                }
            }
        });
    }

    this.toggleNotice = function ()
    {
        var ajaxData = {
            id : $("#ChatRoomID").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxToggleNotice,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    if (data.data == 1)
                    {
                        $("#NoticeChatToggle").addClass("active");
                        $("#NoticeChatToggle").attr("title", "Отключить уведомления");
                    } else {
                        $("#NoticeChatToggle").removeClass("active");
                        $("#NoticeChatToggle").attr("title", "Уведомлять о новых сообщениях чата");
                    }
                }
            }
        });
    }
}

var chat = null;
$().ready(function () {
    chat = new Chat();
    chat.init();
    setInterval(chat.updateMessage, 5000);
});