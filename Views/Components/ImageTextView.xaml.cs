using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Components
{
    public partial class ImageTextView : ContentView
    {
        const float TextOffset = 1.15f;

        public ImageTextView()
        {
            InitializeComponent();
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.Margin = new Thickness(5, 0, 0, 0);
                    break;
                case Device.Android:
                    this.Margin = new Thickness(5, 10, 0, 0);
                    break;
            }
        }

        void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;

            var canvas = surface.Canvas;

            canvas.Clear();

            var circleFill = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = GetColorForText()
            };

            canvas.DrawCircle((float)Height, (float)Height, (float)Height, circleFill);

            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColors.White,
                TextSize = (float)Height / TextOffset,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName(FontFamily)
            };

            canvas.DrawText(Text, (float)Height, (float)Height * TextOffset, textPaint);
        }

        public SKColor GetColorForText()
        {
            //TODO: determine color based on Text property
            return SKColors.Orange;
        }

        #region Bindable properties

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create("Text", typeof(string), typeof(ImageTextView), string.Empty,
                                    BindingMode.OneWay, null, (bindable, oldValue, newValue) =>
                                    {
                                        var view = (bindable as ImageTextView);
                                        view.ForceLayout();
                                    });



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }



        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create("FontFamily", typeof(string), typeof(ImageTextView), string.Empty,
                                    BindingMode.OneWay, null, (bindable, oldValue, newValue) =>
                                    {
                                        var view = (bindable as ImageTextView);
                                        view.ForceLayout();
                                    });

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        #endregion


    }
}
