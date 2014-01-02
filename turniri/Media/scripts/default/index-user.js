function IndexUser() 
{
    var _this = this;
    this.ajaxUserPlatformGames = "/User/PlatformGames";
    this.ajaxUserPlayGame = "/User/PlayGame";
    this.ajaxUserStopGame = "/User/StopGame";
    this.ajaxUserGames = "/User/Games";
    this.ajaxUserVoteReputation = "/User/VoteReputation";
    this.ajaxUserVoteGrade = "/User/VoteGrade";
    this.ajaxAcceptMatch = "/Message/SubmitMatch";
    this.ajaxCancelMatch = "/Message/CancelMatch";
    this.ajaxGetChargePercent = "/Money/GetChargePercent";
    this.ajaxGetWithdrawPercent = "/Money/GetWithdrawPercent";
    this.ajaxMoneyRecharge = "/user/money-recharge";
    this.ajaxMoneyWithdraw = "/user/money-withdraw";

    this.init = function () {
        $(".platform_games").click(function () {
            var id = $(this).attr("id").substring("PlatformGames_".length);

            $.ajax({
                type: "GET",
                url: _this.ajaxUserPlatformGames,
                data: { id: id },
                success: function (data) {
                    $("#GameWrapper").html(data);
                    _this.initGameGallery();
                }
            });
        });

        $(".lets_play").live("click", function () {
            var id = $(this).closest(".gallery-item").attr("id").substring("Game_".length);
            $.ajax({
                type: "GET",
                url: _this.ajaxUserPlayGame,
                data: { id: id },
                success: function (data) {
                    $("#Game_" + id).hide();
                }
            });
        });

        $(".stop_play").live("click", function () {
            var id = $(this).closest(".gallery-item").attr("id").substring("Game_".length);
            $.ajax({
                type: "GET",
                url: _this.ajaxUserStopGame,
                data: { id: id },
                success: function (data) 
                {
                    if (data.result == "ok") {
                        $("#Game_" + id).remove();
                        _this.initGameGallery();
                    } else {
                        ShowError("Невозможно удалить игру, где вы участвуете в турнире, матче или команде");
                    }
                }
            });
        });

        $("#MyGames").click(function () {
            var id = $("#ID").val();
            $("#GameWrapper").load(_this.ajaxUserGames + "/" + id, function ()
            {
                _this.initGameGallery();
            });
        });

        $("#RemoveGames").click(function() {
            var id = $("#ID").val();
            $("#GameWrapper").load(_this.ajaxUserGames + "/" + id, function ()
            {
                _this.initGameGallery();
                $(".stop_play").show();
            });
        });

        _this.initReputation();

        $("#ReputationWrapper .canvote .icon-silver-star, #ReputationWrapper .canvote .icon-gold-star").live("click",
            function ()
            {
                _this.voteReputation($(this));
            });

        _this.initGameGallery();

        $(".icon-plus").live("click", function () {
            _this.vote(1);
        });
        $(".icon-minus").live("click", function () {
            _this.vote(-1);
        });

        $(".turnirs").each(function () {
            $(".switcher li:first", $(this)).click();
        });
       

        $(".show-match").live("click", function () {
            var id = $(this).closest(".match-item").attr("id").substring("Match_".length);
            match.ShowMatch(id);
        });

        $(".accept-match").live("click", function () {
            var id = $(this).closest(".match-item").attr("id").substring("Match_".length);
            _this.acceptMatch(id);
        });

        $(".decline-match").live("click", function () {
            var id = $(this).closest(".match-item").attr("id").substring("Match_".length);
            _this.declineMatch(id);
        });


        $(".make-friend").click(function () {
            var id = $(this).closest(".profile").attr("id").substring("User_".length);
            friends.addFriend(id);
        });

        $(".write-message").click(function () {
            var id = $(this).closest(".profile").attr("id").substring("User_".length);
            messages.writeMessageShow(id);
        });

        $(".write-fight-message").click(function () {
            var id = $(this).closest(".profile").attr("id").substring("User_".length);
            messages.writeFightMessageShow(id);
        });

        $(".write-invoice-message").click(function () {
            var id = $(this).closest(".profile").attr("id").substring("User_".length);
            messages.writeInvoiceMessageShow(id);
        });

        $(".send-money").click(function () {
            var id = $(this).closest(".profile").attr("id").substring("User_".length);
            money.userToUserShow(id);
        });

        if ($("#MatchID").length > 0) {
            var val = $("#MatchID").val();
            if (parseInt(val, 10) > 0) {
                match.ShowMatch(val);
            }
        }

        if ($("#MoneyListWrapper").length > 0)
        {
            $("#MoneyListWrapper .paging-list a").live("click", function () {
                var href = $(this).attr("href");

                $.ajax({
                    type: "GET",
                    url: href,
                    success: function (data) {
                        $("#MoneyListWrapper").html(data);
                    }
                });
                return false;
            });

            _this.initMoneyRecharge();
            _this.initMoneyWithdraw();

            if (window.location.hash = "#charge") {
                $("#tab_money_2").click();
            }
        }
    };


    
    this.initReputation = function () {
        $(".canvote .icon-silver-star, .canvote .icon-gold-star").hover(
            function () {
                var wrapper = $(this).parent();
                var index = $(this).index() + 1;
                $(".sprite:lt(" + index + ")", wrapper).addClass("icon-gold-important");
                $(".sprite:gt(" + (index - 1)+ ")", wrapper).addClass("icon-silver-important");
            },
            function () {
                var wrapper = $(this).parent();
                var index = $(this).index() + 1;
                $(".sprite:lt(" + index + ")", wrapper).removeClass("icon-gold-important");
                $(".sprite:gt(" + (index - 1) + ")", wrapper).removeClass("icon-silver-important");
            });
    }

    this.initGameGallery = function () {
        /* --- Game gallery --- */
        var total = 0;
        $('.gallery-item').each(function () {
            total += $(this).width() + 15;
        });
        _this.container().css({ "width": (total + 265) + "px" });

        if (total > 857) {
            _this.leftButton().show();
            _this.rightButton().show();
            _this.leftButton().unbind("click");
            _this.leftButton().click(function () {
                _this.moveRight();
            });
            _this.rightButton().unbind("click");
            _this.rightButton().click(function () {
                _this.moveLeft();
            });
        } else {
            _this.leftButton().hide();
            _this.rightButton().hide();
        }
    }
    
    this.container = function () {
        return $(".game-gallery-wrapper .inner-gallery-wrapper .conteiner");
    }
    this.leftButton = function () {
        return $(".game-gallery-wrapper .gallery-outer-wrapper .left-arrow");
    }

    this.rightButton = function () {
        return $(".game-gallery-wrapper .gallery-outer-wrapper .right-arrow");
    }

    this.updateMoneyAmount = function () {
        var amount = 0;
        if ($("#Sum").val() != "") {
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
        if (max < amount) {
            $(".not-enough-money").show();
            $("#SubmitBtn").attr("disabled", "disabled");
        } else {
            $(".not-enough-money").hide();
            $("#SubmitBtn").removeAttr("disabled");
        }
    }

    this.moveLeft = function () {
        var obj = $('.gallery-item', _this.container()).first();
        var objWidth = obj.width();
        _this.container().stop(true, true).animate({
            left: -1 * objWidth
        }, 500, function () {
            // Animation complete.
            obj.detach();
            _this.container().css({ "left": "0" });
            obj.appendTo(_this.container());
        });

    }

    this.moveRight = function () {

        var obj = $('.gallery-item', _this.container()).last();
        var objWidth = obj.width();
        obj.detach();
        _this.container().css({ "left": -1 * objWidth });
        obj.prependTo(_this.container());
        _this.container().stop().animate({
            left: 0
        }, 500);
    };

    this.voteReputation = function (item)
    {
        var typeStr = item.closest(".star-wrapper").data("type");
        var type = "";
        switch (typeStr)
        {
            case "connection":
                type = 1;
                break;
            case "honest":
                type = 2;
                break;
            case "responsibility":
                type = 3;
                break;
        }
        var ajaxData = {
            id: $("#ID").val(),
            mark : item.index() + 1, 
            type : type
        }
        $.ajax({
            type: "POST",
            url: _this.ajaxUserVoteReputation,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    _this.updateReputation();
                }
            }
        });
    }

    this.vote = function (grade)
    {
        var ajaxData = {
            id: $("#ID").val(),
            grade: grade
        }
        $.ajax({
            type: "POST",
            url: _this.ajaxUserVoteGrade,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    _this.updateReputation();
                }
            }
        });
    }

    this.updateReputation = function () {
        $("#ReputationWrapper").load("/User/Reputation/" + $("#ID").val(), function () {
            _this.initReputation();
        });
    }

    this.declineMatch = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxCancelMatch,
            data: { id: id },
            success: function (data) {
                window.location.reload();
            }
        });
    }

    this.acceptMatch = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxAcceptMatch,
            data: { id: id },
            success: function (data) {
                window.location.reload();
            }
        });
    }

    this.initMoneyRecharge = function ()
    {
        $("#Sum").keyup(function (event) {
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
            _this.updateSum();
        });

        $("#Provider").change(function () {
            _this.getPercent();
        });

        _this.getPercent();

        $("#MoneyRechargeBtn").click(function () {
            var ajaxData = $("#MoneyRechargeForm").serialize();
            $.ajax({
                type: "POST",
                url: _this.ajaxMoneyRecharge,
                data: ajaxData,
                success: function (data) {
                    $("#MoneyRechargeWrapper").html(data);
                    $("#Provider").change(function () {
                        _this.getPercent();
                    });
                    _this.getPercent();
                }
            });
            return false;
        });
    }

    this.initMoneyWithdraw = function () {

        $("#SumGold").keyup(function (event) {
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
            _this.updateSumGold();
        });

        $("#ProviderWithdraw").change(function () {
            _this.getPercentWithdraw();
        });

        $("#MoneyWithdrawBtn").click(function ()
        {
            if ($("#Account").val() == "")
            {
                $("#Account").addClass("input-validation-error");
                return false;
            }
            var ajaxData = $("#MoneyWithdrawForm").serialize();
            $.ajax({
                type: "POST",
                url: _this.ajaxMoneyWithdraw,
                data: ajaxData,
                success: function (data) {
                    $("#MoneyWithdrawWrapper").html(data);
                    _this.initMoneyWithdraw();
                }
            });
            return false;
        });

        _this.getPercentWithdraw();
    }

    this.getPercent = function () {
        var type = $("#Provider").val();

        $.ajax({
            type: "POST",
            url: _this.ajaxGetChargePercent,
            data: { type: type },
            success: function (data) {
                if (data.result == "ok")
                {
                    $("#Percent").val(data.percent);
                    _this.updateSum();
                }
            }
        })
    }

    this.getPercentWithdraw = function () {
        var type = $("#ProviderWithdraw").val();

        $.ajax({
            type: "POST",
            url: _this.ajaxGetWithdrawPercent,
            data: { type: type },
            success: function (data) {
                if (data.result == "ok") {
                    $("#PercentWithdraw").val(data.percent);
                    _this.updateSumGold();
                }
            }
        })
    }


    this.updateSumGold = function ()
    {
        var amount = 0;
        if ($("#SumGold").val() != "") {
            var sum = parseFloat($("#SumGold").val());
            var percent = parseFloat($("#PercentWithdraw").val());
            amount = sum * (1 - percent / 100);
            amount = Math.round(amount * 100) / 100;
        }
        $("#WithdrawValue").text(amount);
    }

    this.updateSum = function () {
        var amount = 0;
        if ($("#Sum").val() != "") {
            var sum = parseFloat($("#Sum").val());
            var percent = parseFloat($("#Percent").val());
            amount = sum * (1 - percent / 100);
            amount = Math.round(amount * 100) / 100;
        }
        $("#PayValue").text(amount);
    }
}

var indexUser = null;
$().ready(function () {
    indexUser = new IndexUser();
    indexUser.init();
});