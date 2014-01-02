function LeagueGroup() {
    var _this = this;

    this.init = function () {
        _this.initRow($("body"));
    }

    this.initRow = function (parent) {
        $(".participant-level", parent).change(function () {
            _this.updateGroupLeagueParticipant($(this));
        });

        $(".tournament-season", parent).change(function () {
            _this.updateGroupLeagueTournament($(this));
        });
    }


    this.updateGroupLeagueParticipant = function (item) {
        var id = item.data("id");
        var ajaxData =
        {
            id: item.data("id"),
            levelId: item.val(),
            seasonId: $("#SeasonID").val()
        };
        $.ajax({
            type: "POST",
            url: "/admin/League/SetGroupLeagueParticipant",
            data: ajaxData,
            success: function (data)
            {
                if (data.result == "error")
                {
                    alert(data.message);
                }
                _this.updateGroupRow(id);
            },
            error: function () {
                window.location.reload();
            }
        });
    }

    this.updateGroupLeagueTournament = function (item)
    {
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
            url: "/admin/League/SetLeagueGroupParticipantTournament",
            data: ajaxData,
            success: function (data) {
                if (data.result == "error")
                {
                    alert(data.message);
                }
                _this.updateGroupRow(id);
            },
            error: function () {
                window.location.reload();
            }
        });
    }

    this.updateGroupRow = function (id) {
        
        var ajaxData =
            {
            id: id,
            seasonId: $("#SeasonID").val(),
        };
        $.ajax({
            type: "GET",
            url: "/admin/League/GroupRow",
            data: ajaxData,
            success: function (data)
            {
                $("#GroupRow_" + id).html(data);
                _this.initRow($("#GroupRow_" + id));
            },
            error: function ()
            {
                window.location.reload();
            }
        });
    }
}

var leagueGroup;
$().ready(function () {
    leagueGroup = new LeagueGroup();
    leagueGroup.init();
});