namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class Config {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Config));

        public virtual T GetAppSetting<T>(string name, T defaultValue = default(T), bool required = false) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            T result = default(T);
            string appSettingValue = ConfigurationManager.AppSettings[name];
            if (appSettingValue != null) {
                result = (T)Convert.ChangeType(appSettingValue, typeof(T));
            }
            else {
                result = defaultValue;
            }

            if (required && result == null) {
                string message = string.Format("Missing required configuration value [{0}]",name);
                log.Error(message);
                throw new ConfigurationErrorsException(message);
            }

            return result;
        }

        public virtual ConnectionStringSettings GetConnectionString(string name,bool required = false) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            ConnectionStringSettings result = null;

            result = ConfigurationManager.ConnectionStrings[name];
            if (required && result == null) {
                string message = string.Format("Missing required connection string in config [{0}]",name);
                log.Error(message);
                throw new ConfigurationErrorsException(message);
            }

            return result;
        }
    }
}
