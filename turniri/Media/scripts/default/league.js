function League() {
    var _this = this;

    this.init = function () {
        $(".psiswitcher li:first").addClass("current");

        $(".levelContentSwitch").click(function () {

            $.ajax({
                type: "GET",
                url: "/League/Level",
                data: { id: $(this).data("id") },
                success: function (data) {
                    $("#LeagueWrapper").html(data);
                }
            })
        });


        $(".report").click(function () {
            var id = $(this).attr("id").substring("Match_".length);
            match.ShowMatch(id);
        });
    };
}

var league = null;
$().ready(function () {
    league = new League();
    league.init();
});