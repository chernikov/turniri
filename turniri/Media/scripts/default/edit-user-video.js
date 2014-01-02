function EditUserVideo() {
    var _this = this;

    this.ajaxProcessUrl = "/UserVideo/ProcessUrl";
    this.init = function () {

        InitBbCodeEditor($("#Text"));

        $("#VideoUrl").blur(function () {
            _this.ProcessVideoUrl();
        });
    };

    this.ProcessVideoUrl = function () {
        var ajaxData = {
            url: $("#VideoUrl").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxProcessUrl,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    $("#VideoCode").val(data.VideoCode);
                    $("#VideoThumb").val(data.VideoThumb);
                    $("#VideoCodeWrapper").html(data.VideoCode);
                    $("#VideoThumbWrapper").empty();
                    $("#VideoThumbWrapper").append($("<img>").attr("src", data.VideoThumb));
                } else {
                    $("#VideoUrl").val("");
                    $("#VideoCode").val("");
                    $("#VideoThumb").val("");
                    $("#VideoCodeWrapper").empty();
                    $("#VideoThumbWrapper").empty();
                }
            },
            error: function ()
            {
                ShowError("Что-то не так");
            }
        });
    };
}

var editUserVideo;
$().ready(function () {
    editUserVideo = new EditUserVideo();
    editUserVideo.init();
});
