function Camera() {
    var _this = this;

    this.ajaxCameraShow = "/Camera/Index";
    this.ajaxCameraMatchShow = "/Camera/Match";

    this.init = function ()
    {
        $(".camera-show").live("click", function () {
            _this.showCamera($(this).data("id"));
        });

        $(".icon-live").live("click", function (e) {
            var id = $(this).closest(".match-item").attr("id").substring("Match_".length);
            _this.showMatchCamera(id);
            e.stopPropagation();
        });

        $(".camera-match-show").live("click", function () {
            _this.showCamera($(this).data("id"), $(this).data("match-id"));
        });
    };

    this.showCamera = function (id) {
        
        $.ajax({
            type: "GET",
            url : _this.ajaxCameraShow,
            data: { id: id },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
            }
        });
    }
   

    this.showMatchCamera = function (id, cameraID) {
        $.ajax({
            type: "GET",
            url: _this.ajaxCameraMatchShow,
            data: { id: id, cameraID: cameraID },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
            }
        });
    }
}

var camera;
$().ready(function () {
    camera = new Camera();
    camera.init();
});
