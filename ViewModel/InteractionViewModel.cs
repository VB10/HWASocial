using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using Firebase.Xamarin.Database;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class InteractionViewModel : BaseViewModel
    {
        Competitions _competition;
        Stream imgByteData;

        string eventKey;
        public Competitions competition
        {
            get
            {
                return _competition;
            }

            set
            {
                _competition = value;
                OnPropertyChanged();
            }
        }
        InteractionResponse _response;

        public InteractionResponse response
        {
            get
            {
                return _response;
            }

            set
            {
                _response = value;
                OnPropertyChanged();
            }
        }
        Image _uploadPhoto;
        public Image uploadPhoto
        {
            get
            {
                return _uploadPhoto;
            }

            set
            {
                _uploadPhoto = value;
                OnPropertyChanged();
            }
        }

        public ICommand sendRequest
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await startAnimation();
                        if (checkUI())
                        {
                            await currentPage.DisplayAlert("Hata", "Tüm alanları eksiksiz doldurun", "Tamam");
                            await stopAnimation();
                            return;
                        }
                        //rresmi post edip gelen url basıyoruz
                        var key = await service.saveUserReply<InteractionResponse>(new List<InteractionResponse> { response }, eventKey, "competitions");
                        response.image = await service.POST_image(typeof(Competitions).Name.ToLower(), key, imgByteData);

                        await stopAnimation();
                        await ToastMessage(Show.SHORT, "Başarılı", AnimationType.Success);
                        await Navigation.PopAsync();
                        MessagingCenter.Send<InteractionViewModel, string>(this, MCenter.deleteSurveyKey.ToString(), eventKey);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error {0}", ex.Message);
                    }



                });
            }
        }

        bool checkUI()
        {
            if (string.IsNullOrEmpty(response.text)) return true;
            else if (imgByteData == null) return true;
            return false;
        }
        public ICommand takePhoto
        {
            get
            {
                return new Command(async () =>
                {
                    await CrossMedia.Current.Initialize();

                    var chosee = await currentPage.DisplayActionSheet("Seçim yapınız", "Kapat", "Fotoğraf çek", "Galeriden seç");
                    if (chosee == "Fotoğraf çek")
                    {
                        try
                        {
                            var img = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                            {
                                AllowCropping = true,
                                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                                CompressionQuality = 50
                            });
                            imgByteData = img.GetStream();
                            uploadPhoto.Source = ImageSource.FromStream(() =>
                            {
                                return img.GetStream();
                            });

                        }
                        catch (Exception ex)
                        {
                            await currentPage.DisplayAlert("Hata", "Cihazınızla ilgili bir sorunla karşılaşıldı.Lütfen hatayı bize bildirin", "Tamam");
                            Debug.WriteLine(ex.Message);
                            return;
                        }

                    }
                    else if (chosee == "Galeriden seç")
                    {
                        var img = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                        {
                            CompressionQuality = 50
                        });
                        if (img == null)
                        {
                            await currentPage.DisplayAlert("Hatırlatma", "Herhangi bir fotoğraf seçmediniz.", "Tamam");
                            return;
                        }
                        imgByteData = img.GetStream();
                        uploadPhoto.Source = ImageSource.FromStream(() =>
                        {
                            return img.GetStream();
                        });
                    }
                });
            }
        }

        public InteractionViewModel(Page page) : base(page)
        {
            competition = new Competitions();
            response = new InteractionResponse();
            uploadPhoto = new Image();
            service = new BoshokuService();
            MessagingCenter.Subscribe<EventViewModel, FirebaseObject<Competitions>>(this, MCenter.competitionToInteractionKey.ToString(), (arg1, item) =>
            {
                MessagingCenter.Unsubscribe<EventViewModel, FirebaseObject<Competitions>>(this, MCenter.competitionToInteractionKey.ToString());
                uploadPhoto.Source = "ic_add_icon";
                eventKey = item.Key;
                competition = item.Object;

            });
            MessagingCenter.Subscribe<EventViewModel, Competitions>(this, MCenter.eventToIntereact.ToString(), (arg1, item) =>
            {
                MessagingCenter.Unsubscribe<EventViewModel, Competitions>(this, MCenter.eventToIntereact.ToString());
                uploadPhoto.Source = "ic_add_icon";
                eventKey = item.key;
                competition = item;

            });

            MessagingCenter.Subscribe<EventViewModel, FirebaseObject<Announcements>>(this, MCenter.announToInteractionKey.ToString(), (arg1, item) =>
            {
                MessagingCenter.Unsubscribe<EventViewModel, FirebaseObject<Announcements>>(this, MCenter.announToInteractionKey.ToString());
                uploadPhoto.Source = "ic_add_icon";
                eventKey = item.Key;
                var trash_competition = new Competitions()
                {
                    compName = item.Object.title,
                    compDesc = item.Object.desc,
                    createdDate = item.Object.createdDate,
                    image = item.Object.image
                };
                competition = trash_competition;

            });

        }




    }
}

