function SocialPost() {

    var _this = this;
    this.ajaxUploadSocialPreview = "/admin/File/UploadSocialPreview";
    this.ajaxUpdateGroupsSelect = "/admin/SocialPost/UpdateGroupsSelect";
    this.ajaxSocialPost = "/admin/SocialPost/SocialPost";

    this.init = function () {
       
        var titlePreview = $("#ChangePreview").text();

        InitUpload($("#ChangePreview")[0],
            false,
            _this.ajaxUploadSocialPreview,
            function (id, fileName, responseJSON) 
            {
                if (responseJSON.result == "ok")
                {
                    _this.ChangePreview(responseJSON.data);
                }
            },
            null, titlePreview);

        this.deletePreview = function ()
        {
            $("#PreviewImage").attr("src", "/Media/images/social-logo.jpg");
            $("#Preview").val("");
        };

        this.ChangePreview = function (data) {
            $("#PreviewImage").attr("src", data.ImagePath);
            $("#Preview").val(data.ImagePath);
        };

        $("#Provider").change(function () {
            _this.updateGroups();
        });

        $("#PublishSocialBtn").click(function () {
            _this.publish();
        });
    };

    this.updateGroups = function () {
        var id = $("#Provider").val();
        $.ajax({
            type: "POST",
            url: _this.ajaxUpdateGroupsSelect,
            data: { id : id },
            success: function (data) {
                $("#GroupSelectWrapper").html(data);
            }
        });
    };

    this.publish = function ()
    {
        var ajaxData = $("#PublishSocialForm").serialize();
        $.ajax({
            type: "POST",
            url: _this.ajaxSocialPost,
            data: ajaxData,
            success: function (data)
            {
                $("#socialWrapper").html(data);
            }
        });
    }
}
