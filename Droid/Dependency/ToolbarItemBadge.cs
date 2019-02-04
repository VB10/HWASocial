using System;
using BoshokuDemo1.Droid.Dependency;
using BoshokuDemo1.Model;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Support.V7.Widget;
using Android.App;

[assembly: Dependency(typeof(ToolbarItemBadge))]
namespace BoshokuDemo1.Droid.Dependency
{
    public class ToolbarItemBadge : IToolbarItemBadge
    {
        //badge count ayarları
        public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var toolbar = CrossCurrentActivity.Current.Activity.FindViewById(Resource.Id.toolbar) as Android.Support.V7.Widget.Toolbar;
                if (toolbar != null)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var id = page.ToolbarItems.IndexOf(item);
                        if (toolbar.Menu.Size() > id)
                        {
                            var menuItem = toolbar.Menu.GetItem(id);
                            BadgeDrawable.SetBadgeText(CrossCurrentActivity.Current.Activity, menuItem, value, backgroundColor.ToAndroid(), textColor.ToAndroid());
                        }
                    }
                }
            });
            
        }
    }
}
