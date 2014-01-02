function EditBanner() {

    var _this = this;

    this.ajaxUploadBanner = "/admin/File/UploadBanner";
    this.ajaxUpdateSize = "/admin/File/UpdateBannerSize";

    this.init = function ()
    {
        if ($("#PreviewBanner").attr("src") == "")
        {
            $("#PreviewBanner").hide();
        }
        $("#DeleteBanner").click(function () {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangeBanner").text();

        InitUpload($("#ChangeBanner")[0],
            false,
            _this.ajaxUploadBanner,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
                if (responseJSON.result == "error") {
                    alert(responseJSON.error);
                }
            },
           [], titlePreview);

        $("#Type").change(function () {
            if ($("#PreviewBanner").attr("src") != "")
            {
                _this.MakeSize($("#ImagePath").val(), $(this).val());
            }
        });
    };

    this.ChangePreview = function (data)
    {
        $("#ImagePath").val(data.filePath);

        _this.MakeSize(data.filePath, $("#Type").val());
    }

    this.DeletePreview = function () {
        $("#ImagePath").val("");
        $("#PreviewBanner").hide();
    }

    this.MakeSize = function (path, type)
    {
        var ajaxData = {
            path: path,
            type: type
        };

        $.ajax({
            type: "GET",
            url: _this.ajaxUpdateSize,
            data: ajaxData,
            success: function (data)
            {
                if (data.result == "ok")
                {
                    $("#PreviewBanner").attr("src", data.path);
                    $("#PreviewBanner").show();
                }
            }
        });
    }
}

var editBanner = null;
$().ready(function () {
    editBanner = new EditBanner();
    editBanner.init();
});