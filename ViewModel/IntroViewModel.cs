using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
	public class IntroViewModel : INotifyPropertyChanged
    {
        string _btnText = "Atla";

        public string btnText
        {
            get
            {
                return _btnText.ToUpper();
            }

            set
            {
                _btnText = value;
                OnPropertyChanged("btnText");
            }
        }

        ObservableCollection<View> _myItemsSource;
        public ObservableCollection<View> MyItemsSource
        {
            set
            {
                _myItemsSource = value;
                OnPropertyChanged();
            }
            get
            {
                return _myItemsSource;
            }
        }
        public IntroViewModel()
        {
            MyItemsSource = new ObservableCollection<View>()
            {
				new CachedImage() { Source = "intro1",  Aspect = Aspect.AspectFill },
				new CachedImage() { Source = "intro2", Aspect = Aspect.AspectFill },
				new CachedImage() { Source = "intro3", Aspect = Aspect.AspectFill },
				new CachedImage() { Source = "intro4",  Aspect = Aspect.AspectFill }
			};

            //MyCommand = new Command(() =>
            //{
            //    Debug.WriteLine("Position selected.");
            //});
        }
        public Command MyCommand { protected set; get; }


        #region Değişiklikleri yakalama
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}

