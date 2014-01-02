function EditForumMessage() {

    var _this = this;

    this.AjaxCreateMessage = "/admin/ForumMessage/Create";
    this.AjaxEditMessage = "/admin/ForumMessage/Edit";
    this.AjaxDeleteMessage = "/admin/ForumMessage/Delete";

    this.messageItem = null;
    this.init = function () {
        $("#AddMessage").click(function () {
            _this.ShowMessageDialog();
        });

        $(".answer-message").click(function () {
            var id = $(this).data("id");
            _this.ShowMessageDialog(id);
        });

        $(".edit-message").click(function () {
            var id = $(this).data("id");
            _this.ShowEditMessageDialog(id);
        });

        $(".remove-message").click(function () {
            var messageItem = $(this).closest("tr");
            var id = $(this).data("id");
            _this.RemoveMessage(messageItem, id);
        });

        $("#ForumMessageSubmit").live("click", function () {
            _this.MessageSubmit();
            return false;
        });
    };

    this.ShowMessageDialog = function (parentId)
    {
        var ajaxData = {
            id: $("#ID").val(),
            parentId: parentId
        };
        $.ajax({
            type: "GET",
            url: _this.AjaxCreateMessage,
            data: ajaxData,
            success: function (data) {
                $("#newMessageWrapper").modal();
                $("#newMessageWrapper").html(data);
            }
        });
    };

    this.ShowEditMessageDialog = function (id)
    {
        var ajaxData = {
            id: id
        };
        $.ajax({
            type: "GET",
            url: _this.AjaxEditMessage,
            data: ajaxData,
            success: function (data) {
                $("#newMessageWrapper").modal();
                $("#newMessageWrapper").html(data);
            }
        });
    };

    this.MessageSubmit = function ()
    {
        var ajaxData = $("#ForumMessageForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.AjaxEditMessage,
            data: ajaxData,
            beforeSend: function () {
                $("#ForumMessageSubmit").attr("disabled", true);
            },
            success: function (data) {
                $("#newMessageWrapper").html(data);
            }
        });
    }

    this.RemoveMessage = function (messageItem, id)
    {
        $.ajax({
            type: "POST",
            url: _this.AjaxDeleteMessage,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok")
                {
                    messageItem.remove();
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