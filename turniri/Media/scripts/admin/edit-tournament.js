function EditTournament() {

    var _this = this;

    this.ajaxUploadTournamentImage = "/admin/File/UploadTournamentImage";
    this.ajaxCreateGamesSelect = "/admin/Tournament/CreateGamesSelect";
    this.ajaxSelectUser = "/admin/User/SelectUser";
    this.ajaxSelectPlayer = "/admin/User/SelectPlayer";
    this.ajaxGetTournamentForum = "/admin/Tournament/GetForum";

    this.init = function ()
    {
        var item = $(".html-description");
        InitBbCodeEditor(item);

        $("#PlatformID").change(function () {
            _this.updateGames();
        });

        if ($("#ID").val() == "0" && ($("#GameID").val() == "0" || $("#GameID").val() == null)) {
            _this.updateGames();
        }

        $("#TournamentType").change(function () {
            _this.updateTournamentType();
        });
        _this.updateTournamentType();

        $("#CountRound").change(function () {
            _this.updateCountRound();
        });
        _this.updateCountRound();

        $("#DeletePreview").click(function () {
            _this.deletePreview();
        });

        $("#ForumWrapper select").change(function () {
            _this.updateForumTopic($(this));
        });

        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadTournamentImage,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
            },
            null, titlePreview);

        $("#IsLive").change(function () {
            _this.updatePlace();
        });

        $("#GroupTeam").change(function () {
            _this.updateTeam();
        });

        $("#MoneyType").change(function () {
            _this.updateFee();
        });

        _this.initUserSelect();
        _this.updatePlace();
        _this.updateTeam();
        _this.updateFee();
    };

    this.updateGames = function () {
        var id = $("#PlatformID").val();
        $.ajax({
            type: "POST",
            url: _this.ajaxCreateGamesSelect,
            data: { idPlatform : id },
            success: function (data) {
                $("#GameWrapper").html(data);
            }
        });
    };

    this.updateCountRound = function () {
        if ($("#CountRound").val() == "3") {
            $("#HostGuestWrapper").show();
            $("#DoubleGoalInGuestWrapper").show();
        } else {
            $("#HostGuestWrapper").hide();
            $("#DoubleGoalInGuestWrapper").hide();
        }
    };

    this.updateTournamentType = function ()
    {
        if ($("#TournamentType").val() == "4") 
        {
            $("#GroupWrapper").show();
            $("#NonGroupWrapper").show();
        } else {
            $("#GroupWrapper").hide();
            $("#NonGroupWrapper").show();
        }
    };

    this.updateForumTopic = function (item) {
        var value = item.val();
        var ajaxData = {
            id: value
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxGetTournamentForum,
            data: ajaxData,
            success: function (data) {
                $("#ForumWrapper").html(data);
                $("#ForumWrapper select").change(function () {
                    _this.updateForumTopic($(this));
                });
            },
            error: function () {
                alert("Внутренняя ошибка");
            }
        });
    };
    
    this.deletePreview = function() {
        $("#PreviewImage").attr("src", "/Media/images/default_game103.png");
        $("#ImagePath").val("");
    };

    this.ChangePreview = function(data) {
        $("#PreviewImage").attr("src", data.ImagePath);
        $("#ImagePath").val(data.ImagePath);
    };

    this.initUserSelect = function () {
        $("#Moderators,#Admins").each(function (i, item) {
            $(this).ajaxChosen({
                    method: 'GET',
                    url: _this.ajaxSelectUser,
                    dataType: 'json',
                    minTermLength: 2,
                    afterTypeDelay: 300,
                    keepTypingMsg: "Продолжайте печатать...",
                    lookingForMsg: "Ищу...",
                }, function(data) {
                    var terms = { };
                    if (data.result == "ok") {
                        $.each(data.data, function(i, val) {
                            terms[val.id] = val.login;
                        });
                    }
                    return terms;
                });
        });

        $("#Players").each(function (i, item) {
            $(this).ajaxChosen({
                method: 'GET',
                url: _this.ajaxSelectPlayer,
                dataType: 'json',
                minTermLength: 2,
                afterTypeDelay: 300,
                keepTypingMsg: "Продолжайте печатать...",
                lookingForMsg: "Ищу...",
                dataCallback: function (data) {
                    return $.extend(data,
                    {
                        GameID: $("#GameID").val(),
                        GroupTeam: $("#GroupTeam").val(),
                    });
                }
            }, function (data) {
                var terms = {};
                if (data.result == "ok") {
                    $.each(data.data, function (i, val) {
                        terms[val.id] = val.login;
                    });
                }
                return terms;
            });
        });
    };
    
    this.updatePlace = function () {
        if ($("#IsLive:checked").length == 1) {
            $("#PlaceWrapper").show();
        } else {
            $("#PlaceWrapper").hide();
        }
    }

    this.updateTeam = function () {
        if ($("#GroupTeam").val() > 1)
        {
            $("#TeamCountWrapper").show();
            $("#HotReplacementWrapper").show();
        } else {
            $("#TeamCountWrapper").hide();
            $("#HotReplacementWrapper").hide();
        }
    }

    this.updateFee = function () {
        if ($("#MoneyType").val() > 1)
        {
            $("#FeeWrapper").show();
        } else {
            $("#FeeWrapper").hide();
        }
    }
}

var editTournament = null;
$().ready(function () {
    editTournament = new EditTournament();
    editTournament.init();
});