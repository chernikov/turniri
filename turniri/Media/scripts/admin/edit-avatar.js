function EditAvatar() {

    var _this = this;

    this.ajaxUploadAvatar = "/admin/File/UploadAvatar";

    this.init = function () {
        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadAvatar,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
                if (responseJSON.result == "error") {
                    alert(responseJSON.error);
                }
            },
            [],
            titlePreview);
    };

    this.DeletePreview = function() {
        $("#PreviewImage").hide();
        $("#ImagePath18").val("");
        $("#ImagePath26").val("");
        $("#ImagePath30").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").show();
        $("#PreviewImage").attr("src", data.ImagePath30);
        $("#ImagePath18").val(data.ImagePath18);
        $("#ImagePath26").val(data.ImagePath26);
        $("#ImagePath30").val(data.ImagePath30);

    };
}

var editAvatar = null;
$().ready(function () {
    editAvatar = new EditAvatar();
    editAvatar.init();
});