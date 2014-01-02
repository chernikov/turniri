using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;
using System.Globalization;
using Ninject;


namespace turniri.Global
{
    public class Resources : IResources
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public CultureInfo ci { get; set; }

        private ResourceManager manager { get; set; }

        public string this[string ObjectName]
        {
            get
            {
                string RetString = manager.GetString(ObjectName, ci);
                if (string.IsNullOrEmpty(RetString))
                {
                    logger.Debug("Can't find resource " + ObjectName);
                    return ObjectName;
                }
                return RetString;
            }
        }

        public Resources(CultureInfo ci, ResourceManager manager)
        {
            this.ci = ci;
            this.manager = manager;
        }
    }
}