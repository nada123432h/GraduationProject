// Helpers/Settings.cs
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace otp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private const string LanguageKey = "Languagekey";
        private const string txtnameKey = "UserName_key";
               private const string PhoneKey = "phone_key";
        private const string imgpic = "imgpic_key";
        private const string userInfo = "userinfo";
        private const string providerInfo = "userinfo";



        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguageKey, "ar");
            }
            set
            {
                AppSettings.AddOrUpdateValue(LanguageKey, value);
            }

        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault(txtnameKey, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(txtnameKey, value);
            }

        }
        public static string UserEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault("User_Email", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("User_Email", value);
            }

        }

        public static string ProviderEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault("Provider_Email", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Provider_Email", value);
            }

        }


        public static string Phone
        {
            get
            {
                return AppSettings.GetValueOrDefault(PhoneKey, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(PhoneKey, value);
            }


        }

        public static string ImgName
        {
            get
            {
                return AppSettings.GetValueOrDefault(imgpic, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(imgpic, value);
            }

        }

        public static string userinfo
        {
            get
            {
                return AppSettings.GetValueOrDefault(userInfo, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(userInfo, value);
            }

        }


        public static string providerinfo
        {
            get
            {
                return AppSettings.GetValueOrDefault(providerInfo, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(providerInfo, value);
            }

        }


    }
}