using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class MoneyDetailView
    {
        public int ID { get; set; }

        public int? MoneyFeeID { get; set; }

        public int? UserID { get; set; }

        public int? GroupID { get; set; }

        public int? TournamentID { get; set; }

        public Guid Global { get; set; }

        public double SumGold { get; set; }

        public double SumWood { get; set; }

        public double SumCrystal { get; set; }

        public string Description { get; set; }
    }
}