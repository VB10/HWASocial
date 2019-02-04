using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class LoginPage : ContentPage
    {

        LoginViewModel loginVM;
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = loginVM = new LoginViewModel(this);
            NavigationPage.SetHasNavigationBar(this, false);
        }


		protected override bool OnBackButtonPressed()
		{
            return false;
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_buttonLogin.IsVisible) _indicator.HeightRequest = _buttonLogin.Height;
            else _indicator.HeightRequest = 44;
         
        } 

  
        void Handle_IsCheckedChanged(object sender, Xamarin.Forms.TappedEventArgs e)
        {
            loginVM.isCheckBoxChanged((bool)e.Parameter);
        }
    }
}