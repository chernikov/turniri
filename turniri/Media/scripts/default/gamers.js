function Gamers() {
    var _this = this;

    this.init = function ()
    {
        $(".icon-search").click(function () {
            $("#SearchForm")[0].submit();
        });

        $(".make-friend").click(function () {
            var id = $(this).closest(".gamer-item").attr("id").substring("User_".length);
            friends.addFriend(id);
        });

        $(".write-fight-message").click(function () {
            var id = $(this).closest(".gamer-item").attr("id").substring("User_".length);
            if ($("#GameID").length > 0) {
                messages.writeFightMessageShow(id, $("#GameID").val());
            } else {
                messages.writeFightMessageShow(id);
            }
        });

        $(".write-invoice-message").click(function () {
            var id = $(this).closest(".gamer-item").attr("id").substring("User_".length);
            if ($("#GameID").length > 0) {
                messages.writeInvoiceMessageShow(id, $("#GameID").val());
            } else {
                messages.writeInvoiceMessageShow(id);
            }
        });
    };
}


var gamers;
$().ready(function () {
    gamers = new Gamers();
    gamers.init();
});
