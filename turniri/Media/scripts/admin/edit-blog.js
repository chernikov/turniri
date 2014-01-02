function EditBlog() {

    var _this = this;

    this.ajaxUploadPreview = "/Blog/UploadPreview";
    this.init = function () {

        InitBbCodeEditor($("#Text"));

        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });
        var title = $("#ChangePreview").text();
        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadPreview,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
            },
            null, title);
    };

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/no-preview199.png");
        $("#PreviewUrl").val("");
    };
    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.PreviewUrl);
        $("#PreviewUrl").val(data.PreviewUrl);
    };
}

var editBlog;
$().ready(function () {
    editBlog = new EditBlog();
    editBlog.init();
});
