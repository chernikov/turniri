function EditNew() {

    var _this = this;

    this.ajaxUploadNewPreview = "/admin/File/UploadNewPreview";
    this.ajaxUploadNewTitle = "/admin/File/UploadNewTitle";

    this.init = function() {
        var item = $(".html-description");
        InitBbCodeEditor(item);

        $("#DeletePreview").click(function() {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadNewPreview,
            function(id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
            },
            null, titlePreview);

        $("#DeleteTitleImage").click(function() {
            _this.DeleteTitleImage();
        });

        var titleTitle = $("#ChangeTitleImage").text();
        InitUpload($("#ChangeTitleImage")[0],
            false,
            _this.ajaxUploadNewTitle,
            function(id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangeTitleImage(responseJSON.data);
                }
            },
            null, titleTitle);
    };

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/no-image.png");
        $("#PreviewPath").val("");
        $("#AvatarPreviewPath").val("");
    }

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.PreviewPath);
        $("#PreviewPath").val(data.PreviewPath);
        $("#AvatarPreviewPath").val(data.AvatarPreviewPath);
    }
    
    this.DeleteTitleImage = function () {
        $("#TitleImage").attr("src", "/Media/images/no-image.png");
        $("#TitlePath").val("");
        $("#AvatarTitlePath").val("");
    }

    this.ChangeTitleImage = function (data)
    {
        $("#TitleImage").attr("src", data.AvatarTitlePath);
        $("#TitlePath").val(data.TitlePath);
        $("#AvatarTitlePath").val(data.AvatarTitlePath);
    }
}

var editNew = null;
$().ready(function () {
    editNew = new EditNew();
    editNew.init();
});