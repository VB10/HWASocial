using System;
using BoshokuDemo1.iOS.Dependency;
using BoshokuDemo1.Views.Login;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(NoneEntry), typeof(EntRenderer))]

namespace BoshokuDemo1.iOS.Dependency
{
    public class EntRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
			//var border = new CALayer();
   //         nfloat width = 2;
   //         border.BorderColor = UIColor.Black.CGColor;
			//border.Frame = new CoreGraphics.CGRect(0, Control.Frame.Size.Height - width, Control.Frame.Size.Width, Control.Frame.Size.Height);
   //         border.BorderWidth = width;
			//Control.Layer.AddSublayer(border);
			//Control.Layer.MasksToBounds = true;
			Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}
