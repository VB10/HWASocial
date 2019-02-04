using System;
using Android.Content;
using BoshokuDemo1.Droid.Dependency;
using BoshokuDemo1.Views.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly : ExportRenderer(typeof(WebViewZoom),typeof(CustomWebView))]
namespace BoshokuDemo1.Droid.Dependency
{
    public class WebViewZoom : WebViewRenderer
    {
        public WebViewZoom(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {

                //Control.ZoomBy(0.2f);
                Control.Settings.UseWideViewPort = true;
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = true;
                Control.Settings.DomStorageEnabled = true;
                Control.Settings.JavaScriptEnabled = true;
                Control.ScrollbarFadingEnabled = true;
                Control.VerticalScrollBarEnabled = true;
                Control.SetInitialScale(1);




            }

        }
    }
}
