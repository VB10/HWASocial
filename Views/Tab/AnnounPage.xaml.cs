using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using Firebase.Xamarin.Database;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class AnnounPage : ContentPage
    {
        NewsFeedViewModel newsFeedVM;
        async void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            await newsFeedVM.ListViewItem_ApperingCommand((FirebaseObject<Announcements>)e.Item);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            newsFeedVM.OnDisappearing();
            
		}
        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await newsFeedVM.OnAppearing();
            //if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(true, "");

        }
        public AnnounPage()
        {
            InitializeComponent();
            BindingContext = newsFeedVM = new NewsFeedViewModel(this);
            //this.SlideMenu = new DrawerPage();
            Title = "Duyurular";

            MessagingCenter.Subscribe<TabControl, bool>(this, MCenter.tabListResetKey.ToString(), (arg1, arg2) =>
            {

                newsFeedVM.scroolListView(listViewFeed);

            });
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            newsFeedVM.listView_ItemClicked(e);

        }

   


        void Handle_Activated(object sender, System.EventArgs e)
        {
            newsFeedVM.scroolListView(listViewFeed);
        }

      

        void Handle_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            newsFeedVM.imageSucces(sender, e);
        }

    }
}