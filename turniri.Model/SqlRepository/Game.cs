using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Game> Games
        {
            get
            {
                return Db.Games;
            }
        }

        public bool CreateGame(Game instance)
        {
            if (instance.ID == 0)
            {
                Db.Games.InsertOnSubmit(instance);
                Db.Games.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGame(Game instance)
        {
            var cache = Db.Games.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.PlatformID = instance.PlatformID;
                cache.ForumID = instance.ForumID;
				cache.Name = instance.Name;
                cache.Url = instance.Url;
                cache.GameType = instance.GameType;
				cache.Description = instance.Description;
				cache.ImagePath189 = instance.ImagePath189;
				cache.ImagePath103 = instance.ImagePath103;
				cache.ImagePath144v = instance.ImagePath144v;
                cache.ImagePath47 = instance.ImagePath47;
				cache.ImagePath22 = instance.ImagePath22;
				cache.HowToPlay = instance.HowToPlay;
				cache.IsMain = instance.IsMain;
                cache.MaxCountPlayer = instance.MaxCountPlayer;
                cache.GameCategory = instance.GameCategory;
                cache.MetaDescription = instance.MetaDescription;
                cache.Keywords = instance.Keywords;
                Db.Games.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGame(int idGame)
        {
            Game instance = Db.Games.FirstOrDefault(p => p.ID == idGame);
            if (instance != null)
            {
                Db.Games.DeleteOnSubmit(instance);
                Db.Games.Context.SubmitChanges();
                return true;
            }
            return false;
        }
        private bool UpdateGameRole(int id, List<int> users, string code)
        {
            var game = Games.FirstOrDefault(p => p.ID == id);
            if (game != null)
            {
                //delete old
                var list = game.UserRoleGames.Where(p => p.UserRole.Role.Code == code).ToList();
                Db.UserRoleGames.DeleteAllOnSubmit(list);
                Db.UserRoleGames.Context.SubmitChanges();
                //create new
                if (users != null)
                {
                    foreach (var idUser in users)
                    {
                        var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
                        if (user != null)
                        {
                            var userRole = user.UserRoles.FirstOrDefault(p => p.Role.Code == code);
                            if (userRole == null)
                            {
                                var role = Roles.FirstOrDefault(p => p.Code == code);

                                if (role != null)
                                {
                                    userRole = new UserRole()
                                    {
                                        RoleID = role.ID,
                                        UserID = user.ID
                                    };
                                    Db.UserRoles.InsertOnSubmit(userRole);
                                    Db.UserRoles.Context.SubmitChanges();
                                }
                            }
                            var userRoleGame = new UserRoleGame()
                            {
                                UserRoleID = userRole.ID,
                                GameID = game.ID
                            };

                            Db.UserRoleGames.InsertOnSubmit(userRoleGame);
                            Db.UserRoleGames.Context.SubmitChanges();
                        }
                    }
                }
                //check all and delete UserRole
                var listForDelete = UserRoles.Where(p => p.Role.Code == code && !p.UserRoleGames.Any());
                Db.UserRoles.DeleteAllOnSubmit(listForDelete);
                Db.UserRoles.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateGameAdmins(int id, List<int> admins)
        {
            return UpdateGameRole(id, admins, "game_admin");
        }

        public bool UpdateGameModerators(int id, List<int> moderators)
        {
            return UpdateGameRole(id, moderators, "game_moderator");
        }
    }
}