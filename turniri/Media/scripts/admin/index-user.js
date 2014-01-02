function IndexUser() {

    var _this = this;

    this.ajaxChangeMoney = "/admin/User/ChangeMoney";

    this.init = function () {
        $(".change-money").click(function () {
            var id = $(this).data("id");
            _this.showChangeMoneyDialog(id);
            return false;
        });

        $("#ChangeMoneyBtn").live("click", function ()
        {
            _this.submitChangeMoney();
        });
    };

    this.showChangeMoneyDialog = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxChangeMoney,
            data: { id: id },
            success: function (data) {
                $("#popupWrapper").modal();
                $("#popupWrapper").html(data);
            }
        });
    };

    this.submitChangeMoney = function () {
        var ajaxData = $("#ChangeMoneyForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxChangeMoney,
            data: ajaxData,
            success: function (data) {
                $("#popupWrapper").html(data);
            }
        });
    };
}

var indexUser = null;
$().ready(function () {
    indexUser = new IndexUser();
    indexUser.init();
});