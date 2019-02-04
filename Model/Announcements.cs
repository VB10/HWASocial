using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BoshokuDemo1.Model
{
    public class Announcements : INotifyPropertyChanged
    {

        public string dateCreator
        {
            get;
            set;
        }
        public string key
        {
            get;
            set;
        }
        public bool imgVisible
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }

        public string category
        {
            get;
            set;
        }
        public DateTime createdDate
        {
            get;
            set;
        }
        public string desc
        {
            get;
            set;
        }
        public string categoryKey
        {
            get;
            set;
        }
        public string image
        {
            get;
            set;
        }
        public string title
        {
            get;
            set;
        }
        public string employeesOfTheMonthDate
        {
            get;
            set;
        }
        public string newsCreateDate
        {
            get;
            set;
        }
        Aspect _aspect = Aspect.Fill;

        public Aspect aspect
        {
            get
            {
                return _aspect;
            }

            set
            {
                _aspect = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}