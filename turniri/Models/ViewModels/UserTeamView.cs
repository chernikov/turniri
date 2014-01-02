using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class UserTeamView
    {
        public int _ID { get; set; }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int TeamID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage="¬ведите им€")]
        public string UserLogin { get; set; }
    }
}