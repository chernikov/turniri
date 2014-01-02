function LiveTournament() {
    var _this = this;

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
            commonTournament.getPartTournament(item, id);
            return false;
        });
    };
}


var liveTournament;
$().ready(function () {
    liveTournament = new LiveTournament();
    liveTournament.init();
});
