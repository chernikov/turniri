// Utility
if (typeof Object.create !== 'function') {
    Object.create = function( obj ) {
        function F() {};
        F.prototype = obj;
        return new F();
    };
}
(function ($) {
    var BBCodeEditor = {
        
        ajaxUploadImage: "/File/UploadImage",

        smiles: ["", ":-)", ":-(", ":-D", "B-)", ":-O", ";-)", ";-(", "(:|", ":-|", ":-*", ":-P", ":-$", ":^)", "|-)", "|-(", "(inlove)", "]:)", "(talk)", "|-()", ":-&", "(doh)", "x-(", "(wasntme)", "(party)", ":-S", "(mm)", "8-|", ":-X", "(hi)", "(call)", "(devil)", "(angel)", "(envy)", "(wait)", "(hug)", "(makeup)", "(giggle)", "(clap)", "(think)", "(bow)", "(rofl)", "(whew)", "(happy)", "(smirk)", "(nod)", "(shake)", "(punch)", "(emo)", "(ok)", "(n)", "(handshake)", "(h)", "(u)", "(e)", "(f)", "(rain)", "(sun)", "(o)", "(music)", "(movie)", "(ph)", "(coffee)", "(pizza)", "(cash)", "(muscle)", "(cake)", "(beer)", "(d)", "(dance)", "(ninja)", "(*)", "(bandit)", "(bug)", "(drunk)", "(finger)", "(fubar)", "(headbang)", "(heidy)", "(mooning)", "(poolparty)", "(rock)", "(smoking)", "(swear)", "(tmi)", "(toivo)"],

        init: function (options, elem) {
            var self = this;
            
            console.log("init");
            self.elem = elem;
            self.$elem = $(elem);
            self.$elem.addClass("bbcode-textarea");

            self.$wrapper = $("<div>").addClass("bbcode-wrapper");
            self.$elem.before(self.$wrapper);
            var width = self.$elem.width() + parseInt(self.$elem.css("paddingLeft")) + parseInt(self.$elem.css("paddingRight"));
            var margin = self.$elem.css("margin");
            var border = self.$elem.css("border");

            self.$elem.detach();
            //add panel
            self.$panel = $("<div>").addClass("bbcode-panel");
            self.$wrapper.append(self.$panel);
            self.$panel.css({ width: width });
            self.$panel.css({ margin: margin });
            self.$panel.css({ border: border });
            self.$panel.css({ borderColor: self.$panel.css("backgroundColor") });

            self.$wrapper.append(self.$elem);
            self.$elem.css({ position: "relative" });
            self.initButtons();
            
            $(".bbcode-close").live("click", function ()
            {
                $(this).closest(".bbcode-window").remove();
            });
        },

        initButtons: function () {
            var self = this;
            //add buttons
            //bold
            self.$boldButton = $("<div>").addClass("bbcode-button-icon bold").attr("title", "Bold");
            self.$panel.append(self.$boldButton);
            self.$boldButton.click(function () {
                self.insertBbCode("[b]", "[/b]");
            });
            //italic
            self.$italicButton = $("<div>").addClass("bbcode-button-icon italic").attr("title", "Italic");
            self.$panel.append(self.$italicButton);
            self.$italicButton.click(function () {
                self.insertBbCode("[i]", "[/i]");
            });
            //ul
            self.$ulButton = $("<div>").addClass("bbcode-button-icon ul").attr("title", "Unordered list");
            self.$panel.append(self.$ulButton);
            self.$ulButton.click(function () {
                self.insertBbCode("[ulist]", "[/ulist]");
            });
            //ol
            self.$olButton = $("<div>").addClass("bbcode-button-icon ol").attr("title", "Ordered list");
            self.$panel.append(self.$olButton);
            self.$olButton.click(function () {
                self.insertBbCode("[olist]", "[/olist]");
            });
            //li
            self.$liButton = $("<div>").addClass("bbcode-button-icon li").attr("title", "List item");
            self.$panel.append(self.$liButton);
            self.$liButton.click(function () {
                self.insertBbCode("[*]", "[/*]");
            });
            //link
            self.$linkButton = $("<div>").addClass("bbcode-button-icon link").attr("title", "Link");
            self.$panel.append(self.$linkButton);
            self.$linkButton.click(function () {
                self.insertUrl();
            });
            //smile
            self.$smileButton = $("<div>").addClass("bbcode-button-icon smile").attr("title", "Emotions");
            self.$panel.append(self.$smileButton);
            self.$smileButton.click(function () {
                self.insertSmiles();
            });
            //img
            self.$imgButton = $("<div>").addClass("bbcode-button-icon img").attr("title", "Insert Image");
            self.$panel.append(self.$imgButton);
            self.$imgButton.click(function () {
                self.insertImg();
            });
            //code
            self.$codeButton = $("<div>").addClass("bbcode-button-icon code").attr("title", "Insert Code");
            self.$panel.append(self.$codeButton);
            self.$codeButton.click(function () {
                self.insertBbCode("[code]", "[/code]");
            });
            //quote
            self.$quoteButton = $("<div>").addClass("bbcode-button-icon quote").attr("title", "Insert Quote");
            self.$panel.append(self.$quoteButton);
            self.$quoteButton.click(function () {
                self.insertBbCode("[quote=]", "[/quote]");
            });
            //spoiler
            self.$spoilerButton = $("<div>").addClass("bbcode-button-icon spoiler").attr("title", "Insert Spoiler");
            self.$panel.append(self.$spoilerButton);
            self.$spoilerButton.click(function () {
                self.insertBbCode("[spoiler=]", "[/spoiler]");
            });
            //video
            self.$videoButton = $("<div>").addClass("bbcode-button-icon video").attr("title", "Insert Video");
            self.$panel.append(self.$videoButton);
            self.$videoButton.click(function () {
                self.insertBbCode("[video]", "[/video]");
            });
        },

        insertBbCode: function (startCode, endCode) {
            var self = this;
            if (document.selection)
            {
                //For browsers like Internet Explorer
                self.elem.focus();
                sel = document.selection.createRange();
                sel.text = startCode + sel.text + endCode;
                self.elem.focus();
            }
            else if (self.elem.selectionStart || self.elem.selectionStart == '0')
            {
                //For browsers like Firefox and Webkit based
                var startPos = self.elem.selectionStart;
                var endPos = self.elem.selectionEnd;
                var scrollTop = self.elem.scrollTop;

                self.elem.value = self.elem.value.substring(0, startPos) + startCode
                + self.elem.value.substring(startPos, endPos)
                + endCode + self.elem.value.substring(endPos, self.elem.value.length);
                self.$elem.blur().focus();
                self.elem.selectionStart = startPos + startCode.length;
                self.elem.selectionEnd = endPos + startCode.length;
                self.elem.scrollTop = scrollTop;
            } else {
                self.elem.value += startCode + endCode;;
                self.elem.blur().focus();
            }
        },

        insertUrl: function () {
            var self = this;

            self.$modal = $("<div>").addClass("bbcode-window");
            $("<div>").addClass("bbcode-close").appendTo(self.$modal);

            self.$imgWrapper = $("<div>").addClass("bbcode-img-input-wrapper").appendTo(self.$modal);
            $("<div>").addClass("bbcode-title").text("Ссылка:").appendTo(self.$imgWrapper);
            self.$input = $("<input>").attr("type", "text").addClass("bbcode-image-input").appendTo(self.$imgWrapper);
            self.$button = $("<button>").attr("id", "AddImageByLink").text("Добавить").addClass("bbcode-image-button").appendTo(self.$imgWrapper);

            self.$button.click(function () {
                if (self.$input.val()) {
                    self.insertBbCode("[url=" + self.$input.val() + "]", "[/url]");
                    $(".bbcode-close").click();
                }
            });
            self.$modal.appendTo(self.$wrapper);
        },

        insertImg: function () {
            var self = this;

            self.$modal = $("<div>").addClass("bbcode-window");
            $("<div>").addClass("bbcode-close").appendTo(self.$modal);

            self.$upload = $("<div>").addClass("bbcode-upload-image").text("Загрузить изображение").appendTo(self.$modal);
            InitUpload(self.$upload[0], 
                false, 
                self.ajaxUploadImage, 
                function (id, fileName, responseJSON) {
                    if (responseJSON.result == "ok") {
                        self.$input.val(responseJSON.data.imageFile)
                    }
                },
            null, "Загрузить изображение");

            self.$imgWrapper = $("<div>").addClass("bbcode-img-input-wrapper").appendTo(self.$modal);
            $("<div>").addClass("bbcode-title").text("Ссылка:").appendTo(self.$imgWrapper);
            self.$input = $("<input>").attr("type", "text").addClass("bbcode-image-input").appendTo(self.$imgWrapper);
            self.$button = $("<button>").attr("id", "AddImageByLink").text("Добавить").addClass("bbcode-image-button").appendTo(self.$imgWrapper);

            self.$button.click(function () {
                if (self.$input.val()) {
                    self.insertBbCode("[img]" + self.$input.val() + "[/img]", "");
                    $(".bbcode-close").click();
                }
            });
            self.$modal.appendTo(self.$wrapper);
        },

        insertSmiles: function ()
        {
            var self = this;

            self.$modal = $("<div>").addClass("bbcode-window");
           
            $("<div>").addClass("bbcode-close").appendTo(self.$modal);
            self.$modal.appendTo(self.$wrapper);

            self.$imgWrapper = $("<div>").addClass("bbcode-smiles-wrapper").appendTo(self.$modal);
            var height = self.$elem.height() - 100;
            self.$imgWrapper.css({ "max-height": height });
            console.log(height);
            for (var i = 1; i <= 85; i++)
            {
                var smileDiv = $("<div>").addClass("bbcode-smile").data("code", self.smiles[i]).attr("title", self.smiles[i]).appendTo(self.$imgWrapper);
                $("<img>").attr("src", "/Media/files/smiles/" + (i > 9 ? i : "0" + i) + ".gif").appendTo(smileDiv);

            }

            $(".bbcode-smile").click(function () {
                self.insertBbCode($(this).data("code"), "");
                $(".bbcode-close").click();
            });
        }
    }

    $.fn.queryBBCodeEditor = function (options) {
        return this.each(function () {
            var bbCodeEditor = Object.create(BBCodeEditor);
            bbCodeEditor.init(options, this);
            $.data(this, 'queryBBCodeEditor', bbCodeEditor);
        });
    }

    $.fn.queryBBCodeEditor.options = {
        
    };

})(jQuery);