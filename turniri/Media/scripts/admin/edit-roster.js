function EditRoster() {

    var _this = this;

    this.AjaxAddUser = "/admin/Team/AddUser";
    this.ajaxAutocomplete = "/admin/Team/AutocompleteUser";

    this.messageItem = null;
    this.init = function () {
        $("#AddUser").click(function () {
            _this.ShowAddUserDialog();
        });

        $("#AddUserSubmit").live("click", function () {
            _this.AddUserSubmit();
            return false;
        });
    };

    this.ShowAddUserDialog = function (parentId) {
        var ajaxData = {
            id: $("#ID").val(),
        };
        $.ajax({
            type: "GET",
            url: _this.AjaxAddUser,
            data: ajaxData,
            success: function (data) {
                $("#newUserWrapper").modal();
                $("#newUserWrapper").html(data);
                _this.initAutoComplete();
            }
        });
    };

    this.AddUserSubmit = function () {
        var ajaxData = $("#AddUserForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.AjaxAddUser,
            data: ajaxData,
            success: function (data) {
                $("#newUserWrapper").html(data);
                _this.initAutoComplete();
            }
        });
    }

    this.initAutoComplete = function () {
        $("#UserLogin").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxAutocomplete,
                    data: {
                        query: request.term,
                        gameID: $("#GameID").val()
                    },
                    success: function (data)
                    {
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
                $("#UserLogin").val(ui.item.Label);
                $("#UserID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    }
}

var editRoster = null;
$().ready(function () {
    editRoster = new EditRoster();
    editRoster.init();
});