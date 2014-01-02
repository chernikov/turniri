using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class ParticipantView
    {
        public int ID { get; set; }

        public int TournamentID { get; set; }

        public string UserLogin { get; set; }

        public string UserAvatarPath18 { get; set; }

        public string UserAvatarPath26 { get; set; }

        public string UserAvatarPath30 { get; set; }

	    public int UserID {get; set; }

		public int? TournamentGroupID {get; set; }

		public string Name {get; set; }

        public string ImagePath18 { get; set; }

        public string ImagePath26 { get; set; }

        public string ImagePath30 { get; set; }

        public string FullImagePath18 { 
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath18)
                           ? ImagePath18
                           : (!string.IsNullOrWhiteSpace(UserAvatarPath18)
                                  ? UserAvatarPath18
                                  : "/Media/images/default_avatar_18.png");
            } 
        }

        public string FullImagePath26
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath26)
                           ? ImagePath26
                           : (!string.IsNullOrWhiteSpace(UserAvatarPath26)
                                  ? UserAvatarPath26
                                  : "/Media/images/default_avatar_26.png");
            }
        }

        public string FullImagePath30
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath30)
                           ? ImagePath30
                           : (!string.IsNullOrWhiteSpace(UserAvatarPath30)
                                  ? UserAvatarPath30
                                  : "/Media/images/default_avatar_30.png");
            }
        }

    }
}