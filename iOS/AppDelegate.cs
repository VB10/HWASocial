using CarouselView.FormsPlugin.iOS;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Lottie.Forms.iOS.Renderers;
using UIKit;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using HockeyApp.iOS;
using Rg.Plugins.Popup;

namespace BoshokuDemo1.iOS
{
  [Register("AppDelegate")]
  public partial class AppDelegate : XFormsApplicationDelegate
  {
    const string appID = "HOCKEY_APP_ID_HERE";
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      Popup.Init();

      ImageCircleRenderer.Init();
      Devicehelper.iOSDevice = UIDevice.CurrentDevice;
      AnimationViewRenderer.Init();
      CarouselViewRenderer.Init();
      var manager = BITHockeyManager.SharedHockeyManager;
      manager.Configure(appID);
      manager.StartManager();
      manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
      manager.Authenticator.AuthenticateInstallation();

      var container = new SimpleContainer();
      container.Register<IDevice>(t => AppleDevice.CurrentDevice);
      container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
      container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
      Resolver.SetResolver(container.GetResolver());
      FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
      UIRefreshControl.Appearance.TintColor = UIColor.Black;
      global::Xamarin.Forms.Forms.Init();
      LoadApplication(new App());
      return base.FinishedLaunching(app, options);
    }

  }
}
