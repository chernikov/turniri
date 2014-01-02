function LeagueMatches() {
    var ajaxChange = "/admin/League/ChangeMatchesParticipants";
    var _this = this;

    this.init = function () {
        if ($(".draggable").length > 0) {
            $(".draggable").draggable({
                start: function () {
                    $(this).css("z-index", "10");
                },
                stop: function () {
                    $(this).css("z-index", "1");
                }
            });
            $(".droppable").droppable({
                drop: function (event, ui) {
                    var toMatchId = $(this).data("id");
                    var toPlayer1 = $(this).data("player1");
                    var fromMatchId = ui.draggable.data("id");
                    var fromPlayer1 = ui.draggable.data("player1");

                    $.ajax({
                        type: "POST",
                        url: ajaxChange,
                        data: {
                            fromMatchId: fromMatchId,
                            fromPlayer1: fromPlayer1,
                            toMatchId: toMatchId,
                            toPlayer1: toPlayer1
                        },
                        success: function (data) {
                            window.location.reload();
                        },
                        error: function () {
                            alert("Что-то не то");
                        }
                    });
                }
            });
        }
    }
}

var leagueMatches = null;
$().ready(function () {
    leagueMatches = new LeagueMatches();
    leagueMatches.init();
});