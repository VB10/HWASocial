using Android.Content;
using BoshokuDemo1.Droid.Dependency;
using BoshokuDemo1.Views.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoneEntry), typeof(EntRenderer))]
namespace BoshokuDemo1.Droid.Dependency
{
  public class EntRenderer : EntryRenderer
  {
    public EntRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      base.OnElementChanged(e);
      if (e.OldElement == null)
      {
        var nativeEditText = (global::Android.Widget.EditText)Control;
        nativeEditText.SetBackgroundColor(Android.Graphics.Color.Transparent);
        nativeEditText.TextAlignment = Android.Views.TextAlignment.TextStart;
        nativeEditText.SetPadding(0, 5, 0, 0);

      }
    }

  }
}
