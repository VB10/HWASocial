using System;
using BoshokuDemo1.Model;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace BoshokuDemo1.Helper
{
    public class SaveUserData
    {
        private static ISettings AppSettings => CrossSettings.Current;


        public static bool userIntro
        {

            get => AppSettings.GetValueOrDefault(userEnum.userIntro.ToString(), false);
            set => AppSettings.AddOrUpdateValue(userEnum.userIntro.ToString(), value);
        }
        public static string refreshToken
        {

            get => AppSettings.GetValueOrDefault(userEnum.refreshToken.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.refreshToken.ToString(), value);
        }
        //user bilgileri kayıt

        public static string userKey
        {

            get => AppSettings.GetValueOrDefault(userEnum.userKey.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.userKey.ToString(), value);
        }
        public static string userName
        {

            get => AppSettings.GetValueOrDefault(userEnum.userName.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.userName.ToString(), value);
        }
        public static string userPassword
        {

            get => AppSettings.GetValueOrDefault(userEnum.userPassword.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.userPassword.ToString(), value);
        }
        public static string userRegNumber
        {

            get => AppSettings.GetValueOrDefault(userEnum.userRegNumber.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.userRegNumber.ToString(), value);
        }
        public static string userToken
        {

            get => AppSettings.GetValueOrDefault(userEnum.userToken.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.userToken.ToString(), value);
        }
        public static int userBadge
        {

            get => AppSettings.GetValueOrDefault(userEnum.userBadge.ToString(),0);
            set => AppSettings.AddOrUpdateValue(userEnum.userBadge.ToString(), value);
        }

        public static string userLastRead
        {

            get => AppSettings.GetValueOrDefault(userEnum.userLastRead.ToString(), string.Empty);
            set => AppSettings.AddOrUpdateValue(userEnum.userLastRead.ToString(), value);
        }


        public static void deleteData()
        {
            //AppSettings.Remove(userEnum.userImage.ToString());
            AppSettings.Remove(userEnum.userKey.ToString());
            AppSettings.Remove(userEnum.userName.ToString());
            AppSettings.Remove(userEnum.userToken.ToString());
            AppSettings.Remove(userEnum.userPassword.ToString());
            AppSettings.Remove(userEnum.userRegNumber.ToString());
            AppSettings.Remove(userEnum.userBadge.ToString());
            AppSettings.Remove(userEnum.userLastRead.ToString());
            AppSettings.Remove(userEnum.refreshToken.ToString());
            //App.authProvider.Dispose();
        }
        public static void deleteToken()
        {
            //AppSettings.Remove(userEnum.userImage.ToString());;
            AppSettings.Remove(userEnum.userToken.ToString());
            //App.authProvider.Dispose();
        }

    }
    enum userEnum {
        userName ,
        userKey ,
        userToken,
        userPassword,
        userRegNumber,
        userIntro,
        userBadge,
        userLastRead,
        refreshToken
        //userImage

    }
}
