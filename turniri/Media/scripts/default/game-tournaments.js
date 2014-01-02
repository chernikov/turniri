function GameTournaments() {
    var _this = this;

    this.ajaxGetPart = "/Tournament/GetPart";

    this.init = function ()
    {
        $(".turnir .switcher li:first").addClass("current");
        $(".turnir-switcher-content").hide();
        $(".turnir-switcher-content:first").show();

        $(".turnir-paging .paging-list a").live('click', function () {
            var href = $(this).attr("href");
            var wrapper = $(this).closest(".switcher-content");
            $.ajax({
                type: "GET",
                url: href,
                success: function (data) {
                    wrapper.html(data);
                }
            })
            return false;
        });

        $(".get-part-tournament").click(function () {
            var id = $(this).closest(".tournament-item").attr("id").substring("Tournament_".length);
            var item = $(this);
            $.ajax({
                type: "GET",
                url: _this.ajaxGetPart,
                data : {id : id},
                success: function (data) {
                    if (data.result == "ok") {
                        window.location.reload();
                    }
                    if (data.result == "error") {
                        ShowError(data.error);
                    }
                }
            })
            return false;
        });
    };
}


var gameTournaments;
$().ready(function () {
    gameTournaments = new GameTournaments();
    gameTournaments.init();
});
