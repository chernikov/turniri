using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Model
{
    public interface IRepository
    {
        IQueryable<T> GetTable<T>() where T : class;

        #region Role

        IQueryable<Role> Roles { get; }

        bool CreateRole(Role instance);

        bool RemoveRole(int idRole);

        #endregion

        #region UserRole

        IQueryable<UserRole> UserRoles { get; }

        bool CreateUserRole(UserRole instance);

        bool RemoveUserRole(int idUserRole);

        #endregion

        #region Award
        
        IQueryable<Award> Awards { get; }
        
        bool CreateAward(Award instance);
        
        bool UpdateAward(Award instance);
        
        bool RemoveAward(int idAward);
        
        #endregion 

        #region Blog
        
        IQueryable<Blog> Blogs { get; }
        
        bool CreateBlog(Blog instance);
        
        bool UpdateBlog(Blog instance);

        bool UpdateAdminBlog(Blog instance);
        
        bool RemoveBlog(int idBlog);

        bool UpdateVisitBlog(int idBlog);

        int ToggleBlogLike(int idBlog, int idUser);
        
        #endregion 

        #region BlogComment
        
        IQueryable<BlogComment> BlogComments { get; }
        
        bool CreateBlogComment(BlogComment instance);
        
        bool UpdateBlogComment(BlogComment instance);
        
        bool RemoveBlogComment(int idBlogComment);
        
        #endregion 

        #region Comment
        
        IQueryable<Comment> Comments { get; }
        
        bool CreateComment(Comment instance);
        
        bool UpdateComment(Comment instance);
        
        bool RemoveComment(int idComment);
        
        #endregion 

        #region Forum

        IQueryable<Forum> Forums { get; }

        bool CreateForum(Forum instance);

        bool UpdateForum(Forum instance);

        bool RemoveForum(int idForum);

        bool ChangeParentForum(int id, int idParent);

        bool MoveForum(int id, int placeBefore);

        bool SetOrderForum(Forum instance);

        bool UpdateVisitForum(int idForum);

        bool UpdateVisitForumNotice(int idForum, int idUser);

        #endregion 

        #region ForumMessage
        
        IQueryable<ForumMessage> ForumMessages { get; }
        
        bool CreateForumMessage(ForumMessage instance);
        
        bool UpdateForumMessage(ForumMessage instance);

        bool ModerateForumMessage(ForumMessage instance);


        bool RemoveForumMessage(int idForumMessage, int idUser);
        
        #endregion 

        #region Friendship
        
        IQueryable<Friendship> Friendships { get; }
        
        bool CreateFriendship(Friendship instance);

        bool ConfirmFriendship(int idFriendship);
        
        bool DeclineFriendship(int idFriendship);

        bool RemoveFriend(int idUser, int idFriend);
        
        #endregion 

        #region Game
        
        IQueryable<Game> Games { get; }
        
        bool CreateGame(Game instance);
        
        bool UpdateGame(Game instance);
        
        bool RemoveGame(int idGame);

        bool UpdateGameAdmins(int id, List<int> admins);

        bool UpdateGameModerators(int id, List<int> moderators);

        #endregion 

        #region Grade

        bool SetGrade(int senderID, int receiverID, int grade);
        
        #endregion 

        #region Match
        
        IQueryable<Match> Matches { get; }
        
        bool CreateMatch(Match instance);
        
        bool UpdateMatch(Match instance);

        bool UpdateStatusMatch(Match instance);

        bool TechSubmitMatch(Match instance);
        
        bool RemoveMatch(int idMatch);

        bool PlayMatch(int idMatch);

        bool EmptyMatch(int idMatch);

        bool SingleParticipantMatch(int idMatch);

        bool RollbackMatch(int idMatch);

        bool DropSingleMatchWithMessages(int idMatch);

        bool TechAllLoseMatch(int idMatch);

        bool CreateOffMatch(LeagueSeason leagueSeason, LeagueLevel mainLevel, int mainParticipantID, int nextParticipantID);

        void CreateOffMatchNoticeSetPlayers(Match match);
        
        #endregion 

        #region Message
        
        IQueryable<Message> Messages { get; }
        
        bool CreateMessage(Message instance);

        bool RemoveMessage(int idMessage);

        bool ReadSubject(int idSubject, int idUser);

        #endregion 

        #region New
        
        IQueryable<New> News { get; }
        
        bool CreateNew(New instance);
        
        bool UpdateNew(New instance);
        
        bool UpdateVisitNew(int idNew);

        bool RemoveNew(int idNew);

        int ToggleNewLike(int idNew, int idUser);
        
        #endregion 

        #region NewComment
        
        IQueryable<NewComment> NewComments { get; }
        
        bool CreateNewComment(NewComment instance);
        
        bool UpdateNewComment(NewComment instance);
        
        bool RemoveNewComment(int idNewComment);
        
        #endregion 

        #region NewType
        
        IQueryable<NewType> NewTypes { get; }
        
        bool CreateNewType(NewType instance);
        
        bool UpdateNewType(NewType instance);
        
        bool RemoveNewType(int idNewType);
        
        #endregion 

        #region Photo
        
        IQueryable<Photo> Photos { get; }
        
        bool CreatePhoto(Photo instance);
        
        bool BindPhoto(Photo instance);

        bool RemovePhoto(int idPhoto);

        Photo ChangePhoto(int idCurrentPhoto, bool next);

        int TogglePhotoLike(int idPhoto, int idUser);

        bool UpdateVisitPhoto(int idPhoto);
        
        #endregion 

        #region PhotoAlbum
        
        IQueryable<PhotoAlbum> PhotoAlbums { get; }
        
        bool CreatePhotoAlbum(PhotoAlbum instance);
        
        bool UpdatePhotoAlbum(PhotoAlbum instance);
        
        bool RemovePhotoAlbum(int idPhotoAlbum);
        
        #endregion 

        #region PhotoComment
        
        IQueryable<PhotoComment> PhotoComments { get; }
        
        bool CreatePhotoComment(PhotoComment instance);
        
        bool UpdatePhotoComment(PhotoComment instance);
        
        bool RemovePhotoComment(int idPhotoComment);
        
        #endregion 

        #region Platform
        
        IQueryable<Platform> Platforms { get; }
        
        bool CreatePlatform(Platform instance);
        
        bool UpdatePlatform(Platform instance);
        
        bool RemovePlatform(int idPlatform);
        
        #endregion 

        #region Rating
        
        IQueryable<Rating> Ratings { get; }
        
        #endregion 

        #region RatingDetail
        
        IQueryable<RatingDetail> RatingDetails { get; }
        
        void UpdateRatingDetailTournament(int idRatingDetail);

        #endregion 

        #region Reputation
        

        bool SetReputation(int senderID, int receiverID, int type, int mark);
        
        #endregion 

        #region Round
        
        IQueryable<Round> Rounds { get; }
        
        bool CreateRound(Round instance);

        bool UpdateRound(Round instance);

        bool PublishRound(Round instance);

        bool DisputeRound(Round instance);

        bool SubmitRound(Round instance);

        bool TechSubmitRound(Round instance);

        bool RemoveRound(int idRound);

        bool RollbackRound(int idRound);

        #endregion 

        #region Tournament
        
        IQueryable<Tournament> Tournaments { get; }

        IQueryable<Tournament> OpenTournaments { get; }
        
        bool CreateTournament(Tournament instance);
        
        bool UpdateTournament(Tournament instance);
        
        bool RemoveTournament(int idTournament);

        bool StartTournament(int idTournament);

        bool CreateMatches(int idTournament);

        bool ClearMatches(int idTournament);

        bool FinishTournament(int idTournament);

        bool TechFinishTournament(int idTournament);

        bool UpdateTournamentAdmins(int id, List<int> admins);

        bool UpdateTournamentModerators(int id, List<int> moderators);

        bool ChangeMatchParticipant(int fromMatchId, bool fromMatchParticipant1, int toMatchId, bool toMatchParticipant1);

        bool SubstituteParticipant(int matchId, bool participant1, int participationID);

        bool AllocatePlayoff(int idTournament);

        bool ResetAwardsForTournament(int idTournament);

        void CloseAllCommentsInTournament(int idTournament, string filesBaseFolder);

        bool UndoFinishTournament(int idTournament);

        void ReAwardByPlaces(int idTournament);

        #endregion 

        #region Participant

        IQueryable<Participant> Participants { get; }

        bool CreateParticipant(Participant instance); 

        bool RegisterParticipant(int userID, int tournamentID);

        bool ChangeParticipant(int idParticipant, int idUser);

        bool RegisterParticipant(int userID, int tournamentID, int teamID);

        bool SetTeamInParticipant(int userID, int tournamentID, int teamID); 

        bool SaveParticipant(Participant instance); 

        bool ChangeGroupParticipants(int idParticipant1, int idParticipant2);

        bool UpdateParticipants(int idTournament, List<int> users);

        bool UpdateLeagueParticipants(int idTournament, List<int> leagueParticipants);

        bool RemoveParticipant(int idParticipant);
        
        #endregion 

        #region User
        
        IQueryable<User> Users { get; }

        IQueryable<User> NonActivatedUsers { get; }

        IQueryable<User> RegularUsers { get; }

        IQueryable<User> OnlineUsers { get; }

        bool CreateUser(User instance);
        
        bool UpdateUser(User instance);
        
        bool RemoveUser(int idUser);

        User GetUser(string login);

        User Login(string login, string password);

        bool ActivateUser(User instance);

        bool VerifiedEmailUser(User instance);

        bool ChangePassword(User instance);

        bool BanUser(User instance, bool ban);

        bool UserVisitCount(int id);

        void AwardUser(int idAward, int idUser);

        void VisitUser(int id, double Amount);

        #endregion 

        #region UserComment
        
        IQueryable<UserComment> UserComments { get; }
        
        bool CreateUserComment(UserComment instance);
        
        bool UpdateUserComment(UserComment instance);
        
        bool RemoveUserComment(int idUserComment);
        
        #endregion 

        #region UserVideo
        
        IQueryable<UserVideo> UserVideos { get; }
        
        bool CreateUserVideo(UserVideo instance);
        
        bool UpdateUserVideo(UserVideo instance);
        
        bool RemoveUserVideo(int idUserVideo);

        bool UpdateVisitUserVideo(int idUserVideo);
        
        #endregion 

        #region UserVideoComment
        
        IQueryable<UserVideoComment> UserVideoComments { get; }
        
        bool CreateUserVideoComment(UserVideoComment instance);
        
        bool UpdateUserVideoComment(UserVideoComment instance);
        
        bool RemoveUserVideoComment(int idUserVideoComment);
        
        #endregion 

        #region Video
        
        IQueryable<Video> Videos { get; }
        
        bool CreateVideo(Video instance);
        
        bool UpdateVideo(Video instance);
        
        bool RemoveVideo(int idVideo);

        bool UpdateVisitVideo(int idVideo);
        
        #endregion 

        #region VideoComment
        
        IQueryable<VideoComment> VideoComments { get; }
        
        bool CreateVideoComment(VideoComment instance);
        
        bool UpdateVideoComment(VideoComment instance);
        
        bool RemoveVideoComment(int idVideoComment);
        
        #endregion 

        #region UserGame
        
        IQueryable<UserGame> UserGames { get; }
        
        bool CreateUserGame(UserGame instance);
        
        bool RemoveUserGame(int idUserGame);
        
        #endregion 

        #region TournamentGroup
        
        IQueryable<TournamentGroup> TournamentGroups { get; }
        
        bool CreateTournamentGroup(TournamentGroup instance);
        
        bool UpdateTournamentGroup(TournamentGroup instance);
        
        bool RemoveTournamentGroup(int idTournamentGroup);
        
        #endregion 

        #region Tour
        
        IQueryable<Tour> Tours { get; }
        
        bool CreateTour(Tour instance);
        
        bool UpdateTour(Tour instance);
        
        bool RemoveTour(int idTour);
        
        #endregion 

        #region MatchComment
        
        IQueryable<MatchComment> MatchComments { get; }
        
        bool CreateMatchComment(MatchComment instance);
        
        bool UpdateMatchComment(MatchComment instance);
        
        bool RemoveMatchComment(int idMatchComment);
        
        #endregion 

        #region Subject
        
        IQueryable<Subject> Subjects { get; }
        
        bool CreateSubject(Subject instance);
        
        bool UpdateSubject(Subject instance);
        
        bool RemoveSubject(int idSubject);
        
        #endregion 

        #region Page
        
        IQueryable<Page> Pages { get; }
        
        bool CreatePage(Page instance);
        
        bool UpdatePage(Page instance);
        
        bool RemovePage(int idPage);
        
        #endregion 

        #region ForumLog
        
        IQueryable<ForumLog> ForumLogs { get; }
        
        bool CreateForumLog(ForumLog instance);
        
        #endregion 

        #region Poll
        
        IQueryable<Poll> Polls { get; }
        
        bool CreatePoll(Poll instance);
        
        bool UpdatePoll(Poll instance);
        
        bool RemovePoll(int idPoll);
        
        #endregion 

        #region PollVote
        
        IQueryable<PollVote> PollVotes { get; }
        
        bool CreatePollVote(PollVote instance);
        
        #endregion 

        #region PollItem
        
        IQueryable<PollItem> PollItems { get; }
        
        bool CreatePollItem(PollItem instance);
        
        bool UpdatePollItem(PollItem instance);
        
        bool RemovePollItem(int idPollItem);
        
        #endregion 

        #region Distribution
        
        IQueryable<Distribution> Distributions { get; }
        
        bool CreateDistribution(Distribution instance);
        
        bool UpdateDistribution(Distribution instance);
        
        bool RemoveDistribution(int idDistribution);

        bool StartDistribution(int idDistribution);

        bool ClearDistribution(int idDistribution);
        
        #endregion 

        #region Mail

        IQueryable<Mail> Mails { get; }

        bool SaveMail(Mail instance);

        bool PushMail(Mail instance);

        Mail PopMail();

        Mail PopMail(Guid id);

        void ClearMailBody(Guid id);

        bool RemoveMail(Guid id);

        #endregion 

        #region UserRoleGame
        
        IQueryable<UserRoleGame> UserRoleGames { get; }
        
        bool CreateUserRoleGame(UserRoleGame instance);
        
        bool UpdateUserRoleGame(UserRoleGame instance);
        
        bool RemoveUserRoleGame(int idUserRoleGame);
        
        #endregion 

        #region UserRoleTournament
        
        IQueryable<UserRoleTournament> UserRoleTournaments { get; }
        
        bool CreateUserRoleTournament(UserRoleTournament instance);
        
        bool UpdateUserRoleTournament(UserRoleTournament instance);
        
        bool RemoveUserRoleTournament(int idUserRoleTournament);
        
        #endregion 

        #region ChatRoom
        
        IQueryable<ChatRoom> ChatRooms { get; }
        
        bool CreateChatRoom(ChatRoom instance);
        
        bool UpdateChatRoom(ChatRoom instance);
        
        bool RemoveChatRoom(int idChatRoom);

        bool UpdateVisitChatRoomNotice(int idChatRoom, int idUser);

        bool ToggleTranslateChatRoom(int idChatRoom);
        
        #endregion 

        #region ChatMessage
        
        IQueryable<ChatMessage> ChatMessages { get; }
        
        bool CreateChatMessage(ChatMessage instance);
        
        bool RemoveChatMessage(int idChatMessage);
        
        #endregion 

        #region ChatBannedUser
        
        IQueryable<ChatBannedUser> ChatBannedUsers { get; }
        
        bool CreateChatBannedUser(ChatBannedUser instance);
        
        bool RemoveChatBannedUser(int idChatBannedUser);
        
        #endregion 

        #region TournamentCondition
        
        IQueryable<TournamentCondition> TournamentConditions { get; }
        
        bool CreateTournamentCondition(TournamentCondition instance);
        
        bool UpdateTournamentCondition(TournamentCondition instance);
        
        bool RemoveTournamentCondition(int idTournamentCondition);
        
        #endregion 

        #region GlobalUnique
        
        IQueryable<GlobalUnique> GlobalUniques { get; }

        bool SaveGlobalUnique(GlobalUnique instance);
        
        #endregion 

        #region Team
        
        IQueryable<Team> Teams { get; }
        
        bool CreateTeam(Team instance, int? hotReplacement);
        
        bool UpdateTeam(Team instance);
        
        bool RemoveTeam(int idTeam);

        bool CloseTeam(int idTeam);
        
        #endregion 

        #region UserTeam
        
        IQueryable<UserTeam> UserTeams { get; }
        
        bool CreateUserTeam(UserTeam instance);
        
        bool UpdateUserTeam(UserTeam instance);
        
        bool RemoveUserTeam(int idUserTeam);

        bool ReplaceUser(int idUserTeam, int idUser);
        
        #endregion 

        #region UserAward
        
        IQueryable<UserAward> UserAwards { get; }
        
        bool CreateUserAward(UserAward instance);
        
        bool UpdateUserAward(UserAward instance);
        
        bool RemoveUserAward(int idUserAward);
        
        #endregion 

        #region Camera
        
        IQueryable<Camera> Cameras { get; }
        
        bool CreateCamera(Camera instance);
        
        bool UpdateCamera(Camera instance);
        
        bool RemoveCamera(int idCamera);
        
        #endregion 

        #region Group
        
        IQueryable<Group> Groups { get; }
        
        bool CreateGroup(Group instance);
        
        bool UpdateGroup(Group instance);
        
        bool RemoveGroup(int idGroup);

        bool AcceptGroup(int idGroup);

        bool ChangeGroupLeader(int idGroup, int idUser);

        bool VisitGroup(int idGroup);

        bool UpdateGroupMembers(int idGroup, List<int> members);

        bool PurgeGroup(int idGroup);

        #endregion 

        #region UserGroup
        
        IQueryable<UserGroup> UserGroups { get; }
        
        bool CreateUserGroup(UserGroup instance);
        
        bool RemoveUserGroup(int idUserGroup);

        bool AcceptUserGroup(int idUserGroup);

       
        
        #endregion 

        #region UserRoleGroup
        
        IQueryable<UserRoleGroup> UserRoleGroups { get; }
        
        bool CreateUserRoleGroup(UserRoleGroup instance);
        
        bool RemoveUserRoleGroup(int idUserRoleGroup);

        bool SwitchGroupRole(int idUser, int idGroup, int idRole);
        
        #endregion 

        #region TeamReplacement
        
        IQueryable<TeamReplacement> TeamReplacements { get; }
        
        #endregion 

        #region MatchRoaster
        
        IQueryable<MatchRoaster> MatchRoasters { get; }
        
        bool CreateMatchRoaster(MatchRoaster instance);
        
        bool UpdateMatchRoaster(MatchRoaster instance);
        
        bool RemoveMatchRoaster(int idMatchRoaster);
        
        #endregion 

        #region GroupRating
        
        IQueryable<GroupRating> GroupRatings { get; }
        
        bool CreateGroupRating(GroupRating instance);
        
        bool UpdateGroupRating(GroupRating instance);
        
        bool RemoveGroupRating(int idGroupRating);
        
        #endregion 

        #region Background
        
        IQueryable<Background> Backgrounds { get; }
        
        bool CreateBackground(Background instance);
        
        bool UpdateBackground(Background instance);
        
        bool RemoveBackground(int idBackground);
        
        #endregion 

        #region Banner
        
        IQueryable<Banner> Banners { get; }
        
        bool CreateBanner(Banner instance);
        
        bool UpdateBanner(Banner instance);

        bool RemoveBanner(int idBanner);

        bool ClickBanner(int idBanner);

        bool ViewBanner(int idBanner);

        #endregion 

        #region Notice
        
        IQueryable<Notice> Notices { get; }
        
        bool CreateNotice(Notice instance);

        bool ReadNotice(int idNotice);
        
        bool RemoveNotice(int idNotice);
        
        #endregion 

        #region NoticeAction
        
        IQueryable<NoticeAction> NoticeActions { get; }
        
        bool CreateNoticeAction(NoticeAction instance);
        
        bool RemoveNoticeAction(int idNoticeAction);
        
        #endregion 

        #region NoticeDistribution
        
        IQueryable<NoticeDistribution> NoticeDistributions { get; }
        
        bool CreateNoticeDistribution(NoticeDistribution instance);
        
        bool UpdateNoticeDistribution(NoticeDistribution instance);
        
        bool RemoveNoticeDistribution(int idNoticeDistribution);

        bool ClearNoticeDistribution (int idNoticeDistribution);
        #endregion 

        #region ChatRoomLog
        
        IQueryable<ChatRoomLog> ChatRoomLogs { get; }
        
        bool CreateChatRoomLog(ChatRoomLog instance);
        
        #endregion 

        #region SocialPost
        
        IQueryable<SocialPost> SocialPosts { get; }
        
        bool CreateSocialPost(SocialPost instance);
        
        bool UpdateSocialPost(SocialPost instance);
        
        bool RemoveSocialPost(int idSocialPost);
        
        #endregion 

        #region Social
        
        IQueryable<Social> Socials { get; }
        
        bool CreateSocial(Social instance);
        
        bool UpdateSocial(Social instance);

        bool UpdateTranslateSocial(Social instance);
        
        bool RemoveSocial(int idSocial);

        #endregion 

        #region BlogSocialPost
        
        IQueryable<BlogSocialPost> BlogSocialPosts { get; }
        
        bool CreateBlogSocialPost(BlogSocialPost instance);
        
        bool UpdateBlogSocialPost(BlogSocialPost instance);
        
        bool RemoveBlogSocialPost(int idBlogSocialPost);
        
        #endregion 

        #region NewSocialPost
        
        IQueryable<NewSocialPost> NewSocialPosts { get; }
        
        bool CreateNewSocialPost(NewSocialPost instance);
        
        bool UpdateNewSocialPost(NewSocialPost instance);
        
        bool RemoveNewSocialPost(int idNewSocialPost);
        
        #endregion 

        #region PhotoAlbumSocialPost
        
        IQueryable<PhotoAlbumSocialPost> PhotoAlbumSocialPosts { get; }
        
        bool CreatePhotoAlbumSocialPost(PhotoAlbumSocialPost instance);
        
        bool UpdatePhotoAlbumSocialPost(PhotoAlbumSocialPost instance);
        
        bool RemovePhotoAlbumSocialPost(int idPhotoAlbumSocialPost);
        
        #endregion 

        #region TournamentSocialPost
        
        IQueryable<TournamentSocialPost> TournamentSocialPosts { get; }
        
        bool CreateTournamentSocialPost(TournamentSocialPost instance);
        
        bool UpdateTournamentSocialPost(TournamentSocialPost instance);
        
        bool RemoveTournamentSocialPost(int idTournamentSocialPost);
        
        #endregion 

        #region SocialPostImage
        
        IQueryable<SocialPostImage> SocialPostImages { get; }
        
        bool CreateSocialPostImage(SocialPostImage instance);
        
        bool UpdateSocialPostImage(SocialPostImage instance);
        
        bool RemoveSocialPostImage(int idSocialPostImage);
        
        #endregion 

        #region SocialGroup
        
        IQueryable<SocialGroup> SocialGroups { get; }
        
        bool CreateSocialGroup(SocialGroup instance);
        
        bool UpdateSocialGroup(SocialGroup instance);
        
        bool RemoveSocialGroup(int idSocialGroup);
        
        #endregion 

        #region VideoSocialPost
        
        IQueryable<VideoSocialPost> VideoSocialPosts { get; }
        
        bool CreateVideoSocialPost(VideoSocialPost instance);
        
        bool UpdateVideoSocialPost(VideoSocialPost instance);
        
        bool RemoveVideoSocialPost(int idVideoSocialPost);
        
        #endregion 

        #region MainCamera
        
        IQueryable<MainCamera> MainCameras { get; }
        
        bool CreateMainCamera(MainCamera instance);
        
        bool UpdateMainCamera(MainCamera instance);
        
        bool RemoveMainCamera(int idMainCamera);
        
        #endregion 

        #region MoneyDetail
        
        IQueryable<MoneyDetail> MoneyDetails { get; }

        Guid CreateTripleMoneyDetail(MoneyDetail from, MoneyDetail to, MoneyDetail fee = null);

        bool CreateMoneyDetail(MoneyDetail instance, Guid uniqueGuid, int? moneyFeeID = null);

        bool SubmitMoney(Guid moneyGuid);

        bool DiscardMoney(Guid moneyGuid);
        #endregion 

        #region MoneyFee
        
        IQueryable<MoneyFee> MoneyFees { get; }
        
        bool CreateMoneyFee(MoneyFee instance);
        
        bool UpdateMoneyFee(MoneyFee instance);
        
        bool RemoveMoneyFee(int idMoneyFee);
        
        #endregion 

        #region BalanceSiteMoney
        
        BalanceSiteMoney BalanceSiteMoney { get; }

        BalanceSiteMoney BalanceSiteRecharge { get; }

        void RecalculateAll();

        void RemoveUnsubmitted();

        void RemoveMoneyTransaction(Guid guid);
        #endregion 

        #region Recharge
        
        IQueryable<Recharge> Recharges { get; }
        
        bool CreateRecharge(Recharge instance);
        
        bool UpdateRecharge(Recharge instance);

        bool RemoveRecharge(int id);

        bool RemoveUnSubmittedRecharge();
        
        #endregion 

        #region MoneyWithdraw
        
        IQueryable<MoneyWithdraw> MoneyWithdraws { get; }
        
        bool CreateMoneyWithdraw(MoneyWithdraw instance);

        bool RemoveMoneyWithdraw(int idMoneyWithdraw);

        bool SubmitMoneyWithdraw(int idMoneyWithdraw);
        #endregion 

        #region BannedWord
        
        IQueryable<BannedWord> BannedWords { get; }
        
        bool CreateBannedWord(BannedWord instance);
        
        bool UpdateBannedWord(BannedWord instance);
        
        bool RemoveBannedWord(int idBannedWord);
        
        #endregion 

        #region Catalog
        
        IQueryable<Catalog> Catalogs { get; }
        
        bool CreateCatalog(Catalog instance);
        
        bool UpdateCatalog(Catalog instance);
        
        bool RemoveCatalog(int idCatalog);

        #endregion 

        #region Cart
        
        IQueryable<Cart> Carts { get; }
        
        bool CreateCart(Cart instance);
        
        bool UpdateCart(Cart instance);

        bool UpdateLastVisitDateCart(Cart instance);
        
        bool RemoveCart(int idCart);

        bool ClearCart(int idCart);
        
        #endregion 

        #region Product
        
        IQueryable<Product> Products { get; }
        
        bool CreateProduct(Product instance);
        
        bool UpdateProduct(Product instance);
        
        bool RemoveProduct(int idProduct);

        bool RestoreProduct(int idProduct);

        bool UpdateVisitCountProduct(int idProduct);

        bool ClearProductPrices(int idProduct, List<int> exist);

        bool ClearProductImages(int idProduct, List<int> exist);

        bool ClearProductVariations(int idProduct, List<int> exist);

        bool ClearProductVideos(int idProduct, List<int> exist);

        bool UpdateProductCatalogs(int idProduct, List<int> exist);

        

        #endregion 

        #region ProductCode
        
        IQueryable<ProductCode> ProductCodes { get; }
        
        bool CreateProductCode(ProductCode instance);
        
        bool UpdateProductCode(ProductCode instance);
        
        bool RemoveProductCode(int idProductCode);

        bool ReserveProductCode(ProductCode instance, CartProduct cartProduct);

        bool UnReserveProductCode(ProductCode instance);

        bool SellProductCode(ProductCode instance);

        #endregion 

        #region CartProduct
        
        IQueryable<CartProduct> CartProducts { get; }
        
        bool CreateCartProduct(CartProduct instance);

        bool UpdateCartProductQuantity(CartProduct instance);
        
        bool RemoveCartProduct(int idCartProduct);
        
        #endregion 

        #region PromoAction
        
        IQueryable<PromoAction> PromoActions { get; }
        
        bool CreatePromoAction(PromoAction instance);
        
        bool UpdatePromoAction(PromoAction instance);
        
        bool RemovePromoAction(int idPromoAction);

        bool ChangeStatePromoAction(int idPromoAction, bool open);
        
        #endregion 

        #region PromoCode
        
        IQueryable<PromoCode> PromoCodes { get; }
        
        bool CreatePromoCode(PromoCode instance);
        
        bool UpdatePromoCode(PromoCode instance);
        
        bool RemovePromoCode(int idPromoCode);

        bool GeneratePromoCodes(int promoActionId, int quantity, string referralCode);

        #endregion 

        #region ProductPrice
        
        IQueryable<ProductPrice> ProductPrices { get; }
        
        bool CreateProductPrice(ProductPrice instance);
        
        bool UpdateProductPrice(ProductPrice instance);
        
        bool RemoveProductPrice(int idProductPrice);
        
        #endregion 

        #region Category
        
        IQueryable<Category> Categories { get; }
        
        bool CreateCategory(Category instance);
        
        bool UpdateCategory(Category instance);
        
        bool RemoveCategory(int idCategory);
        
        #endregion 

        #region SimilarProduct
        
        IQueryable<SimilarProduct> SimilarProducts { get; }

        bool UpdateSimilarProducts(int productId, List<int> productsList);
        
        #endregion 

        #region ProductImage
        
        IQueryable<ProductImage> ProductImages { get; }
        
        bool CreateProductImage(ProductImage instance);
        
        bool UpdateProductImage(ProductImage instance);
        
        bool RemoveProductImage(int idProductImage);

        ProductImage ChangeProductScreenshot(int idProductImage, bool next);
        
        #endregion 

        #region ProductVideo
        
        IQueryable<ProductVideo> ProductVideos { get; }
        
        bool CreateProductVideo(ProductVideo instance);
        
        bool UpdateProductVideo(ProductVideo instance);
        
        bool RemoveProductVideo(int idProductVideo);
        
        #endregion 

        #region ProductReview
        
        IQueryable<ProductReview> ProductReviews { get; }
        
        bool CreateProductReview(ProductReview instance);
        
        bool UpdateProductReview(ProductReview instance);
        
        bool RemoveProductReview(int idProductReview);
        
        #endregion 

        #region Vendor
        
        IQueryable<Vendor> Vendors { get; }
        
        bool CreateVendor(Vendor instance);
        
        bool UpdateVendor(Vendor instance);
        
        bool RemoveVendor(int idVendor);
        
        #endregion 

        #region ProductVariation
        
        IQueryable<ProductVariation> ProductVariations { get; }
        
        bool CreateProductVariation(ProductVariation instance);
        
        bool UpdateProductVariation(ProductVariation instance);
        
        bool RemoveProductVariation(int idProductVariation);
        
        #endregion 

        #region MoneyNotify
        
        IQueryable<MoneyNotify> MoneyNotifies { get; }
        
        bool CreateMoneyNotify(MoneyNotify instance);
        
        bool UpdateMoneyNotify(MoneyNotify instance);
        
        bool RemoveMoneyNotify(int idMoneyNotify);

        void ClearMoneyNotices();
        
        #endregion 

        #region League
        
        IQueryable<League> Leagues { get; }
        
        bool CreateLeague(League instance);
        
        bool UpdateLeague(League instance);
        
        bool RemoveLeague(int idLeague);
        
        #endregion 

        #region LeagueLevel
        
        IQueryable<LeagueLevel> LeagueLevels { get; }
        
        bool CreateLeagueLevel(LeagueLevel instance);
        
        bool UpdateLeagueLevel(LeagueLevel instance);
        
        bool RemoveLeagueLevel(int idLeagueLevel);
        
        #endregion 

        #region LeagueSeason
        
        IQueryable<LeagueSeason> LeagueSeasons { get; }
        
        bool CreateLeagueSeason(LeagueSeason instance);
        
        bool UpdateLeagueSeason(LeagueSeason instance);

        bool UpdateLeagueSeasonStatus(LeagueSeason instance, LeagueSeason.StatusEnum status);
        
        bool RemoveLeagueSeason(int idLeagueSeason);

        bool LeagueSeasonRecalculatePlaces(int idLeagueSeason);
        
        #endregion 

        #region LeagueParticipant
        
        IQueryable<LeagueParticipant> LeagueParticipants { get; }
        
        bool CreateLeagueParticipant(LeagueParticipant instance);
        
        bool UpdateLeagueParticipant(LeagueParticipant instance);
        
        bool RemoveLeagueParticipant(int idLeagueParticipant);

        
        #endregion 
    }
}