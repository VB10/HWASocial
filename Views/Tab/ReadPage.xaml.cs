using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class ReadPage : ContentPage
    {
        ReadViewModel readViewModel;
        public ReadPage()
        {
            InitializeComponent();
            BindingContext = readViewModel = new ReadViewModel(this);
            if (Device.RuntimePlatform == Device.Android) _navIcon.HeightRequest = 40;
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }


    }
}
