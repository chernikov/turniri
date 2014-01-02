using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class User
    {
        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }
            return roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Any(role => UserRoles.Any(p => string.Compare(p.Role.Code, role, true) == 0));
        }

        public bool IsOnline
        {
            get
            {
                return LastVisitDate.AddMinutes(5) > DateTime.Now;
            }
        }

        public string FullAvatarPath173
        {
            get { return AvatarPath173 ?? "/Media/images/default_avatar_173.png"; }
        }

        public string FullAvatarPath96
        {
            get { return AvatarPath96 ?? "/Media/images/default_avatar_96.png"; }
        }

        public string FullAvatarPath84
        {
            get { return AvatarPath84 ?? "/Media/images/default_avatar_84.png"; }
        }

        public string FullAvatarPath57
        {
            get { return AvatarPath57 ?? "/Media/images/default_avatar_57.png"; }
        }

        public string FullAvatarPath30
        {
            get { return AvatarPath30 ?? "/Media/images/default_avatar_30.png"; }
        }

        public string FullAvatarPath26
        {
            get { return AvatarPath26 ?? "/Media/images/default_avatar_26.png"; }
        }

        public string FullAvatarPath18
        {
            get { return AvatarPath18 ?? "/Media/images/default_avatar_18.png"; }
        }

        public int TotalForumMessages
        {
            get { return ForumMessages1.Count; }
        }

        public int TotalForumModeratedMessages
        {
            get { return ForumMessages.Count; }
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

        public IEnumerable<Award> AwardPage(int page = 1, int itemPerPage = 20)
        {
            return UserAwards.Select(p => p.Award).OrderByDescending(p => p.AwardedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int AwardsCount
        {
            get { return UserAwards.Count; }
        }

        public int AwardCountPage(int itemPerPage = 20)
        {
            return AwardsCount / itemPerPage + (AwardsCount % itemPerPage != 0 ? 1 : 0);
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

        public IEnumerable<PhotoAlbum> SubPhotoAlbums
        {
            get
            {
                return PhotoAlbums.AsEnumerable();
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

        public int UserVideoCountPage(int itemPerPage = 5)
        {
            return UserVideosCount / itemPerPage + (UserVideosCount % itemPerPage != 0 ? 1 : 0);
        }

        public int UserCommentCount
        {
            get { return UserComments.Count; }
        }

        public IEnumerable<Comment> UserCommentPage(int page = 1, int itemPerPage = 8)
        {
            return UserComments.Select(p => p.Comment).OrderByDescending(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int UserCommentCountPage(int itemPerPage = 8)
        {
            return UserCommentCount / itemPerPage + (UserVideosCount % itemPerPage != 0 ? 1 : 0);
        }

        public IEnumerable<Game> SubGames
        {
            get { return UserGames.Select(p => p.Game).AsEnumerable(); }
        }

        public IQueryable<Match> AllMatches
        {
            get
            {
                //player1
                var matches1 = Participants.SelectMany(p => p.Matches);
                //player2
                var matches2 = Participants.SelectMany(p => p.Matches1);

                var allMatches = matches1.Union(matches2).AsQueryable();

                return matches1.Union(matches2).AsQueryable();
            }
        }

        public bool HasTournament
        {
            get
            {
                return AllMatches.Any(p => p.TournamentID != null);
            }
        }

        public IQueryable<Tournament> AllTournaments
        {
            get
            {
                return Participants.Where(p => p.TournamentID.HasValue).Select(p => p.Tournament).OrderByDescending(p => p.ID).AsQueryable();
            }
        }

        public IQueryable<Match> WinnerMatches
        {
            get
            {
                //winner 
                return Participants.SelectMany(p => p.Matches2).Where(p => p.Status == (int)Match.MatchStatusEnum.Submit).AsQueryable();
            }
        }

        public bool HasGame(int idGame)
        {
            return UserGames.Any(p => p.GameID == idGame);
        }

        public Rating Rating(int idGame)
        {
            var rating = Ratings.FirstOrDefault(p => p.GameID == idGame);
            if (rating != null)
            {
                return rating;
            }
            return new Rating();
        }

        public int Level(int idGame)
        {
            var rating = Ratings.FirstOrDefault(p => p.GameID == idGame);
            if (rating != null)
            {
                return rating.Level;
            }
            return 0;
        }

        public int CountAllMatches(int idGame)
        {
            return AllMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.Submit && !p.Technical).Count(p => p.GameID == idGame);
        }

        public int CountWins(int idGame)
        {
            return WinnerMatches.Count(p => p.GameID == idGame && !p.Technical);
        }

        public int CountDraws(int idGame)
        {
            return AllMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.Submit && !p.Technical && p.WinnerID == null).Count(p => p.GameID == idGame);
        }

        public int CountLoses(int idGame)
        {
            return AllMatches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit && !p.Technical && p.GameID == idGame) - CountWins(idGame) - CountDraws(idGame);
        }

        public static int LevelByPoints(int points)
        {
            if (points >= 83200)
            {
                return 20;
            }
            if (points >= 59200)
            {
                return 19;
            }
            if (points >= 41200)
            {
                return 18;
            }
            if (points >= 29200)
            {
                return 17;
            }
            if (points >= 20200)
            {
                return 16;
            }
            if (points >= 14200)
            {
                return 15;
            }
            if (points >= 9700)
            {
                return 14;
            }
            if (points >= 6700)
            {
                return 13;
            }
            if (points >= 4780)
            {
                return 12;
            }
            if (points >= 3340)
            {
                return 11;
            }
            if (points >= 2380)
            {
                return 10;
            }
            if (points >= 1660)
            {
                return 9;
            }
            if (points >= 1180)
            {
                return 8;
            }
            if (points >= 820)
            {
                return 7;
            }
            if (points >= 580)
            {
                return 6;
            }
            if (points >= 400)
            {
                return 5;
            }
            if (points >= 180)
            {
                return 4;
            }
            if (points >= 90)
            {
                return 3;
            }
            if (points >= 30)
            {
                return 2;
            }
            return 1;
        }

        public static int PointsToNextLevel(int points)
        {
            if (points >= 83200)
            {
                return 119200;
            }
            if (points >= 59200)
            {
                return 83200;
            }
            if (points >= 41200)
            {
                return 59200;
            }
            if (points >= 29200)
            {
                return 41200;
            }
            if (points >= 20200)
            {
                return 29200;
            }
            if (points >= 14200)
            {
                return 20200;
            }
            if (points >= 9700)
            {
                return 14200;
            }
            if (points >= 6700)
            {
                return 9700;
            }
            if (points >= 4780)
            {
                return 6700;
            }
            if (points >= 3340)
            {
                return 4780;
            }
            if (points >= 2380)
            {
                return 3340;
            }
            if (points >= 1660)
            {
                return 2380;
            }
            if (points >= 1180)
            {
                return 1660;
            }
            if (points >= 820)
            {
                return 1180;
            }
            if (points >= 580)
            {
                return 820;
            }
            if (points >= 400)
            {
                return 580;
            }
            if (points >= 180)
            {
                return 400;
            }
            if (points >= 90)
            {
                return 180;
            }
            if (points >= 30)
            {
                return 90;
            }
            return 30;
        }

        public IQueryable<Reputation> MyReputations
        {
            get
            {
                return Reputations.AsQueryable();
            }
        }

        public IQueryable<Grade> MyGrades
        {
            get
            {
                return Grades.AsQueryable();
            }
        }

        public IQueryable<Grade> MyVotedGrades
        {
            get
            {
                return Grades1.AsQueryable();
            }
        }

        public bool IsVotedGrade(int idUser, int grade)
        {
            return MyVotedGrades.Any(p => p.ReceiverID == idUser && p.Grade1 == grade);
        }

        public double ReputationHonestByUser(User user)
        {
            if (user == null)
            {
                return ReputationHonest;
            }
            var reputation = Reputations.FirstOrDefault(p => p.SenderID == user.ID && p.ReputationType == (int)Model.Reputation.ReputationTypeEnum.Honest);
            if (reputation != null)
            {
                return reputation.Mark;
            }
            return 0;
        }

        public double ReputationResponsibilityByUser(User user)
        {
            if (user == null)
            {
                return ReputationResponsibility;
            }
            var reputation = Reputations.FirstOrDefault(p => p.SenderID == user.ID && p.ReputationType == (int)Model.Reputation.ReputationTypeEnum.Responsibility);
            if (reputation != null)
            {
                return reputation.Mark;
            }
            return 0;
        }

        public double ReputationConnectionByUser(User user)
        {
            if (user == null)
            {
                return 0;
            }
            var reputation = Reputations.FirstOrDefault(p => p.SenderID == user.ID && p.ReputationType == (int)Model.Reputation.ReputationTypeEnum.Connection);
            if (reputation != null)
            {
                return reputation.Mark;
            }
            return 0;
        }

        public bool IsTournamentAdmin(int idTournament)
        {
            if (InRoles("admin"))
            {
                return true;
            }

            if (UserRoles.Any(p => p.Role.Code == "tournament_admin"
                && p.UserRoleTournaments.Any(r => r.TournamentID == idTournament)))
            {
                return true;
            }

            if (UserRoles.Any(p => p.Role.Code == "game_admin"
                && p.UserRoleGames.Any(r =>
                    r.Game.Tournaments.Any(t => t.ID == idTournament))))
            {
                return true;
            }
            return false;
        }

        public bool IsGameAdmin(int idGame)
        {
            if (InRoles("admin"))
            {
                return true;
            }
            if (UserRoles.Any(p => p.Role.Code == "game_admin"
                && p.UserRoleGames.Any(r => r.GameID == idGame)))
            {
                return true;
            }
            return false;
        }

        public bool HasLose(int idTournament)
        {
            var allMatches = AllMatches.Any(p =>
                p.TournamentID == idTournament &&
                (p.Tour.TourType == (int)Tour.TourTypeEnum.DoubleEliminationMaster
                || p.Tour.TourType == (int)Tour.TourTypeEnum.DoubleEliminationSecondary)
                && p.Status == (int)Match.MatchStatusEnum.Submit && p.WinnerID != ID);
            return allMatches;
        }

        public bool CanGetPart(Tournament tournament)
        {
            return CanGetPartPlace(tournament) && CanGetPartRating(tournament) && !AlreadyGetPartInTeam(tournament);
        }

        public bool CanGetPartPlace(Tournament tournament)
        {
            if (tournament.MaxPlayersCount.HasValue && tournament.MaxPlayersCount <= tournament.RegisteredPlayersCount)
            {
                return false;
            }
            return true;
        }

        public bool CanGetPartRating(Tournament tournament)
        {
            var rating = Ratings.FirstOrDefault(p => p.GameID == tournament.GameID && p.IsActive);
            if (rating != null)
            {
                if (tournament.MinLevel.HasValue && rating.Level < tournament.MinLevel)
                {
                    return false;
                }
                if (tournament.MaxLevel.HasValue && rating.Level > tournament.MaxLevel)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool CanGetGroupPart(Tournament tournament)
        {
            if (InRoles("group_captain"))
            {
                if (GroupByGame(tournament.GameID) == null)
                {
                    return false;
                }
                if (UserRoles.Any(p => string.Compare(p.Role.Code, "group_captain", true) == 0 && p.UserRoleGroups.Any(r => r.Group.GameID == tournament.GameID)))
                {
                    return CanGetPartPlace(tournament) && CanGetGroupPartRating(tournament) && !AlreadyGetPartInTeam(tournament);
                }
            }
            return false;
        }

        public bool CanGetGroupPartRating(Tournament tournament)
        {
            var group = GroupByGame(tournament.GameID);

            if (group != null)
            {
                if (tournament.MinLevel.HasValue && group.Level < tournament.MinLevel)
                {
                    return false;
                }
                if (tournament.MaxLevel.HasValue && group.Level > tournament.MaxLevel)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public bool AlreadyGetPartInTeam(Tournament tournament)
        {
            return UserTeams.Any(p => p.Team.Participants.Any(r => r.TournamentID == tournament.ID));
        }

        public IQueryable<Message> IncomeMessages
        {
            get
            {
                return Messages.Where(p => !p.IsDeleted && p.MatchID == null && p.GroupID == null && !p.IsSend).AsQueryable();
            }
        }

        public IQueryable<Message> OutcomeMessages
        {
            get
            {
                return Messages1.Where(p => !p.IsDeleted && p.MatchID == null && p.GroupID == null && p.IsSend).AsQueryable();
            }
        }

        public IQueryable<Message> FightMessages
        {
            get
            {
                return Messages.Where(p => !p.IsSend).Union(Messages1.Where(p => p.IsSend)).Where(p => !p.IsDeleted && p.MatchID != null).AsQueryable();
            }
        }

        public IQueryable<Message> InvoiceMessages
        {
            get
            {
                return Messages.Where(p => !p.IsSend).Union(Messages1.Where(p => p.IsSend)).Where(p => !p.IsDeleted && p.GroupID != null).AsQueryable();
            }
        }

        public bool AnyMessages
        {
            get
            {
                return Messages.Where(p => !p.IsDeleted).Union(Messages1.Where(p => !p.IsDeleted)).Any();
            }
        }

        public bool AnyIncomeMessage
        {
            get
            {
                return Messages.Any(p => !p.IsDeleted && p.MatchID == null && p.GroupID == null && !p.IsSend);
            }
        }

        public bool AnyOutcomeMessage
        {
            get
            {
                return Messages1.Any(p => !p.IsDeleted && p.MatchID == null && p.GroupID == null && p.IsSend);
            }
        }

        public bool AnyFightMessage
        {
            get
            {
                return Messages.Any(p => !p.IsDeleted && p.MatchID != null && !p.IsSend);
            }
        }

        public EntitySet<Friendship> ActiveFriendships
        {
            get
            {
                return Friendships1;
            }
        }

        public List<Friendship> FullFriendships
        {
            get
            {
                return Friendships1.Where(p => p.Approved).ToList();
            }
        }

        public bool FullFriendshipsAny
        {
            get
            {
                return Friendships1.Any(p => p.Approved);
            }
        }

        public List<Friendship> FullFriendshipsPage(int page = 1, int itemPerPage = 10)
        {
            return Friendships1.Where(p => p.Approved).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
        }

        public int FullFriendshipsCount(int itemPerPage = 10)
        {
            return Friendships1.Count(p => p.Approved) / itemPerPage + (Friendships1.Count(p => p.Approved) % itemPerPage != 0 ? 1 : 0);
        }

        public EntitySet<Friendship> PassiveFriendships
        {
            get
            {
                return Friendships;
            }
        }

        public bool PassiveFriendshipsAny
        {
            get
            {
                return Friendships.Any(p => !p.Approved);
            }
        }

        public List<Friendship> PassiveFriendshipsPage(int page = 1, int itemPerPage = 10)
        {
            return Friendships.Where(p => !p.Approved).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
        }

        public int PassiveFriendshipsCount(int itemPerPage = 10)
        {
            return Friendships.Count(p => !p.Approved) / itemPerPage + (Friendships.Count(p => !p.Approved) % itemPerPage != 0 ? 1 : 0);
        }

        public bool HasFriend(int id)
        {
            return FullFriendships.Any(p => p.ReceiverID == id);
        }

        public bool AskForFriend(int id)
        {
            return Friendships1.Any(p => p.ReceiverID == id);
        }

        public bool MeAskedForFriendship(int id)
        {
            return Friendships.Any(p => p.SenderID == id);
        }

        public bool AskedForNewFriend
        {
            get
            {
                return Friendships.Any(p => !p.Approved);
            }
        }

        public IEnumerable<Award> LastAwards
        {
            get
            {
                return UserAwards.OrderByDescending(p => p.AddedDate).Select(p => p.Award).Take(3);
            }
        }

        public IQueryable<Match> SingleMatches
        {
            get
            {
                return AllMatches.Where(p => p.TournamentID == null).AsQueryable();
            }
        }

        public IEnumerable<Match> CurrentMatches
        {
            get
            {
                return SingleMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.DefinedPlayers);
            }
        }

        public IEnumerable<Match> OutcomingMatches
        {
            get
            {
                return SingleMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.Created && p.Message != null && p.Message.SenderID == ID);
            }
        }

        public IEnumerable<Match> IncomingMatches
        {
            get
            {
                return SingleMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.Created && p.Message != null && p.Message.ReceiverID == ID);
            }
        }

        public IEnumerable<Match> PlayedOutMatches
        {
            get
            {
                return SingleMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.Submit);
            }
        }

        public IQueryable<Match> TournamentMatches
        {
            get
            {
                return AllMatches.Where(p => p.TournamentID != null).AsQueryable();
            }
        }

        public IEnumerable<Match> CurrentTournamentMatches
        {
            get
            {
                return TournamentMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.DefinedPlayers);
            }
        }

        public IEnumerable<Match> PlayedTournamentMatches
        {
            get
            {
                return TournamentMatches.Where(p => p.Status == (int)Match.MatchStatusEnum.Submit && p.Tournament.Status != (int)Tournament.StatusEnum.Closed);
            }
        }

        public bool HasUnreadMail(int type)
        {
            switch (type)
            {
                case (int)Message.MessageType.All:
                    return Messages.Any(p => !p.IsSend && !p.IsReaded && !p.IsDeleted);
                case (int)Message.MessageType.Simple:
                    return Messages.Any(p => p.MatchID == null && p.GroupID == null && !p.IsSend && !p.IsReaded && !p.IsDeleted);
                case (int)Message.MessageType.Fight:
                    return Messages.Any(p => p.MatchID != null && !p.IsSend && !p.IsReaded && !p.IsDeleted);
            }
            return false;
        }

        public IQueryable<Tournament> AdminTournaments
        {
            get
            {
                var list = UserRoles.Where(p => p.Role.Code == "tournament_admin").SelectMany(p => p.UserRoleTournaments.Select(r => r.Tournament)).AsQueryable();
                list = list.Union(
                    UserRoles.Where(p => p.Role.Code == "game_admin")
                        .SelectMany(p => p.UserRoleGames
                            .SelectMany(r => r.Game.Tournaments)).AsQueryable()
                    );
                return list;
            }
        }

        public IQueryable<Tournament> ModeratorTournaments
        {
            get
            {
                var list = UserRoles.Where(p => p.Role.Code == "tournament_moderator").SelectMany(p => p.UserRoleTournaments.Select(r => r.Tournament)).AsQueryable();
                list = list.Union(
                    UserRoles.Where(p => p.Role.Code == "game_moderator")
                        .SelectMany(p => p.UserRoleGames
                            .SelectMany(r => r.Game.Tournaments)).AsQueryable()
                    );
                return list;
            }
        }

        public bool HasAdminTournament
        {
            get
            {
                return AdminTournaments.Any(p => p.Status != (int)Tournament.StatusEnum.Closed);
            }
        }

        public IQueryable<Game> AdminGames
        {
            get
            {
                return UserRoles.Where(p => p.Role.Code == "game_admin").SelectMany(p => p.UserRoleGames.Select(r => r.Game)).AsQueryable();
            }
        }

        public IQueryable<Game> ModeratorGames
        {
            get
            {
                return UserRoles.Where(p => p.Role.Code == "game_moderator").SelectMany(p => p.UserRoleGames.Select(r => r.Game)).AsQueryable();
            }
        }

        public IEnumerable<UserRole> SubUserRoles
        {
            get
            {
                return UserRoles.AsEnumerable();
            }
        }

        /// <summary>
        /// Играю в эти игры
        /// </summary>
        public IEnumerable<Game> Games
        {
            get
            {
                return UserGames.Select(p => p.Game).AsEnumerable();
            }
        }

        public IList<string> GameNames
        {
            get
            {
                return UserGames.Select(p => p.Game.Name).ToList();
            }
        }

        public string StatusColor
        {
            get
            {
                if (InRoles("admin"))
                {
                    return "superadmin";
                }
                if (InRoles("game_admin,tournament_admin"))
                {
                    return "admin";
                }
                if (InRoles("tournament_moderator,game_moderator"))
                {
                    return "moderator";
                }
                return "";
            }

        }

        public bool InGroup(int id)
        {
            return UserGroups.Any(p => p.GroupID == id && p.Status == (int)UserGroup.StatusEnum.Granded);
        }

        public bool InPotentialGroup(int id)
        {
            return UserGroups.Any(p => p.GroupID == id && p.Status == (int)UserGroup.StatusEnum.Asked);
        }

        public bool CanBeMemberOfGroup(Group group)
        {
            if (group == null)
            {
                return false;
            }
            return !UserGroups.Any(p => p.Group.GameID == group.GameID);
        }

        public bool IsRoleOfGroup(Group group, string role)
        {
            if (group == null)
            {
                return false;
            }
            return UserRoles.Any(p => p.UserRoleGroups.Any(r => r.GroupID == group.ID) && string.Compare(p.Role.Code, role, true) == 0);
        }

        public bool IsLeaderOfGroup(Group group)
        {
            return IsRoleOfGroup(group, "group_leader") || group.UserID == ID;
        }

        public IQueryable<Group> GroupsWhereImMember
        {
            get
            {
                return Groups.Union(UserGroups.Select(p => p.Group)).Distinct().AsQueryable();
            }
        }

        public IQueryable<Group> GroupsWhereImLeader
        {
            get
            {
                return Groups.Union(UserGroups.Select(p => p.Group))
                    .Where(p =>
                        p.UserRoleGroups.Any(r =>
                            r.UserRole.UserID == ID &&
                            string.Compare(r.UserRole.Role.Code, "group_leader", true) == 0)).AsQueryable();
            }
        }

        public Group GroupByGame(int gameId)
        {
            var userGroup = UserGroups.FirstOrDefault(p => p.Group.GameID == gameId);
            if (userGroup != null)
            {
                return userGroup.Group;
            }
            return null;
        }

        /// <summary>
        /// тот кто ищет в команду игроков (является лидером в команде по этой игре)
        /// </summary>
        public bool IsScout(int gameId)
        {
            return GroupsWhereImMember.Any(p => p.GameID == gameId);
        }

        public IQueryable<Group> GroupsForInvoiceFor(int potentialUserID)
        {
            return GroupsWhereImLeader.Where(p =>
                        !p.UserGroups.Any(r => r.UserID == potentialUserID)
                      && p.UserGroups.Any(r => r.Group.Game.UserGames.Any(g => g.UserID == potentialUserID)));
        }

        public IQueryable<Group> AdminGroups
        {
            get
            {
                if (InRoles("game_admin"))
                {
                    return AdminGames.SelectMany(p => p.Groups);
                }
                return null;
            }
        }

        public bool IsParticipantInTournament(int idTournament)
        {
            return Participants.Any(p => p.TournamentID == idTournament) ||
                 UserTeams.Any(p => p.Accepted && p.Team.Participants.Any(r => r.TournamentID == idTournament));
        }

        public bool HasAnyGroup
        {
            get
            {
                return UserGroups.Any(p => p.Status == (int)UserGroup.StatusEnum.Granded);
            }
        }

        public bool HasAnyGame
        {
            get
            {
                return UserGames.Any();
            }
        }

        public int NewsCount
        {
            get
            {
                return News.Count;
            }
        }

        public bool AnyNotice
        {
            get
            {
                return Notices.Any(p => (p.Type != (int)Notice.TypeEnum.Forum && p.Type != (int)Notice.TypeEnum.Chat) || p.ReadedDate == null);
            }
        }

        public bool HasAdvansedSocial(Social.ProviderType type)
        {
            return Socials.Any(p => p.Provider == (int)type && p.IsAdvansed);
        }

        public Social SocialByType(Social.ProviderType type)
        {
            return Socials.First(p => p.Provider == (int)type && p.IsAdvansed);
        }


        public int? PlaceOfSeason(LeagueSeason season)
        {
            var participant = Participants
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
            var participant = Participants
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
            var participant = Participants
               .SelectMany(p => p.LeagueParticipants)
               .FirstOrDefault(r => r.LeagueSeasonID == season.ID);
            if (participant != null)
            {
                return participant.LeagueLevelID;
            }
            return null;
        }

        public Tournament TournamentOfSeason(LeagueSeason season)
        {
            var participant = Participants
               .SelectMany(p => p.LeagueParticipants)
               .FirstOrDefault(r => r.LeagueSeasonID == season.ID);
            if (participant != null)
            {
                return participant.Participant.Tournament;
            }
            return null;
        }

        public bool GetLeagueLevelPart(LeagueLevel level, LeagueSeason season)
        {
            var participants = level.LeagueParticipants.Where(p => p.LeagueSeasonID == season.ID).Select(p => p.Participant);
            return participants.Any(p => p.UserID == ID) || participants.Where(p => p.TeamID != null).Select(p => p.Team).Any(p => p.UserTeams.Any(r => r.UserID == ID));
        }

        public bool IsPartOf(Participant participant)
        {
            
            return participant.UserID == ID || 
                (participant.Team != null ? 
                    participant.Team.UserTeams.Any(p => p.UserID == ID) : 
                    false);
        }
    }
}