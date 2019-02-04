using Android.Content;
using BoshokuDemo1.Droid.Dependency;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavPageRenderer))]

namespace BoshokuDemo1.Droid.Dependency
{
  public class NavPageRenderer : NavigationPageRenderer
    {
        public NavPageRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //if (toolbar != null)
            //{
            //    //var textCenter = FindViewById(Resource.Id.textCenter) as TextView;
            //    //textCenter.Text = e.NewElement.Title;
            //    toolbar.Left = 0;
            //}
        }

    }
}
