function Money()
{
    var _this = this;

    this.ajaxTournamentUserFee = "/Money/TournamentUserFee";
    this.ajaxTournamentUserReturn = "/Money/TournamentUserReturn";
    this.ajaxTournamentGroupReturn = "/Money/TournamentGroupReturn";
    this.ajaxUserToUser = "/Money/UserToUser";
    this.ajaxUserToGroup = "/Money/UserToGroup";
    this.ajaxGroupToUser = "/Money/GroupToUser";

    this.callback = null;
    this.init = function ()
    {
    }

    this.tournamentUserFee = function (tournamentID, callback)
    {
        this.callback = callback;
        $.ajax({
            type: "GET",
            url: _this.ajaxTournamentUserFee,
            data: { id: tournamentID },
            success: function (data)
            {
                if (data == "OK")
                {
                    return;
                }
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initPopup();
            }
        });
    }

    this.tournamentUserReturn = function (tournamentID, callback) {
        this.callback = callback;
        $.ajax({
            type: "GET",
            url: _this.ajaxTournamentUserReturn,
            data: { id: tournamentID },
            success: function (data) {
                if (data == "OK") {
                    return;
                }
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initPopup();
            }
        });
    }

    this.tournamentGroupReturn = function (tournamentID, callback) {
        this.callback = callback;
        $.ajax({
            type: "GET",
            url: _this.ajaxTournamentGroupReturn,
            data: { id: tournamentID },
            success: function (data) {
                if (data == "OK") {
                    return;
                }
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initPopup();
            }
        });
    }


    this.initPopup = function ()
    {
        $("#SubmitBtn").click(function () {
            var ajaxData = $("#MoneyPayForm").serialize();
            $.ajax({
                type: "POST",
                url: "/Money/" + $("#Action").val(),
                data: ajaxData,
                success: function (data)
                {
                    if (data.result == "ok")
                    {
                        if (_this.callback != null) {
                            _this.callback(data);
                        }
                    }
                }
            });
        });

        $("#ChargeBtn").click(function () {
            window.location = "/user/money#charge";
        });
    }

    this.userToUserShow = function (userID)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxUserToUser,
            data: { id: userID },
            success: function (data) {
                if (data == "OK") {
                    return;
                }
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initUserPopup();
            }
        });
    }

    this.userToGroupShow = function (groupID)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxUserToGroup,
            data: { id: groupID },
            success: function (data) {
                if (data == "OK")
                {
                    return;
                }
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initUserPopup();
            }
        });
    }

    this.groupToUserShow = function (groupID) {
        $.ajax({
            type: "GET",
            url: _this.ajaxGroupToUser,
            data: { id: groupID },
            success: function (data) {
                if (data == "OK") {
                    return;
                }
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                _this.initUserPopup();
            }
        });
    }

    this.initUserPopup = function ()
    {
        _this.updateMoneyType();
        _this.updateMoneyAmount();

        $("#Type").change(function () {
            _this.updateMoneyType();
        });

        $("#Sum").keyup(function (event) {
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
            _this.updateMoneyAmount();
        });

        $("#SubmitBtn").click(function () {
            var ajaxData = $("#MoneyPayForm").serialize();
            $.ajax({
                type: "POST",
                url: "/Money/" + $("#Action").val(),
                data: ajaxData,
                success: function (data) {
                    $(".gray-background").show();
                    $("#PopupWrapper").html(data);
                    $(".popup-window", $("#PopupWrapper")).centerInClient();
                    _this.initUserPopup();
                }
            });
            return false;
        });
    }

    this.updateMoneyType = function ()
    {
        if ($("#Type").val() == 1)
        {
            $("#TypeMoney").attr("class", "sprite icon-gold type-money");
        }
        if ($("#Type").val() == 2)
        {
            $("#TypeMoney").attr("class", "sprite icon-wood type-money");
        }
        if ($("#Type").val() == 3)
        {
            $("#TypeMoney").attr("class", "sprite icon-crystal type-money");
        }
        _this.updateMoneyAmount();

      
    }

    this.updateMoneyAmount = function ()
    {
        var amount = 0;
        if ($("#Sum").val() != "")
        {
            var sum = parseFloat($("#Sum").val());
            if ($("#Type").val() == 1) {
                var percent = parseFloat($("#Percent").val());
                amount = sum * (1 + percent / 100);
                amount = Math.round(amount * 100) / 100;
            } else {
                amount = sum;
            }
        } 
        $("#PayValue").text(amount);
        var max = 0;
        if ($("#Type").val() == 1) {
            max = parseFloat($("#MaxGold").val());
        }
        if ($("#Type").val() == 2) {
            max = parseFloat($("#MaxWood").val());
        }
        if ($("#Type").val() == 3) {
            max = parseFloat($("#MaxCrystal").val()); 
        }
        if (max < amount) {
            $(".not-enough-money").show();
            $("#SubmitBtn").attr("disabled", "disabled");
        } else {
            $(".not-enough-money").hide();
            $("#SubmitBtn").removeAttr("disabled");
        }
    }
}

var money = null;
$().ready(function ()
{
    money = new Money();
    money.init();
});
