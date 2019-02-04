using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;
using BoshokuDemo1.Helper;
using BoshokuDemo1.ViewModel;
using BoshokuDemo1.Service;

namespace BoshokuDemo1.Views.Popup
{
  public partial class ErrorPopPage : PopupPage
  {
    public ErrorPopPage()
    {
      InitializeComponent();
      MessagingCenter.Subscribe<BaseViewModel, string>(this, MCenter.errorPagePopKey.ToString(), (page, data) =>
      {

        var error = data;
        _labelError.Text = error;
        MessagingCenter.Unsubscribe<BaseViewModel, string>(this, MCenter.errorPagePopKey.ToString());
      });

      MessagingCenter.Subscribe<BoshokuService, string>(this, MCenter.errorPagePopKey.ToString(), (page, data) =>
      {

        var error = data;
        _labelError.Text = error;
        MessagingCenter.Unsubscribe<BoshokuService, string>(this, MCenter.errorPagePopKey.ToString());
      });
    }
    protected override void OnDisappearing()
    {
      base.OnDisappearing();
      //_labelError.Text = "İnternet bağlantınızda sorun bulunmakta";

    }

    protected override bool OnBackgroundClicked()
    {
      Navigation.PopAllPopupAsync();
      return base.OnBackgroundClicked();

    }
    async protected override void OnAppearing()
    {
      base.OnAppearing();
      var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
      var deviceWidth = (uint)mainDisplayInfo.Width;
      var errorLayoutHeight = (int)_errorBtn.Height + 20;
      await _errorBtn.ScaleTo(1, deviceWidth, Easing.BounceIn);
      //await _errorBtn.ScaleTo(1, 200);
      await _errorBtn.TranslateTo(1, (0 - errorLayoutHeight), deviceWidth);

      try
      {
        await Navigation.PopAllPopupAsync();

      }
      catch (System.Exception ex)
      {

      }

      await _errorBtn.ScaleTo(0, deviceWidth, Easing.SpringOut);
    }
  }
}
