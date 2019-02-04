using BoshokuDemo1.Views.Tab;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.DrawerMenu
{
    public class MasterControlMenu : MasterDetailPage
    {


		public  MasterControlMenu()
        {
            var master  = new DrawerPage();
            var detail = new TabControl();
            Icon = "";
            master.Icon = "";
            NavigationPage.SetTitleIcon(this, null);
            Master = master;
            Detail = detail;
            
            //android master icon remove does not support 
            IsGestureEnabled = Device.RuntimePlatform == Device.Android ? true: false;
            MasterBehavior = MasterBehavior.SplitOnPortrait;

            MessagingCenter.Subscribe<DrawerPage, bool>(this, "change", (page, data) =>
            {
                this.IsPresented = data;
            });
            MessagingCenter.Subscribe<AnnounPage, bool>(this, "change", (page, data) =>
            {
                this.IsPresented = data;
            });
            MessagingCenter.Subscribe<
                           TabControl, bool>(this, "change", (page, data) =>
            {

                this.IsPresented = !this.IsPresented;
               

            });
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

        }




    }
}

