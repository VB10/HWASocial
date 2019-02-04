using System;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Popup
{
  public partial class AnimationPopupPage : PopupPage
  {
    public AnimationPopupPage()
    {
      InitializeComponent();
      MessagingCenter.Subscribe<BaseViewModel, Toast>(this, MCenter.toastAnimationKey.ToString(), (page, data) =>
      {
        string animationSource;
        switch (data.type)
        {
          case AnimationType.Success:
            animationSource = "Animation/done.json";
            break;
          default:
            animationSource = "sca.json";
            break;
        }
        _animationlabel.Text = data.text;
        _anim.Animation = animationSource;
              //ekrana kalma süresi
              switch (data.display)
        {

          case Show.LOOP:
            break;
          default:

            int second = data.display == Show.SHORT ? 2 : 4;
                  //_animation.Animation = "Animation/sca.json";
                  Device.StartTimer(TimeSpan.FromSeconds(second), () =>
                  {
                    try
                    {
                      Navigation.PopAllPopupAsync();

                    }
                    catch (Exception ex)
                    {
                      Console.WriteLine("Page dismiss" + ex.Message);
                    }
                    return false;
                  });
            break;
        }



        MessagingCenter.Unsubscribe<BaseViewModel, Toast>(this, MCenter.toastAnimationKey.ToString());
      });

    }

    protected override bool OnBackgroundClicked()
    {
      Navigation.PopAllPopupAsync();
      return base.OnBackgroundClicked();

    }

  }
}
