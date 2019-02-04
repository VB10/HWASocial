using System;
using Android.Content;
using BoshokuDemo1.Droid.Dependency;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(SearchBar), typeof(SearchBarRend))]
namespace BoshokuDemo1.Droid.Dependency
{
    public class SearchBarRend : SearchBarRenderer
    {
        public SearchBarRend(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            Control.QueryTextChange += Control_QueryTextChange;
        }
        void Control_QueryTextChange(object sender, Android.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewText))
            {
                Control.ClearFocus();
            }
        }
    }
}
