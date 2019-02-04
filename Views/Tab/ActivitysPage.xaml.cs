using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class ActivitysPage : ContentPage
    {
        ActivitysViewModel activityVM;
        public ActivitysPage()
        {
            InitializeComponent();
            BindingContext = activityVM = new ActivitysViewModel(this);
            //NavigationPage.SetTitleIcon(this, "navIcon");
            Title = "Etkinlikler";


        }
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            _lst.SelectedItem = null;
            activityVM.Handle_ItemSelected(e);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await activityVM.OnAppearing();
        }

    }
}
