function EditPage() {

    var _this = this;


    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);
    };
}

var editPage = null;
$().ready(function () {
    editPage = new EditPage();
    editPage.init();
});