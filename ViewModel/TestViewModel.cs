using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class TestViewModel : BaseViewModel
    {
        string _name;

        public string name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public TestViewModel(Page currentPage) : base(currentPage)
        {
        }
        public ICommand testCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await startAnimation();
                });
            }
        }
    }
}
