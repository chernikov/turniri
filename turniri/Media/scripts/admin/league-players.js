function LeaguePlayers() {
    _this = this;

    this.init = function () {
        _this.initRow($("body"));
    }

    this.initRow = function (parent) {
        $(".participant-level", parent).change(function () {
            _this.updateUserLeagueParticipant($(this));
        });

        $(".tournament-season", parent).change(function () {
            _this.updateUserLeagueTournament($(this));
        });
    }

    this.updateUserLeagueParticipant = function (item)
    {
        var id = item.data("id");
        var ajaxData =
        {
            id: item.data("id"),
            levelId: item.val(),
            seasonId: $("#SeasonID").val()
        };
        $.ajax({
            type: "POST",
            url: "/admin/League/SetUserLeagueParticipant",
            data: ajaxData,
            success: function (data) {
                if (data.result == "error") {
                    alert(data.message);
                }
                _this.updatePlayerRow(id);
            }
        });
    }

    this.updateUserLeagueTournament = function (item) {
        var id = item.data("id");
        var ajaxData =
        {
            id: id,
            levelId: item.data("level-id"),
            tournamentId: item.val(),
            seasonId: $("#SeasonID").val(),
        };
        $.ajax({
            type: "POST",
            url: "/admin/League/SetLeagueParticipantTournament",
            data: ajaxData,
            success: function (data) {
                if (data.result == "error") {
                    alert(data.message);
                }
                _this.updatePlayerRow(id);
            },
            error: function () {
                window.location.reload();
            }
        });
    }

    this.updatePlayerRow = function (id)
    {
        var ajaxData = {
            id: id,
            seasonId: $("#SeasonID").val(),
        };
        $.ajax({
            type: "GET",
            url: "/admin/League/PlayerRow",
            data: ajaxData,
            success: function (data) {
                $("#PlayerRow_" + id).html(data);
                _this.initRow($("#PlayerRow_" + id));
            },
            error: function () {
                window.location.reload();
            }
        });
    }
}

var leaguePlayers = null;
$().ready(function () {
    leaguePlayers = new LeaguePlayers();
    leaguePlayers.init();
});