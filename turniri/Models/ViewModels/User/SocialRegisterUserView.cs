using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace turniri.Models.ViewModels.User
{
    public class SocialRegisterUserView
    {
        public Model.Social.ProviderType Provider { get; set; }

        public string Identifier { get; set; }

        public string UserInfo { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public bool VerifiedEmail { get; set; }

        public string Phone { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Avatar { get; set; }

        public DateTime? Birthdate {get; set; }

    }
}