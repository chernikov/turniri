function EditBackground() {

    var _this = this;

    this.ajaxUploadBackground = "/admin/File/UploadBackground";

    this.init = function ()
    {
        if ($("#PreviewBackground").attr("src") == "")
        {
            $("#PreviewBackground").hide();
        }
        $("#DeleteBackground").click(function () {
            _this.DeleteBackground();
        });

        var titlePreview = $("#ChangeBackground").text();

        InitUpload($("#ChangeBackground")[0],
            false,
            _this.ajaxUploadBackground,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangeBackground(responseJSON.data);
                }
                if (responseJSON.result == "error")
                {
                    alert(responseJSON.error);
                }
            },
           [], titlePreview);
    };

    this.ChangeBackground = function (data)
    {
        $("#ImagePath").val(data.filePath);
        $("#PreviewBackground").attr("src", data.preview);
        $("#PreviewBackground").show();
    }

    this.DeleteBackground = function () {
        $("#ImagePath").val("");
        $("#PreviewBackground").hide();
    }
}

var editBackground = null;
$().ready(function () {
    editBackground = new EditBackground();
    editBackground.init();
});