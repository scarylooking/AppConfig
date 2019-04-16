using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AppConfig
{
    public class ConfigurationHelper
    {
        private readonly Configuration _config;

        public ConfigurationHelper()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public void WriteEncryptedValue(string key, string value)
        {
            WriteValue(key, value, true);
        }

        public void WriteClearTextValue(string key, string value)
        {
            WriteValue(key, value, false);
        }

        public string ReadEncryptedValue(string key, string defaultValue = null)
        {
            return ReadValue(key, defaultValue, true);
        }

        public string ReadClearTextValue(string key, string defaultValue = null)
        {
            return ReadValue(key, defaultValue, false);
        }

        private void WriteValue(string key, string value, bool encryptValue)
        {
            if (_config.AppSettings.Settings.AllKeys.Contains(key))
            {
                _config.AppSettings.Settings.Remove(key);
            }

            _config.AppSettings.Settings.Add(key, encryptValue ? Encrypt(value) : value);

            _config.Save();
        }

        private string ReadValue(string key, string defaultValue, bool decryptValue)
        {
            if (!_config.AppSettings.Settings.AllKeys.Contains(key))
            {
                return defaultValue;
            }

            var decryptedValue = _config.AppSettings.Settings[key].Value;

            return decryptValue ? Decrypt(decryptedValue) : decryptedValue;
        }

        private static string Encrypt(string unencryptedValue)
        {
            var unencryptedInputBytes = Encoding.Unicode.GetBytes(unencryptedValue);
            var encryptedValue = ProtectedData.Protect(unencryptedInputBytes, null, DataProtectionScope.CurrentUser);
            var base64EncryptedValue = Convert.ToBase64String(encryptedValue);

            return base64EncryptedValue;
        }

        private static string Decrypt(string encryptedBase64Value)
        {
            var encryptedInputBytes = Convert.FromBase64String(encryptedBase64Value);
            var unencryptedValueBytes = ProtectedData.Unprotect(encryptedInputBytes, null, DataProtectionScope.CurrentUser);
            var unencryptedValue = Encoding.Unicode.GetString(unencryptedValueBytes);

            return unencryptedValue;
        }
    }
}