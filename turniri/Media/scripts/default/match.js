function Match() {
    var _this = this;

    this.ajaxGetMatch = "/Match/Index";
    this.ajaxUserTournamentReputation = "/User/TournamentReputation";
    this.ajaxUserVoteReputation = "/User/VoteReputation";
    this.ajaxPublishRound = "/Round/Publish";
    this.ajaxSubmitRound = "/Round/Submit";
    this.ajaxSubmitTechMatch = "/Round/TechSubmit";
    this.ajaxSubmitTechAllLoser = "/Round/TechAllLoserSubmit";
    this.ajaxDisputeRound = "/Round/Dispute";
    this.ajaxUploadImage = "/Comment/UploadImage";
    this.ajaxCreateMatchComment = "/Match/CreateComment";
    var ajaxDeleteComment = "/Comment/Delete";
    var ajaxRollbackRound = "/Round/RollbackRound";
    var ajaxRollbackMatch = "/Match/RollbackMatch";

    this.init = function () {
        $(".send-match-comment").live("click", function () {
            _this.sendMatchComment($(this));
        });

        $(".comment-list li .close").live("click", function () {
            _this.RemoveComment($(this));
        });

        $("#PopupWrapper .canvote .icon-silver-star,#PopupWrapper .canvote .icon-gold-star").live("click",
           function () {
               _this.voteReputation($(this));
           });
    };

    this.ShowMatch = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxGetMatch,
            data: { id: id },
            success: function (data) {
                $("#PopupWrapper").html(data);
                 var tournamentUrl = $("#TournamentUrl").val();
                if (typeof (tournamentUrl) != "undefined") {
                    common.rewriteUrl("/tournament/" + tournamentUrl + "?matchID=" + id);
                } else {
                    var url = window.location.href;
                    var indexOf = url.indexOf("?");
                    if (indexOf > -1) {
                        url = url.substring(0, indexOf) + "?matchID=" + id;
                    } else {
                        url += "?matchID=" + id;
                    }
                    common.rewriteUrl(url);
                }
                _this.initPopup();
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                $(".gray-background").show();
            }
        });
    }

    this.initPopup = function () {
        _this.initReputation();

        $(".set-winner").click(function () {
            _this.setWin($(this).attr("id").substring("Winner_".length));
        });

        $(".set-tech-winner").click(function () {
            var player1 = $(this).data("player1");
            _this.setTechWin(player1);
        });

        $(".set-tech-lose").click(function () {
            _this.setTechAllLose();
        });

        $(".PublishResult").click(function () {
            _this.publishResult($(this).attr("id").substring("Publish_".length));
        });
        $(".SubmitResult").click(function () {
            _this.submitResult($(this).attr("id").substring("Submit_".length));
        });
        $(".DisputeResult").click(function () {
            _this.disputeResult($(this).attr("id").substring("Dispute_".length));
        });

        $('.comment-list.scroll-pane').jScrollPane({ autoReinitialise: true });

        $(".rollback-round").click(function () {
            _this.RollbackRound($(this));
        });

        $(".rollback-match").click(function () {
            _this.RollbackMatch($(this));
        });

        $(".screenshot").loupe();
        _this.initMatchComment();
    }

    this.initMatchComment = function () {
        if ($(".UploadMatchImage").length > 0) {
            var title = $(".UploadMatchImage").text();
            InitUpload($(".UploadMatchImage")[0],
                false,
                _this.ajaxUploadImage,
                function (id, fileName, responseJSON) {
                    if (responseJSON.result == "ok") {
                        _this.uploadMatchImage(responseJSON.data);
                    }
                },
                null, title);
        }
    }

    this.initReputation = function () {
        $(".canvote .stars .sprite").unbind('mouseenter mouseleave');
        $(".canvote .stars .sprite").hover(
            function () {
                var wrapper = $(this).parent();

                var before = $(this).index() + 1;
                var after = $(this).index();
                $(".sprite:lt(" + before + ")", wrapper).addClass("icon-gold-important");
                $(".sprite:gt(" + after + ")", wrapper).addClass("icon-silver-important");
            },
            function () {
                var wrapper = $(this).parent();
                var before = $(this).index() + 1;
                var after = $(this).index();
                $(".sprite:lt(" + before + ")", wrapper).removeClass("icon-gold-important");
                $(".sprite:gt(" + after + ")", wrapper).removeClass("icon-silver-important");
            });
    }

    this.voteReputation = function (item) {
        var typeStr = item.closest(".stars").data("type");

        var type = "";
        switch (typeStr) {
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
        var userId = item.closest(".reputation").attr("id").substring("User_".length);
        var senderId = $(".user-sender", item.closest(".reputation")).attr("id").substring("SenderUser_".length);
        var ajaxData = {
            id: userId,
            mark: item.index() + 1,
            type: type
        }
        $.ajax({
            type: "POST",
            url: _this.ajaxUserVoteReputation,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    $.ajax({
                        type: "GET",
                        url: _this.ajaxUserTournamentReputation,
                        data: { id: userId, userID: senderId },
                        success: function (data) {
                            $("#TournamentReputationWrapper").html(data);
                            _this.initReputation();
                        }
                    });
                }
            }
        });
    }

    this.setWin = function (num) {
        var numWinPos = num.indexOf("_");
        var numRound = num.substring(0, numWinPos);
        $("#Score_" + numRound + "_1").val("0");
        $("#Score_" + numRound + "_2").val("0");
        $("#Score_" + num).val("1");

        $(".result-table .icon-medal-1").remove();
        $("#Result_" + num).append($("<div class='icon-medal-1 sprite'>"));
    }

    this.setTechWin = function (player1) {
        var ajaxData =
        {
            ID: $("#ID").val()
        };
        if (player1) {
            ajaxData.Score1 = 3;
            ajaxData.Score2 = 0;
        } else {
            ajaxData.Score1 = 0;
            ajaxData.Score2 = 3;
        }
        $.ajax({
            type: "POST",
            url: _this.ajaxSubmitTechMatch,
            data: ajaxData,
            success: function () {
                window.location.reload();
            }
        });
    }

    this.setTechAllLose = function () {
        var ajaxData =
        {
            id: $("#ID").val()
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxSubmitTechAllLoser,
            data: ajaxData,
            success: function () {
                window.location.reload();
            }
        });
    }


    this.publishResult = function (roundId) {
        var ajaxData = {
            ID: roundId,
            Score1Text: $("#Score_" + roundId + "_1").val(),
            Score2Text: $("#Score_" + roundId + "_2").val()
        }
        $.ajax({
            type: "POST",
            url: _this.ajaxPublishRound,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
                if (data.result == "error") {
                    ShowError(data.error);
                    return;
                }
                window.location.reload();
            }
        });
    }

    this.disputeResult = function (roundId) {
        $.ajax({
            type: "POST",
            url: _this.ajaxDisputeRound,
            data: { id: roundId },
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
                if (data.result == "error") {
                    ShowError(data.error);
                }
            }
        });
    }

    this.submitResult = function (roundId) {
        $.ajax({
            type: "POST",
            url: _this.ajaxSubmitRound,
            data: { id: roundId },
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
                if (data.result == "error") {
                    ShowError(data.error);
                }
            }
        });
    }

    this.uploadImage = function (data, item) {
        var id = item.attr("id").substring("RoundItem_".length);
        $(".uploaded-image", item).append($("<img src='" + data.ImagePath + "' />"));
        $("#ImagePath_" + id).val(data.ImagePath);
    }

    this.uploadMatchImage = function (data) {
        $(".uploaded-image").append($("<img src='" + data.ImagePath + "' />"));
        $("#ImagePath").val(data.ImagePath);
    }

    this.sendMatchComment = function (item) {
        var form = item.closest("form");
        var data = form.serialize();
        $(".send-match-comment").parent().remove();
        $.ajax({
            type: "POST",
            url: _this.ajaxCreateMatchComment,
            data: data,
            success: function (data) {
                $("#CommentMatchInput").html(data);
                _this.initMatchComment();
            }
        });
    }

    this.RemoveComment = function (item) {
        var id = item.closest("li").attr("id").substring("Comment_".length);
        $.ajax({
            type: "GET",
            url: ajaxDeleteComment,
            data: { id: id },
            success: function (data) {
                $("#Comment_" + id).remove();
            }
        });
    };

    this.RollbackRound = function (item) {
        var id = item.data("id");
        $.ajax({
            type: "GET",
            url: ajaxRollbackRound,
            data: { id: id },
            success: function (data) {
                window.location.reload();
            }
        });
    };

    this.RollbackMatch = function (item) {
        var id = item.data("id");
        $.ajax({
            type: "GET",
            url: ajaxRollbackMatch,
            data: { id: id },
            success: function (data) {
                window.location.reload();
            }
        });
    };
}


var match;
$().ready(function () {
    match = new Match();
    match.init();
});
