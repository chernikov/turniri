function IndexPhotoAlbum() {

    var _this = this;

    this.AjaxMakeFromPhotoAlbum = "/admin/SocialPost/MakeFromPhotoAlbum";
    
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
            url: _this.AjaxMakeFromPhotoAlbum,
            data: {id: id },
            success: function (data) {
                $("#socialWrapper").modal();
                $("#socialWrapper").html(data);
            }
        });
    };

}

var indexPhotoAlbum = null;
$().ready(function () {
    indexPhotoAlbum = new IndexPhotoAlbum();
    indexPhotoAlbum.init();
});