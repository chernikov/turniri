function Messages() {
    var _this = this;

    this.ajaxMessages = "/Message/Messages";
    this.ajaxGetWriteMessage = "/Message/WriteMessage";
    this.ajaxGetWriteFightMessage = "/Message/WriteFightMessage";
    this.ajaxGetWriteInvoiceMessage = "/Message/WriteInvoiceMessage";
    this.ajaxReplyMessage = "/Message/ReplyMessage";
    this.ajaxMessagesAutocomplete = "/Message/AutocompleteUser";
    this.ajaxCreateGamesSelect = "/Message/CreateGamesSelect";
    this.ajaxDeleteMessages = "/Message/DeleteMessages";
    this.ajaxAcceptMatch = "/Message/SubmitMatch";
    this.ajaxCancelMatch = "/Message/CancelMatch";
    this.ajaxAcceptInvoice = "/Message/SubmitInvoice";
    this.ajaxCancelInvoice = "/Message/CancelInvoice";

    this.isFightMessage = false;

    this.tabID = 1;

    this.url = "/Message";
    this.init = function ()
    {
        _this.tabID = $("#TabID").val();
        $("#tab_message_1").click(function () {
            _this.tabID = 1;
            _this.url = "/Message";

            if ($("#MessageID").val() != "") {
                var url = _this.url + "?messageId=" + $("#MessageID").val();
                common.rewriteUrl(url);
            } else {
                common.rewriteUrl(_this.url);
            }
            _this.isFightMessage = false;
            $("#MessageWrapper").empty();
            $(".subject.current").removeClass("current");
            _this.initShowMessage();
            
        });
        $("#tab_message_2").click(function () {
            _this.tabID = 2;
            _this.url = "/Message/Fight";

            if ($("#MessageID").val() != "") {
                var url = _this.url + "?messageId=" + $("#MessageID").val();
                common.rewriteUrl(url);
            } else {
                common.rewriteUrl(_this.url);
            }

            _this.isFightMessage = true;
            $("#MessageWrapper").empty();
            $(".subject.current").removeClass("current");
            _this.initShowMessage();
        });
        $("#tab_message_3").click(function () {
            _this.tabID = 3;

            _this.url = "/Message/Invoice";
            if ($("#MessageID").val() != "") {
                var url = _this.url + "?messageId=" + $("#MessageID").val();
                common.rewriteUrl(url);
            } else {
                common.rewriteUrl(_this.url);
            }

            _this.isFightMessage = false;
            $("#MessageWrapper").empty();
            $(".subject.current").removeClass("current");
            _this.initShowMessage();
        });
        $("#tab_message_4").click(function () {
            _this.tabID = 4;
            _this.url = "/Message/Outcome";
            if ($("#MessageID").val() != "") {
                var url = _this.url + "?messageId=" + $("#MessageID").val();
                common.rewriteUrl(url);
            } else {
                common.rewriteUrl(_this.url);
            }

            _this.isFightMessage = false;
            $("#MessageWrapper").empty();
            $(".subject.current").removeClass("current");
            _this.initShowMessage();
        });
        $("#WriteMessage").click(function () {
            if (_this.isFightMessage)
            {
                _this.writeFightMessageShow();
            }
            else
            {
                _this.writeMessageShow();
            }
        });


        $(".icon-mail").live("click", function () {
            var id = $(this).data("id");
            _this.writeMessageShow(id);
        });

        $(".subject .title").live("click", function () {
            _this.showMessages($(this));
        });

        $("#ReplyMessageButton").live("click", function () {
            _this.replyMessage();
            return false;
        });

        $("#SubmitMatchButton").live("click", function () {
            if ($("#Text").val().length > 0) {
                _this.replyAndAcceptMessage();
            } else {
                _this.acceptMatch();
            }
            return false;
        });

        $("#CancelMatchButton").live("click", function () {
            _this.cancelMatch();
            return false;
        });

        $("#SubmitGroupButton").live("click", function () {
            if ($("#Text").val().length > 0)
            {
                _this.replyAndAcceptInvoice();
            } else {
                _this.acceptInvoice();
            }
            return false;
        });

        $("#CancelGroupButton").live("click", function () {
            _this.cancelInvoice();
            return false;
        });

        $("#deleteMarked").live("click", function () {
            _this.deleteMarked();
        });
        
        $("#tab_message_" + _this.tabID).click();
    };

    this.initShowMessage = function ()
    {
        if ($("#MessageID").val() != "")
        {
            var id = $("#MessageID").val();
            _this.showMessages($("#Subject_" + id));
            $("#MessageID").val("");
        }

    }

    this.writeMessageShow = function (id) {
        var ajaxData = {
        }
        if (typeof (id) != "undefined") {
            ajaxData.id = id;
        }
        
        $.ajax({
            type: "GET",
            url: _this.ajaxGetWriteMessage,
            data: ajaxData,
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initWriteMessagePopup(false);
            }
        });
    }

    this.writeFightMessageShow = function (id, idGame) {
        var ajaxData = {
        }
        if (typeof (id) != "undefined") {
            ajaxData.id = id;
        }
        if (typeof (idGame) != "undefined") {
            ajaxData.idGame = idGame;
        }
        $.ajax({
            type: "GET",
            url: _this.ajaxGetWriteFightMessage,
            data: ajaxData,
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initWriteMessagePopup(true);
            }
        });
    }

    this.writeInvoiceMessageShow = function (id, idGame) {
        var ajaxData =
        {
            id: id
        }
        if (typeof (idGame) != "undefined")
        {
            ajaxData.idGame = idGame;
        }
        $.ajax({
            type: "GET",
            url: _this.ajaxGetWriteInvoiceMessage,
            data: ajaxData,
            success: function (data)
            {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();

                $("#WriteMessageButton").click(function () {
                    _this.writeInvoiceMessage();
                });
            }
        });
    }

    this.initWriteMessagePopup = function (fight) {
        $("#ReceiverLogin").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxMessagesAutocomplete,
                    data: {
                        query: request.term
                    },
                    success: function (data) {

                        response($.map(data.data, function (item) {
                            return {
                                label: item.Label,
                                value: item.Label,
                                id: item.ID
                            }
                        }));
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $("#ReceiverLogin").val(ui.item.Label);
                $("#ReceiverID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
        if (fight) {
            $("#WriteMessageButton").click(function () {
                _this.writeFightMessage();
            });
        } else {
            $("#WriteMessageButton").click(function () {
                _this.writeMessage();
            });
        }

        if ($("#PlatformID").length > 0)
        {
            $("#PlatformID").change(function () {
                _this.updateGames();
            });
            if ($("#GameID").val() == "0") {
                _this.updateGames();
            }
        }
    }

    this.writeMessage = function () {
        var ajaxData = $("#WriteMessageForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxGetWriteMessage,
            data: ajaxData,
            beforeSend: function () {
                $("#WriteMessage").attr("disabled", true);
            },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initWriteMessagePopup(false);
            }
        });
    }

    this.writeFightMessage = function () {
        var ajaxData = $("#WriteMessageForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxGetWriteFightMessage,
            data: ajaxData,
            beforeSend: function () {
                $("#WriteMessage").attr("disabled", true);
            },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initWriteMessagePopup(true);
            }
        });
    }

    this.writeInvoiceMessage = function () {
        var ajaxData = $("#WriteMessageForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxGetWriteInvoiceMessage,
            data: ajaxData,
            beforeSend: function () {
                $("#WriteMessage").attr("disabled", true);
            },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                $("#WriteMessageButton").click(function () {
                    _this.writeInvoiceMessage();
                });
            }
        });
    }

    this.showMessages = function (item)
    {
        var id = item.closest(".subject").attr("id").substring("Subject_".length);
        $(".subject.current").removeClass("current");
        item.closest(".subject").addClass("current");
        $(".icon-letter-blue:not(.not-openable)", item.closest(".subject")).addClass("icon-letter-readed-blue").removeClass("icon-letter-blue");
        $(".icon-letter-red:not(.not-openable)", item.closest(".subject")).addClass("icon-letter-readed-red").removeClass("icon-letter-red");
        $.ajax({
            type: "GET",
            url: _this.ajaxMessages,
            data: { id: id },
            success: function (data)
            {
                $("#MessageWrapper").html(data);
                $("#Subject_" + id).addClass("current");
                common.rewriteUrl(_this.url + "?messageId=" + id);
                notice.updateUnreadMessage();
                notice.updateCount();
            }
        });
    }

    this.replyMessage = function () {
        var ajaxData = $("#ReplyMessageForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxReplyMessage,
            data: ajaxData,
            beforeSend: function () {
                $("#WriteMessage").attr("disabled", true);
            },
            success: function (data) {
                $("#ReplyMessageWrapper").html(data);
            }
        });
    }

    this.deleteMarked = function () {
        $(".subject .checkbox input[type=checkbox]:checked", $("#tab_message_" + _this.tabID + "_content")).each(function () {
            var id = $(this).closest(".subject").attr("id").substring("Subject_".length);
            $.ajax({
                type: "GET",
                url: _this.ajaxDeleteMessages,
                data: { id: id },
                success: function () {
                    $("#Subject_" + id).remove();
                    if ($(".subject", $("#tab_message_" + _this.tabID + "_content")).length == 0) {
                        window.location.reload();
                    }
                }
            });
        });
    };

    this.updateGames = function () {
        var id = $("#PlatformID").val();
        $.ajax({
            type: "POST",
            url: _this.ajaxCreateGamesSelect,
            data: { idPlatform: id },
            success: function (data) {
                $("#GameWrapper").html(data);
            }
        });
    };

    this.replyAndAcceptMessage = function () {
        var ajaxData = $("#ReplyMessageForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxReplyMessage,
            data: ajaxData,
            beforeSend: function () {
                $("#WriteMessage").attr("disabled", true);
            },
            success: function (data) {
                $.ajax({
                    type: "GET",
                    url: _this.ajaxAcceptMatch,
                    data: { id: $("#MatchID").val() },
                    success: function (data) {
                        window.location.reload();
                    }
                });
            }
        });
    }

    this.acceptMatch = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxAcceptMatch,
            data: { id: $("#MatchID").val() },
            success: function (data) {
                window.location.reload();
            }
        });
    }

    this.cancelMatch = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxCancelMatch,
            data: { id: $("#MatchID").val() },
            success: function (data) {
                window.location.reload();
            }
        });
    }

    this.replyAndAcceptInvoice = function ()
    {
        var ajaxData = $("#ReplyMessageForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxReplyMessage,
            data: ajaxData,
            success: function (data) {
                $.ajax({
                    type: "GET",
                    url: _this.ajaxAcceptInvoice,
                    data: { id: $("#GroupID").val() },
                    success: function (data) {
                        window.location.reload();
                    }
                });
            }
        });
    }

    this.acceptInvoice = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxAcceptInvoice,
            data: { id: $("#GroupID").val() },
            success: function (data) {
                window.location.reload();
            }
        });
    }

    this.cancelInvoice = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxCancelInvoice,
            data: { id: $("#SubjectID").val() },
            success: function (data) {
                window.location.reload();
            }
        });
    }
}


var messages;
$().ready(function () {
    messages = new Messages();
    messages.init();
});
