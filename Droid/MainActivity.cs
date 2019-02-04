using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;

namespace BoshokuDemo1.Droid
{
  [Activity(Label = "HWA Social", LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/logo", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const string appID = "HOCKEY_APP_ID_HERE";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            MetricsManager.Register(Application, appID);


            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            StatusBarHelper.DecorView = this.Window.DecorView;
            ImageCircleRenderer.Init();
            CarouselViewRenderer.Init();
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            FFImageLoading.Forms.Droid.CachedImageRenderer.Init(true);

            LoadApplication(new App());


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnResume()
        {
            base.OnResume();
            CrashManager.Register(this, appID, new MyCrashManagerListener());
        }

    }
}
public class MyCrashManagerListener : CrashManagerListener
{

    public override bool ShouldAutoUploadCrashes()
    {
        return true;
    }
}
