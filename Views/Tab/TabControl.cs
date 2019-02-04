using System;
using BoshokuDemo1.Helper;
using BoshokuDemo1.ViewModel;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Login;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace BoshokuDemo1.Views.Tab
{
    public class TabControl : Xamarin.Forms.TabbedPage
    {

        Page pageRead = new ReadPage();
        public TabControl()
        {
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarSelectedItemColor(Color.Red);
            On<Xamarin.Forms.PlatformConfiguration.Android>().EnableSwipePaging();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsLegacyColorModeEnabled(true);




            var pageAnnoun = new NavigationPage(new AnnounPage())
            {
                Icon = "tab_news",
                Title = "Duyurular"
            };
            var pageActivitys = new NavigationPage(new ActivitysPage())
            {
                Icon = "tab_origami",
                Title = "Etkinlikler"
            };
            var pageData = new NavigationPage(new UserDataPage())
            {
                Icon = "tab_write",
                Title = "Bize Bildir"
            };

            var pageMaster = new NavigationPage(new Page())
            {
                Icon = "tab_right_align",
                Title = "Hızlı Menu",
            };
            var pageSettings = new NavigationPage(new UserSettingPage())
            {
                Icon = "tab_profil",
                Title = "Profil"
            };
            pageAnnoun.BarTextColor = Color.White;
            pageSettings.BarTextColor = Color.White;
            pageActivitys.BarTextColor = Color.White;
            pageData.BarTextColor = Color.White;

            Children.Add(pageAnnoun);
            Children.Add(pageActivitys);
            //Children.Add(pageTest);
            Children.Add(pageData);
            Children.Add(pageSettings);
            Children.Add(pageMaster);
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);



            this.BarBackgroundColor = Color.White;
            MessagingCenter.Subscribe<DrawerPage, CurrentTabPageChange>(this, "changeCurrentPage", (arg1, _enum) =>
            {

                switch (_enum)
                {
                    case CurrentTabPageChange.pageActivity:
                        if (CurrentPage != pageActivitys)
                        {
                            this.CurrentPage = pageActivitys;
                        }
                        break;
                    case CurrentTabPageChange.pageAnnoun:

                        if (CurrentPage != pageAnnoun) this.CurrentPage = pageAnnoun;
                        else
                        {
                            if (CurrentPage.Navigation.NavigationStack.Count > 1)
                            {
                                CurrentPage.Navigation.PopAsync(true);
                            }
                        }
                        break;
                }
                MessagingCenter.Unsubscribe<TabControl,
                bool>(this, "changeCurrentPage");
            });

            this.CurrentPageChanged += (object sender, System.EventArgs e) =>
            {

                if (this.CurrentPage == pageMaster)
                {
                    this.CurrentPage = pageAnnoun;
                    MessagingCenter.Send<TabControl, bool>(this, "change", true);
                    this.CurrentPage.InputTransparent = false;
                }
            };
        }



        public void sendReset(string title)
        {

            if (this.CurrentPage.Title == title && title == Children[0].Title)
            {
                MessagingCenter.Send<TabControl, bool>(this, MCenter.tabListResetKey.ToString(), true);
            }

        }

        protected override bool OnBackButtonPressed()
        {
            var count = this.CurrentPage.Navigation.NavigationStack.Count;
            //MARK Android read page back add.
            if (count == 2)
            {
                return base.OnBackButtonPressed();

            }

            return true;
        }





    }


}