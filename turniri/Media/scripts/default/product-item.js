function ProductItem()
{
    var _this = this;

    this.init = function ()
    {
        $(".image-item").live("click", function () {
            var id = $(this).data("id");
            _this.showPhoto(id);
        });

        $(".image").live("click", function (e) {
            var id = $(this).closest(".image-wrapper").data("id");
            _this.changePhoto(id, true);
        });

        $(".image-wrapper .right-arrow").live("click", function (e) {
            var id = $(this).closest(".image-wrapper").data("id");
            _this.changePhoto(id, true);
            e.stopPropagation();
        });

        $(".image-wrapper .left-arrow").live("click", function (e) {
            var id = $(this).closest(".image-wrapper").data("id");
            _this.changePhoto(id, false);
            e.stopPropagation();
        });

        $(".icon-play-video").click(function () {
            var id = $(this).closest("li").data("id");
            _this.showVideo(id);
        });

        $(".buy").click(function () {
            _this.addToCart($(this).data("id"), $(this).data("variation"));
        });
    };

    this.showPhoto = function (id) {
        $.ajax({
            type: "GET",
            url: "/Products/ViewScreenshot",
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
            url: "/Products/ChangeScreenshot",
            data: {
                next: next,
                id: id
            },
            success: function (data) {
                $("#PhotoWrapper").html(data);
            }
        });
    }

    this.showVideo = function (id) {
        $.ajax({
            type: "GET",
            url: "/Products/ShowVideo",
            data:  { id: id },
            success: function (data) {
                $("#video_popup").show();
                $("#video_popup .video").html(data);
                $("#video_popup").centerInClient();
                $('.gray-background').show();
            }
        });
    };

    this.addToCart = function (id, idVariation)
    {
        $.ajax({
            type: "GET",
            url: "/Cart/AddToCart",
            data: { id: id, idVariation : idVariation },
            success: function (data)
            {
                if (data.result == "ok")
                {
                    ShowInfoMessage("Товар добавлен в корзину");
                }
                cart.updateTopCart();
                if (data.result == "error") {
                    ShowError(data.error);
                }
            }
        });
    }
}


var productItem;
$().ready(function () {
    productItem = new ProductItem();
    productItem.init();
});
