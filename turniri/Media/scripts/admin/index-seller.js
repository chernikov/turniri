function IndexSeller() {

    var _this = this;

    this.AjaxCreateEditor = "/admin/Seller/Create";
    this.ajaxAutocomplete = "/admin/User/SelectUser";

    this.init = function () {
        $("#AddSellerBtn").click(function () {
            _this.ShowAddDialog();
        });

        $("#AddSellerSubmit").live("click", function () {
            _this.AddEditorSubmit();
            return false;
        });

        $("#UserLogin").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: _this.ajaxAutocomplete,
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        response($.map(data.data, function (item) {
                            return {
                                label: item.login,
                                value: item.login,
                                id: item.id
                            }
                        }));
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $("#UserLogin").val(ui.item.Label);
                $("#UserID").val(ui.item.id);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    };

    this.ShowAddDialog = function ()
    {
        $("#newSellerWrapper").modal();
        $("#UserLogin").val("");
        $("#UserID").val("");
    };


    this.AddEditorSubmit = function ()
    {
        if($("#UserID").val() != "") {
            $.ajax({
                type: "POST",
                url: _this.AjaxCreateEditor,
                data: { id: $("#UserID").val() },
                success: function (data) {
                    window.location.reload();
                }
            });
        } else {
            alert("Выберите пользователя");
        }
    }
}

var indexSeller = null;
$().ready(function () {
    indexSeller = new IndexSeller();
    indexSeller.init();
});