function EditGroup() {
    var _this = this;

    this.ajaxUploadGroupLogo = "/File/UploadGroupLogo";


    this.init = function ()
    {
        if ($("#ChangePreview").length > 0) {
            var titlePreview = $("#ChangePreview").text();

            InitUpload($("#ChangePreview")[0],
                false,
                _this.ajaxUploadGroupLogo,
                function (id, fileName, responseJSON) {
                    if (responseJSON.result == "ok") {
                        _this.ChangePreview(responseJSON.data);
                    }
                },
                null, titlePreview);

            $("#DeletePreview").click(function () {
                _this.deletePreview();
            });
        }
        InitBbCodeEditor($("#Description"));

        $(".send-money").click(function () {
            var id = $("#ID").val();
            money.userToGroupShow(id);
        });

        $(".send-money-to-player").click(function () {
            var id = $("#ID").val();
            money.groupToUserShow(id);
        });

        if ($("#MoneyListWrapper").length > 0) {
            $("#MoneyListWrapper .paging-list a").live("click", function () {
                var href = $(this).attr("href");

                $.ajax({
                    type: "GET",
                    url: href,
                    success: function (data) {
                        $("#MoneyListWrapper").html(data);
                    }
                });
                return false;
            });
        }
    };

    this.deletePreview = function () {
        $("#PreviewImage").attr("src", "/Media/images/default_group_173.png");
        $("#LogoPath173").val("");
        $("#LogoPath96").val("");
        $("#LogoPath84").val("");
        $("#LogoPath57").val("");
        $("#LogoPath30").val("");
        $("#LogoPath26").val("");
        $("#LogoPath18").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.LogoPath173);
        $("#LogoPath173").val(data.LogoPath173);
        $("#LogoPath96").val(data.LogoPath96);
        $("#LogoPath84").val(data.LogoPath84);
        $("#LogoPath57").val(data.LogoPath57);
        $("#LogoPath30").val(data.LogoPath30);
        $("#LogoPath26").val(data.LogoPath26);
        $("#LogoPath18").val(data.LogoPath18);
    };
}

var editGroup;
$().ready(function () {
    editGroup = new EditGroup();
    editGroup.init();
});
