function EditGroup() {

    var _this = this;

    this.ajaxUploadGroupLogo = "/File/UploadGroupLogo";
    this.ajaxGroupAddPlayer = "/admin/Group/AddPlayer";
    this.ajaxGroupDeletePlayer = "/admin/Group/DeletePlayer";
    this.ajaxGroupAcceptPlayer = "/admin/Group/AcceptPlayer";
    this.ajaxGroupSwitchRole = "/admin/Group/SwitchRole";
    this.ajaxGroupUserList = "/admin/Group/UserList";
    this.ajaxSelectUser = "/admin/User/SelectUser";
    this.ajaxSelectGame = "/admin/User/SelectGame";

    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);

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

        _this.initUserSelect();

        $("#DeletePreview").click(function () {
            _this.deletePreview();
        });

        $("#AddPlayerModal").on("show", function () {
            $("#MemberUserID").val("");
            $("#MemberUserLogin").val("");
        });

        $("#AddPlayerButton").click(function ()
        {
            _this.AddPlayer();
        });

        $(".remove-line").live("click", function () {
            var item = $(this).closest(".UserGroupWrapper");
            _this.removeLine(item);
        });

        $(".accept-player").live("click", function () {
            var item = $(this).closest(".UserGroupWrapper");
            _this.acceptPlayer(item);
        });

        $(".role-switcher").live("click", function () {
            var id = $(this).closest(".UserGroupWrapper").data("id");
            var roleId = $(this).data("id");
            _this.switchRole(id, roleId);
        });

    };

    this.initUserSelect = function ()
    {
        $("#UserID").ajaxChosen({
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

        $("#MemberUserLogin").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxSelectUser,
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        response($.map(data.data, function (item) {
                            return {
                                label: item.login,
                                value: item.login,
                                id: item.id
                            }
                        }));
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $("#MemberUserLogin").val(ui.item.Label);
                $("#MemberUserID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });

        $("#Games").chosen();
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

    this.AddPlayer = function ()
    {
        if ($("#MemberUserID").val() != "" || $("#MemberUserID").val() != "0") {
            $.ajax({
                type: "POST",
                url : _this.ajaxGroupAddPlayer,
                data: {
                    UserID: $("#MemberUserID").val(),
                    GroupID: $("#ID").val()
                },
                success: function (data) {
                    if (data.result == "ok")
                    {
                        $("#AddPlayerModal").modal('hide');
                        _this.updatePlayerList();
                    }
                }
            });
        }
    }

    this.updatePlayerList = function()
    {
        $.ajax({
            type: "POST",
            url: _this.ajaxGroupUserList,
            data: {
                id:  $("#ID").val(),
            },
            success: function (data)
            {
                $("#UserGroupListWrapper").html(data);
            }
        });
    }

    this.removeLine = function (item)
    {
        $.ajax({
            type: "POST",
            url: _this.ajaxGroupDeletePlayer,
            data: { id: item.data("id") },
            success: function (data)
            {
                if (data.result == "ok") {
                    item.remove();
                }
            }
        });
    }

    this.acceptPlayer = function (item) {
        $.ajax({
            type: "POST",
            url: _this.ajaxGroupAcceptPlayer,
            data: { id: item.data("id") },
            success: function (data) {
                if (data.result == "ok") {
                    _this.updatePlayerList();
                }
            }
        });
    }

    this.switchRole = function(id, roleId)
    {
        $.ajax({
            type: "POST",
            url: _this.ajaxGroupSwitchRole,
            data: { id: id, roleId : roleId },
            success: function (data)
            {
                if (data.result == "ok") {
                    _this.updatePlayerList();
                }
            }
        });
    }
}

var editGroup = null;
$().ready(function () {
    editGroup = new EditGroup();
    editGroup.init();
});