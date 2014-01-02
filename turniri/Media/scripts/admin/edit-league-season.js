function EditLeagueSeason()
{
    var _this = this;

    this.init = function ()
    {
        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            "/admin/File/UploadLeagueImage",
            function (id, fileName, responseJSON) {
                if (responseJSON.success) {
                    _this.ChangePreview(responseJSON);
                }
            },
            null, titlePreview);

        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });
    }

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/default_game.jpg?width=105&height=105&mode=crop");
        $("#Image").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.fileUrl + "?width=105&height=105&mode=crop");
        $("#Image").val(data.fileUrl);
    };

}

var editLeagueSeason = null;
$().ready(function () {
    editLeagueSeason = new EditLeagueSeason();
    editLeagueSeason.init();
});