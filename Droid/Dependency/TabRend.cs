using Android.Content;
using Android.Support.Design.Widget;
using BoshokuDemo1.Droid.Dependency;
using BoshokuDemo1.Views.Tab;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabControl), typeof(TabRend))]

namespace BoshokuDemo1.Droid.Dependency
{
    public class TabRend : TabbedPageRenderer , TabLayout.IOnTabSelectedListener
    {
     
        public TabRend(Context context) : base(context)
        {
        }
        void TabLayout.IOnTabSelectedListener.OnTabReselected(TabLayout.Tab tab)
        {
            //TODO FIX IT  next release
            var mainTabPage = Element as TabControl;
            mainTabPage.sendReset(tab.Text);
        }
    
}
}
