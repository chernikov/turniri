function Comment() {

    var _this = this;

    this.init = function () 
    {
        InitBbCodeEditor($("#Message"));
    };
}

var comment = null;
$().ready(function () {
    comment = new Comment();
    comment.init();
});