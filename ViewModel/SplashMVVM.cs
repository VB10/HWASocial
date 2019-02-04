using System;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Login;
using BoshokuDemo1.Views.Popup;
using Com.OneSignal;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class SplashMVVM : BaseViewModel
    {
        Image _splashImage;

        public Image splashImage
        {
            get
            {
                return _splashImage;
            }

            set
            {
                _splashImage = value;
                OnPropertyChanged();
            }
        }

        public SplashMVVM(Page currentPage , Image splashImage): base(currentPage)
        {
            this.splashImage = splashImage;

        }
        private void checkCurrentConnection(NetworkAccess network)
        {
            switch (network)
            {
                case NetworkAccess.None:
                    startAnimation();
                    break;
                default:
                    Console.Write("Internet connection success");
                    break;
            }
        }
        internal async Task onAppering()
        {
            //ios default splash içerisinde android eklemek için
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    await splashImage.ScaleTo(1, 2000);
                    break;
                default:
                    break;
            }
            controlUser();
        }

        private void controlUser()
        {
           
            if (!SaveUserData.userIntro) currentPage.Navigation.PushModalAsync(new IntroPage());
            else
            {
                var currentConnect = Connectivity.NetworkAccess;
                if (!string.IsNullOrEmpty(SaveUserData.userKey))
                {
                    OneSignal.Current.SetSubscription(true);
                    App.Current.MainPage = new MasterControlMenu();
                }
                else
                {
                    OneSignal.Current.SetSubscription(false);
                    App.Current.MainPage = new LoginPage();
                }
                checkCurrentConnection(currentConnect);

            }
        }


    }
}
