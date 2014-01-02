function Poll() {
    var _this = this;

    this.ajaxPoll = "/Poll";
    this.ajaxUpdatePoll = "/Poll/Voted";

    this.init = function () {
        
        $("#Vote").click(function ()
        {
            var pollItems = [];
            $.each($("input[name=PollItems\[\]]:checked"),
            function (i, item) {
                pollItems.push(parseInt($(this).val()));
            });

            _this.Vote(pollItems);
        });
    
        $("#DontVote").click(function () {
            _this.Vote(null);
        });
    };

    this.Vote = function (items)
    {
        var ajaxData = {
            id: $("#PollID").val(),
            pollItems : []
        };
        $.each(items, function (i, item) {
            ajaxData.pollItems.push(item);
        });
        $.ajax({
            type: "POST",
            dataType: "json",
            crossDomain: true,
            traditional: true,
            url: _this.ajaxPoll,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    _this.updatePoll();
                    
                }
            }
        });
    }

    this.updatePoll = function () {
        var ajaxData = {
            id: $("#PollID").val()
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxUpdatePoll,
            data: ajaxData,
            success: function (data) {
                $(".forum-poll-wrapper").html(data);
            }
        });
    }
}


var poll;
$().ready(function () {
    poll = new Poll();
    poll.init();
});
