using System;
using Android.App;
using Android.Views;

namespace BoshokuDemo1.Droid
{
    public static class StatusBarHelper
    {
        public static View DecorView
        {
            get;
            set;
        }
        public static ActionBar AppActionBar
        {
            get;
            set;
        }
    }
}
