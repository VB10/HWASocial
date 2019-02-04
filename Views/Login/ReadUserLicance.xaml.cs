using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class ReadUserLicance : ContentPage
    {
        UserLicanceViewModel userLicanceViewModel;
        public ReadUserLicance()
        {
            InitializeComponent();
            BindingContext = userLicanceViewModel = new UserLicanceViewModel(this);
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await userLicanceViewModel.onAppering();
        }

        async void Handle_Navigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
        {
            await userLicanceViewModel.onFinish();
        }
        protected override bool OnBackButtonPressed()
        {
             userLicanceViewModel.onFinish();
            return base.OnBackButtonPressed();

        }
    }
}
