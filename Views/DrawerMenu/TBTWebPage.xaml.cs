using System;
using System.Collections.Generic;
using BoshokuDemo1.Model;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.DrawerMenu
{
  public partial class TBTWebPage : ContentPage
  {
    protected override void OnAppearing()
    {
      base.OnAppearing();
      //buttonHome.Text = string.IsNullOrEmpty(this.Title) ? "" : this.Title;
      //if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(false, "Bildirimler");

    }
    public TBTWebPage()
    {
      InitializeComponent();
      //if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(true, "asdas");

      //Navi
      //NavigationPage.SetTitleIcon(this, "navIcon");
      //Title = "sa";

      MessagingCenter.Subscribe<DrawerPage, string>(this, "webPage", (page, data) =>
      {
        _webTBT.Source = getWebUrl(data);
        MessagingCenter.Unsubscribe<DrawerPage, string>(this, "webPage");
      });
      //if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(false, "Bildirimler");

    }
    protected override bool OnBackButtonPressed()
    {
      return base.OnBackButtonPressed();
    }

    void Handle_BackClicked(object sender, System.EventArgs e)
    {
      Navigation.PopModalAsync(true);
    }

    string getWebUrl(string param)
    {

      switch (param[param.Length - 1])
      {
        case '1':
          return "https://www.toyota-boshokutr.com/TBTHakkinda.aspx";
        case '2':
          return "https://www.youtube.com/channel/UCdUaAKTLJrPZFStzEJnpQAg/featured";
        case '3':
          return "https://www.facebook.com/HardwareAndro-1823700344524878/";
        case '4':
          return "https://www.instagram.com/hardwareandro10/";
        case '5':
          return "https://twitter.com/HardwareAndro";
        case '6':
          return "http://kodcocuk.com/";
        case '7':
          return "https://medium.com/@hardwareandro";

        default:
          return "error";

      }

    }
  }
}
