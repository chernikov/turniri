function EditForumMessage() {

    var _this = this;

    this.AjaxCreateMessage = "/Forum/CreateMessage";
    this.AjaxRemoveMessage = "/Forum/RemoveMessage";
    this.AjaxEditMessage = "/Forum/EditMessage";
    this.AjaxItemMessage = "/Forum/Item";
    this.AjaxToggleNotice = "/Forum/ToggleForumNotice";

    this.messageItem = null;
    this.init = function () 
    {
        $(".answer-message").live("click", function () {
            var messageItem = $(this).closest(".message-item");
            var id = messageItem.attr("id").substring("ForumMessage_".length);
            _this.AddMessage(id);
        });

        $(".remove-message").live("click", function () {
            var messageItem = $(this).closest(".message-item");
            _this.RemoveMessage(messageItem);
        });

        $(".edit-message").live("click", function () {
            var messageItem = $(this).closest(".message-item");
            var id = messageItem.attr("id").substring("ForumMessage_".length);
            _this.EditMessage(id);
        });

        $("#EditMessageSubmit").live("click", function () {
            _this.EditMessageSubmit();
            return false;
        });
        $("#ForumMessageSubmit").live("click", function () {
            _this.MessageSubmit();
            return false;
        });

        $("#ToggleForumNotice").live("click", function () {
            _this.ToggleForumNotice();
            return false;
        });
    };

    this.AddMessage = function (parentId) {
        var ajaxData = {
            id: $("#ID").val(),
            parentId: parentId
        };
        $.ajax({
            type: "GET",
            url: _this.AjaxCreateMessage,
            data: ajaxData,
            success: function (data) {
                $("#MessageWrapper").html(data);
                $.scrollTo('#MessageWrapper', 1000);
            }
        });
    };

    this.MessageSubmit = function () {
        var ajaxData = $("#ForumMessageForm").serialize();
        
        $.ajax({
            type: "POST",
            url: _this.AjaxCreateMessage,
            data: ajaxData,
            beforeSend : function() {
                $("#ForumMessageSubmit").attr("disabled", true);
            },
            success: function (data) {
                $("#MessageWrapper").html(data);
            }
        });
    };

    this.RemoveMessage = function (messageItem) {
        var id = messageItem.attr("id").substring("ForumMessage_".length);
        $.ajax({
            type: "POST",
            url: _this.AjaxRemoveMessage,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    _this.UpdateItem(id);
                    
                }
            }
        });
    };

    this.EditMessage = function (id) {
        $.ajax({
            type: "GET",
            url: _this.AjaxEditMessage,
            data: { id: id },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                InitBbCodeEditor($("#EditMessage"));
            }
        });
    };

    this.EditMessageSubmit = function () {
        var ajaxData = $("#EditMessageForm").serialize();
        var id = $("#EditMessageID").val();
        $.ajax({
            type: "POST",
            url: _this.AjaxEditMessage,
            data: ajaxData,
            success: function (data) {
                if (data.result = "ok")
                {
                    _this.UpdateItem(id);
                    common.closePopup();
                }
            }
        });
    }

    this.UpdateItem = function (id) {
        $.ajax({
            type: "POST",
            url: _this.AjaxItemMessage,
            data: {id : id},
            success: function (data)
            {
                $("#ForumMessage_" + id).html(data);
                common.replaceQuote();
            }
        });
    }

    this.ToggleForumNotice = function () {
        
        $.ajax({
            type: "POST",
            url: _this.AjaxToggleNotice,
            data: { id: $("#ForumID").val() },
            success: function (data) {
                if (data.result == "ok") {
                    if (data.data == 1)
                    {
                        $("#ToggleForumNotice").addClass("active");
                    }
                    if (data.data == 0)
                    {
                        $("#ToggleForumNotice").removeClass("active");
                    }
                }
            }
        });
    }
}

var editForumMessage = null;
$().ready(function () {
    editForumMessage = new EditForumMessage();
    editForumMessage.init();
});