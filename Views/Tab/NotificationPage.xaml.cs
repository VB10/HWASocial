using System;
using System.Collections.Generic;
using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class NotificationPage : ContentPage
    {
        NotifyViewModel notifyViewModel;
        public NotificationPage()
        {
            InitializeComponent();
            BindingContext = notifyViewModel = new NotifyViewModel(this);
            //android icon kaldırma
            if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().removeIcon(false,"Bildirimler");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            notifyViewModel.onAppearing();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            notifyViewModel.listView_ItemClicked(e);
        }
    }
}
