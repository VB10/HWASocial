using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using BoshokuDemo1.Views.Popup;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class UserRequestMVVM : BaseViewModel
    {

        //display alert işlemleri için
        Stream imgSource;
        public List<department> listDepartments;
        public List<string> pickerData
        {
            get
            {
                return listDepartments.Select(x => x.name).ToList();
            }
        }
        Image _infoPhoto = new Image() { Source = "ic_add_icon" };
        public Image infoPhoto
        {
            get
            {
                return _infoPhoto;
            }

            set
            {
                _infoPhoto = value;
                OnPropertyChanged();
            }
        }
        public ICommand sendRequest
        {
            get
            {
                return new Command(async () =>
                {
                    if (ControlText())
                    {
                        var request = new FeedBacks()
                        {
                            desc = descText,
                            title = titleText,
                            department = pickerSelected,
                            createdDate = DateTime.Now
                        };


                        await ToastMessage(Show.LOOP,"Gönderiliyor..",AnimationType.Default);
                        try
                        {
                            await SaveUserRequest(request, imgSource);
                            await currentPage.DisplayAlert("Başarılı", "İsteğiniz gönderildi.", "Tamam");
                            ClearAll();
                        }
                        catch (Exception ex)
                        {
                            await currentPage.DisplayAlert("Hata", "Bir hatayla karşılaşıldı", "Tamam");
                            Debug.WriteLine("User data error" + ex.Message);
                        }
                        await stopAnimation();
                    }
                    else
                    {
                        await currentPage.DisplayAlert("Hata", "Lütfen Tüm alanları eksiksiz doldurun", "Tamam");
                    }
                });
            }
        }

        public ICommand takePhoto
        {
            get
            {
                return new Command(async () =>
                {
                    await CrossMedia.Current.Initialize();

                    var chosee = await currentPage.DisplayActionSheet("Seçim yapınız", "Kapat", "Fotoğraf çek", "Galeriden seç");
                    switch (chosee)
                    {
                        case "Fotoğraf çek":
                            try
                            {
                                var _img = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                                {
                                    AllowCropping = true,
                                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                                    CompressionQuality = 50
                                });
                                imgSource = _img.GetStream();
                                infoPhoto.Source = ImageSource.FromStream(() =>
                                {
                                    return _img.GetStream();
                                });

                            }
                            catch (Exception ex)
                            {
                                await currentPage.DisplayAlert("Hata", "Cihazınızla ilgili bir sorunla karşılaşıldı.Lütfen hatayı bize bildirin", "Tamam");
                                Debug.WriteLine(ex.Message);
                                return;
                            }
                            break;

                        case "Galeriden seç":

                            var img = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                            {
                                CompressionQuality = 50
                            });
                            if (img == null)
                            {
                                await currentPage.DisplayAlert("Hatırlatma", "Herhangi bir fotoğraf seçmediniz.", "Tamam");
                                return;
                            }
                            imgSource = img.GetStream();
                            infoPhoto.Source = ImageSource.FromStream(() =>
                            {
                                return img.GetStream();
                            });
                            break;

                        default:
                            return;
                    }

                });
            }
        }

        public UserRequestMVVM(Page page) : base(page)
        {
            service = new BoshokuService();
            listDepartments = new List<department>(){new department{key="-L3-jRbXhj7lglCULUc0",name="İnsan Kaynakları"},
                new department{key="-L3-jUMxK107f7s0amiH",name="İş Sağlığı ve Güvenliği"}
            };

        }
        public async Task<bool> SaveUserRequest(FeedBacks req, Stream imgStream)
        {
            try
            {
                req.department = listDepartments.Find(x => x.name == req.department).key;
                //önce bir datayı kayıt ediyoruz 
                var postData = await service.POST_object(req);
                //var postData = App.fireClient.Child("feedbacks").PostAsync(req);
                if (imgStream != null)
                {
                    //yolladığımız datan gelen key id ile bizim resmimizin adını key adı vererek yolluyoruz
                    //arka tarafta imgmızda artık hazır şimdi son haliyle put edip güncelliyourz               
                    req.image = await service.POST_image(typeof(FeedBacks).Name.ToLower(), postData.Key, imgStream);
                    await service.PUT_object(req, postData.Key);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UserRequest Error" + ex.Message);
                return false;
            }
        }

        bool ControlText()
        {
            //text control
            if (string.IsNullOrEmpty(_pickerSelected)) return false;
            else if (string.IsNullOrEmpty(descText)) return false;
            else if (string.IsNullOrEmpty(titleText)) return false;
            else return true;
        }
        private void ClearAll()
        {
            descText = string.Empty;
            titleText = string.Empty;
            infoPhoto.Source = "ic_add_icon";
            pickerSelected = string.Empty;
        }

        //sadece ilk girildğinde gözükecek kısım
        bool firstPlay;
        public void OnAppearing()
        {
            if (firstPlay)
            {
                firstPlay = !firstPlay;
                currentPage.DisplayAlert("Bilgilendirme", "Şirket içinde tespit ettiğiniz uygunsuzluk ya da sorunları bu başlık altında bildirebilirsiniz.", "Tamam");
            }
        }
        string _descText;

        public string descText
        {
            get
            {
                return _descText;
            }

            set
            {
                _descText = value;
                OnPropertyChanged();
            }
        }

        string _titleText;

        public string titleText
        {
            get
            {
                return _titleText;
            }

            set
            {
                _titleText = value;
                OnPropertyChanged();
            }
        }

        string _departmantText;

        public string departmantText
        {
            get
            {
                return _departmantText;
            }

            set
            {
                _departmantText = value;
                OnPropertyChanged();
            }
        }
        string _pickerSelected;

        public string pickerSelected
        {
            get
            {
                return _pickerSelected;
            }

            set
            {
                _pickerSelected = value;
                OnPropertyChanged();
            }
        }

    }
    public class department
    {

        public string key
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
    }
}