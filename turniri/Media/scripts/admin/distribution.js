function Distribution()
{
    var _this = this;
    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);
    };
}

var distribution = null;
$().ready(function () {
    distribution = new Distribution();
    distribution.init();
});