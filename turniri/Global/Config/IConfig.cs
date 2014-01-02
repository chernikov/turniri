using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using turniri.Social.Vkontakte;
using turniri.Social.Twitter;
using turniri.Social.Facebook;
using turniri.Social.Google;

namespace turniri.Global.Config
{
    public interface IConfig
    {
        string ConnectionStrings(string connectionString);

        string CultureCode { get; }

        CultureInfo Culture { get; }

        bool DebugMode { get;  }

        string AdminEmail { get;  }

        IQueryable<MimeType> MimeTypes {  get; }

        MailSettings MailSettings { get; }

        bool EnableMail { get; }

        IQueryable<MailTemplate> MailTemlates { get; }

        IQueryable<IconSize> IconSizes {get;}

        string Host { get; }

        IVkAppConfig VkAppConfig { get; }

        ITwitterAppConfig TwitterAppConfig { get; }

        IFbAppConfig FacebookAppConfig { get; }

        IGoogleAppConfig GoogleAppConfig { get; }

        double FreeCharge { get; }

        string YandexWallet { get; }

        string YandexSecret { get; }

        double MinWithdraw { get; }

        string QiwiID { get; }

        string QiwiPassword { get; }
    }
}
