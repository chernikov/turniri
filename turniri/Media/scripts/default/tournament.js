function Tournament() {
    var _this = this;

    this.ajaxTour = "/Tournament/Tours";
    this.ajaxGroup = "/Tournament/Group";
    this.ajaxStatisticRoundRobin = "/Tournament/StatisticRoundRobin";
    this.ajaxStatisticGroup = "/Tournament/StatisticGroup";
    this.ajaxGroupPart = "/Tournament/GroupPart";
    this.ajaxPlayoffPart = "/Tournament/PlayoffPart";
    this.ajaxTakePartTeam = "/Team/TakePart";
    this.ajaxTakeOffPartTeam = "/Team/TakeOffPart";
    this.ajaxAcceptUser = "/Team/AcceptUser";
    this.ajaxAddUser = "/Team/AddUser";
    this.ajaxDeclineUser = "/Team/DeclineUser";
    this.ajaxCloseTeam = "/Team/Close";
    this.ajaxReplaceUserInRoster = "/Team/ReplaceUser";


    this.teamOffUser = null;

    this.init = function () {
        $(".icon-info, .group-match-item").live("click", function ()
        {
            var id = $(this).closest(".match-item").attr("id").substring("Match_".length);
            match.ShowMatch(id);
        });
       
        if ($("#GroupShow").length > 0) {
            $("#TournamentGroupID").val($("#GroupShow").val());
            _this.showGroup($("#GroupShow").val());
        }

        if ($("#PlayOff").length > 0) {
            if ($("#PlayOff").val()) {
                _this.showPlayOff();
            }
        }

        if ($("#RulesOn").length > 0) {
            if ($("#RulesOn").val()) {
                $("#rules").show();
            }
        }

        if ($("#MatchShow").length > 0) {
            var matchId = $("#MatchShow").val();
            match.ShowMatch(matchId);
        }

        $("#switchRules").live("click", function () {
            $("#rules").toggle();
        });

        $("#switchDescription").live("click", function () {
            $("#description").toggle();
        });

        if ($("#TourID").length > 0) {
            $("#TourID").change(function () {
                var id = $("#TourID").val();

                $.ajax({
                    type: "GET",
                    url: _this.ajaxTour,
                    data: { id: id },
                    success: function (data) {
                        $("#Tours").html(data);
                    }
                });
            });
        }

        $("#StatisticCommon").live("click", function () {
            _this.showStatistic(null);
        });
        $("#StatisticHome").live("click", function () {
            _this.showStatistic(true);
        });
        $("#StatisticGuest").live("click", function () {
            _this.showStatistic(false);
        });

        $("#StatisticGroupCommon").live("click", function () {
            _this.showStatisticGroup(null);
        });
        $("#StatisticGroupHome").live("click", function () {
            _this.showStatisticGroup(true);
        });
        $("#StatisticGroupGuest").live("click", function () {
            _this.showStatisticGroup(false);
        });

        _this.initGroup();
        $("#GroupPartButton").live("click", function () {
            $.ajax({
                type: "GET",
                url: _this.ajaxGroupPart,
                data: { id: $("#TournamentID").val() },
                success: function (data) {
                    $("#TablePartWrapper").html(data);
                    _this.initGroup();
                    _this.showGroup($("#TournamentGroupID").val());
                }
            });
        });

        $("#PlayoffPartButton").live("click", function () {
            _this.showPlayOff();
        });

        $(".get-part-tournament").click(function () {
            var id = $("#TournamentID").val();
            var item = $(this);
            commonTournament.getPartTournament(item, id);
            return false;
        });

        $(".register-team-tournament").click(function () {
            var id = $("#TournamentID").val();
            var item = $(this);
            commonTournament.addTeamTournament(item, id);
            return false;
        });

        $(".set-team-name-tournament").click(function () {
            var id = $("#TournamentID").val();
            commonTournament.showSetTeamName(id);
            return false;
        });


        $(".take-part").click(function () {
            var id = $(this).data("id");

            _this.takePart(id);
        });
       
        $(".take-off-part").click(function () {
            var id = $(this).data("id");

            _this.takeOffPart(id);
        });

        $(".close-team").live("click", function () {
            _this.closeTeam($(this));
        });

        $(".accept-user-item").live("click", function () {
            _this.acceptUserItem($(this));
        });

        $(".decline-user-item").live("click", function () {
            _this.declineUserItem($(this));
        });

        $(".add-user-item").live("click", function () {
            _this.addUserItem($(this));
        });

        $(".off-user-item").live("click", function ()
        {
            $(".cancel-off-user-item").addClass("hidden");
            $(".on-user-item").addClass("hidden");
            _this.teamOffUser = $(this).data("id");
            $(".off-user-item").addClass("hidden");
            $(".on-user-item").removeClass("hidden");
            $(".cancel-off-user-item", $(this).closest(".action")).removeClass("hidden");
            if ($("#IsGroup").val() == "False")
            {
                team.ReplaceMode = true;
                $("#SelectPlayerWrapper").removeClass("hidden");
            }
        });

        $(".cancel-off-user-item").live("click", function () {
            $(".cancel-off-user-item").addClass("hidden");
            $(".off-user-item").removeClass("hidden");
            $(".on-user-item").addClass("hidden");
            _this.teamOffUser = null;

            if ($("#IsGroup").val() == "False") {
                team.ReplaceMode = false;
                $("#SelectPlayerWrapper").addClass("hidden");
            }
        });

        $(".on-user-item").live("click", function () {
            if (_this.teamOffUser != null)
            {
                _this.replaceUserTeam($(this).data("id"), _this.teamOffUser);
            }
        });

       
    };


    this.initGroup = function () {
        if ($("#TournamentGroupID").length > 0) {
            $("#TournamentGroupID").change(function () {
                _this.showGroup($(this).val());
            });
        }
    }

    this.showStatistic = function (type) {
        var id = $("#StatisticID").val();

        $.ajax({
            type: "GET",
            url: _this.ajaxStatisticRoundRobin,
            data: { id: id, HomeGuest: type },
            success: function (data) {
                $("#StatisticWrapper").html(data);
            }
        });
    };

    this.showStatisticGroup = function (type) {
        var id = $("#StatisticGroupID").val();
        $.ajax({
            type: "GET",
            url: _this.ajaxStatisticGroup,
            data: { id: id, HomeGuest: type },
            success: function (data) {
                $("#StatisticWrapper").html(data);
            }
        });
    };

    this.showGroup = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxGroup,
            data: { id: id },
            success: function (data) {
                $("#GroupWrapper").html(data);

                common.updateScrollBars();
                var tournamentUrl = $("#TournamentUrl").val();
                if (typeof (tournamentUrl) != "undefined") {
                    common.rewriteUrl("/tournament/" + tournamentUrl + "/?groupID=" + id);
                }
            }
        });

        $.ajax({
            type: "GET",
            url: _this.ajaxStatisticGroup,
            data: { id: id },
            success: function (data) {
                $("#StatisticWrapper").html(data);
                $('.scroll-pane', $("#StatisticWrapper")).jScrollPane({ autoReinitialise: true });
            }
        });
    }

    this.showPlayOff = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxPlayoffPart,
            data: { id: $("#TournamentID").val() },
            success: function (data) {
                $("#TablePartWrapper").empty();
                $("#TablePartWrapper").html(data);
                common.updateScrollBars();
                $("#StatisticWrapper").empty();
                var tournamentUrl = $("#TournamentUrl").val();
                if (typeof (tournamentUrl) != "undefined") {
                    common.rewriteUrl("/tournament/" + TournamentUrl + "/?playOff=true");
                }
            }
        });
    }

    this.takePart = function (id) {
        $.ajax({
            type: "POST",
            url: _this.ajaxTakePartTeam,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
            }
        });
    }

    this.takeOffPart = function (id) {
        $.ajax({
            type: "POST",
            url: _this.ajaxTakeOffPartTeam,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
            }
        });
    }

    this.closeTeam = function (item)
    {
        var id = item.data('id');
        $.ajax({
            type: "POST",
            url: _this.ajaxCloseTeam,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    team.showTeam(id);
                }
            }
        });
    }

    this.acceptUserItem = function (item) {
        var id = item.data('id');
        $.ajax({
            type: "POST",
            url: _this.ajaxAcceptUser,
            data: { id: id },
            success: function (data) {
                team.showTeam($("#TeamID").val());
            }
        });
    }

    this.declineUserItem = function (item) {
        var id = item.data('id');
        $.ajax({
            type: "POST",
            url: _this.ajaxDeclineUser,
            data: { id: id },
            success: function (data) {
                team.showTeam($("#TeamID").val());
            }
        });
    }

    this.addUserItem = function (item)
    {
        var id = item.data('id');
        var teamId = item.data('team');
        $.ajax({
            type: "POST",
            url: _this.ajaxAddUser,
            data: { id: id, teamId : teamId },
            success: function (data) {
                team.showTeam($("#TeamID").val());
            }
        });
    }

    this.replaceUserTeam = function (inId, offId)
    {
        $.ajax({
            type : "GET", 
            url: _this.ajaxReplaceUserInRoster,
            data: { inId: inId, offId: offId },
            success: function (data) {
                team.showTeam($("#TeamID").val());
            }
        });
    }
}

var tournament;
$().ready(function () {
    tournament = new Tournament();
    tournament.init();
});
