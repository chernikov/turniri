function IndexTournament() {

    var _this = this;

    this.AjaxMakeFromTournament = "/admin/SocialPost/MakeFromTournament";
    
    this.messageItem = null;

    this.init = function () {
        $(".social").click(function () {
            var id = $(this).data("id");
            _this.ShowMessageDialog(id);
            return false;
        });
    };

    this.ShowMessageDialog = function (id) 
    {
        var ajaxData = 
        $.ajax({
            type: "GET",
            url: _this.AjaxMakeFromTournament,
            data: {id: id },
            success: function (data) {
                $("#socialWrapper").modal();
                $("#socialWrapper").html(data);
            }
        });
    };

}

var indexTournament = null;
$().ready(function () {
    indexTournament = new IndexTournament();
    indexTournament.init();
});