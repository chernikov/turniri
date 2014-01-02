function NotifyGroup() {
    var _this = this;

    this.init = function ()
    {
        InitBbCodeEditor($("#Text"));
    };
}

var notifyGroup;
$().ready(function () {
    notifyGroup = new NotifyGroup();
    notifyGroup.init();
});
