using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Views.Login;
using Com.OneSignal;
using Xamarin.Forms;
using Xamarin.Essentials;
using FFImageLoading;
using BoshokuDemo1.Service;

namespace BoshokuDemo1.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        SqliteManager manager;
        public string appVersion
        {
            get
            {
                return AppInfo.VersionString;
            }
        }
        public string userInfo
        {
            get
            {
                return SaveUserData.userName;
            }
        }
        public string userNameShort
        {
            get
            {
                if (string.IsNullOrEmpty(userInfo)) return "HWA";
                var array = userInfo.Split(' ');
                var shortText = string.Empty;
                foreach (var item in array)
                {
                    shortText += item[0];
                }
                return shortText;
            }
        }

        public ICommand passwordNavigation
        {
            get
            {
                return new Command(async () =>
                {
                    var navigationPage = new PasswordPage();
                    if (Device.RuntimePlatform == Device.iOS) navigationPage.Title = "Şifre Değiştir";
                    await currentPage.Navigation.PushAsync(navigationPage, true);
                });
            }
        }

        public ICommand exitApp
        {
            get
            {
                return new Command(async () =>
                {
                    var checkExit = await currentPage.DisplayAlert("Çıkış Yap", "Çıkış yapmak istediğinize emin misiniz?", "Evet", "Hayır");
                    if (checkExit)
                    {
                        SaveUserData.userBadge = 0;
                        SaveUserData.deleteData();
                        OneSignal.Current.SetSubscription(false);
                        manager.DeleteData();
                        manager.DeleteSimpe();
                        await ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);
                        await currentPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()){BarTextColor = Color.White}, true);
                    }
                    else
                    {
                        return;
                    }
                });
            }
        }


        public SettingsViewModel(Page page) : base(page)
        {
            manager = new SqliteManager();


        }

    }
}
