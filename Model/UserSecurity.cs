using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BoshokuDemo1.Model
{
    public class UserSecurity : INotifyPropertyChanged
    {

        string _secuirtyQuestion;

        public string secuirtyQuestion
        {
            get
            {
                return _secuirtyQuestion;
            }

            set
            {
                _secuirtyQuestion = value;
                OnPropertyChanged();
            }
        }
        string _securityResponse;

        public string securityResponse
        {
            get
            {
                return _securityResponse;
            }

            set
            {
                _securityResponse = value;
                OnPropertyChanged();
            }
        }
        string _newPassword;

        public string newPassword
        {
            get
            {
                return _newPassword;
            }

            set
            {
                _newPassword = value;
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
