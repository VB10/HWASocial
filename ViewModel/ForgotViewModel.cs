using System;
using System.Collections.Generic;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class ForgotViewModel : BaseViewModel
    {
        public List<Forgot> forgotList;
        //ilk seviye sicil numarası ikinci seviye hatırlatma
        public int level
        {
            get;
            set;
        }
        public Users currentUser
        {
            get;
            set;
        }
        bool _errorBool = false;

        internal void TextChanged(string newTextValue)
        {
            if (string.IsNullOrEmpty(newTextValue)) errorBool = true;
            else errorBool = false;
        }

        public bool errorBool
        {
            get
            {
                return _errorBool;
            }

            set
            {
                _errorBool = value;
                OnPropertyChanged();
            }
        }

        Forgot _forgotModel;

        public Forgot forgotModel
        {
            get
            {
                return _forgotModel;
            }

            set
            {
                _forgotModel = value;
                OnPropertyChanged();
            }
        }
        string key;

        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    switch (level)
                    {
                        case 0:
                            if (string.IsNullOrEmpty(forgotModel.entryText))
                            {
                                errorBool = true;
                                return;
                            }
                            else errorBool = false;


                            var tempUser = await authService.SignInAnonymouslyAsync();

                            key = tempUser.FirebaseToken;

                            await ToastMessage("Kontrol ediliyor");

                            var data = await service.GET_object_AUTH<Users>(forgotModel.entryText.Trim() + "@hardwareandro.com", key);
                            //işlem sonunda temp user delete
                            await service.DELETE_USER(key);

                            await stopAnimation();



                            if (data == null || data.fullName == null)
                            {
                                var result = await currentPage.DisplayAlert("Hata", "Kullanıcı veya güvenlik cevabı bulunamadı", "Kapat", "Geri Dön");
                                switch (result)
                                {
                                    case true:
                                        return;
                                    case false:
                                        await Navigation.PopAsync(true);
                                        break;
                                    default:
                                        break;
                                }
                                return;
                            }
                            else
                            {

                                currentUser = data;


                                level++;
                                forgotList[level].labelText = currentUser.security.secuirtyQuestion;
                                forgotModel = forgotList[level];

                            }




                            break;
                        case 1:

                            if (forgotModel.entryText.Trim().ToLower() == currentUser.security.securityResponse.ToLower())
                            {
                                await ToastMessage(Show.SHORT, "Başarılı", AnimationType.Success);
                                level++;
                                forgotList[level].entryText = currentUser.security.newPassword.Base64Decode();
                                forgotModel = forgotList[level];

                            }
                            else
                            {
                                var result = await currentPage.DisplayAlert("Hata", "Cevabınız Hatalı.", "Kapat", "Tekrar Dene");
                                switch (result)
                                {
                                    case true:
                                        return;
                                    case false:
                                        await Navigation.PopAsync(true);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case 2:
                            await Navigation.PopAsync(true);
                            break;

                        default:
                            break;
                    }
                });
            }
        }

        public ForgotViewModel(Page page) : base(page)
        {
            level = 0;
            currentUser = new Users();
            forgotList = new List<Forgot>(){
                new Forgot{labelText="Kullanıcı Adı ",entryText = string.Empty,buttonText = "İleri"},
                new Forgot{labelText="",entryText = string.Empty,buttonText = "Cevapla"},
                new Forgot{labelText="Şifreniz : ",entryText = string.Empty,buttonText = "Geri Dön"}
            };
            forgotModel = new Forgot();
            forgotModel = forgotList[0];
            currentPage.Disappearing += onDisappearing;


        }

        async void onDisappearing(object sender, EventArgs e)
        {
            await service.DELETE_USER(key);

        }


    }
}
