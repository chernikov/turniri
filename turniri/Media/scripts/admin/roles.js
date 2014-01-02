function Roles() {
    var _this = this;


    this.ajaxRemoveRole = "/admin/Role/RemoveRole";
    this.ajaxRemoveGameRole = "/admin/Role/RemoveGameRole";
    this.ajaxRemoveTournamentRole = "/admin/Role/RemoveTournamentRole";

    this.init = function ()
    {
        $(".game-role-remove").live("click", function () {
            _this.GameRoleRemove($(this).closest(".role"));
        });

        $(".tournament-role-remove").live("click", function () {
            _this.TournamentRoleRemove($(this).closest(".role"));
        });
        $(".role-remove").live("click", function () {
            _this.RoleRemove($(this).closest(".role"));
        });
    }

    this.GameRoleRemove = function (item)
    {
        var ajaxData = {
            id: item.data("id")
        };

        $.ajax({
            type: "GET",
            url: _this.ajaxRemoveGameRole,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    item.remove();
                }
            }
        });
    }

    this.TournamentRoleRemove = function (item) {
        var ajaxData = {
            id: item.data("id")
        };

        $.ajax({
            type: "GET",
            url: _this.ajaxRemoveTournamentRole,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    item.remove();
                }
            }
        });
    }

    this.RoleRemove = function (item) {
        var ajaxData = {
            id: item.data("id")
        };

        $.ajax({
            type: "GET",
            url: _this.ajaxRemoveRole,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    item.remove();
                }
            }
        });
    }

}


var roles = null;
$().ready(function () {
    roles = new Roles();

    roles.init();
});