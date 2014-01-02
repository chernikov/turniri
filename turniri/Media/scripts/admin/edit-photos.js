function EditPhotos() {
    var _this = this;
    this.ajaxUploadPhoto = "/admin/File/UploadPhoto";
    this.ajaxBindPhoto = "/admin/File/BindPhoto";
    this.ajaxRemovePhoto = "/admin/PhotoAlbum/RemovePhoto";

    this.init = function () {
        if ($("#AddPhoto").length > 0) {
            var title = "Загрузить";
            InitUpload($("#AddPhoto")[0],
                true,
                _this.ajaxUploadPhoto,
                function (id, fileName, responseJSON) {
                    if (responseJSON.result == "ok") {
                        _this.AddPhoto(responseJSON.data);
                        _this.Bind(responseJSON.data.ID);
                    }
                },
                null, title);
        }

        $(".close").live("click", function () {
            _this.RemovePhoto($(this));
        });
    };

    this.AddPhoto = function (data) {
        var li = $("<li class='span2'>");
        var obj = $("<div>").addClass("image-item").addClass("thumbnail").attr("id", "Photo_" + data.ID).appendTo(li);
        $("<button class='close'>&times;</button>").appendTo(obj);
        $("<img>").attr("src", data.SmallPath).appendTo(obj);
        $(".thumbnails").prepend(li);
    };

    this.Bind = function(id) {
        var ajaxData = {
            id: id,
            idPhotoAlbum: $("#ID").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxBindPhoto,
            data: ajaxData,
            error: function() {
                $("#Photo_" + id).remove();
            }
        });
    };

    this.RemovePhoto = function (item) 
    {
        var id = item.closest(".image-item").attr("id").substring("Photo_".length);
        $.ajax({
            type: "POST",
            url: _this.ajaxRemovePhoto,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    $("#Photo_" + id).parent().remove();
                }
            }
        });
    };
}


var editPhotos;
$().ready(function () {
    editPhotos = new EditPhotos();
    editPhotos.init();
});
