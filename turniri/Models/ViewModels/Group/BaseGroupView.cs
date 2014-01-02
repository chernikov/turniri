using ManageAttribute;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels.User;
using turniri.Attributes.Validation;


namespace turniri.Models.ViewModels
{
    public abstract class BaseGroupView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите наименование")]
        public string Name { get; set; }

        public string SavedUrl { get; set; }

        [Required(ErrorMessage = "Введите латинское наименование")]
        [ValidLatinValidation(ErrorMessage = "Наименование должно состоять из латинских букв, цифр и знака \"_\"")]
        public string Url { get; set; }

        public int GameID { get; set; }


        public int UserID { get; set; }
        
        [Required(ErrorMessage = "Добавьте изображение")]
        public string LogoPath173 { get; set; }

        public string LogoPath96 { get; set; }

        public string LogoPath84 { get; set; }

        public string LogoPath57 { get; set; }

        public string LogoPath30 { get; set; }

        public string LogoPath26 { get; set; }

        public string LogoPath18 { get; set; }

        public string FullLogoPath173
        {
            get { return LogoPath173 ?? "/Media/images/default_group_173.png"; }
        }

        public string FullLogoPath96
        {
            get { return LogoPath96 ?? "/Media/images/default_group_96.png"; }
        }

        public string FullLogoPath84
        {
            get { return LogoPath84 ?? "/Media/images/default_group_84.png"; }
        }

        public string FullLogoPath57
        {
            get { return LogoPath57 ?? "/Media/images/default_group_57.png"; }
        }

        public string FullLogoPath30
        {
            get { return LogoPath30 ?? "/Media/images/default_group_30.png"; }
        }

        public string FullLogoPath26
        {
            get { return LogoPath26 ?? "/Media/images/default_group_26.png"; }
        }

        public string FullLogoPath18
        {
            get { return LogoPath18 ?? "/Media/images/default_group_18.png"; }
        }

        public string Description { get; set; }

        
    }
}