function CommonTournament() {
    var _this = this;

    this.ajaxAddTeam = "/Team/Add";
    this.ajaxRegisterTeam = "/Team/Register";
    this.ajaxSetTeamName = "/Team/SetName";
    this.ajaxUploadAvatar = "/Team/UploadAvatar";

    this.init = function () {

        $("#RegisterTeamButton").live("click", function () {
            _this.submitRegisterTeamForm();
            return false;
        });

        $("#SetTeamNameButton").live("click", function () {
            _this.submitSetTeamNameForm();
            return false;
        });

        $("#AcceptTournamentWithGame").live("click", function () {
            var id = $(this).data("id");
            var mainItem = $("#Tournament_" + id);
            item = $(".get-part-tournament", mainItem);

            _this.getPartTournament(item, id, true);
            return false;
        });
    };


    this.getPartTournament = function (item, id, force, guid)
    {
        if (force == undefined)
        {
            force = false;
        }
        if (guid == undefined)
        {
            guid = null;
        }
        var mainItem = $("#Tournament_" + id);
        $.ajax({
            type: "GET",
            url: "/Tournament/GetPart",
            data: { id: id, force : force, moneyGuid : guid },
            success: function (data)
            {
                if (data.result == "choise")
                {
                    $.ajax({
                        type: "GET",
                        url: "/Tournament/AddGame",
                        data: { id: id },
                        success: function (data) {
                            $(".gray-background").show();
                            $("#PopupWrapper").html(data);
                            $(".popup-window", $("#PopupWrapper")).centerInClient();
                        }
                    });
                }
                if (data.result == "need-money") 
                {
                    money.tournamentUserFee(id, function (data)
                    {
                        _this.getPartTournament(item, data.id, true, data.guid);
                    });
                }

                if (data.result == "return-money")
                {
                    money.tournamentUserReturn(id, function (data)
                    {
                        _this.getPartTournament(item, data.id, false, data.guid);
                    });
                }

                if (data.result == "return-group-money") {
                    money.tournamentGroupReturn(id, function (data) {
                        _this.getPartTournament(item, data.id, false, data.guid);
                    });
                }
                if (data.result == "ok")
                {
                    window.location.reload();
                }
                if (data.result == "error")
                {
                    ShowError(data.error);
                }
            }
        });
    }

    this.addTeamTournament = function (item, id) {
        var mainItem = $("#Tournament_" + id);
        $.ajax({
            type: "GET",
            url: _this.ajaxAddTeam,
            data: { id: id },
            success: function (data)
            {
                if (data.result == "ok") {
                    if (data.data == "register") {
                        _this.showRegisterTeam(id);
                    }
                    if (data.data == "remove") {
                        window.location.reload();
                    }
                }
                if (data.result == "error")
                {
                    ShowError(data.error);
                }
            }
        });
    }

    this.showRegisterTeam = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxRegisterTeam,
            data: { id: id },
            success: function (data)
            {
                _this.initTeamForm(data);
            }
        });
    }

    this.showSetTeamName = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxSetTeamName,
            data: { id: id },
            success: function (data) {
                _this.initTeamForm(data);
            }
        });
    }

    this.initTeamForm = function (data) {
        $(".gray-background").show();
        $("#PopupWrapper").html(data);
        $(".popup-window", $("#PopupWrapper")).centerInClient();

        var title = $(".UploadAvatar").text();
        InitUpload($(".UploadAvatar")[0],
            false,
            _this.ajaxUploadAvatar,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.uploadTeamAvatar(responseJSON.data);
                }
            },
            null, title);

    }

    this.uploadTeamAvatar = function (data) {
            $(".uploaded-image").html("<img src='" + data.ImagePath30 + "' />");
            $("#ImagePath18").val(data.ImagePath18);
            $("#ImagePath26").val(data.ImagePath26);
            $("#ImagePath30").val(data.ImagePath30);
    }

    this.submitRegisterTeamForm = function () 
    {
        var ajaxData = $("#RegisterTeamForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxRegisterTeam,
            data: ajaxData,
            success: function (data) {
                _this.initTeamForm(data);
            }
        });
    }


    this.submitSetTeamNameForm = function () {
        var ajaxData = $("#SetTeamNameForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxSetTeamName,
            data: ajaxData,
            success: function (data) {
                _this.initTeamForm(data);
            }
        });
    }

}

var commonTournament;
$().ready(function () {
    commonTournament = new CommonTournament();
    commonTournament.init();
});
