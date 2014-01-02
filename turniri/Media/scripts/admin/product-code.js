function ProductCode() {
    var _this = this;

    this.ajaxUploadCodeImage = "/admin/File/UploadCodeImage";

    this.init = function () {
       
        _this.initForm();
        $("#RestBtn").click(function () {
            _this.RestCode();
            return false;
        });

        $(".delete").live("click", function () {
            _this.DeleteProductCode($(this).data("id"));
        });

        $(".change").live("click", function () {
            _this.ChangeProductCode($(this).data("id"));
        });

        $(".unreserve").live("click", function () {
            _this.UnreserveProductCode($(this).data("id"));
        });
        _this.ClearForm();
    }

    this.initForm = function ()
    {
        if ($("#ChangePreview").length > 0) {
            $("#DeletePreview").click(function () {
                _this.DeleteImage();
            });

            var titlePreview = $("#ChangePreview").text();

            InitUpload($("#ChangePreview")[0],
                false,
                _this.ajaxUploadCodeImage,
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

            $("#AddCodeBtn").click(function () {
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
            ProductPriceID : $("#ProductPriceID").val(),
            ProductID: $("#ProductID").val(),
            Code: $("#Code").val()
        };

        $.ajax({
            type: "POST",
            url: "/admin/Product/AddCode",
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    _this.ClearForm();
                    _this.UpdateTable();
                } else {
                    alert(data.error);
                }
            }
        });
    }

    this.RestCode = function () {
        var ajaxData = {
            ProductID: $("#ProductID").val(),
            Rest: $("#Rest").val()
        };

        $.ajax({
            type: "POST",
            url: "/admin/Product/RestCode",
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    _this.ClearForm();
                    _this.UpdateTable();
                } else {
                    alert(data.error);
                }
            }
        });
    }

    this.ClearForm = function () {
        _this.DeleteImage();
        $("#ID").val("0");
        $("#Code").val("");
    }

    this.UpdateTable = function () {
        $.ajax({
            type: "GET",
            url: "/admin/Product/CodeTable",
            data: { id: $("#ProductID").val() },
            success: function (data) {
                $("#CodesTableWrapper").html(data);
            }
        });
    }

    this.DeleteProductCode = function (id) {
        $.ajax({
            type: "GET",
            url: "/admin/Product/DeleteProductCode",
            data: { id: id },
            success: function (data) {
                if (data.result = "ok")
                {
                    window.location.reload();
                }
            }
        });
    }


    this.ChangeProductCode = function (id) {
        $.ajax({
            type: "GET",
            url: "/admin/Product/EditProductCode",
            data: { id: id },
            success: function (data) {
                $("#EditProductCodeWrapper").html(data);
                _this.initForm();
            }
        });
    }

    this.UnreserveProductCode = function (id) {
        $.ajax({
            type: "GET",
            url: "/admin/Product/UnReserveProductCode",
            data: { id: id },
            success: function (data) {
                _this.UpdateTable();
            }
        });
    }
}


var productCode = null;
$().ready(function () {
    productCode = new ProductCode();
    productCode.init();
});