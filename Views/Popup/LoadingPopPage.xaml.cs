using BoshokuDemo1.Helper;
using BoshokuDemo1.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Popup
{
    public partial class LoadingPopPage : PopupPage
    {
        public LoadingPopPage()
        {
            InitializeComponent();
            this.IsEnabled = false;

            MessagingCenter.Subscribe<BaseViewModel, string>(this, MCenter.toastKey.ToString(), (page, data) =>
            {
                //ekrana kalma süresi
                _loadLabel.Text = data;
                MessagingCenter.Unsubscribe<BaseViewModel, string>(this, MCenter.toastKey.ToString());
            });
        }
    }
}
