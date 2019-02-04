using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Login;
using Com.OneSignal;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class NewUserMVVM : BaseViewModel
    {
        Random random;
        string _rePassword;


        List<string> _securityList;

        public List<string> securityList
        {
            get
            {
                return _securityList;
            }

            set
            {
                _securityList = value;
                OnPropertyChanged();
            }
        }

        public ICommand saveUserResponse
        {
            get
            {
                return new Command(async () =>
                {
                    if (check())
                    {
                        await startAnimation();
                        var result = await App.hardwareService.POST_updateUser(userSecurity.newPassword.Trim());

                        if (result)
                        {
                            SaveUserData.userPassword = userSecurity.newPassword.Base64Encode();
                            var user = new Users();
                            user.fullName = string.IsNullOrEmpty(SaveUserData.userName) ? "HardwareAndro" : SaveUserData.userName;
                            userSecurity.newPassword = userSecurity.newPassword.Trim().Base64Encode();
                            userSecurity.securityResponse = userSecurity.securityResponse.Trim().ToLower();
                            user.security = userSecurity;
                            user.password = SaveUserData.userPassword;
                            user.regNumber = SaveUserData.userRegNumber;

                            await App.hardwareService.PUT_object<Users>(user, SaveUserData.userKey, SaveUserData.userToken);
                            await Navigation.PushModalAsync(new MasterControlMenu(), true);
                            OneSignal.Current.SetSubscription(true);
                            await stopAnimation();


                        }
                        else
                        {
                            await stopAnimation();

                            await ToastMessage(Show.LONG, "Bir sorun bulundu.Tekrar giriş yapın.", AnimationType.False);
                            await Navigation.PushModalAsync(new LoginPage(), true);

                        }
                    }

                });
            }
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
        public NewUserMVVM(Page page) : base(page)
        {
            random = new Random();
            userSecurity = new UserSecurity();

        }

        private bool check()
        {
            string errText = "";
            bool err = true;
            if (string.IsNullOrEmpty(userSecurity.newPassword))
            {
                errText = "Yeni şifre girmelisiniz";
                err = false;
            }
            else if (string.IsNullOrEmpty(userSecurity.secuirtyQuestion))
            {
                errText = "Güvenlik sorusu seçmelisiniz.";

                err = false;
            }
            else if (string.IsNullOrEmpty(userSecurity.securityResponse))
            {
                errText = "Güvenlik cevabı girmelisiniz.";
                err = false;
            }
            else if (string.IsNullOrEmpty(rePassword))
            {
                errText = "Yeni şifre tekrar girmelisiniz.";
                err = false;
            }
            else if (userSecurity.newPassword != rePassword)
            {
                errText = "Yeni şifreniz tekrarı ile uyuşmuyor.";
                err = false;
            }

            if (err) return true;
            else
            {
                DisplayError(errText);
                return false;
            }

        }

        public string rePassword
        {
            get
            {
                return _rePassword;
            }

            set
            {
                _rePassword = value;
                OnPropertyChanged();
            }
        }
        string _selectedPicker;

        public string selectedPicker
        {
            get
            {
                return _selectedPicker;
            }

            set
            {
                _selectedPicker = value;
                OnPropertyChanged();
            }
        }
        UserSecurity _userSecurity;

        public UserSecurity userSecurity
        {
            get
            {
                return _userSecurity;
            }

            set
            {
                _userSecurity = value;
                OnPropertyChanged();
            }
        }


        public async Task onAppering()
        {
            var list = await App.hardwareService.GET_ListObject<SecurityQuestions>();
            //picker için listeyi stringe convert ediyoruz
            securityList = list[random.Next(list.Count)].questions.ConvertAll(obj => obj.q);

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

    }
}
