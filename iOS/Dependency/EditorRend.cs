using System;
using BoshokuDemo1.iOS.Dependency;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(EditorRend))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class EditorRend : EditorRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.Layer.BorderWidth = new nfloat(0.5);
            Control.Layer.BorderColor = UIColor.LightGray.CGColor;
        }
    }
}
