function CreateForumSubject() {

    var _this = this;

    this.init = function () 
    {
        InitBbCodeEditor($("#Message_Message"));
    };
}

var createForumSubject = null;
$().ready(function () {
    createForumSubject = new CreateForumSubject();
    createForumSubject.init();
});