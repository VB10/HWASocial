
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace BoshokuDemo1.Droid
{
    [Activity(Label = "TB Social", LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/logo", Theme = "@style/Theme.Splash", MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartUp();
        }
        async void StartUp()
        {
            await Task.Delay(300);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
