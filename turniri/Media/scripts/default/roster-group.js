function RosterGroup() {

    var _this = this;

    this.ajaxGroupSwitchRole = "/Group/SwitchRole";

    this.leaderRoleID = 7;
    this.captainRoleID = 8;
    this.init = function ()
    {
        $(".switch-captain").click(function ()
        {
            _this.switchRole($(this), _this.captainRoleID);
        });
        $(".switch-leader").click(function ()
        {
            _this.switchRole($(this), _this.leaderRoleID);
        });
    };

    this.switchRole = function (item, roleId)
    {
        $.ajax({
            type: "POST",
            url: _this.ajaxGroupSwitchRole,
            data: { id: item.data("id"), roleId: roleId },
            success: function (data) {
                if (data.result == "ok") {
                    item.toggleClass("active");
                }
            }
        });
    }
}

var rosterGroup = null;
$().ready(function () {
    rosterGroup = new RosterGroup();
    rosterGroup.init();
});