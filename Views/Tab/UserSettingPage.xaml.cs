using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
	public partial class UserSettingPage : ContentPage
    {
		SettingsViewModel settingsVM;
        public UserSettingPage()
        {
            InitializeComponent();
            //NavigationPage.SetTitleIcon(this, "navIcon");
            BindingContext = settingsVM = new SettingsViewModel(this);
            Title = "Profil";


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}