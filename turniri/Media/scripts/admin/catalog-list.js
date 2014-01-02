function CatalogList() {
    var _this = this;
    this.catalogPrefix = "CatalogID_";
    this.productPrefix = "ProductID_";

    this.AjaxUpdateCatalogOrderUrl = "/admin/Catalog/AjaxCatalogOrder";
    this.AjaxUpdateCatalogMoveUrl = "/admin/Catalog/AjaxCatalogMove";

    this.init = function () {
        var sortingCatalogHolder = $(".catalogList");
        sortingCatalogHolder.sortable({
            placeholder: 'catalog-placeholder ui-state-highlight',
            stop: function (event, ui) {

                var sortingInfo = [];
                $("> .catalogItem", $(this)).each(function () {
                    sortingInfo.push($(this).data("id"));
                });
                var isNeedUpdate = false;
                var itemId = ui.item.data("id");
                var ajaxData = null;

                for (var i = sortingInfo.length; i--;) {
                    if (sortingInfo[i] != itemId) {
                        continue;
                    }
                    ajaxData =
                    {
                        id: itemId,
                        replaceTo: i + 1
                    };
                    isNeedUpdate = true;
                    break;
                }

                if (!isNeedUpdate) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: _this.AjaxUpdateCatalogOrderUrl,
                    data: ajaxData,
                    success: function (data) {
                        if (data.result == "error") {
                            $(this).sortable('cancel');
                            _this.showErrors(data.errors);
                        } else {
                            window.location.reload();
                        }
                    },
                    error: function () {
                        $(this).sortable('cancel');
                        alert("Внутренняя ошибка");
                    }
                });
            }
        });

        $(".catalogItem").droppable({
            accept: '.subCatalogItem',
            greedy: true,
            drop: function (event, ui) {
                var id = ui.draggable.parent().data("id");
                var moveTo = $(this).data("id");

                var ajaxData = { id: id, moveTo: moveTo };
                if (id == moveTo) {
                    return false;
                }
                $.ajax({
                    type: "POST",
                    url: _this.AjaxUpdateCatalogMoveUrl,
                    data: ajaxData,
                    success: function (data) {
                        if (data.result == "ok") {
                            window.location.reload();
                        }
                        if (data.result == "error") {
                            $(this).droppable('cancel');
                            _this.showErrors(data.errors);
                        }
                    },
                    error: function () {
                        $(this).droppable('cancel');
                        alert("Внутренняя ошибка");
                    }
                });
            }
        });

        $(".subCatalogItem").draggable({
            helper: function (event) {
                return $("<div class='ui-widget-move'>Перемещение</div>");
            },
            start: function (event, ui) {
                $(".catalogItem").addClass("highlight");
            },
            stop: function (event, ui) {
                $(".catalogItem").removeClass("highlight");
            }
        });

        $(".open").on("click", function () {
            _this.getCatalog($(this));
        });

        $("#CollapseAll").click(function () {
            _this.toggleAll();
        });
    }

    this.getCatalog = function (item) {
        var id = item.closest(".catalogItem").data("id");
        var wrapper = $("> .sub-wrapper", item.closest(".catalogItem"));
        wrapper.toggle();
    }

    this.toggleAll = function () {
        if ($("#CollapseAll").data("collapsed")) {
            $("#CollapseAll").data("collapsed", false);
            $("#CollapseAll").text("Свернуть всё");
            $(".sub-wrapper").show();
        } else {
            $("#CollapseAll").data("collapsed", true);
            $("#CollapseAll").text("Разкрыть всё");
            $(".sub-wrapper").hide();
        }

    }
}

var catalogList;
$().ready(function () {
    catalogList = new CatalogList();
    catalogList.init();
});