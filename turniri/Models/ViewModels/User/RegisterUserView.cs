using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using turniri.Attributes.Validation;


namespace turniri.Models.ViewModels.User
{
    public class RegisterUserView : BaseUserView
    {
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage="Подтвердите пароль")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Подтвердите Email")]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "Email не совпадают")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int BirthdateDay { get; set; }

        public int BirthdateMonth { get; set; }

        public int BirthdateYear { get; set; }

        public IEnumerable<SelectListItem> BirthdateDaySelectList
        {
            get
            {
                for(int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                                     {
                                         Value = i.ToString(),
                                         Text = i.ToString(),
                                         Selected = BirthdateDay == i
                                     };
                }
            }
        }

        public IEnumerable<SelectListItem> BirthdateMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = BirthdateMonth == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> BirthdateYearSelectList
        {
            get
            {
                for (int i = 1910; i < DateTime.Now.Year; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdateYear == i
                    };
                }
            }
        }

        [Required(ErrorMessage = "Введите телефон")]
        [ValidPhone(ErrorMessage = "Введите корректный телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите код с картинки")]
        public string Captcha { get; set; }

        public bool Agreement { get; set; }
    }
}