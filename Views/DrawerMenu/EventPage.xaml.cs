using System;
using System.Collections.Generic;
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.DrawerMenu
{
    public partial class EventPage : ContentPage
    {
        EventViewModel eventViewModel;
        public EventPage()
        {
            InitializeComponent();
            BindingContext = eventViewModel = new EventViewModel(this);
            //NavigationPage.SetTitleIcon(this, "navIcon");
            Title = "Yarışma";

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await eventViewModel.onAppering();
        }


        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await eventViewModel.SelectedItem(e);
            ((ListView)sender).SelectedItem = null;

        }
    }
}
