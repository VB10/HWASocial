using BoshokuDemo1.iOS.Dependency;
using BoshokuDemo1.Views.Components;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(SearchBarRend))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class SearchBarRend : SearchBarRenderer
    {
        public SearchBarRend()
        {
        }
		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
            base.OnElementChanged(e);
            Control.TextChanged+= Control_TextChanged;
           
        }

        void Control_TextChanged(object sender, UIKit.UISearchBarTextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.SearchText))
            {
                Control_ShouldBeginEditing(this.Control);
            }
        }

        private void Control_ShouldBeginEditing(UISearchBar control)
        {//keyboard dismiss
            control.EndEditing(true);
        }
	}
}
