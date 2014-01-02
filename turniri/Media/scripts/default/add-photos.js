function AddPhotos() {
    var _this = this;
    this.ajaxUploadPhoto = "/Photo/UploadPhoto";
    this.ajaxViewPhoto = "/Photo/View";
    this.ajaxChangePhoto = "/Photo/ChangeView";
    this.ajaxBindPhoto = "/Photo/BindPhoto";
    this.ajaxRemovePhoto = "/Photo/RemovePhoto";

    this.init = function ()
    {
        if ($("#AddPhoto").length > 0) {
            var title = $("#AddPhoto div").text();
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

        $(".image-item").live("click", function() {
            var id = $(this).attr("id").substring("Photo_".length);
            _this.showPhoto(id);
        });

        $(".icon-close-image").click(function (e) {
            _this.deleteImage($(this));
            e.stopPropagation();
        });

        $(".image").live("click", function (e)
        {
            var id = $(this).closest(".image-wrapper").data("id");
            _this.changePhoto(id, true);
        });

        $(".image-wrapper .right-arrow").live("click", function (e)
        {
            var id = $(this).closest(".image-wrapper").data("id");
            _this.changePhoto(id, true);
            e.stopPropagation();
        });

        $(".image-wrapper .left-arrow").live("click", function (e) {
            var id = $(this).closest(".image-wrapper").data("id");
            _this.changePhoto(id, false);
            e.stopPropagation();
        });
    };

    this.AddPhoto = function(data) {
        var obj = $("<div>").addClass("image-item").attr("id", "Photo_" + data.ID);
        obj.append("<div class='icon-close-image sprite'></div>");
        $("<img>").attr("src", data.SmallPath).appendTo(obj);
        $(".icon-close-image", obj).click(function (e) {
            _this.deleteImage($(this));
            e.stopPropagation();
        });
        $("#PhotoAlbumList").prepend(obj);
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

    this.deleteImage = function (item) {
        var id = item.closest(".image-item").attr("id").substring("Photo_".length);
        $.ajax({
            type: "POST",
            url: _this.ajaxRemovePhoto,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    $("#Photo_" + id).remove();
                }
            }
        });
    }

    this.showPhoto = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxViewPhoto,
            data: { id: id },
            success: function (data) {
                $("#gallery_popup").show();
                $("#PhotoWrapper").html(data);
                $("#gallery_popup").centerInClient({ forceAbsolute: true });
                var topCss = $("#gallery_popup").position().top;
                if (topCss > 320) {
                    topCss = topCss - 300;
                } else {
                    topCss = 20;
                }
                $("#gallery_popup").css("top", topCss + "px");
                $('.gray-background').show();
            }
        });
    }

    this.changePhoto = function (id, next) {
        $.ajax({
            type: "GET",
            url: _this.ajaxChangePhoto,
            data: {
                next : next,
                id: id
            },
            success: function (data) {
                $("#PhotoWrapper").html(data);
            }
        });
    }
}


var addPhotos;
$().ready(function () {
    addPhotos = new AddPhotos();
    addPhotos.init();
});
