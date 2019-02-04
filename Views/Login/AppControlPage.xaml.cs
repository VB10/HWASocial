using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class AppControlPage : ContentPage
    {
        AppControlViewModel appControlViewModel;
        public AppControlPage()
        {
            InitializeComponent();
            BindingContext = appControlViewModel = new AppControlViewModel(this);
            NavigationPage.SetHasNavigationBar(this, false);

        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await appControlViewModel.controlUser();
        }
   

    }
}
