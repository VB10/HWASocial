using System;
using BoshokuDemo1.iOS.Dependency;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(WebViewZoomRenderer))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class WebViewZoomRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            var element = (UIWebView)NativeView;
            element.ScalesPageToFit = true;;
            base.OnElementChanged(e);
        }
    }
}
