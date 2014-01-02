function EditUser() {
    var _this = this;

    this.ajaxChangePassword = "/User/ChangePasswordAjax";
    this.ajaxUploadAvatar = "/User/UploadAvatar";


    this.init = function () {
        $("#changePasswordButton").live("click", function () {
            _this.ChangePassword();
            return false;
        });
        $("#DeleteAvatar").click(function () {
            _this.DeleteAvatar();
        });

        var title = $("#ChangeAvatar").text();
        InitUpload($("#ChangeAvatar")[0],
            false,
            _this.ajaxUploadAvatar,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.ChangeAvatar(responseJSON.data);
                }
            },
            null, title);
    };

    this.DeleteAvatar = function() {
        $("#AvatarImage").attr("src", "/Media/images/default_avatar_173.png");
        $("#AvatarPath173").val("");
        $("#AvatarPath96").val("");
        $("#AvatarPath84").val("");
        $("#AvatarPath57").val("");
        $("#AvatarPath30").val("");
        $("#AvatarPath26").val("");
        $("#AvatarPath18").val("");
    };
    this.ChangeAvatar = function(data) {
        $("#AvatarImage").attr("src", data.Avatar173Url);
        $("#AvatarPath173").val(data.Avatar173Url);
        $("#AvatarPath96").val(data.Avatar96Url);
        $("#AvatarPath84").val(data.Avatar84Url);
        $("#AvatarPath57").val(data.Avatar57Url);
        $("#AvatarPath30").val(data.Avatar30Url);
        $("#AvatarPath26").val(data.Avatar26Url);
        $("#AvatarPath18").val(data.Avatar18Url);
    };

    this.ChangePassword = function() {
        var ajaxData = {
            ID: $("#ID").val(),
            Password: $("#Password").val(),
            NewPassword: $("#NewPassword").val(),
            ConfirmPassword: $("#ConfirmPassword").val()
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxChangePassword,
            data: ajaxData,
            success: function(data) {
                $("#ChangePasswordWrapper").html(data);
            },
            error: function ()
            {
                ShowError("Что-то не так");
            }
        });
    };
}

var editUser;
$().ready(function () {
    editUser = new EditUser();
    editUser.init();
});
