function TournamentMatches() {
    var ajaxChange = "/admin/Tournament/ChangeMatchesParticipants";
    this.ajaxAutocompleteUser = "/admin/Tournament/AutocompleteUser";
    this.ajaxSubstituteParticipant = "/admin/Tournament/SubstituteParticipant";
    var _this = this;

    this.init = function () {
        if ($(".draggable").length > 0) {
            $(".draggable").draggable({
                start: function () {
                    $(this).css("z-index", "10");
                },
                stop: function () {
                    $(this).css("z-index", "1");
                }
            });
            $(".droppable").droppable({
                drop: function (event, ui) {
                    var toMatchId = $(this).data("id");
                    var toPlayer1 = $(this).data("player1");
                    var fromMatchId = ui.draggable.data("id");
                    var fromPlayer1 = ui.draggable.data("player1");

                    $.ajax({
                        type: "POST",
                        url: ajaxChange,
                        data: {
                            fromMatchId: fromMatchId,
                            fromPlayer1: fromPlayer1,
                            toMatchId: toMatchId,
                            toPlayer1: toPlayer1
                        },
                        success: function (data) {
                            window.location.reload();
                        },
                        error: function () {
                            alert("Что-то не то");
                        }
                    });
                }
            });
        }

        $("#Login").autocomplete({
            source: function (request, response)
            {
                $.ajax({
                    url: _this.ajaxAutocompleteUser,
                    data: {
                        query: request.term
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
                $("#Login").val(ui.item.Label);
                $("#ParticipantID").val(ui.item.id);
            },
            open: function () {},
            close: function () {}
        });

        $(".changeable a").click(function (e) {
            e.stopPropagation();

            var item = $(this).closest(".changeable");
            $("#MatchID").val(item.data('id'));
            $("#Participant1").val(item.data('player1'));
            $("#ParticipantID").val("");
            $("#Login").val("");
            $('#changeParticipantModal').modal('show');
        });

        $("#SubmitSubstituteBtn").click(function () {
            
            var ajaxData = {
                matchId : $("#MatchID").val(),
                participant1 : $("#Participant1").val(),
                participantID : $("#ParticipantID").val()
            };
            $.ajax({
                type: "POST",
                url: _this.ajaxSubstituteParticipant,
                data: ajaxData,
                success: function (data)
                {
                    window.location.reload();
                }
            });
        });
    }
}

var tournamentMatches = null;
$().ready(function () {
    tournamentMatches = new TournamentMatches();
    tournamentMatches.init();
});