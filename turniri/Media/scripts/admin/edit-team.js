function EditTeam() {
    var _this = this;

    this.ajaxUploadAvatar = "/Team/UploadAvatar";
    this.ajaxAutocomplete = "/admin/Team/AutocompleteUser";

    this.init = function ()
    {
        var title = $("#ChangePreview").text();
        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadAvatar,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.changePreview(responseJSON.data);
                }
            },
            null, title);

        $("#DeletePreview").click(function () {
            _this.deletePreview();
        });
       
        $("#CaptainLogin").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxAutocomplete,
                    data: {
                        query: request.term,
                        gameID: $("#TournamentGameID").val()
                    },
                    success: function (data) {

                        response($.map(data.data, function (item) {
                            return {
                                label: item.Label,
                                value: item.Label,
                                id: item.ID
                            }
                        }));
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $("#CaptainName").val(ui.item.Label);
                $("#CaptainID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    };

    this.changePreview = function (data) {
        $("#PreviewImage").attr("src", data.ImagePath30);
        $("#ImagePath18").val(data.ImagePath18);
        $("#ImagePath26").val(data.ImagePath26);
        $("#ImagePath30").val(data.ImagePath30);
    }

    this.deletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/default_avatar_30.png");
        $("#ImagePath18").val("");
        $("#ImagePath26").val("");
        $("#ImagePath30").val("");
    };
}




var editTeam = null;
$().ready(function () {
    editTeam = new EditTeam();
    editTeam.init();
});