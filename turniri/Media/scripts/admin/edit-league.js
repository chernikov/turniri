function EditLeague() {
    var _this = this;

    this.init = function ()
    {
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

        $("#IsGroup").change(function () {
            _this.updateTeam();
        });

        _this.updateTeam();

        var item = $(".html-description");
        InitBbCodeEditor(item);
    };

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/default_game.jpg?width=105&height=105&mode=crop");
        $("#Image").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.fileUrl + "?width=105&height=105&mode=crop");
        $("#Image").val(data.fileUrl);
    };

    this.updateTeam = function () {
        if ($("#IsGroup").val() == "true") {
            $("#TeamCountWrapper").show();
            $("#HotReplacementWrapper").show();
        } else {
            $("#TeamCountWrapper").hide();
            $("#HotReplacementWrapper").hide();
        }
    }
}

var editLeague;
$().ready(function () {
    editLeague = new EditLeague();
    editLeague.init();
});

