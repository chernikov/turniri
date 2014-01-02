function Shop()
{
    var _this = this;

    this.init = function ()
    {
        _this.initContent();
        //filter
        _this.initFilter();
    }

    this.initContent = function () {
        $(".buy").click(function () {
            _this.addToCart($(this).data("id"));
        });
    }

    this.initFilter = function ()
    {
        $(".platform-switcher").click(function ()
        {
            $(this).find(".checkbox").toggleClass("selected");
            _this.UpdatePlatform();
            _this.updateFilter();
            return false;
        });
        $(".price-switcher").click(function ()
        {
            $(".price-switcher .radiobox").removeClass("selected");
            $(this).find(".radiobox").addClass("selected");
            $("#Price").val($(this).data("value"));
            _this.updateFilter();
            return false;
        });

        $(".catalog-switcher").click(function () {
            $(".catalog-switcher .radiobox").removeClass("selected");
            $(this).find(".radiobox").addClass("selected");
            $("#CatalogID").val($(this).data("value"));
            _this.updateFilter();
            return false;
        });

        $(".type-switcher").click(function () {
            $(".type-switcher .radiobox").removeClass("selected");
            $(this).find(".radiobox").addClass("selected");
            $("#TypeCategory").val($(this).data("value"));
            _this.updateFilter();
            return false;
        });

        $(".period-switcher").click(function () {
            $(".period-switcher .radiobox").removeClass("selected");
            $(this).find(".radiobox").addClass("selected");
            $("#Period").val($(this).data("value"));
            _this.updateFilter();
            return false;
        });

        $("#TypeHeader .remove").click(function () {
            $(".type-switcher .radiobox").removeClass("selected");
            $("#TypeCategory").val("0");
            _this.updateFilter();
            return false;
        });

        $("#PlatformHeader .remove").click(function () {
            $(".platform-switcher .checkbox").toggleClass("selected");
            _this.updateFilter();
            return false;
        });

        $("#CatalogHeader .remove").click(function () {
            $(".catalog-switcher .radiobox").removeClass("selected");
            $("#CatalogID").val("");
            _this.UpdatePlatform();
            _this.updateFilter();
            return false;
        });

        $("#PriceHeader .remove").click(function () {
            $(".price-switcher .radiobox").removeClass("selected");
            $("#Price").val("");
            _this.updateFilter();
            return false;
        });

        $("#PeriodHeader .remove").click(function () {
            $(".period-switcher .radiobox").removeClass("selected");
            $("#Period").val("");
            _this.updateFilter();
            return false;
        });

        $("#searchString").blur(function () {
            _this.updateFilter();
        });
    }

    this.UpdatePlatform = function ()
    {
        $("#selectedPlatformValues").empty();
        $(".platform-switcher").each(function (i, item) {
            var data = $(this).data("value");
            var isChecked = $(this).find("div").hasClass("selected");

            if (isChecked) {
                var key = guid();
                $("<input>").attr("type", "hidden").attr("name", "SelectedPlatform.index").val(key).appendTo($("#selectedPlatformValues"));
                $("<input>").attr("type", "hidden").attr("name", "SelectedPlatform\["+key+"\]").val(data).appendTo($("#selectedPlatformValues"));
            }
        });

    }
    this.updateFilter = function ()
    {
        var ajaxData = $("#ShopFilterForm").serialize();
        $.ajax({
            type: "POST",
            url: "/shopaction/ShopFilter",
            data : ajaxData,
            success: function (data)
            {
                $("#ShopFilter").html(data);
                _this.initFilter();
            }
        });

        $.ajax({
            type: "POST",
            url: "/shopaction/Content",
            data: ajaxData,
            success: function (data)
            {
                $("#ShopContent").html(data);
                _this.initContent();
            }
        });
    }

    this.addToCart = function (id) {
        $.ajax({
            type: "GET",
            url: "/Cart/AddToCart",
            data : {id : id},
            success: function (data) {
                if (data.result == "ok") {
                    ShowInfoMessage("Товар добавлен в корзину");
                }
                _this.updateTopCart();
                if (data.result == "error")
                {
                    ShowError(data.error);
                }
            }
        });
    }

    this.updateTopCart = function ()
    {
        $.ajax({
            type: "GET",
            url: "/Cart/TopCart",
            success: function (data)
            {
                $("#CartWrapper").html(data);
            }
        });
    }
}

function s4() 
{
    return Math.floor((1 + Math.random()) * 0x10000)
               .toString(16)
               .substring(1);
};

function guid() {
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
           s4() + '-' + s4() + s4() + s4();
}

var shop = null;
$().ready(function () {
    shop = new Shop();
    shop.init();
});