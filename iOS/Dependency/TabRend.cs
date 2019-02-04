using BoshokuDemo1.iOS.Dependency;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using BoshokuDemo1.Views.Tab;

[assembly: ExportRenderer(typeof(TabControl), typeof(TabRend))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class TabRend : TabbedRenderer
    {
        public TabRend()
        {
        }

        public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        {
            //send data tabcontrol
            var mainTabPage = Element as TabControl;
            mainTabPage.sendReset(item.Title);
        }
    
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);


            //NativeView.AddGestureRecognizer(new UISwipeGestureRecognizer(() => SelectNextTab(1)) { Direction = UISwipeGestureRecognizerDirection.Left, ShouldRecognizeSimultaneously = ShouldRecognizeSimultaneously });
            //NativeView.AddGestureRecognizer(new UISwipeGestureRecognizer(() => SelectNextTab(-1)) { Direction = UISwipeGestureRecognizerDirection.Right, ShouldRecognizeSimultaneously = ShouldRecognizeSimultaneously });
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            UITextAttributes normalTextAttributes = new UITextAttributes();
            normalTextAttributes.TextColor = UIColor.Red;
            UITabBarItem.Appearance.SetTitleTextAttributes(normalTextAttributes, UIControlState.Selected);
            UITabBar.Appearance.SelectedImageTintColor = UIColor.Red;
        }

        void SelectNextTab(int direction)
        {
            int nextIndex = TabbedPage.GetIndex(Tabbed.CurrentPage) + direction;
            if (nextIndex < 0 || nextIndex >= Tabbed.Children.Count) return;
            var nextPage = Tabbed.Children[nextIndex];
            Tabbed.CurrentPage = nextPage;
        }

        static bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer) => gestureRecognizer != otherGestureRecognizer;
    }
}
