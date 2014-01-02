function Cart() {
    var _this = this;

    this.init = function ()
    {
        this.reinit();

        $(".show-popup-code").click(function ()
        {
            _this.showCode($(this).data("id"));
            
        });
    }

    this.reinit = function () {

        $(".input-quantity").keydown(function (event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });
        $(".input-quantity").blur(function () {
            var id = $(this).data("id");
            var value = $(this).val();
            _this.updateCount(id, value);
        });

        $(".remove-game").click(function () {
            var id = $(this).data("id");
            _this.removeCartProduct(id);
        });

        $("#ClearCartBtn").click(function () {
            _this.clearCart();
        });

        $("#NextStep").click(function () {
            window.location = "/cart/process";
        });
    }

    this.updateCount = function (id, value) {
        $.ajax({
            type: "GET",
            url: "/Cart/UpdateQuantity",
            data: { id: id, value: value },
            success: function (data) {
                if (data.result == "ok") {
                    _this.updateCart();
                }
                if (data.result == "error") {
                    ShowError(data.error);
                    _this.updateCart();
                }
            }
        });
    }

    this.removeCartProduct = function (id) {
        $.ajax({
            type: "GET",
            url: "/Cart/RemoveCartProduct",
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    _this.updateCart();
                }
            }
        });
    }

    this.clearCart = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/ClearCart",
            success: function (data) {
                if (data.result == "ok") {
                    _this.updateCart();
                }
            }
        });
    }

    this.updateCart = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Cart",
            success: function (data) {
                $("#tab_cart_1_content").html(data);
                _this.reinit();
                shop.updateTopCart();
            }
        });
    }

    this.updateTopCart = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/TopCart",
            success: function (data)
            {
                $("#CartWrapper").html(data);
            }
        });
    }

    this.showCode = function (id) {
        $.ajax({
            type: "GET",
            url: "/Cart/PopupCodeImage",
            data: { id: id },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
                $(".screenshot").loupe();
            }
        });
    }
}

var cart = null;
$().ready(function () {
    cart = new Cart();
    cart.init();
});