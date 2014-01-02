using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using turniri.Model;
using System.ComponentModel.DataAnnotations;


namespace turniri.Models.ViewModels
{
    public class LeagueLevelView
    {
        public int _ID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int ID { get; set; }

        public int LeagueID { get; set; }

        [Required(ErrorMessage="Введите наименование")]
        public string Name { get; set; }

        public int Level { get; set; }

        public int Quantity { get; set; }

        [Required(ErrorMessage="Добавьте изображение")]
        public string Image { get; set; }

        public string FullImage
        {
            get { return string.IsNullOrWhiteSpace(Image) ? "/Media/images/default_game.jpg" : Image; }
        }
    }
}