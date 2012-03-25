namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class Config {
        public T GetConfigSetting<T>(string name, T defaultValue = default(T), bool required = false) {
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
                throw new ConfigurationException(message);
            }

            return result;
        }
    }
}
