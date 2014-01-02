function IndexBlog() {

    var _this = this;

    this.AjaxMakeFromBlog = "/admin/SocialPost/MakeFromBlog";
    
    this.messageItem = null;

    this.init = function () {
        $(".social").click(function () {
            var id = $(this).data("id");
            _this.ShowMessageDialog(id);
            return false;
        });
    };

    this.ShowMessageDialog = function (id) 
    {
        $.ajax({
            type: "GET",
            url: _this.AjaxMakeFromBlog,
            data: {id: id },
            success: function (data) {
                $("#socialWrapper").modal();
                $("#socialWrapper").html(data);
            }
        });
    };

}

var indexBlog = null;
$().ready(function () {
    indexBlog = new IndexBlog();
    indexBlog.init();
});