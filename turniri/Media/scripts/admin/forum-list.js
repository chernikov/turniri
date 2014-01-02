function ForumList() {
    var _this = this;
    var errorDialog;

    this.AjaxGetForumUrl = "/admin/Forum/SubForum";
    this.AjaxUpdateForumMoveUrl = "/admin/Forum/AjaxForumMove";
    this.AjaxUpdateForumOrderUrl = "/admin/Forum/AjaxForumOrder";

    this.init = function ()
    {
        _this.initDraggable();

        _this.initSortable($(".forumList"));
        var sortingForumHolder = $(".forumList");
       

        $(".open").live("click", function () {
            _this.getCatalog($(this));
        });
    }

    this.initSortable = function (item)
    {
        item.sortable({
            placeholder: 'forum-placeholder ui-state-highlight',
            stop: function (event, ui) {
                var sortingInfo = [];
                $("> .forumItem", $(this)).each(function() 
                {
                    sortingInfo.push($(this).data("id"));
                });
                var isNeedUpdate = false;
                var itemId = ui.item.data("id");

                var ajaxData = null;
                for (var i = sortingInfo.length; i--;)
                {
                    if (sortingInfo[i] != itemId)
                    {
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

                if (!isNeedUpdate)
                {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: _this.AjaxUpdateForumOrderUrl,
                    data: ajaxData,
                    success: function (data) {
                        if (data.result == "error") {
                            $(this).sortable('cancel');
                            alert("Ошибка");
                        }
                    },
                    error: function () {
                        $(this).sortable('cancel');
                        alert("Внутренняя ошибка");
                    }
                });
            }
        });
    }

    this.initDraggable = function ()
    {
        $(".forumItem").droppable({
            accept: '.subForumItem',
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
                    url: _this.AjaxUpdateForumMoveUrl,
                    data: ajaxData,
                    success: function (data) {
                        if (data.result == "ok") {
                            window.location.reload();
                        }
                        if (data.result == "error") {
                            $(this).droppable('cancel');
                        }
                    },
                    error: function () {
                        $(this).droppable('cancel');
                        alert("Внутренняя ошибка");
                    }
                });
            }
        });

        $(".subForumItem").draggable({
            helper: function (event) {
                return $("<div class='ui-widget-move'>Перемещение</div>");
            },
            start: function (event, ui) {
                $(".forumItem").addClass("highlight");
            },
            stop: function (event, ui) {
                $(".forumItem").removeClass("highlight");
            }
        });
    }

    this.getCatalog = function (item)
    {
        var id = item.closest(".forumItem").data("id");
        var wrapper = $(".sub-wrapper", item.closest(".forumItem"));
        if ($(".sub-forum", wrapper).length > 0) {
            wrapper.toggle();
        } else {
            $.ajax({
                type: "GET",
                url: _this.AjaxGetForumUrl,
                data: { id: id },
                success: function (data) {
                    wrapper.html(data);
                    _this.initDraggable();
                    _this.initSortable($(".forumList"), wrapper);
                }
            });
        }
    }
}

var forumList;
$().ready(function () {
    forumList = new ForumList();
    forumList.init();
});