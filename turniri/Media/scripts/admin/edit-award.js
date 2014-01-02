function EditAward() {

    var _this = this;

    this.ajaxUploadAwardIcon = "/admin/File/UploadAwardIcon";

    this.init = function () {
        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadAwardIcon,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.iconPath);
                }
                if (responseJSON.result == "error") {
                    alert(responseJSON.error);
                }
            },
           [], titlePreview);
    };

    this.DeletePreview = function() {
        $("#PreviewImage").attr("src", "/Media/images/default_award.png");
        $("#IconPath").val("");
    };

    this.ChangePreview = function(path) {
        $("#PreviewImage").attr("src",path);
        $("#IconPath").val(path);
        
    };
}

var editAward = null;
$().ready(function () {
    editAward = new EditAward();
    editAward.init();
});