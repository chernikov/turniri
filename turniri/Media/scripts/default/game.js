function Game() {
    var _this = this;

    this.init = function ()
    {
        $(".turnirs .switcher li:first").addClass("current");
        $(".turnirs-wrapper .switcher-content").hide();
        $(".turnirs-wrapper .switcher-content:first").show();

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
            commonTournament.getPartTournament(item, id);
            return false;
        });
    };
}


var game;
$().ready(function () {
    game = new Game();
    game.init();
});
