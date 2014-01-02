using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;

namespace turniri.Models.ViewModels
{ 
	public class TeamView
    {
        public int _ID { get; set; }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int MoneyType { get; set; }
        
        public double? Fee { get; set; }

        public int? GroupID { get; set; }

        public int TournamentID { get; set; }

        public bool Disabled { get; set; }

        public int TournamentGameID { get; set; }

        [Required(ErrorMessage = "Введите наименование команды")]
		public string Name {get; set; }

		public string ImagePath18 {get; set; }

		public string ImagePath26 {get; set; }

        [Required(ErrorMessage="Добавьте аватарку")]
		public string ImagePath30 {get; set; }

		public DateTime AddedDate {get; set; }

        public string FullImagePath30
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath30)
                           ? ImagePath30 : "/Media/images/default_avatar_30.png";
            }
        }

        public int CaptainID { get; set; }

        [Required]
        public string CaptainLogin { get; set; }
    }
}