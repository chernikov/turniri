function LoadCssDynamically(fileName) {
    var fileref = $('<link>');
    fileref.attr("rel", "stylesheet");
    fileref.attr("type", "text/css");
    fileref.attr("href", fileName);
    $("head").append(fileref);
}

function LoadJsDynamically(fileName) {
    var fileref = $('<script>');
    fileref.attr("type", "text/javascript");
    fileref.attr("src", fileName);

    $("head").append(fileref);
}


function InitUpload(item, multiple, url, oncomplete, extensions, title) {
    if (extensions == null) {
        extensions = [];
    }
    if (typeof (qq) == 'undefined') {
        LoadCssDynamically("/Media/css/fineuploader.css");
        LoadJsDynamically("/Media/scripts/jquery.fineuploader-3.0.js");
    }

    var obj = new qq.FineUploader({
        element: item,
        multiple: multiple,
        request: {
            endpoint: url
        },
        text: {
            uploadButton: title
        },
        callbacks: {
            onComplete: oncomplete
        },
        validation: {
            allowedExtensions: extensions
        }
    });
}

function InitBbCodeEditor(item) {
    var _this = this;

    if ($.fn.queryBBCodeEditor == null) {
        LoadCssDynamically("/Media/css/bbCodeEditor.css");
        LoadJsDynamically("/Media/scripts/jquery.bbCodeEditor.js");
    }
    item.queryBBCodeEditor();
}


function Common() 
{
    var _this = this;

    this.init = function () 
    {
        $(".btn-danger, .delete-action").live("click", function () {
            if (!$(this).hasClass("no-stop"))
            {
                return confirm("Вы действительно хотите удалить?");
            }
        });

        $(".stop-action").live("click", function () {
            return confirm("Вы действительно хотите это сделать?");
        });

        //все дейтпикеры
        $.datepicker.setDefaults($.datepicker.regional["ru"]);
        $(".datePicker").datepicker({
            dateFormat: "dd.mm.yy",
            changeYear: true,
            yearRange: '-80:+1'
        });

        $('.spoiler > span').live("click", function ()
        {
            var spoiler = $(this).closest(".spoiler");
            $(".wrapper", spoiler).toggleClass("hidden");
        });
    };
}

var common;
$().ready(function () {
    common = new Common();
    common.init();
});


function Lang()
{
    var _this = this;

    this.init = function () 
    {
        $("#selectedLang").change(function () {
            $("#SelectLangForm").submit();
        });
    };
}

var lang;
$().ready(function () {
    lang = new Lang();
    lang.init();
});
