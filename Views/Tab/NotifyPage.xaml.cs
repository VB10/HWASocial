using System;
using System.Collections.Generic;
using System.Linq;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using BoshokuDemo1.Views.Read;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class NotifyPage : ContentPage
    {
        AnnouncemenentsViewModel announMvvm = new AnnouncemenentsViewModel();


        protected async override  void OnAppearingAsync()
        {
                base.OnAppearing();
              await announMvvm.getAnnounData();
        }


        public NotifyPage()
        {
            InitializeComponent();

         
           
            _ls.BindingContext = announMvvm.AnnList;
        }


        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

            _ls.BindingContext = announMvvm.searchText(e.NewTextValue);
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            var choose = await DisplayActionSheet("Kategorik Sırala", "Kapat", "", CategoryHelper.CateogryList.Select(item => {
                return item.title;
            }).ToArray());
            if (choose == "Kapat" || choose == "") return;
            else if (choose == "Genel") _ls.BindingContext = announMvvm.AnnList;
            else _ls.BindingContext = announMvvm.searchCategory(choose);


        }

        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            announMvvm.pullRefresh();
            _ls.EndRefresh();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (_ls.SelectedItem != null)
            {
                var data = (Announcements)e.SelectedItem;

                MessagingCenter.Send(data, "xas");

                Navigation.PushAsync(new ReadAnnouncPage(),true);

                _ls.SelectedItem = null;
            }


        }



    }
}
