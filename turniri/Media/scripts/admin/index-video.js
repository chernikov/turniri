function IndexVideo() {

    var _this = this;

    this.AjaxMakeFromVideo = "/admin/SocialPost/MakeFromVideo";
    
    this.messageItem = null;

    this.init = function () {
        $(".social").click(function () {
            var id = $(this).data("id");
            _this.ShowMessageDialog(id);
            return false;
        });
    };

    this.ShowMessageDialog = function (id) 
    {
        var ajaxData = 
        $.ajax({
            type: "GET",
            url: _this.AjaxMakeFromVideo,
            data: {id: id },
            success: function (data) {
                $("#socialWrapper").modal();
                $("#socialWrapper").html(data);
            }
        });
    };

}

var indexVideo = null;
$().ready(function () {
    indexVideo = new IndexVideo();
    indexVideo.init();
});