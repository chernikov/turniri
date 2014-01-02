function EditForum() {

    var _this = this;

    this.ajaxUploadForumTopicPreview = "/admin/File/UploadForumPreview";

    this.init = function () {
        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangePreview").text();
        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadForumTopicPreview,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.previewPath);
                }
                if (responseJSON.result == "wrong-size") {
                    alert(responseJSON.error);
                }
            },
            ['png', 'gif'], titlePreview);
    };

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/forum_default_preview.png");
        $("#ImagePath").val("");
    }

    this.ChangePreview = function (previewPath) {
        $("#PreviewImage").attr("src", previewPath);
        $("#ImagePath").val(previewPath);
    }
}

var editForum = null;
$().ready(function () {
    editForum = new EditForum();
    editForum.init();
});