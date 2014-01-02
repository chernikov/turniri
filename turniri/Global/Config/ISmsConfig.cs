using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Global.Config
{
    public interface ISmsConfig
    {
        string SmsAPIKey { get; }

        string SmsSender { get; }

        string SmsTemplateUri { get; }

    }
}