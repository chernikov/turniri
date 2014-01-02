function IndexNew() {

    var _this = this;

    this.AjaxMakeFromNew = "/admin/SocialPost/MakeFromNew";
    
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
        var ajaxData = 
        $.ajax({
            type: "GET",
            url: _this.AjaxMakeFromNew,
            data: {id: id },
            success: function (data) {
                $("#socialWrapper").modal();
                $("#socialWrapper").html(data);
            }
        });
    };

}

var indexNew = null;
$().ready(function () {
    indexNew = new IndexNew();
    indexNew.init();
});