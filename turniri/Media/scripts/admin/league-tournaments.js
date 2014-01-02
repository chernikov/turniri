function LeagueTournaments()
{
    var _this = this;

    this.init = function ()
    {
        $(".AddTournaments").click(function () {
            _this.showAddTournaments($(this).data("id"), $(this).data("season"));
        });

        $(".RemoveTournaments").click(function () {
            _this.removeTournaments($(this).data("id"), $(this).data("season"));
        });

        $("#AddTournamentsBtn").click(function () {
            _this.addTournaments();
        });
    }

    this.showAddTournaments = function (id, seasonId)
    {
        var ajaxData = {
            id: id,
            seasonId : seasonId
        };
        $.ajax({
            type: "GET",
            url: "/admin/League/AddTournaments",
            data: ajaxData,
            success: function (data)
            {
                $("#addTournamentModal").modal();
                $("#addTournamentModalWrapper").html(data);
            }
        });
    }

    this.addTournaments = function ()
    {
        var ajaxData = $("#AddTournamentsForm").serialize();
        $.ajax({
            type: "POST",
            url: "/admin/League/AddTournaments",
            data: ajaxData,
            success: function (data) {
                $("#addTournamentModal").modal();
                $("#addTournamentModalWrapper").html(data);
            }
        });
    }

    this.removeTournaments = function (id, seasonId) {
        var ajaxData = {
            id: id,
            seasonId: seasonId
        };
        $.ajax({
            type: "POST",
            url: "/admin/League/RemoveTournaments",
            data: ajaxData,
            success: function (data)
            {
                window.location.reload();
            }
        });
    }

}

var leagueTournaments = null;
$().ready(function () {
    leagueTournaments = new LeagueTournaments();
    leagueTournaments.init();
});