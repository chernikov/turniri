function CartItem() {
    var _this = this;

    this.init = function () {
        $(".add-code").click(function () {
            _this.showAddCode($(this).data("id"));
        });

        $(".add-real").click(function () {
            _this.addReal($(this).data("id"));
        });

        $('.variation-image').popover({
            delay: {
                hide: 100,
                show : 100
            }
        });
    }

    this.showAddCode = function (id) {
        $.ajax({
            type: "GET",
            url: "/admin/Cart/AddCode",
            data: { id: id },
            success: function (data) {
                $("#AddCodeWrapper").modal();
                $("#AddCodeWrapper").html(data);
                _this.initForm();
            }
        });
    }

    this.addReal = function (id) {
        $.ajax({
            type: "GET",
            url: "/admin/Cart/AddReal",
            data: { id: id },
            success: function (data) {
                if (data.result == "ok")
                {
                    window.location.reload();
                }
            }
        });
    }


    this.initForm = function () {
        if ($("#ChangePreview").length > 0) {
            $("#DeletePreview").click(function () {
                _this.DeleteImage();
            });

            var titlePreview = $("#ChangePreview").text();

            InitUpload($("#ChangePreview")[0],
                false,
                "/admin/File/UploadCodeImage",
                function (id, fileName, responseJSON) {
                    if (responseJSON.result == "ok") {
                        _this.ChangeImage(responseJSON.data);
                    }
                    if (responseJSON.result == "error") {
                        alert(responseJSON.error);
                    }
                },
                ["jpg", "png"],
                titlePreview);

            $("#AddCodeSubmit").click(function () {
                _this.AddCode();
                return false;
            });
        }
    }

    this.DeleteImage = function () {
        $("#PreviewImage").attr("src", "/Media/images/default.png");
        $("#Image").val("");
    };


    this.ChangeImage = function (data) {
        $("#PreviewImage").show();
        $("#PreviewImage").attr("src", data.codeImage);
        $("#Image").val(data.codeImage);
    };

    this.AddCode = function () {
        var ajaxData = {
            ID: $("#ID").val(),
            Image: $("#Image").val(),
            CartProductID: $("#CartProductID").val(),
            ProductPriceID: $("#ProductPriceID").val(),
            ProductVariationID: $("#ProductVariationID").val(),
            ProductID: $("#ProductID").val(),
            Code: $("#Code").val()
        };

        $.ajax({
            type: "POST",
            url: "/admin/Cart/AddCode",
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                } 
            }
        });
    }
}

var cartItem = null;

$().ready(function () {
    cartItem = new CartItem();
    cartItem.init();
});