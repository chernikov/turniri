function Friends() {
    var _this = this;

    this.ajaxAddFriend = "/Friend/AddFriend";
    this.ajaxRemoveFriend = "/Friend/RemoveFriend";
    this.ajaxConfirmFriendship = "/Friend/ConfirmFriendship";
    this.ajaxDeclineFriendship = "/Friend/DeclineFriendship";

    this.init = function () {
        _this.tabID = $("#TabID").val();
        
        $("#tab_friends_1").click(function () {
            _this.tabID = 1;
            if (window.location.pathname == "/Friend/Income") {
                common.rewriteUrl("/Friend");
            }
        });
        $("#tab_friends_2").click(function () {
            _this.tabID = 2;
            common.rewriteUrl("/Friend/Income");
        });
        $("#tab_friends_" + _this.tabID).click();
        $(".friend-wrapper .paging .paging-list a").live('click', function () {
            var href = $(this).attr("href");
            var wrapper = $(this).closest(".switcher-content");
            $.ajax({
                type: "GET",
                url: href,
                success: function (data) {
                    wrapper.html(data);
                }
            })
            return false;
        });

        $(".add-user").live("click", function () {
            var id = $(this).closest(".friend-item").attr("id").substring("User_".length);
            _this.addFriend(id);
        });

        $(".delete-user").live("click", function () {
            var id = $(this).closest(".friend-item").attr("id").substring("User_".length);
            _this.deleteFriend(id);
        });

        $(".add-friend").live("click", function () {
            var id = $(this).closest(".friend-item").attr("id").substring("Friendship_".length);
            _this.addFriendship(id);
        });

        $(".reject-friend").live("click", function () {
            var id = $(this).closest(".friend-item").attr("id").substring("Friendship_".length);
            _this.deleteFriendship(id);
        });

        $(".write-message").live("click", function () {
            var id = $(this).closest(".friend-item").attr("id").substring("User_".length);
            messages.writeMessageShow(id);
        });

        $(".write-fight-message").live("click", function () {
            var id = $(this).closest(".friend-item").attr("id").substring("User_".length);
            messages.writeFightMessageShow(id);
        });

    };

    this.addFriend = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxAddFriend,
            data: { id: id },
            success: function (data) {
                $(".gray-background").show();
                $("#PopupWrapper").html(data);
                $(".popup-window", $("#PopupWrapper")).centerInClient();
            }
        });
    }

    this.deleteFriend = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxRemoveFriend,
            data: { id: id },
            success: function (data)
            {
                $(".delete-user", $("#User_" + id)).addClass("add-user").text("Добавить в друзья").removeClass("delete-user");
            }
        });
    }

    this.addFriendship = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxConfirmFriendship,
            data: { id: id },
            success: function (data) {
                window.location.reload();
                
            }
        });
    }

    this.deleteFriendship = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxDeclineFriendship,
            data: { id: id },
            success: function (data) {
                window.location.reload();
            }
        });
    }

}


