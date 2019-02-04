using System;
using BoshokuDemo1.Helper;
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class IntroPage : ContentPage
    {
        IntroViewModel _introVM;
        public IntroPage()
        {
            InitializeComponent();
            BindingContext = _introVM = new IntroViewModel();
        }
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (carousel.Position + 1 == _introVM.MyItemsSource.Count)
            {
                SaveUserData.userIntro = true;
                Navigation.PushModalAsync(new NavigationPage(new LoginPage()) { BarTextColor = Color.White }, true);
            }
            else
            {
                carousel.Position += 1;
            }
        }
        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            if (e.NewValue + 1 == _introVM.MyItemsSource.Count)
            {
                _introVM.btnText = "Başla";
            }
            else
            {
                _introVM.btnText = "Atla";
            }
        }
    }
}
