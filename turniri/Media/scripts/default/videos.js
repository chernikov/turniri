function Videos() 
{
    var _this = this;
    var ajaxGetCode = "/Video/VideoCode";
    this.init = function () {
        $(".icon-play-video").click(function () {

            var id = $(this).closest("li").attr("id").substring("Video_".length);
            _this.ShowCode(id);

        });
    };

    this.ShowCode = function(id) {
        var ajaxData = {
            id: id
        };
        $.ajax({
            type: "GET",
            url: ajaxGetCode,
            data: ajaxData,
            success: function(data) {
                $("#video_popup").show();
                $("#video_popup .video").html(data);
                $("#video_popup").centerInClient();
                $('.gray-background').show();
            }
        });
    };
}

var videos = null;
$().ready(function () {
    videos = new Videos();
    videos.init();
});