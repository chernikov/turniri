function EditLeagueLevel()
{
    var _this = this;
    this.init = function () {
        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });

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
    };

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/default_game.jpg?width=105&height=105&mode=crop");
        $("#Image").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.fileUrl + "?width=105&height=105&mode=crop");
        $("#Image").val(data.fileUrl);
    };
}


var editLeagueLevel;
$().ready(function () {
    editLeagueLevel = new EditLeagueLevel();
    editLeagueLevel.init();
});

