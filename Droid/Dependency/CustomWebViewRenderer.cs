using System;
using Android.Content;
using BoshokuDemo1.Droid.Dependency;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace BoshokuDemo1.Droid.Dependency
{

    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                //Control.SetInitialScale(1);
                //Control.Settings.UseWideViewPort = true;
                //Control.Settings.LoadWithOverviewMode = true;
                Control.Settings.LoadsImagesAutomatically = true;
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = true;
                Control.Settings.DomStorageEnabled = true;
                Control.Settings.JavaScriptEnabled = true;
                //Control.ScrollbarFadingEnabled = true;
                //Control.VerticalScrollBarEnabled = true;
            }

        }
    }
}
