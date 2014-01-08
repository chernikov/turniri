using System.Web.Mvc;

namespace turniri.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "error",
                "error",
                new { controller = "Error", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "notFoundPage",
                "not-found-page",
                new { controller = "Error", action = "NotFoundPage" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "sitemap",
                "sitemap.xml",
                new { controller = "Home", action = "Sitemap" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "Page",
                "page/{url}",
                new { controller = "Page", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* User */
            context.MapRoute(
                null,
                "user/edit",
                new { controller = "User", action = "Edit" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/money",
                new { controller = "User", action = "Money" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/money-list",
                new { controller = "User", action = "MoneyList" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/money-recharge",
                new { controller = "User", action = "MoneyRecharge" },
                new[] { "turniri.Areas.Default.Controllers" }
            );


            context.MapRoute(
                null,
                "user/money-withdraw",
                new { controller = "User", action = "MoneyWithdraw" },
                new[] { "turniri.Areas.Default.Controllers" }
            );


            context.MapRoute(
                null,
                "user/register",
                new { controller = "User", action = "Register" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/register-success",
                new { controller = "User", action = "RegisterSuccess" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/captcha",
                new { controller = "User", action = "Captcha" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/activate",
                new { controller = "User", action = "Activate" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/ResendActivation",
                new { controller = "User", action = "ResendActivation" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/ResendVerifyEmail",
                new { controller = "User", action = "ResendVerifyEmail" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/VerifyEmail",
                new { controller = "User", action = "VerifyEmail" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/RedirectVerifyEmail",
                new { controller = "User", action = "RedirectVerifyEmail" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/UploadAvatar",
                new { controller = "User", action = "UploadAvatar" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/change-password",
                new { controller = "User", action = "ChangePassword" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/ChangePasswordAjax",
                new { controller = "User", action = "ChangePasswordAjax" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/games",
                new { controller = "User", action = "Games" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/PlatformGames",
                new { controller = "User", action = "PlatformGames" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/PlayGame",
                new { controller = "User", action = "PlayGame" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/StopGame",
                new { controller = "User", action = "StopGame" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/Reputation",
                new { controller = "User", action = "Reputation" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/TournamentReputation",
                new { controller = "User", action = "TournamentReputation" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/VoteReputation",
                new { controller = "User", action = "VoteReputation" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/VoteGrade",
                new { controller = "User", action = "VoteGrade" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/Matches",
                new { controller = "User", action = "Matches" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/Tournaments",
                new { controller = "User", action = "Tournaments" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/award/{login}",
                new { controller = "Award", action = "Index", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/group/{login}",
                new { controller = "User", action = "Group", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/blog/{login}",
                new { controller = "Blog", action = "Index", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/photo/{login}",
                new { controller = "Photo", action = "Index", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/video/{login}",
                new { controller = "UserVideo", action = "Index", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/friends/{login}",
                new { controller = "Friend", action = "Index", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/{login}",
                new { controller = "User", action = "Index", login = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "users",
                new { controller = "Users", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* Group */
            context.MapRoute(
                null,
                "group/GroupControlPanel",
                new { controller = "Group", action = "GroupControlPanel" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
               null,
               "group/MoneyList/{id}",
               new { controller = "Group", action = "MoneyList" },
               new[] { "turniri.Areas.Default.Controllers" }
           );

            context.MapRoute(
                null,
                "group/create",
                new { controller = "Group", action = "Create" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/edit/{id}",
                new { controller = "Group", action = "Edit", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/roster/{url}",
                new { controller = "Group", action = "Roster" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/transfers/{url}",
                new { controller = "Group", action = "Transfers" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/award/{url}",
                new { controller = "Group", action = "Award" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/AcceptPlayer/{id}",
                new { controller = "Group", action = "AcceptPlayer", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/RemovePlayer/{id}",
                new { controller = "Group", action = "RemovePlayer", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/SwitchRole/{id}",
                new { controller = "Group", action = "SwitchRole", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/Leave/{id}",
                new { controller = "Group", action = "Leave", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/Enter/{id}",
                new { controller = "Group", action = "Enter", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/InvoiceControlPart/{id}",
                new { controller = "Group", action = "InvoiceControlPart", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/Notify/{id}",
                new { controller = "Group", action = "Notify", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/users/{url}",
                new { controller = "Group", action = "Users" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/blog/{url}",
                new { controller = "Blog", action = "Group" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/photo/{url}",
                new { controller = "Photo", action = "Group" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/video/{url}",
                new { controller = "UserVideo", action = "Group" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "group/{url}",
                new { controller = "Group", action = "Item" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "groups",
                new { controller = "Group", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );


            /* Blog */
            context.MapRoute(
                null,
                "blog/LastComments/{id}",
                new { controller = "Blog", action = "LastComments", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/LastCommentsGroup/{id}",
                new { controller = "Blog", action = "LastCommentsGroup", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/create/{id}",
                new { controller = "Blog", action = "Create", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/edit/{id}",
                new { controller = "Blog", action = "Edit", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/delete/{id}",
                new { controller = "Blog", action = "Delete", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/UploadPreview",
                new { controller = "Blog", action = "UploadPreview" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/CreateComment/{id}",
                new { controller = "Blog", action = "CreateComment", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/ToggleLike",
                new { controller = "Blog", action = "ToggleLike", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "blog/{url}",
                new { controller = "Blog", action = "Item" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* Forum */
            context.MapRoute(
                null,
                "forum/UserOnline",
                new { controller = "Forum", action = "UserOnline" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/VisitersOnline",
                new { controller = "Forum", action = "VisitersOnline" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/TotalCount",
                new { controller = "Forum", action = "TotalCount" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/Create",
                new { controller = "Forum", action = "Create" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/CreateMessage",
                new { controller = "Forum", action = "CreateMessage", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/RemoveMessage",
                new { controller = "Forum", action = "RemoveMessage", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/EditMessage",
                new { controller = "Forum", action = "EditMessage", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/ToggleForumNotice",
                new { controller = "Forum", action = "ToggleForumNotice", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/IsNoticed/{id}",
                new { controller = "Forum", action = "IsNoticed", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/Item/{id}",
                new { controller = "Forum", action = "Item", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forum/{url}",
                new { controller = "Forum", action = "Index", url = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* Game */
            context.MapRoute(
                null,
                "game/Menu/{id}",
                new { controller = "Game", action = "Menu", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/UserRating/{id}",
                new { controller = "Game", action = "UserRating", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/TournamentsList/{id}",
                new { controller = "Game", action = "TournamentsList", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/PartialTournamentsList/{id}",
                new { controller = "Game", action = "PartialTournamentsList", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/FutureMatches/{id}",
                new { controller = "Game", action = "FutureMatches", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/CurrentMatches/{id}",
                new { controller = "Game", action = "CurrentMatches", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/achieve/{platformUrl}/{url}",
                new { controller = "Game", action = "Achieve" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/tournaments/{platformUrl}/{url}",
                new { controller = "Game", action = "Tournaments" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/group/{platformUrl}/{url}",
                new { controller = "Game", action = "Group" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/gamers/{platformUrl}/{url}",
                new { controller = "Game", action = "Gamers" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/how-to-play/{platformUrl}/{url}",
                new { controller = "Game", action = "HowToPlay" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "game/{platformUrl}/{url}",
                new { controller = "Game", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /*League */
            context.MapRoute(
                null,
                "league/Level",
                new { controller = "League", action = "Level" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "league/{url}",
                new { controller = "League", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* New */
            context.MapRoute(
                null,
                "new/CreateComment/{id}",
                new { controller = "New", action = "CreateComment", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "new/ToggleLike",
                new { controller = "New", action = "ToggleLike" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "new/{url}",
                new { controller = "New", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* Tournament */
            context.MapRoute(
                null,
                "tournaments",
                new { controller = "Tournament", action = "All" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/List",
                new { controller = "Tournament", action = "List" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/UserRating/{id}",
                new { controller = "Tournament", action = "UserRating", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/Teams/{id}",
                new { controller = "Tournament", action = "Teams", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/StatisticRoundRobin/{id}",
                new { controller = "Tournament", action = "StatisticRoundRobin", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/Group/{id}",
                new { controller = "Tournament", action = "Group", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/GroupPart/{id}",
                new { controller = "Tournament", action = "GroupPart", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/PlayoffPart/{id}",
                new { controller = "Tournament", action = "PlayoffPart", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/StatisticGroup/{id}",
                new { controller = "Tournament", action = "StatisticGroup", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/SelectListTours/{id}",
                new { controller = "Tournament", action = "SelectListTours", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/Tours/{id}",
                new { controller = "Tournament", action = "Tours", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/SelectListGroups/{id}",
                new { controller = "Tournament", action = "SelectListGroups", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/GetPart/{id}",
                new { controller = "Tournament", action = "GetPart", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/FinishTournament/{id}",
                new { controller = "Tournament", action = "FinishTournament", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/Start/{id}",
                new { controller = "Tournament", action = "Start", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/AllocatePlayoff/{id}",
                new { controller = "Tournament", action = "AllocatePlayoff", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/AddGame/{id}",
                new { controller = "Tournament", action = "AddGame", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "tournament/{platformUrl}/{gameUrl}/{url}",
                new { controller = "Tournament", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "shop",
                new { controller = "Shop", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );
            context.MapRoute(
                null,
                "shopaction/{action}/{id}",
                new { controller = "Shop", action = "Index", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "shop/{*path}",
                new { controller = "Shop", action = "Index", path = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "product/{path}",
                new { controller = "Product", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "products/{action}/{id}",
                new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );
            /* Photo */
            context.MapRoute(
                null,
                "photos",
                new { controller = "PublicPhoto", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/Create",
                new { controller = "Photo", action = "Create" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/edit",
                new { controller = "Photo", action = "Edit" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/edit/{id}",
                new { controller = "Photo", action = "Edit", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/delete/{id}",
                new { controller = "Photo", action = "Delete", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/UploadPhoto",
                new { controller = "Photo", action = "UploadPhoto" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/BindPhoto",
                new { controller = "Photo", action = "BindPhoto" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/View",
                new { controller = "Photo", action = "View" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/ChangeView",
                new { controller = "Photo", action = "ChangeView" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/CreateComment/{id}",
                new { controller = "Photo", action = "CreateComment", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/RemovePhoto/{id}",
                new { controller = "Photo", action = "RemovePhoto", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/ToggleLike",
                new { controller = "Photo", action = "ToggleLike" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/UpdatePhoto",
                new { controller = "Photo", action = "UpdatePhoto" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "photo/{url}",
                new { controller = "Photo", action = "Item", url = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            /* Video */
            context.MapRoute(
                null,
                "video/VideoCode/{id}",
                new { controller = "Video", action = "VideoCode", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "video/CreateComment/{id}",
                new { controller = "Video", action = "CreateComment", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "video/{url}",
                new { controller = "Video", action = "Item", url = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "videos",
                new { controller = "Video", action = "Index", url = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "cart",
                new { controller = "Cart", action = "Index" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "cart/process",
                new { controller = "Cart", action = "Process" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "yandex-money",
                new { controller = "Money", action = "AcceptYandexMoney" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "qiwi-success",
                new { controller = "Money", action = "AcceptQiwiMoney" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "qiwi-fail",
                new { controller = "Money", action = "FailQiwiMoney" },
                new[] { "turniri.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "turniri.Areas.Default.Controllers" }
            );
        }
    }
}
