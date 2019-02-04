using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using BoshokuDemo1.Views.Popup;
using Firebase.Storage;
using Firebase.Xamarin.Auth;
using Firebase.Xamarin.Database;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
  public class BaseViewModel : INotifyPropertyChanged
  {

    public readonly FirebaseClient fireClient = new FirebaseClient(firebaseDatabaseUrl);
    public readonly FirebaseAuthProvider authService = new FirebaseAuthProvider(new FirebaseConfig(firebaseConfig));
    public static FirebaseAuth authUser = new FirebaseAuth();

    public const string firebaseDatabaseUrl = App.firebaseDatabaseUrl;
    public const string firebaseStorageUrl = App.firebaseStorageUrl;
    public const string firebaseConfig = App.firebaseConfig;





    public readonly static FirebaseStorage fireStorage =
   new FirebaseStorage(
    firebaseStorageUrl,
    new FirebaseStorageOptions
    {
      AuthTokenAsyncFactory = () => Task.FromResult(SaveUserData.userToken),
      ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
    });


    protected Page currentPage
    {
      get;
      set;
    }

    public BoshokuService service;

    public INavigation Navigation
    {
      get
      {
        return currentPage.Navigation;
      }
    }


    public void setNavigationBar(bool val)
    {
      NavigationPage.SetHasNavigationBar(currentPage, val);
    }

    public void SaveBadge()
    {
      if (SaveUserData.userBadge <= 0 || SaveUserData.userBadge > 15)
      {
        SaveUserData.userBadge = 0;
        return;
      }
      DependencyService.Get<IToolbarItemBadge>().SetBadge(currentPage, currentPage.ToolbarItems.First(), $"" + SaveUserData.userBadge, Color.Red, Color.White);

    }


    async public Task ToastMessage(Show delay, string text, AnimationType _type)
    {
      await currentPage.Navigation.PushPopupAsync(new AnimationPopupPage(), true);
      MessagingCenter.Send<BaseViewModel, Toast>(this, MCenter.toastAnimationKey.ToString(), new Toast() { display = delay, text = text, type = _type });

    }

    /// <summary>
    /// Toasts the message.
    /// </summary>
    /// <returns>The message.</returns>
    /// <param name="text">Text.</param>

    async public Task ToastMessage(string text)
    {
      await currentPage.Navigation.PushPopupAsync(new LoadingPopPage(), true);
      MessagingCenter.Send<BaseViewModel, string>(this, MCenter.toastKey.ToString(), text);

    }
    /// <summary>
    /// Displaies the error.
    /// </summary>
    /// <param name="message">Message.</param>
    public void DisplayError(string message)
    {
      currentPage.DisplayAlert("Hata", message, "Tamam");
    }
    public void DisplaySuccess(string message)
    {
      currentPage.DisplayAlert("Başarılı", message, "Tamam");
    }
    async public Task startAnimation()
    {
      await currentPage.Navigation.PushPopupAsync(new LoadingPopPage(), true);
    }
    async public Task startErrorAnimation(string text)
    {
      await currentPage.Navigation.PushPopupAsync(new ErrorPopPage(), true);
      MessagingCenter.Send<BaseViewModel, string>(this, MCenter.errorPagePopKey.ToString(), text);
    }
    async public Task startErrorAnimation()
    {
      string text = "İnternet bağlantınızda sorun bulunmakta";
      await currentPage.Navigation.PushPopupAsync(new ErrorPopPage(), true);
      MessagingCenter.Send<BaseViewModel, string>(this, MCenter.errorPagePopKey.ToString(), text);
    }
    async public Task stopAnimation()
    {
      await currentPage.Navigation.PopAllPopupAsync(true);

    }
    public BaseViewModel(Page currentPage)
    {
      this.currentPage = currentPage;
      service = new BoshokuService();
      currentPage.Appearing += onApering;
      Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

    }
    async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
      await checkCurrentConnection();
    }

    async void onApering(object sender, EventArgs e)
    {
      await checkCurrentConnection();

    }
    async Task checkCurrentConnection()
    {
      var network = Connectivity.NetworkAccess;
      switch (network)
      {
        case NetworkAccess.None:
          await startErrorAnimation();
          break;
        default:
          Console.Write("Internet connection success");
          break;
      }
    }

    public NetworkAccess networkAccess { get => Connectivity.NetworkAccess; }

    #region Değişiklikleri yakalama
    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
