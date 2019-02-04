using BoshokuDemo1.iOS.Dependency;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRend))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class NavigationPageRend : NavigationRenderer
    {
        public NavigationPageRend()
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationBar.TintColor = UIColor.White;
            NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            NavigationBar.Translucent = true;
            //NavigationBar.TopItem.Title =
            var label = new UILabel();
            //get the default title
            label.Text = "HardwareAndro";
           
            label.Font = UIFont.PreferredSubheadline;
            label.TextAlignment = UITextAlignment.Left;
            label.TextColor = UIColor.White;
            label.SizeToFit();
            NavigationBar.TopItem.LeftBarButtonItem = new UIBarButtonItem(label);
            //NavigationItem.LeftBarButtonItem.Title = "sadasdasd";
            ////empty the default title (try with empty string or null if empty doesnt work)
            //NavigationController.NavigationBar.TopItem.Title = "";
            //NavigationController.NavigationBar.TopItem.LeftBarButtonItem = new UIBarButtonItem(label);

        }
         

    }
}
