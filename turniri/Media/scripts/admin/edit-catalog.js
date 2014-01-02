function EditCatalog() {

    var _this = this;

    this.ajaxUploadTournamentImage = "/admin/File/UploadCatalogImage";
    
    this.init = function ()
    {
        var item = $(".html-description");
        InitBbCodeEditor(item);

        var titlePreview = $("#ChangePreview").text();
        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadTournamentImage,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
            },
            null, titlePreview);
    };

    this.deletePreview = function ()
    {
        $("#PreviewImage").attr("src", "/Media/images/default_catalog.png");
        $("#Photo").val("");
    };

    this.ChangePreview = function(data) {
        $("#PreviewImage").attr("src", data.Photo);
        $("#Photo").val(data.Photo);
    };
}

var editCatalog = null;
$().ready(function () {
    editCatalog = new EditCatalog();
    editCatalog.init();
});