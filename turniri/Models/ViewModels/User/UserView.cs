using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Attributes.Validation;
using turniri.Model;


namespace turniri.Models.ViewModels.User
{
    public class UserView : BaseUserView
    {
        public string AvatarPath173 { get; set; }

        public string AvatarPath96 { get; set; }

        public string AvatarPath84 { get; set; }

        public string AvatarPath57 { get; set; }

        public string AvatarPath30 { get; set; }

        public string AvatarPath26 { get; set; }

        public string AvatarPath18 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        [ValidPhone(ErrorMessage = "Введите корректный телефон")]
        public string Phone { get; set; }

        public int BirthdateDay { get; set; }

        public int BirthdateMonth { get; set; }

        public int BirthdateYear { get; set; }

        public IEnumerable<SelectListItem> BirthdateDaySelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
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

        public string PlaystationID { get; set; }

        public string XboxGametag { get; set; }

        public string EAAccount { get; set; }

        public string SteamAccount { get; set; }

        public string GarenaAccount { get; set; }

        public string ICQ { get; set; }

        public string Skype { get; set; }

        public string Vk { get; set; }

        public string FullAvatarPath173
        {
            get { return AvatarPath173 ?? "/Media/images/default_avatar_173.png"; }
        }

        public bool Subscription { get; set; }

        [StringLength(500, ErrorMessage = "Допустимая длина подписи 500 символов")]
        public string Signature { get; set; }
    }
}