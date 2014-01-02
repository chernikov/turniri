function Team() {
    var _this = this;

    this.ajaxShowTeam = "/Team/Index";
    this.ajaxUserAutoComplete = "/Team/AutocompleteUser"
    this.ajaxAddPlayer = "/Team/AddPlayer";
    this.ajaxReplaceUser = "/Team/ReplaceUser";

    this.replaceMode = false;

    this.init = function ()
    {
        $(".show-team").live("click", function () {
            var id = $(this).data("id");
            _this.showTeam(id);
            return false;
        });

        $(".add-player").live("click", function () {
            if ($("#SelectPlayerWrapper").hasClass("hidden")) {
                $("span", $(this)).text("Отмена");
            } else {
                $("span", $(this)).text("Добавить");
            }
            _this.ReplaceMode = false;
            $("#SelectPlayerWrapper").toggleClass("hidden");
        });

        $("#SelectPlayerButton").live("click", function () {
            if (team.ReplaceMode)
            {
                _this.replacePlayer();
            } else {
                _this.addPlayer();
            }
        });
    };

    this.initPopup = function ()
    {
        $("#SelectPlayer").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxUserAutoComplete,
                    data: {
                        query: request.term,
                        gameID: $("#GameID").val()
                    },
                    success: function (data) {
                        response($.map(data.data, function (item) {
                            return {
                                label: item.Label,
                                value: item.Label,
                                id: item.ID
                            }
                        }));
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $("#SelectPlayer").val(ui.item.Label);
                $("#SelectPlayerID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    }

    this.showTeam = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxShowTeam,
            data: { id: id },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $('.team-popup-window .scroll-pane').jScrollPane({ autoReinitialise: true });
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initPopup();
            }
        });
    }

    this.addPlayer = function () {
        var ajaxData =
        {
            UserID: $("#SelectPlayerID").val(),
            TeamID: $("#TeamID").val()
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxAddPlayer,
            data: ajaxData,
            success: function (data)
            {
                if (data.result == "ok")
                {
                    _this.showTeam($("#TeamID").val());
                } else {
                    $(".error", $("#SelectPlayerWrapper")).text(data.error);
                }
            }
        });
    }

  

    this.replacePlayer = function ()
    {
        var ajaxData =
           {
               inId: $("#SelectPlayerID").val(),
               offId: tournament.teamOffUser
           };
        $.ajax({
            type: "POST",
            url: _this.ajaxReplaceUser,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    _this.showTeam($("#TeamID").val());
                } else {
                    $(".error", $("#SelectPlayerWrapper")).text(data.error);
                }
            }
        });
    }
}

var team;
$().ready(function () {
    team = new Team();
    team.init();
});
