using System;
using System.Configuration;

namespace MyContractsGenerator.Common
{
    public static class GlobalAppSettings
    {
        public static int ImageUploadMinSizeBytes
        {
            get { return Get<int>("ImageUploadMinSizeBytes"); }
        }
        
        public static int ImageUploadMaxSizeBytes
        {
            get { return Get<int>("ImageUploadMaxSizeBytes"); }
        }

        public static string ApplicationVersion
        {
            get { return Get<string>("ApplicationVersion"); }
        }

        public static string ApplicationBaseUrl
        {
            get { return Get<string>("ApplicationBaseUrl"); }
        }

        #region Get

        private static T Get<T>(string name)
        {
            if (ConfigurationManager.AppSettings[name] == null)
            {
                throw new ApplicationException(string.Format("AppSetting {0} absent", name));
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[name], typeof(T));
        }

        private static T GetWithDefault<T>(string name, T defaultValue)
        {
            if (ConfigurationManager.AppSettings[name] == null)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[name], typeof(T));
        }

        #endregion
    }
}