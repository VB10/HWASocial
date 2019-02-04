using BoshokuDemo1.iOS.Dependency;
using BoshokuDemo1.Model;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: Dependency(typeof(ToolbarItemBadge))]
namespace BoshokuDemo1.iOS.Dependency
{
  public class ToolbarItemBadge : IToolbarItemBadge
    {
        public ToolbarItemBadge()
        {
        }

        public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{});
                var renderer = Platform.GetRenderer(page);
                if (renderer == null)
                {
                    renderer = Platform.CreateRenderer(page);
                    Platform.SetRenderer(page, renderer);
                }
                var vc = renderer.ViewController;
                var rightButtonITems = vc?.ParentViewController?.NavigationItem?.RightBarButtonItems;
                var idx = page.ToolbarItems.IndexOf(item);
                if (rightButtonITems != null && rightButtonITems.Length > idx) 
                {
                    var barItem = rightButtonITems[idx];
                    if (barItem != null) 
                    {
                        barItem.UpdateBadge(value, backgroundColor.ToUIColor(), textColor.ToUIColor());
                    }
                }
            
        }
    }
}
