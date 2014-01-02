function CreateComment() {

    var _this = this;
    var ajaxDeleteComment = "/Comment/Delete";
    this.init = function ()
    {
        $("#SubmitComment").live("click", function () {
            _this.SubmitComment();
            return false;
        });

        $(".comment-list li .close").live("click", function () {
            _this.RemoveComment($(this));
        });
    };

    this.SubmitComment = function () {
        $.ajax({
            type: "POST",
            before: function () {
              /*  $("#SubmitComment").attr("disabled", "disabled");*/
            },
            url: $("#CommentForm").attr("action"),
            data: $("#CommentForm").serialize(),
            success: function (data) {
                $("#CommentInput").html(data);
            }
        });
    };


    this.RemoveComment = function (item)
    {
        var id = item.closest("li").attr("id").substring("Comment_".length);
        $.ajax({
            type: "GET",
            url: ajaxDeleteComment,
            data: {id : id},
            success: function (data) {
                $("#Comment_" + id).remove();
            }
        });
    };
}

var createComment = null;
$().ready(function () {
    createComment = new CreateComment();
    createComment.init();
});