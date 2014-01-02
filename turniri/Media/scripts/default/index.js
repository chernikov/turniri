function Index() {
    var _this = this;

    this.currentItem = null;

    this.init = function () {
        $('.main-gallery li').live('click', function () {
            _this.changeMainGallery($(this));
        });
        _this.currentItem = $('.main-gallery li').first();

        $('.main-gallery li:first .image-item').addClass('current');
        $('.main-image > div').hide();
        $('.main-image > div:first').show();

        $(".main-news .switcher li.current").removeClass("current");
        $(".main-news .switcher li:first").addClass("current");
        $(".news-wrapper .switcher-content").hide();
        $(".news-wrapper .switcher-content:first").show();

        $(".news_preview").click(function () {
            window.location = "/new/" + $(this).data("url");
        });
        $(".news-wrapper .paging .paging-list a").live('click', function () {
            var href = $(this).attr("href");
            var wrapper = $(this).closest(".news-type");
            $.ajax({
                type: "GET",
                url: href,
                success: function (data) {
                    wrapper.html(data);
                }
            })
            return false;
        });

        $(".report").click(function () {
            var id = $(this).attr("id").substring("Match_".length);
            match.ShowMatch(id);
        });

        if ($("#MatchID").length > 0) {
            var val = $("#MatchID").val();
            if (parseInt(val, 10) > 0) {
                match.ShowMatch(val);
            }
        }
        _this.autoSlide();
    };

    this.changeMainGallery = function (item)
    {
        _this.currentItem = item;
        var id = item.attr('id').substring("NewThumb_".length);

        $('.main-image > div:visible').fadeOut(1000);
        $('.main-gallery li .image-item.current').removeClass('current');
        $('.image-item', item).addClass('current');
        $("#NewPreview_" + id).fadeIn(1000);
    }

    this.autoSlide = function () {
        setInterval(function () {
            _this.currentItem = _this.currentItem.next();
            if (_this.currentItem.length == 0) {
                _this.currentItem = $('.main-gallery li').first();
            }
            _this.changeMainGallery(_this.currentItem);
        }, 5000);
    };
}


var index;
$().ready(function () {
    index = new Index();
    index.init();
});