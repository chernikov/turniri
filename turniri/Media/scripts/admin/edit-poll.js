function EditPoll() {

    var _this = this;

    this.ajaxGetPollItem = "/admin/Poll/GetPollItem";

    this.init = function ()
    {
        $("#AddNewItem").click(function () {
            _this.addNewItem();
        });

        $(".poll-list .item .remove").live("click", function () {
            $(this).closest(".item").remove();
        });
    }

    this.addNewItem = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxGetPollItem,
            success: function (data) {
                $("#PollList").append(data);
            }
        });
    }
}


var editPoll = null;
$().ready(function () {
    editPoll = new EditPoll();
    editPoll.init();
});