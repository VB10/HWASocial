using System;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using Firebase.Xamarin.Database;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
  public class ReadViewModel : BaseViewModel
    {

   

        public ReadViewModel(Page page) : base(page)
        {

            //newfeed den gelme durumu
            MessagingCenter.Subscribe<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.dataAnnouncementsKey.ToString(), async (Page, data) =>
            {
                await getWebSource(data.Key, data.Object, false);
                MessagingCenter.Unsubscribe<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.dataAnnouncementsKey.ToString());
            });
            //notify den gelme durumu
            MessagingCenter.Subscribe<NotifyViewModel, Announcements>(this, "dataAnnouncements", async (Page, data) =>
             {
                 await getWebSource(data.key, data, false);
                 MessagingCenter.Unsubscribe<NotifyViewModel, Announcements>(this, "dataAnnouncements");
             });
            //push notification da gelme durumu
            MessagingCenter.Subscribe<NewsFeedViewModel, string>(this, "dataAnnouncementsNotify", async (Page, key) =>
            {
                var contentBody = await App.hardwareService.GET_object<Announcements>(key, typeof(Announcements).Name.ToLower());
                await getWebSource(key, contentBody, false);
                MessagingCenter.Unsubscribe<NewsFeedViewModel, string>(this, "dataAnnouncementsNotify");
            });

            //newsfeed actviiy durumu
            MessagingCenter.Subscribe<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.dataActivityAnnounKey.ToString(), async (Page, data) =>
            {
                await getWebSource(data.Key, new Announcements
                {
                    image = data.Object.image,
                    title = data.Object.title,
                    createdDate = data.Object.createdDate

                }, true);
                MessagingCenter.Unsubscribe<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.dataActivityAnnounKey.ToString());
            });
            //notify de etkinlik iteme yönlendirme durumu
            MessagingCenter.Subscribe<NotifyViewModel, Announcements>(this, "dataActivity_announ", async (Page, data) =>
            {
                await getWebSource(data.key, new Announcements
                {
                    image = data.image,
                    title = data.title,
                    createdDate = data.createdDate

                }, true);
                MessagingCenter.Unsubscribe<NotifyViewModel, Announcements>(this, "dataActivity_announ");
            });
            //push  notification
            MessagingCenter.Subscribe<ActivitysViewModel, string>(this, "dataActivity_View", async (Page, key) =>
            {
                var contentBody = await App.hardwareService.GET_object<Activities>(key, typeof(Activities).Name.ToLower());
                await getWebSource(key, new Announcements
                {
                    image = contentBody.image,
                    title = contentBody.title,
                    createdDate = contentBody.createdDate
                }, true);
                MessagingCenter.Unsubscribe<ActivitysViewModel, string>(this, "dataActivity_View");
            });
        }


        async Task getWebSource(string key, Announcements model, bool isActivity)
        {
            //activity ve newsfeed ayırma
            var contentData = new SimpleNews();
            if (isActivity)
            {
                contentData = await App.hardwareService.GET_object<SimpleNews>(key, typeof(Activities).Name.ToLower());
            }
            else
            {
                contentData = await App.hardwareService.GET_object<SimpleNews>(key, typeof(Announcements).Name.ToLower());
            }
            try
            {
                webViewSource = convertToHtml(model.image, model.title, contentData.content, model.createdDate.ToShortDateString());
                webViewVisible = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("html parse error" + ex.Message);
            }
            //tempLoading = false;
            animationVisible = false;
        }

        public HtmlWebViewSource convertToHtml(string image, string title, string data, string date)
        {
            //boş resim gelme durumu fix
            image = image.Contains("http") ? image : string.Empty;
            //web servis ten gelen datayı html web source çeviriyoruz data editörden geliyor
            return new HtmlWebViewSource
            {
                Html = String.Format("<html>" +
                                     "<head>" +
                                     " <meta name ='viewport' content = 'width=device-width, initial-scale=1, maximum-scale=5' > " +
                                     "<link rel='stylesheet' href='https://use.fontawesome.com/releases/v5.0.13/css/all.css' integrity='sha384-DNOHZ68U8hZfKXOrtjWvjxusGo9WQnrNx2sqG0tfsghAvtVlRW3tvkXWZh58N9jp' crossorigin='anonymous'>" +
                                     "</head>" +
                                     "<body bgcolor='#ECF0F1'>" +
                                     "<img src='{0}' style =\"width: 100%;height: auto;\"></img>" +
                                     "<h3 style='text-align:center;font-size:20;color:#E74C3C'>{1}</h3>  " +
                                     "<h5 style='text-align: right;color:#2C3E50'><i class='far fa-calendar-alt'> {3}</i> </h5>" +
                                     "<hr> " +
                                     "<p>{2}</p>" +
                                     "</body>" +
                                     "</html>", image, title, data, date)

            };

        }

        #region bool değişkenler
        bool _animationVisible = true;
        HtmlWebViewSource _webViewSource;
        bool _webViewVisible = false;

        public bool animationVisible
        {
            get => _animationVisible;
            set
            {
                _animationVisible = value;
                OnPropertyChanged();
            }
        }
        public HtmlWebViewSource webViewSource
        {
            get => _webViewSource;
            set
            {
                _webViewSource = value;
                OnPropertyChanged();
            }
        }
        public bool webViewVisible
        {
            get => _webViewVisible;
            set
            {
                _webViewVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion


    }
}
