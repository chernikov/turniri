using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Models.Info
{
    public class BothTexts
    {
        public enum TextTypeEnum
        {
            Blog = 0x01, 
            ForumMessage = 0x02, 
            Game = 0x03, 
            New = 0x04, 
            Page = 0x05
        }

        public int ID { get; set; }

        public string HtmlText { get; set; }

        public string BbCodeText { get; set; }

        public TextTypeEnum TextType { get; set; }
    }
}