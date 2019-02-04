using System;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Views.Login;
using Com.OneSignal;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
  public class LoginViewModel : BaseViewModel
  {

    Users _user;

    public Users user
    {
      get
      {

        return _user;
      }

      set
      {
        _user = value;
        OnPropertyChanged();
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

    public ICommand checkBoxCommand
    {
      get
      {
        return new Command(() =>
        {
          checkBoxBool = !checkBoxBool;
          //passwor görünür kılma engelleme
          if (!checkBoxBool) checkBoxIcon = "ic_check_false";
          else checkBoxIcon = "ic_check_true";

        });
      }
    }

    public ICommand passwordResetCommand
    {
      get
      {
        return new Command(() =>
        {
          Navigation.PushAsync(new PasswordForgotPage(), true);
        });
      }
    }

    public ICommand licenseCommand
    {
      get
      {
        return new Command(async () =>
        {
          //passwor görünür kılma engelleme
          await Navigation.PushAsync(new ReadUserLicance(), false);
        });
      }
    }

    public ICommand loginCommand
    {
      get
      {
        return new Command(async () =>
        {
          loadingComplate = true;
          buttonEnabled = false;

          try
          {
            if (checkBoxBool)
            {
              App.authUser = await App.authProvider.SignInWithEmailAndPasswordAsync(user.regNumber.ToLower().Trim() + "@hardwareandro.com", user.password);
              SaveUserData.userRegNumber = user.regNumber.ToLower().Trim() + "@hardwareandro.com";
              SaveUserData.userPassword = user.password.Base64Encode();
              SaveUserData.userKey = App.authUser.User.LocalId.ToString();
              SaveUserData.refreshToken = App.authUser.RefreshToken;
              SaveUserData.userToken = App.authUser.FirebaseToken.ToString();
              App.authUser.ExpiresIn *= 100;

              try
              {
                Users userDetails = await service.GET_object<Users>(App.authUser.User.LocalId);
                if (string.IsNullOrEmpty(userDetails.fullName))
                {
                  //eğer yeni gelen ve  ismi girilmemiş ise default name atama
                  SaveUserData.userName = "HardwareAndro";
                }
                if (userDetails.security.newPassword != null)
                {
                  //securtiy boş ise demekki kurtarma girmemiş demektir diyip devam ediyoruz.
                  //SaveUserData.userToken = App.authUser.FirebaseToken;
                }
              }
              catch (Exception ex)
              {



                await Navigation.PushModalAsync(new NavigationPage(new NewUserPage()), true);
                Console.WriteLine("error" + ex.ToString());
                return;
              }

              //first login user



              OneSignal.Current.SetSubscription(true);

              await Navigation.PushModalAsync(new NavigationPage(new AppControlPage()), true);

            }
            else
            {
              await startErrorAnimation("Kullanıcı sözleşmesini onaylamalısınız.");
            }

          }
          catch (Exception ex)
          {
            error();
            Console.WriteLine("User Err" + ex.Message);

          }


          //await getUsers();

          loadingComplate = false;
          buttonEnabled = true;
        });
      }
    }
    public void isCheckBoxChanged(bool changed)
    {
      checkBoxBool = changed;
    }

    public LoginViewModel(Page page) : base(page)
    {
      user = new Users();
    }

    private void error()
    {
      errorText = "Kullanıcı adı veya şifre hatalı";
      Device.StartTimer(TimeSpan.FromSeconds(2), () =>
      {

        errorText = string.Empty;
        return false;
      });

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

    string _errorText;

    public string errorText
    {
      get
      {
        return _errorText;
      }

      set
      {
        _errorText = value;
        OnPropertyChanged();
      }
    }
    string _checkBoxIcon = "ic_check_false";

    public string checkBoxIcon
    {
      get
      {
        return _checkBoxIcon;
      }

      set
      {
        _checkBoxIcon = value;
        OnPropertyChanged();
      }
    }

    bool _checkBoxBool;

    public bool checkBoxBool
    {
      get
      {

        return _checkBoxBool;
      }

      set
      {
        _checkBoxBool = value;
        OnPropertyChanged();
      }
    }

    #region boolDatalar


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

    bool _loadingComplate = false;

    //listview pull to refresh
    public bool loadingComplate
    {
      get
      {
        return _loadingComplate;
      }

      set
      {
        _loadingComplate = value;
        OnPropertyChanged();
      }
    }

    bool _buttonEnabled = true;

    //listview pull to refresh
    public bool buttonEnabled
    {
      get
      {
        return _buttonEnabled;
      }

      set
      {
        _buttonEnabled = value;
        OnPropertyChanged();
      }
    }

    #endregion


  }
}

