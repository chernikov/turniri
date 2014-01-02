function NoticeDistribution() {

    var _this = this;

    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);
    };
}

var noticeDistribution = null;
$().ready(function () {
    noticeDistribution = new NoticeDistribution();
    noticeDistribution.init();
});