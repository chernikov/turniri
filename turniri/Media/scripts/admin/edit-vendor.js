function Vendor() {
    var _this = this;

    this.init = function ()
    {
        InitBbCodeEditor($(".html-description"));
    }
}


var vendor = null;

$().ready(function () {
    vendor = new Vendor();
    vendor.init();
});