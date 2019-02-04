using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using BoshokuDemo1.Views.Popup;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class PasswordPage : ContentPage
    {
        PasswordViewModel passworViewModel;
        public PasswordPage()
        {
            InitializeComponent();
            BindingContext = passworViewModel = new PasswordViewModel(this);
            if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(false, "Şifre Değiştir");

        }
    
    }
}
