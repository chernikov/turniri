function Connect() {
    var _this = this;

    this.ajaxToggleTranslate = "/Connect/ToggleTranslate";

    this.init = function ()
    {
        $(".toggle-translate").click(function () {
            var type = $(this).data("type");
            var isCheck = $(this).attr("checked") == "checked";
            $.ajax({
                type: "POST",
                url: _this.ajaxToggleTranslate,
                data: { type: type, isCheck : isCheck },
                success: function ()
                {
                    
                }
            });
        });
    }


}

var connect = null;
$().ready(function () {
    connect = new Connect();
    connect.init();
  
});