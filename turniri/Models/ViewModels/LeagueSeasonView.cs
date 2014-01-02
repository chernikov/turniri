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
    public class LeagueSeasonView
    {
        public int _ID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int ID { get; set; }

        public int LeagueID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndMainTourDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required(ErrorMessage="Введите наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Добавьте изображение")]
        public string Image { get; set; }

        public string FullImage
        {
            get { return string.IsNullOrWhiteSpace(Image) ? "/Media/images/default_game.jpg" : Image; }
        }

        public LeagueSeasonView()
        {
            StartDate = DateTime.Now;
            EndMainTourDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
    }
}