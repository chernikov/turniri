function EditVideo() {

    var _this = this;

    this.ajaxProcessUrl = "/admin/Video/ProcessUrl";


    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);

        $("#processVideoUrl").click(function () {
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
            error: function () {
                alert("Что-то не так");
            }
        });
    }
}

var editVideo = null;
$().ready(function () {
    editVideo = new EditVideo();
    editVideo.init();
});