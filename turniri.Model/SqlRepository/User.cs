using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<User> Users
        {
            get
            {
                return Db.Users;
            }
        }

        public IQueryable<User> NonActivatedUsers
        {
            get
            {
                return Db.Users.Where(p => p.ActivatedDate == null);
            }
        }
        public IQueryable<User> RegularUsers
        {
            get
            {
                return Db.Users.Where(p => p.ActivatedDate.HasValue && !p.Banned);
            }
        }

        public IQueryable<User> OnlineUsers
        {
            get { return RegularUsers.Where(p => p.LastVisitDate.AddMinutes(5) > DateTime.Now); }
        }

        public bool CreateUser(User instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.LastVisitDate = DateTime.Now;
                instance.ActivatedLink = StringExtension.CreateRandomPassword(6).ToUpper();
                instance.Reputation = 0;
                instance.Subscription = true;
                Db.Users.InsertOnSubmit(instance);
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                if (cache.Email != instance.Email)
                {
                    cache.Email = instance.Email;
                    cache.VerifiedEmail = false;
                    cache.ActivatedLink = StringExtension.CreateRandomPassword(6, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$_-").ToUpper();
                }

                cache.Login = instance.Login;
                cache.AvatarPath173 = instance.AvatarPath173;
                cache.AvatarPath96 = instance.AvatarPath96;
                cache.AvatarPath84 = instance.AvatarPath84;
                cache.AvatarPath57 = instance.AvatarPath57;
                cache.AvatarPath30 = instance.AvatarPath30;
                cache.AvatarPath26 = instance.AvatarPath26;
                cache.AvatarPath18 = instance.AvatarPath18;
                cache.FirstName = instance.FirstName;
                cache.LastName = instance.LastName;
                cache.Country = instance.Country;
                cache.City = instance.City;
                cache.Phone = instance.Phone;
                cache.Birthdate = instance.Birthdate;
                cache.PlaystationID = instance.PlaystationID;
                cache.XboxGametag = instance.XboxGametag;
                cache.EAAccount = instance.EAAccount;
                cache.SteamAccount = instance.SteamAccount;
                cache.GarenaAccount = instance.GarenaAccount;
                cache.ICQ = instance.ICQ;
                cache.Skype = instance.Skype;
                cache.Vk = instance.Vk;
                cache.Subscription = instance.Subscription;
                cache.Signature = instance.Signature;
                cache.Address = instance.Address;
                Db.Users.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUser(int idUser)
        {
            User instance = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null)
            {
                Db.Users.DeleteOnSubmit(instance);
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public User GetUser(string login)
        {
            var user = Db.Users.FirstOrDefault(p => (string.Compare(p.Email, login, true) == 0 || string.Compare(p.Login, login, true) == 0) && !p.Banned);

            //check notices 
            CheckSystemNotices(user);

            return user;
        }

        public void VisitUser(int id, double freeAmount)
        {
            try
            {
                var user = Db.Users.FirstOrDefault(p => p.ID == id);
                if (user != null)
                {
                    if (DateTime.Now.Date != user.LastVisitDate.Date)
                    {
                        var moneyDetail = new MoneyDetail()
                       {
                           SumWood = freeAmount,
                           UserID = user.ID,
                           Description = "Ежедневный подарок при заходе на сайт",
                           Submited = true
                       };
                        CreateMoneyDetail(moneyDetail, Guid.NewGuid());
                    }
                    user.LastVisitDate = DateTime.Now;
                    Db.Users.Context.SubmitChanges();
                }
            }
            catch { }
        }

        public User Login(string login, string password)
        {
            return Db.Users.FirstOrDefault(p => (string.Compare(p.Email, login, true) == 0 || string.Compare(p.Login, login, true) == 0) && p.Password == password);
        }

        public bool ActivateUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                if (!cache.ActivatedDate.HasValue)
                {
                    cache.ActivatedDate = DateTime.Now;
                }
                cache.VerifiedEmail = true;
                var notices = cache.Notices.Where(p => p.Type == (int)Notice.TypeEnum.Activate || p.Type == (int)Notice.TypeEnum.VerifiedEmail).ToList();
                Db.Notices.DeleteAllOnSubmit(notices);
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool VerifiedEmailUser(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.VerifiedEmail = true;
                var notices = cache.Notices.Where(p => p.Type == (int)Notice.TypeEnum.Activate || p.Type == (int)Notice.TypeEnum.VerifiedEmail).ToList();
                Db.Notices.DeleteAllOnSubmit(notices);
                Db.Notices.Context.SubmitChanges();
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ChangePassword(User instance)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Password = instance.Password;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool BanUser(User instance, bool ban)
        {
            var cache = Db.Users.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Banned = ban;
                Db.Users.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UserVisitCount(int id)
        {
            try
            {
                var cache = Db.Users.FirstOrDefault(p => p.ID == id);
                if (cache != null)
                {
                    cache.VisitCount++;
                    Db.Users.Context.SubmitChanges();
                    return true;
                }
            }
            catch { }
            return false;
        }

        public void AwardUser(int idAward, int idUser)
        {
            var award = Db.Awards.FirstOrDefault(p => p.ID == idAward);
            var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (award != null && user != null)
            {
                var userAward = new UserAward()
                {
                    AwardID = award.ID,
                    UserID = user.ID,
                };
                CreateUserAward(userAward);
                AddExpoToUser(null, award.Tournament, award.Point, award.Name, user);
            }
        }

        private void CheckSystemNotices(User user)
        {
            if (!user.ActivatedDate.HasValue)
            {
                var any = Db.Notices.Any(p => p.User.ID == user.ID && p.Type == (int)Notice.TypeEnum.Activate);

                if (!any)
                {
                    var activateNotice = new Notice()
                    {
                        ReceiverID = user.ID,
                        Text = "Активируйте свой аккаунт",
                        Caption = "Активация",
                        Type = (int)Notice.TypeEnum.Activate,
                        IsCloseForRead = false
                    };
                    CreateNotice(activateNotice);

                    var actionNoticeAction = new NoticeAction()
                    {
                        ActionUrl = "/User/ResendActivation",
                        Direct = true,
                        NoticeID = activateNotice.ID,
                        IsResolveNotice = false,
                        Name = "Выслать активацию"
                    };

                    CreateNoticeAction(actionNoticeAction);
                }
            }
            else
            {
                var any = Db.Notices.Any(p => p.User.ID == user.ID && p.Type == (int)Notice.TypeEnum.VerifiedEmail);

                if (!any)
                {
                    if (!user.VerifiedEmail)
                    {

                        var verifyNotice = new Notice()
                        {
                            ReceiverID = user.ID,
                            Text = "Подтвердите свой email",
                            Caption = "Верификация",
                            Type = (int)Notice.TypeEnum.VerifiedEmail,
                            IsCloseForRead = false
                        };
                        CreateNotice(verifyNotice);

                        var actionNoticeAction = new NoticeAction()
                        {
                            ActionUrl = "/User/ResendVerifyEmail",
                            Direct = true,
                            NoticeID = verifyNotice.ID,
                            IsResolveNotice = false,
                            Name = "Верифицировать"
                        };

                        CreateNoticeAction(actionNoticeAction);
                    }
                }
            }
        }
    }
}