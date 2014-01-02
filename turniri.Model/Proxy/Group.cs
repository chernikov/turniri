using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Group
    {
        public enum StateType
        {
            Registered = 0x00, 
            Live = 0x01, 
            Removed = 0x02
        }

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

        public int MembersCount
        {
            get
            {
                return UserGroups.Count(p => p.Status == (int)UserGroup.StatusEnum.Granded);
            }
        }

        public IEnumerable<User> Members
        {
            get
            {
                return UserGroups.Select(p => p.User);
            }
        }

        public IEnumerable<User> Captains
        {
            get
            {
                return UserGroups.Where(p => p.Status == (int)UserGroup.StatusEnum.Granded).
                    Select(p => p.User).Where(p => 
                        p.UserRoles.Any(ur => 
                            string.Compare(ur.Role.Code, "group_captain", true) == 0
                            && ur.UserRoleGroups.Any(ug => ug.GroupID == ID)));;

            }
        }

        public bool AskedForNewMembers
        {
            get
            {
                return UserGroups.Any(p => p.Status == (int)UserGroup.StatusEnum.Asked);
            }
        }

        public bool IsMemberOf(User user)
        {
            if (user == null)
            {
                return false;
            }
            return UserGroups.Any(p => p.UserID == user.ID);
        }

        public bool IsGrantedMemberOf(User user)
        {
            if (user == null)
            {
                return false;
            }
            return UserGroups.Any(p => p.UserID == user.ID && p.Status == (int)UserGroup.StatusEnum.Granded);
        }

        public int TotalUserRating
        {
            get
            {
                return UserGroups.Select(p => p.User).ToList().Sum(p => p.Rating(GameID).TotalScore);
            }
        }

        public IEnumerable<UserGroup> SubUserGroups
        {
            get
            {
                return UserGroups.OrderByDescending(p => p.User.Ratings.Any(r => r.GameID == GameID) ? p.User.Ratings.First(r => r.GameID == GameID).TotalScore : 0).AsEnumerable();
            }
        }

        public IEnumerable<UserGroupHistory> SubUserGroupsHistories
        {
            get
            {
                return UserGroupHistories.OrderByDescending(p => p.AddedDate).AsEnumerable();
            }
        }

        public IEnumerable<UserGroup> GrantedUserGroups
        {
            get
            {
                return UserGroups.Where(p => p.Status == (int)UserGroup.StatusEnum.Granded).OrderByDescending(p => p.User.Ratings.Any(r => r.GameID == GameID) ? p.User.Ratings.First(r => r.GameID == GameID).TotalScore : 0).AsEnumerable();
            }
        }

        public IEnumerable<Award> AwardPage(int page = 1, int itemPerPage = 20)
        {
            return Awards.OrderByDescending(p => p.AwardedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int AwardsCount
        {
            get { return Awards.Count; }
        }

        public int AwardCountPage(int itemPerPage = 20)
        {
            return AwardsCount / itemPerPage + (AwardsCount % itemPerPage != 0 ? 1 : 0);
        }

        public int BlogsCount
        {
            get { return Blogs.Count; }
        }

        public IEnumerable<Blog> BlogPage(int page = 1, int itemPerPage = 5)
        {
            return Blogs.OrderByDescending(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int CountPage(int itemPerPage = 5)
        {
            return BlogsCount / itemPerPage + (BlogsCount % itemPerPage != 0 ? 1 : 0);
        }

        public IEnumerable<Comment> LastBlogComments
        {
            get
            {
                return
                    Blogs.SelectMany(p => p.BlogComments).Select(p => p.Comment).OrderByDescending(p => p.AddedDate).
                        Take(6);
            }
        }

        public int UserVideosCount
        {
            get { return UserVideos.Count; }
        }

        public IEnumerable<UserVideo> UserVideoPage(int page = 1, int itemPerPage = 5)
        {
            return UserVideos.OrderByDescending(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int UserVideosCountPage(int itemPerPage = 5)
        {
            return UserVideosCount / itemPerPage + (UserVideosCount % itemPerPage != 0 ? 1 : 0);
        }


        public IEnumerable<PhotoAlbum> SubPhotoAlbums
        {
            get
            {
                return PhotoAlbums.AsEnumerable();
            }
        }

        public int? PlaceOfSeason(LeagueSeason season)
        {
            var participant = User.Participants
                .SelectMany(p => p.LeagueParticipants)
                .FirstOrDefault(r => r.LeagueSeasonID == season.ID);
            if (participant != null)
            {
                return participant.Place;
            }
            return null;
        }

        public string ColorPlaceOfSeason(LeagueSeason season)
        {
            var participant = User.Participants
               .SelectMany(p => p.LeagueParticipants)
               .FirstOrDefault(r => r.LeagueSeasonID == season.ID);

            if (participant != null)
            {
                var level = participant.LeagueLevel;

                var quantity = level.Quantity;

                if (participant.Place <= quantity / 2)
                {
                    return "success";
                }
                if (participant.Place <= quantity / 4 * 3)
                {
                    return "warning";
                }
                return "important";
            }
            return string.Empty;
        }

        public int? LeagueLevel(LeagueSeason season)
        {
            var participant = User.Participants
                .SelectMany(p => p.LeagueParticipants)
                .FirstOrDefault(r => r.LeagueSeasonID == season.ID);
            if (participant != null)
            {
                return participant.LeagueLevelID;
            }
            return null;
        }

        public string LeagueLevelName(LeagueSeason season)
        {
            var participant = User.Participants
                .SelectMany(p => p.LeagueParticipants)
                .FirstOrDefault(r => r.LeagueSeasonID == season.ID);
            if (participant != null)
            {
                return participant.LeagueLevel.Name;
            }
            return null;
        }



        public Tournament TournamentOfSeason(LeagueSeason season)
        {
            var participant = User.Participants
               .SelectMany(p => p.LeagueParticipants)
               .FirstOrDefault(r => r.LeagueSeasonID == season.ID);
            if (participant != null)
            {
                return participant.Participant.Tournament;
            }
            return null;
        }
	}
}