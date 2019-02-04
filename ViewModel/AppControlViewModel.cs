using System;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Login;
using Com.OneSignal;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
  public class AppControlViewModel : BaseViewModel
  {
    private bool _isPageLoading = true;

    public AppControlViewModel(Page page) : base(page)
    {

    }

    public bool isPageLoading
    {
      get => _isPageLoading;
      set
      {
        _isPageLoading = value;
        OnPropertyChanged();
      }
    }


    internal async Task controlUser()
    {
      if (!string.IsNullOrEmpty(SaveUserData.userKey))
      {

        if (!await service.AUTO_LOGIN())
        {

          await checkInternetConnection(base.networkAccess);
          Connectivity.ConnectivityChanged += async (object sender, ConnectivityChangedEventArgs e) =>
          {
            await checkInternetConnection(e.NetworkAccess);
          };
        }
        else
        {
          if (!await versionCheck()) return;

          var detail = await getUserDetail();
          if (detail)
          {

            OneSignal.Current.SetSubscription(true);
            //App.Current.MainPage = new MasterControlMenu();
            Device.BeginInvokeOnMainThread(async () =>
           {
             var navPage = new MasterControlMenu();
             Navigation.InsertPageBefore(navPage, this.currentPage);
             await Navigation.PopToRootAsync();

           });
          }
        }
      }
      else
      {
        await goLoginPage();
      }
    }

    async Task checkInternetConnection(NetworkAccess access)
    {
      switch (access)
      {
        case NetworkAccess.None:
          isPageLoading = false;
          return;
        default:
          isPageLoading = true;
          await controlUser();
          break;
      }
    }

    async Task goLoginPage()
    {
      OneSignal.Current.SetSubscription(false);
      await Navigation.PushModalAsync(new NavigationPage(new LoginPage()) { BarTextColor = Color.White });
    }
    async Task<bool> getUserDetail()
    {

      try
      {
        var userDetails = await service.GET_object<Users>(SaveUserData.userKey);
        SaveUserData.userName = userDetails.fullName;
        if (userDetails.security.newPassword != null)
        {
          //SaveUserData.userToken = App.authUser.FirebaseToken;
          return true;
        }
      }
      catch (Exception ex)
      {
        await Navigation.PushModalAsync(new NavigationPage(new NewUserPage()), true);
        Console.WriteLine("error" + ex.ToString());
      }
      return false;
    }

    async Task<string> clientVersionCheck()
    {
      try
      {
        var tempUser = await authService.SignInAnonymouslyAsync();
        string key = tempUser.FirebaseToken;
        //client value one object get
        var client = await service.GET_object_AUTH<MobileClient>("client", key, true);
        //işlem sonunda temp user delete
        await service.DELETE_USER(key);
        return client;
      }
      catch (Exception)
      {
        await startErrorAnimation();
        return string.Empty;
      }


    }


    async Task<bool> versionCheck()
    {
      string clientVersion = await clientVersionCheck();
      if (string.IsNullOrEmpty(clientVersion)) return true;
      if (AppInfo.VersionString != clientVersion)
      {
        var update = await base.currentPage.DisplayAlert("Yeni Versiyon Bulundu", "Uygulamayı lütfen güncelleyiniz.", "Kapat", "Güncelle");
        if (update)
        {
          await versionCheck();
        }
        else
        {

          if (DeviceInfo.Platform == DevicePlatform.iOS)
          {
            await Browser.OpenAsync("https://beta.itunes.apple.com/v1/app/1355474315", BrowserLaunchMode.External);
          }
          else if (DeviceInfo.Platform == DevicePlatform.Android)
          {
            await Browser.OpenAsync("https://rink.hockeyapp.net/download/a", BrowserLaunchMode.External);
          }
          else
          {
          }

        }
        return false;

      }
      else
      {
        return true;
      }
    }



  }
}

