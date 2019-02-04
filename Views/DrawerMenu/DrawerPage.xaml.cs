using System;
using System.Collections.Generic;
using BoshokuDemo1.Model;
using Xamarin.Forms;
using System.Linq;
using BoshokuDemo1.Views.Tab;
using BoshokuDemo1.Helper;

namespace BoshokuDemo1.Views.DrawerMenu
{
    public partial class DrawerPage : ContentPage
    {

        List<LocalCategory> categoryList = new List<LocalCategory>();
        List<List<LocalCategory>> newlistTRY = new List<List<LocalCategory>>();

        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            _lw.IsRefreshing = true;
            _lw.BindingContext = LocalCategory.getListMain();
            _backButton.IsVisible = false;
            _lw.IsRefreshing = false;
            nextCount = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        public DrawerPage()
        {
            InitializeComponent();
            Title = "required";
            Icon = "";
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            NavigationPage.SetHasBackButton(this, false);
            categoryList = LocalCategory.getListMain();
            _lw.BindingContext = categoryList;

        }


        //TODO MVVM düzelt bunlar sil

        string temp_Category;
        string temp_CategoryKey;
        int nextCount = 0;

        //BACK
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            nextCount--;
            if (nextCount <= 0)
            {
                _backButton.IsVisible = false;
                _lw.BindingContext = categoryList;
                nextCount = 0;
                temp_Category = "";
                newlistTRY.Clear();
            }
            else
            {
                _lw.BindingContext = newlistTRY[nextCount];

            }
        }
        //NEXT
        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (_lw.SelectedItem != null)
            {

                nextCount++;
                var item = (LocalCategory)e.SelectedItem;

                if (item.isSub)
                {
                    _backButton.IsVisible = true;
                    temp_Category = item.val;
                    temp_CategoryKey = item.key;
                    _lw.BindingContext = item.sub;
                    newlistTRY.Insert(0, item.sub);
                }
                else
                {
                    _backButton.IsVisible = false;
                    if (item.key == null)
                    {
                        searchOther(item);
                    }
                    //karakter keyleri spesific normalde dbden gelen keyler YA ve H localde web ve kategorik cliclkleri ayırt etmek için
                    else if (item.key == "-123YA")
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new SurveyPage()) { BarTextColor = Color.White, Title = "Anket Soruları" }, true);
                    }
                    else if (item.key == "-123YE")
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new EventPage()) { BarTextColor = Color.White }, true);
                    }
                    else if (item.key.Contains("-123H"))
                    {

                        var navigationPage = new TBTWebPage();
                        navigationPage.Title = item.val;
                        await this.Navigation.PushModalAsync(new NavigationPage(navigationPage) { BarTextColor = Color.White }, true);
                        if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(true, item.val);

                        MessagingCenter.Send<DrawerPage, string>(this, "webPage", item.key);
                    }
                    else
                    {
                        searchOther(item);

                    }

                    MessagingCenter.Send<DrawerPage, bool>(this, "change", false);
                    //masteri kapatmak için
                    _lw.BeginRefresh();
                    _lw.BindingContext = categoryList;
                    nextCount = 0;
                }

                _lw.SelectedItem = null;

            }


        }

        private void searchOther(LocalCategory item)
        {
            //eğer asıl sayfada değilse oraya yönlendirmek için

            switch (item.key)
            {
                case "-AC123":
                    MessagingCenter.Send<DrawerPage, bool>(this, "drawerActivity", true);
                    MessagingCenter.Send<DrawerPage, CurrentTabPageChange>(this, "changeCurrentPage", CurrentTabPageChange.pageActivity);
                    break;
                case "-AC124":
                    MessagingCenter.Send<DrawerPage, bool>(this, "drawerActivity", false);
                    MessagingCenter.Send<DrawerPage, CurrentTabPageChange>(this, "changeCurrentPage", CurrentTabPageChange.pageActivity);
                    break;
                default:
                    if (!string.IsNullOrEmpty(item.key))
                    {
                        MessagingCenter.Send<DrawerPage, Search>(this, "drawerAnn", new Search() { categoryKey = item.key, monthNumbE = -1 });
                        MessagingCenter.Send<DrawerPage, CurrentTabPageChange>(this, "changeCurrentPage", CurrentTabPageChange.pageAnnoun);
                    }
                    else
                    {
                        var search = new Search();
                        search.monthNumbS = Enum.GetNames(typeof(Months)).ToList().IndexOf(item.val);
                        search.categoryKey = temp_CategoryKey;
                        search.category = temp_Category;
                        MessagingCenter.Send<DrawerPage, Search>(this, "drawerAnn", search);
                        MessagingCenter.Send<DrawerPage, CurrentTabPageChange>(this, "changeCurrentPage", CurrentTabPageChange.pageAnnoun);
                    }
                    break;
            }
            temp_Category = "";
            temp_CategoryKey = "";

        }

    }
}