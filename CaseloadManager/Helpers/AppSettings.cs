using System;

namespace CaseloadManager.Helpers
{
    public static class AppSettings
    {
        public static string ScriptVersion
        {
            get
            {
                var result = System.Configuration.ConfigurationManager.AppSettings["ScriptVersion"];
                if (result == null)
                    throw new Exception("AppSettings in web.config does not contain the key ScriptVersion");

                return result;
            }
        }
        public static string SiteURL
        {
            get
            {
                var result = System.Configuration.ConfigurationManager.AppSettings["SiteURL"];
                if (result == null)
                    throw new Exception("AppSettings in web.config does not contain the key SiteURL");

                return result;
            }
        }
    }
}