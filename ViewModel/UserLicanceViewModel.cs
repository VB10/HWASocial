using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class UserLicanceViewModel : BaseViewModel
    {
        public UserLicanceViewModel(Page page) : base(page)
        {
        }
        string _licanceUrl;

        public string licanceUrl
        {
            get
            {
                return _licanceUrl;
            }

            set
            {
                _licanceUrl = value;
                OnPropertyChanged();
            }
        }

        async public Task onAppering()
        {
            licanceUrl = "https://drive.google.com/file/d/1hOvRjFhKRQ3py2BMMNUWAliHTR24RFeu/view?usp=sharing";
            await startAnimation();
        }

        async public Task onFinish()
        {
            await stopAnimation();
        }
    }
}
