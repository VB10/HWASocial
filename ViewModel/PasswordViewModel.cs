using System;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Views.Popup;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class PasswordViewModel : BaseViewModel
    {
        public PasswordViewModel(Page page) : base(page)
        {
        }

        public ICommand changeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (checkInputs())
                    {
                        await Navigation.PushPopupAsync(new LoadingPopPage(), true);
                        var result = await App.hardwareService.POST_updateUser(newPassword);
                        await Navigation.PopPopupAsync();
                        if (result)
                        {
                            SaveUserData.userPassword = newPassword.Base64Encode();
                            DisplaySuccess("Şifreniz değiştirildi");
                            clearInputs();
                            await Navigation.PopAsync();

                        }
                        else
                        {
                            DisplayError("Bir hata ile karşılaşıldı");
                        }
                    }
                });
            }
        }

        bool checkInputs()
        {
            if (string.IsNullOrEmpty(oldPassword))
            {
                DisplayError("Eski şifreniz boş geçilemez.");
                return false;
            }
            else if (string.IsNullOrEmpty(newPassword))
            {
                DisplayError("Yeni şifreniz boş geçilemez.");
                return false;
            }
            else if (string.IsNullOrEmpty(reNewPassword))
            {
                DisplayError("Yeni şifrenizin tekrarı boş geçilemez.");
                return false;
            }
            else if (oldPassword != SaveUserData.userPassword.Base64Decode())
            {
                DisplayError("Eski şifrenizi hatalı girdiniz.");
                return false;
            }
            else if (newPassword != reNewPassword)
            {
                DisplayError("Yeni şifre ve tekrarı uyuşmuyor.");
                return false;
            }
            else if (newPassword.Length < 6)
            {
                DisplayError("Yeni şifreniz 6 haneden küçük olamaz.");
                return false;
            }
            else return true;

        }
        void clearInputs()
        {
            newPassword = string.Empty;
            reNewPassword = string.Empty;
            oldPassword = string.Empty;

        }


        public ICommand visibleCommand
        {
            get
            {
                return new Command(() =>
                {
                    //passwor görünür kılma engelleme
                    if (isPassword) eyeIcon = "ic_eye_hide";
                    else eyeIcon = "ic_eye_visible";
                    isPassword = !isPassword;

                });
            }
        }
        public ICommand visibleCommandRe
        {
            get
            {
                return new Command(() =>
                {
                    //passwor görünür kılma engelleme
                    if (isPasswordRe) eyeIconRe = "ic_eye_hide";
                    else eyeIconRe = "ic_eye_visible";
                    isPasswordRe = !isPasswordRe;

                });
            }
        }
        public ICommand visibleCommandOld
        {
            get
            {
                return new Command(() =>
                {
                    //passwor görünür kılma engelleme
                    if (isPasswordOld) eyeIconOld = "ic_eye_hide";
                    else eyeIconOld = "ic_eye_visible";
                    isPasswordOld = !isPasswordOld;

                });
            }
        }
        string _oldPassword;

        public string oldPassword
        {
            get
            {
                return _oldPassword;
            }

            set
            {
                _oldPassword = value;
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
        string _reNewPassword;

        public string reNewPassword
        {
            get
            {
                return _reNewPassword;
            }

            set
            {
                _reNewPassword = value;
                OnPropertyChanged();
            }
        }

        bool _isPasswordRe = true;
        public bool isPasswordRe
        {
            get
            {
                return _isPasswordRe;
            }

            set
            {
                _isPasswordRe = value;
                OnPropertyChanged();
            }
        }

        bool _isPasswordOld = true;
        public bool isPasswordOld
        {
            get
            {
                return _isPasswordOld;
            }

            set
            {
                _isPasswordOld = value;
                OnPropertyChanged();
            }
        }

        bool _isPassword = true;
        public bool isPassword
        {
            get
            {
                return _isPassword;
            }

            set
            {
                _isPassword = value;
                OnPropertyChanged();
            }
        }
        string _eyeIcon = "ic_eye_visible";

        public string eyeIcon
        {
            get
            {
                return _eyeIcon;
            }

            set
            {
                _eyeIcon = value;
                OnPropertyChanged();
            }
        }
        string _eyeIconRe = "ic_eye_visible";

        public string eyeIconRe
        {
            get
            {
                return _eyeIconRe;
            }

            set
            {
                _eyeIconRe = value;
                OnPropertyChanged();
            }
        }
        string _eyeIconOld = "ic_eye_visible";

        public string eyeIconOld
        {
            get
            {
                return _eyeIconOld;
            }

            set
            {
                _eyeIconOld = value;
                OnPropertyChanged();
            }
        }
    }
}
