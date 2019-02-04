using System;
using BoshokuDemo1.iOS.Dependency;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(PageRend), typeof(ContentPage))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class PageRend : PageRenderer
    {
        public PageRend()
        {
            this.View.LayoutMargins = new UIKit.UIEdgeInsets(20f, 0f, 0f, 0f);

            
        }
    }
}
