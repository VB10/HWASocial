using SlideOverKit;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class TestPage2 : ContentPage
    {
        public TestPage2()
        {
            InitializeComponent();
            //this.Slide

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Navigation.PushModalAsync(new DrawerPage(), true);

        }
        void Handle_Pressed(object sender, System.EventArgs e)
        {
            //Navigation.PushAsync(new PasswordForgotPage(), true);
            MessagingCenter.Send<TestPage2, bool>(this, "change", true);

        }
    }
}
