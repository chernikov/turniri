function Subscriber() {

    var _this = this;

    this.AjaxAddSubscriber = "/admin/Subscriber/Add";
    this.AjaxDeleteSubscriber = "/admin/Subscriber/Delete";

    this.init = function () {
        $("#Add").click(function () {
            _this.ShowAddDialog();
        });

        $("#Add").click(function () {
            _this.ShowAddDialog();
        });
        $("#AddSubscriberSubmit").live("click", function () {
            _this.addSubscriber();
        });

        $(".remove-subscriber").click(function () {
            _this.deleteSubscriber($(this));
        });

        $(".btn-danger, .delete-action").die("click");
    };

    this.ShowAddDialog = function ()
    {
        $.ajax({
            type: "GET",
            url: _this.AjaxAddSubscriber,
            data : {id : $("#DistributionID").val() },
            success: function (data) {
                $("#addWrapper").modal();
                $("#addWrapper").html(data);
                _this.initAddDialog();
            }
        });
    }

    this.initAddDialog = function ()
    {
        $("#TournamentWrapper").hide();
        $("#GameWrapper").hide();
        $("#RoleID").change(function () {
            _this.showOtherDropList($(this));
        });
        _this.initUserSelect();
        _this.showOtherDropList($("#RoleID"));
    }

    this.showOtherDropList = function (item)
    {
        $("#TournamentWrapper").hide();
        $("#GameWrapper").hide();
        if (item.val() == 2 || item.val() == 4 || item.val() == 101) {
            $("#GameWrapper").show();
        };
        if (item.val() == 3 || item.val() == 5 || item.val() == 102) {
            $("#TournamentWrapper").show();
        }
    }

    this.initUserSelect = function () {
        $(".chzn-select").each(function (i, item) {
            $(this).ajaxChosen();
        });
    };

    this.addSubscriber = function () {
        var ajaxData = $("#AddSubscriberForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.AjaxAddSubscriber,
            data: ajaxData,
            success: function (data) {
                $("#addWrapper").html(data);
                _this.initAddDialog();
            }
        });
    }

    this.deleteSubscriber = function (item)
    {
        $.ajax({
            type: "POST",
            url: _this.AjaxDeleteSubscriber,
            data: { id: item.data("id") },
            success: function (data) {
                item.closest("tr").remove();
            }
        });
    }
}

var subscriber = null;
$().ready(function () {
    subscriber = new Subscriber();
    subscriber.init();
});