function EditProduct() {
    var _this = this;

    this.init = function () {
        var item = $(".html-description");
        InitBbCodeEditor(item);

        $("#DeletePreview").click(function ()
        {
            _this.DeletePreview();
        });

        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            "/admin/File/UploadProductImage",
            function (id, fileName, responseJSON) {
                if (responseJSON.success)
                {
                    _this.ChangePreview(responseJSON);
                }
            },
            null, titlePreview);

        $("#CatalogListWrapper select").change(function () {
            _this.updateCatalogList($(this));
        });

        $("#AddProductPrice").click(function () {
            $.ajax({
                type: "GET",
                url: "/admin/Product/AddProductPrice",
                success: function (data) {
                    $("#ProductPriceListWrapper").append(data);
                }
            })
        });

        $(document).on("click", ".remove-line-link", function ()
        {
            $(this).closest(".ProductPriceWrapper").remove();
        });

        var titleAddProductImage = $("#AddProductImage").html();
        InitUpload($("#AddProductImage")[0],
            true,
            "/admin/File/UploadProductScreenshot",
            function (id, fileName, responseJSON) {
                if (responseJSON.success) {
                    _this.AddScreenshot(responseJSON);
                }
            },
            null, titleAddProductImage);

        $(document).on("click", ".remove-image", function ()
        {
            $(this).closest(".ProductImageWrapper").remove();
        });

        var titleAddProductVariation = $("#AddProductVariation").html();
        InitUpload($("#AddProductVariation")[0],
        true,
         "/admin/File/UploadProductVariation",
        function (id, fileName, responseJSON) {
            if (responseJSON.success) {
                _this.AddVariation(responseJSON);
            }
        },
        null, titleAddProductVariation);

        $(document).on("click", ".remove-variation", function () {
            $(this).closest(".ProductVariationWrapper").remove();
        });

        $("#AddProductVideo").click(function () {
            $.ajax({
                type: "GET",
                url: "/admin/Product/AddProductVideo",
                success: function (data) {
                    $("#ProductVideoListWrapper").append(data);
                }
            })
        });

        $(document).on("click", ".remove-video", function () {
            $(this).closest(".ProductVideoWrapper").remove();
        });

        $(document).on("click", ".submit-video", function () {
            _this.ProcessVideo($(this).closest(".ProductVideoWrapper"));
            return false;
        });

        $("#ProductCatalogs").chosen();

        $("#DeleteBackground").click(function () {
            _this.DeleteBackground();
        });

        var titleBackground = $("#ChangeBackground").text();

        InitUpload($("#ChangeBackground")[0],
            false,
            "/admin/File/UploadProductBackground",
            function (id, fileName, responseJSON) {
                if (responseJSON.success) {
                    _this.ChangeBackground(responseJSON);
                }
            },
            null, titleBackground);

        $("#ProductsList").ajaxChosen({
            method: 'GET',
            url: "/admin/Product/SelectProduct",
            dataType: 'json',
            minTermLength: 2,
            afterTypeDelay: 300,
            keepTypingMsg: "Продолжайте печатать...",
            lookingForMsg: "Ищу..."
        }, function (data) {
            var terms = {};
            if (data.result == "ok") {
                $.each(data.data, function (i, val) {
                    terms[val.id] = val.name;
                });
            }
            return terms;
        });
    };

    this.DeletePreview = function ()
    {
        $("#PreviewImage").attr("src", "/Media/images/default_game.jpg");
        $("#Image").val("");
    };

    this.ChangePreview = function (data) {
        $("#PreviewImage").attr("src", data.fileUrl + "?width=284&height=402&mode=crop");
        $("#Image").val(data.fileUrl);
    };

    this.DeleteBackground = function () {
        $("#PreviewBackground").attr("src", "/Media/images/default.png");
        $("#Background").val("");
    };

    this.ChangeBackground = function (data) {
        $("#PreviewBackground").attr("src", data.fileUrl + "?width=284&height=402&mode=crop");
        $("#Background").val(data.fileUrl);
    };

    this.AddScreenshot = function (responseJSON)
    {
        $.ajax({
            type: "GET",
            url: "/admin/Product/AddProductImage",
            data: { image: responseJSON.fileUrl },
            success: function (data)
            {
                $("#ProductImageListWrapper").append(data);
            }
        });
    };

    this.AddVariation = function (responseJSON) {
        $.ajax({
            type: "GET",
            url: "/admin/Product/AddProductVariation",
            data: { image: responseJSON.fileUrl },
            success: function (data) {
                $("#ProductVariationListWrapper").append(data);
            }
        });
    };

    this.ProcessVideo = function (item)
    {
        var url = $(".video-url", item).val();
        var key = $(".video-key", item).val();
        $.ajax({
            type: "GET",
            url :"/admin/Product/ProcessVideo",
            data: { url: url, key : key },
            success: function (data)
            {
                item.replaceWith(data);
            }
        });
    }
}

var editProduct;
$().ready(function () {
    editProduct = new EditProduct();
    editProduct.init();
});

