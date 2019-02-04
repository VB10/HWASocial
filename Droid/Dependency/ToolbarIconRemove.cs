using Android.Widget;
using BoshokuDemo1.Droid.Dependency;
using BoshokuDemo1.Model;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToolbarIconRemove))]
namespace BoshokuDemo1.Droid.Dependency
{
    public class ToolbarIconRemove : IRemoveIcon
    {

        private static Android.Support.V7.Widget.Toolbar GetToolbar() => (CrossCurrentActivity.Current?.Activity as MainActivity)?.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

        public void removeIcon(bool visible, string data)
        {


            var toolbar = CrossCurrentActivity.Current.Activity.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {

                toolbar.NavigationIcon = null;
                toolbar.ContentInsetStartWithNavigation = 0;
                toolbar.ContentInsetEndWithActions = 0;

                //toolbar.Title = "TB Social";  //Activity Property
                //toolbar.TitleMarginStart = 0;
                //toolbar.Subtitle = toolbar.Title;

            }


        }

    }

}
