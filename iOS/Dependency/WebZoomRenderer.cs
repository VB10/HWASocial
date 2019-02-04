using System;
using BoshokuDemo1.iOS.Dependency;
using BoshokuDemo1.Views.Components;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly : ExportRenderer(typeof(CustomWebView),typeof(WebZoomRenderer))]

namespace BoshokuDemo1.iOS.Dependency
{
    public class WebZoomRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            var element = (UIWebView)NativeView;
            element.ScalesPageToFit = true;
            base.OnElementChanged(e);
        }
    }
}
