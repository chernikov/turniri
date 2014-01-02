using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class BannerView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Задайте путь")]
        public string Path { get; set; }

        [Required(ErrorMessage = "Укажите ссылку")]
        public string Url { get; set; }
        
        public int Type { get; set; }

        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem() { Value = "0", Text = "Большой баннер", Selected = Type == 0 };
                yield return new SelectListItem() { Value = "1", Text = "Маленький баннер", Selected = Type == 1 };
            }
        }

        [Required(ErrorMessage = "Добавьте изображение")]
        public string ImagePath { get; set; }

        public bool IsOn { get; set; }
    }
}