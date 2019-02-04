using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class PasswordForgotPage : ContentPage
    {
        ForgotViewModel forgotViewModel;
        public PasswordForgotPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            BindingContext =  forgotViewModel = new ForgotViewModel(this);
            Title = "Şifremi Unuttum";
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            forgotViewModel.TextChanged(e.NewTextValue);
        }
    }
}
