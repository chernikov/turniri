function Comment() {

    var _this = this;

    this.init = function () {
        var item = $(".comment-html-description");
        InitBbCodeEditor(item);
    };
}

var comment = null;
$().ready(function () {
    comment = new Comment();
    comment.init();
});