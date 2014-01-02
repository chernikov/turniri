function Toss() 
{
    var ajaxChange = "/admin/Tournament/ChangeParticipants";
    var _this = this;

    this.init = function () {
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
                var participant1ID = $(this).attr("id").substring("ParticipantPlace_".length);
                var participant2ID = ui.draggable.attr("id").substring("Participant_".length);
                $.ajax({
                    type: "POST",
                    url: ajaxChange,
                    data: { participant1ID: participant1ID, participant2ID: participant2ID },
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

}

var toss = null;
$().ready(function () {
    toss = new Toss();
    toss.init();
});