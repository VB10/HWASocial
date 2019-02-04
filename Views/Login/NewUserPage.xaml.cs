using System;
using System.Collections.Generic;
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Login
{
    public partial class NewUserPage : ContentPage
    {
        NewUserMVVM newUserMVVM;
        public NewUserPage()
        {
            InitializeComponent();
            BindingContext = newUserMVVM = new NewUserMVVM(this);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await newUserMVVM.onAppering();
        }
    }
}
