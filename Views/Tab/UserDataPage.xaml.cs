using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class UserDataPage : ContentPage
    {
        UserRequestMVVM userRequestMVVM;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            userRequestMVVM.OnAppearing();
        }
        public UserDataPage()
        {
            InitializeComponent();
            BindingContext = userRequestMVVM = new UserRequestMVVM(this);
            Title = "Bize Bildir";
        }
    }
}