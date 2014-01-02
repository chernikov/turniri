function PromoActionList() {
    var _this = this;

    this.ajaxGeneratePromoCode = "/admin/PromoAction/GeneratePromoCode";

    this.init = function () {
        $('#generateCodesModal').bind('show', function () {
            var id = $(this).data('id');

            $.ajax({
                type: "GET",
                url: _this.ajaxGeneratePromoCode,
                data: { id: id },
                success: function (data) {
                    $("#GeneratePromoCodeWrapper").html(data);
                }
            });
        });

        $("#GenerateButton").live("click", function () {
            var ajaxData = $("#GeneratePromoCodeForm").serialize();

            $.ajax({
                type: "POST",
                url: _this.ajaxGeneratePromoCode,
                data: ajaxData,
                success: function (data) {
                    $("#GeneratePromoCodeWrapper").html(data);
                }
            });
            return false;
        });

        $(".generate").click(function (e) {
            e.stopPropagation();

            var id = $(this).data('id');
            $('#generateCodesModal').data('id', id).modal('show');
        });

        $(".show-promocodes").click(function () {
            var id = $(this).data("id");
            var item = $(".promocodes[data-id='" + id + "']");
            if (item.hasClass("hidden")) {
                $(this).text("Спрятать коды");
            } else {
                $(this).text("Показать коды");
            }
            item.toggleClass("hidden");
        });
    }
}

var promoActionList;
$().ready(function () {
    promoActionList = new PromoActionList();
    promoActionList.init();
});