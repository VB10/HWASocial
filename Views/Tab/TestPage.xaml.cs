using System;
using System.Collections.Generic;
using BoshokuDemo1.ViewModel;
using Lottie.Forms;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class TestPage : ContentPage
    {
        //TestViewModel testViewModel;
        public TestPage()
        {
            InitializeComponent();
            //BindingContext = testViewModel = new TestViewModel(this);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        //{
        //    if (e.NewTextValue == null)
        //    {

        //    }
        //}

        //void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        //{
        //    if (e == null)
        //    {

        //    }
        //}

        //void Handle_OnClick(object sender, System.EventArgs e)
        //{
        //    //var anim = sender as AnimationView;
        //    //if (!anim.IsPlaying)
        //    //{
        //    //    anim.Play();


        //    //}
        //    //anim.IsPlaying = !anim.IsPlaying;

        //    Navigation.PopAsync(true);
           


        //}

        //void Handle_OnFinish(object sender, System.EventArgs e)
        //{
        //    var anim = sender as AnimationView;

        //    if (!anim.IsPlaying)
        //    {
        //        anim.Animation = anim.Animation;

        //    }

        //}

        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}
