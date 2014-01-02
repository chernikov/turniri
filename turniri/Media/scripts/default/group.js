function Group() {
  
    var _this = this;

    this.init = function ()
    {
        $(".send-money").click(function () {
            var id = $("#ID").val();
            money.userToGroupShow(id);
        });

        $(".send-money-to-player").click(function () {
            var id = $("#ID").val();
            money.groupToUserShow(id);
        });

        if ($("#MoneyListWrapper").length > 0) {
            $("#MoneyListWrapper .paging-list a").live("click", function () {
                var href = $(this).attr("href");

                $.ajax({
                    type: "GET",
                    url: href,
                    success: function (data) {
                        $("#MoneyListWrapper").html(data);
                    }
                });
                return false;
            });
        }

    };
}

var group = null;
$().ready(function () {
    group = new Group();
    group.init();
});