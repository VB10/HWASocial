using System;

using Xamarin.Forms;

namespace BoshokuDemo1.Views.Components
{
    public class CustomSearchBar : SearchBar
    {
        public CustomSearchBar()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.BackgroundColor = Color.Transparent;
                    break;
                case Device.Android:
                    this.BackgroundColor = Color.White;
                    this.HeightRequest = 40;
                    this.Margin = new Thickness(10, 5, 10, 0);
                    break;
            }        
        }
    }
}

