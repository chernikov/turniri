function EditPromoaction()
{
    var _this = this;

    this.ajaxProductAutocomplete = "/admin/PromoAction/SelectProduct";

    this.init = function () {
        $("#ProductName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxProductAutocomplete,
                    data: {
                        query: request.term
                    },
                    success: function (data) {

                        response($.map(data.data, function (item) {
                            return {
                                label: item.Label,
                                value: item.Label,
                                id: item.ID
                            }
                        }));
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $("#ProductName").val(ui.item.Label);
                $("#ProductID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    }
}


var editPromoaction = null;

$().ready(function () {
    editPromoaction = new EditPromoaction();
    editPromoaction.init();
});