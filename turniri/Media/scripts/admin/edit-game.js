function EditGame() {

    var _this = this;

    this.ajaxUploadGameImage = "/admin/File/UploadGameImage";
    this.ajaxGetGameForum = "/admin/Game/GetForum";
    this.ajaxSelectUser = "/admin/User/SelectUser";

    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);

        $("#DeletePreview").click(function () {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadGameImage,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangePreview(responseJSON.data);
                }
            },
            null, titlePreview);

        $("#ForumWrapper select").change(function () {
            _this.updateForumTopic($(this));
        });

        _this.initUserSelect();
    };

    this.DeletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/default_game.png");
        $("#PreviewPath").val("");
        $("#AvatarPreviewPath").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.ImagePath189);
        $("#ImagePath189").val(data.ImagePath189);
        $("#ImagePath103").val(data.ImagePath103);
        $("#ImagePath144v").val(data.ImagePath144v);
        $("#ImagePath47").val(data.ImagePath47);
        $("#ImagePath22").val(data.ImagePath22);
    };


    this.updateForumTopic = function (item) {
        var value = item.val();
        var ajaxData = {
            id: value
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxGetGameForum,
            data: ajaxData,
            success: function (data) {
                $("#ForumWrapper").html(data);
                $("#ForumWrapper select").change(function () {
                    _this.updateForumTopic($(this));
                });
            },
            error: function () {
                alert("Внутренняя ошибка");
            }
        });
    }

    this.initUserSelect = function () {
        $(".chzn-select").each(function (i, item) {
            $(this).ajaxChosen({
                method: 'GET',
                url: _this.ajaxSelectUser,
                dataType: 'json',
                minTermLength: 2,
                afterTypeDelay: 300
            }, function (data) {
                var terms = {};
                if (data.result == "ok") {
                    $.each(data.data, function (i, val) {
                        terms[val.id] = val.login;
                    });
                }
                return terms;
            });
        });
    };

}

var editGame = null;
$().ready(function () {
    editGame = new EditGame();
    editGame.init();
});