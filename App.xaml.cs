using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Service;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Login;
using BoshokuDemo1.Views.Popup;
using BoshokuDemo1.Views.Tab;
using Com.OneSignal;
using Firebase.Storage;
using Firebase.Xamarin.Auth;
using Firebase.Xamarin.Database;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BoshokuDemo1
{
  public partial class App : Application
  {

    public readonly static FirebaseClient fireClient = new FirebaseClient(firebaseDatabaseUrl);
    public readonly static FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseConfig));
    public readonly static BoshokuService hardwareService = new BoshokuService();
    public readonly static string DbName = "HWASqlite.db3";
    public static FirebaseAuth authUser = new FirebaseAuth();


    #region live
   
    #endregion

    #region demo
    public const string firebaseDatabaseUrl = "URL_DATABASE_HERE";
    public const string firebaseConfig = "FIREBAE_CONFIG_KEY";
    public const string firebaseStorageUrl = "FİREBAE_STORAGE_KEY";

    #endregion

    public readonly static FirebaseStorage fireStorage =
                  new FirebaseStorage(
        firebaseStorageUrl,
        new FirebaseStorageOptions
        {
          AuthTokenAsyncFactory = () => Task.FromResult(SaveUserData.userToken),
          ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
        });

    public App()
    {
      InitializeComponent();
      //CachedImageRenderer.Init();

      //MainPage = new NavigationPage(new ControlPage()) { BarTextColor = Color.White };
      //nativeMainPage();

      //MainPage = new DrawerPage();
      if (!SaveUserData.userIntro)
        MainPage = new IntroPage();
      else if (!String.IsNullOrEmpty(SaveUserData.userKey)) MainPage = new NavigationPage(new AppControlPage());
      else MainPage = new NavigationPage(new LoginPage()) { BarTextColor = Color.White };

      //android için splash page geçişi
      //SaveUserData.userBadge = 2;
      sqliteConnection = new SqliteManager();
      Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
      OneSignal.Current.StartInit("URL_ONESIGNAL_KEY")
         .HandleNotificationReceived(HandleNotificationReceived)
         .HandleNotificationOpened(HandleNotificationOpened)
         .InFocusDisplaying(Com.OneSignal.Abstractions.OSInFocusDisplayOption.Notification)
      .EndInit();
      //OneSignal.Current.SetSubscription(true);
    }





    void HandleNotificationReceived(Com.OneSignal.Abstractions.OSNotification notification)
    {
      //eğer notifcation gelirse +1 artırıyoruz
      //SaveUserData.userBadge += 1;

      MessagingCenter.Send<App, bool>(this, MCenter.notificationReceived.ToString(), true);
    }

    void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
      switch (e.NetworkAccess)
      {
        case NetworkAccess.None:
          this.MainPage.Navigation.PushPopupAsync(new ErrorPopPage(), true);
          break;
        default:
          Console.Write("İnternet connection success");
          break;
      }

    }

    static string data;
    static int category;
    SqliteManager sqliteConnection;
    void HandleNotificationOpened(Com.OneSignal.Abstractions.OSNotificationOpenedResult result)
    {
      if (string.IsNullOrEmpty(SaveUserData.userKey))
      {
        MainPage = new LoginPage();
      }
      else
      {
        //uygulama kapalıyken kısım
        try
        {
          data = result.notification.payload.additionalData["key"].ToString();
          category = Convert.ToInt32(result.notification.payload.additionalData["type"].ToString());
          SaveUserData.userBadge += 1;
          sqliteConnection.Insert(data);
          //SaveUserData.userBadge -= 1;
          switch (category)
          {
            case (int)PushCategory.activities:
              MessagingCenter.Send<App, string>(this, MCenter.dataActivity.ToString(), data);
              break;
            case (int)PushCategory.announcements:
              MessagingCenter.Send<App, string>(this, MCenter.dataNotify.ToString(), data);
              break;
            case (int)PushCategory.survey:
              Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new SurveyPage()));
              MessagingCenter.Send<App, string>(this, MCenter.dataSurvey.ToString(), data);
              break;
            case (int)PushCategory.user_interaction:
              Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new EventPage()));
              MessagingCenter.Send<App, string>(this, MCenter.dataUserInteraction.ToString(), data);
              break;
            case (int)PushCategory.multiple_choice:
              Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new EventPage()));
              MessagingCenter.Send<App, string>(this, MCenter.dataMultipleChoice.ToString(), data);
              break;
          }
        }
        catch (System.Exception ex)
        {
          Debug.Write("Push data error ! " + ex.Message);
        }
      }

    }

    async protected override void OnStart()
    {
      switch (Device.RuntimePlatform)
      {
        case Device.Android:
          if (!string.IsNullOrEmpty(data))
          {
            switch (category)
            {
              case (int)PushCategory.activities:
                MessagingCenter.Send<App, string>(this, MCenter.dataActivity.ToString(), data);
                break;
              case (int)PushCategory.announcements:
                MessagingCenter.Send<App, string>(this, MCenter.dataNotify.ToString(), data);
                break;
              case (int)PushCategory.survey:
                await Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new SurveyPage()));
                MessagingCenter.Send<App, string>(this, MCenter.dataSurvey.ToString(), data);
                break;
            }
          }
          break;
      }
    }
    protected override void OnSleep()
    {

    }
    protected override void OnResume()
    {
      //await versionCheck();

      if (!string.IsNullOrEmpty(SaveUserData.userKey))
      {
        MessagingCenter.Send<App, bool>(this, MCenter.onResume.ToString(), true);
      }

    }


  }
}