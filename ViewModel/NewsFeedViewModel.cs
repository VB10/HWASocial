using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Tab;
using FFImageLoading.Forms;
using Firebase.Xamarin.Database;
using Xamarin.Forms;
namespace BoshokuDemo1.ViewModel
{
    public class NewsFeedViewModel : BaseViewModel
    {
        #region singleData
        Search categoryData;
        SqliteManager sqliteManager;
        #endregion


        #region Listeler

        public ObservableCollection<FirebaseObject<Announcements>> new_AnnList;
        ObservableCollection<FirebaseObject<Announcements>> _annList;
        public ObservableCollection<FirebaseObject<Announcements>> AnnList
        {
            get
            {
                return _annList;
            }
            set
            {
                _annList = value;
                OnPropertyChanged();
            }
        }
        public List<Announcements> notifyList
        {
            get;
            set;
        }
        private ObservableCollection<FirebaseObject<Announcements>> _searchList = new ObservableCollection<FirebaseObject<Announcements>>();
        public ObservableCollection<FirebaseObject<Announcements>> SearchList
        {
            get
            {
                return _searchList;
            }
            set
            {
                _searchList = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    SaveBadge();
                    isEmptyList = false;
                    isVisibleIndicator = true;
                    AnnList = await getAnnounData(OrderChild.key.ToString(), "", OrderChild.key, (int)ListCount.mid);
                    newBadgeCount();
                    categoryData = null;
                    isVisibleIndicator = false;
                    newNotifyButtonVisible = false;
                    upIcon_NavBar(false);
                });
            }
        }
        public ICommand showNewNotify
        {
            get
            {
                return new Command(async () =>
                {
                    notifyEnabled = false;
                    //yeni bildirim yoktur anlamı geliyor
                    if (SaveUserData.userBadge != 0)
                    {
                        isVisibleIndicator = true;
                        await checkSqliteState();
                        isVisibleIndicator = false;

                    }

                    var navigationPage = new NotificationPage();
                    if (Device.RuntimePlatform == Device.iOS) navigationPage.Title = "Bildirimler";
                    notifyEnabled = true;
                    await Navigation.PushAsync(navigationPage, true);
                });
            }
        }


        internal void imageSucces(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            var cachedImage = sender as CachedImage;
            var data = cachedImage.BindingContext as FirebaseObject<Announcements>;
            if (e.ImageInformation.CurrentHeight > e.ImageInformation.CurrentWidth)
            {
                AnnList.FirstOrDefault(x => x.Key == data.Key).Object.aspect = Aspect.AspectFit;
            }
        }


        //liste de geçişler sırasındaki sorunu engelleme
        //bool androidAppering;
        async public Task OnAppearing()
        {

            //toolbar içinde title text yapmak 
            //if (Device.RuntimePlatform == Device.Android) DependencyService.Get<IRemoveIcon>().setTitle("Duyurular");

            if (AnnList.Count == 0)
            {
                AnnList = await getAnnounData(OrderChild.key.ToString(), "", OrderChild.key, (int)ListCount.mid);

                //onappering metodu için 
                if (AnnList.Count > 0)
                {
                    newBadgeCount();
                    await checkSqliteState();

                    //userin ilk gördüğ datası (notficationlardan son gelen)
                    SaveUserData.userLastRead = AnnList.First().Key;
                    isVisibleFeedView = true;

                }
                else if (AnnList.Count == 0) isEmptyList = true;


                SaveUserData.userBadge = sqliteManager.ItemCount();
                SaveBadge();
                isPageLoading = false;
                loadingComplate = true;
            }
            else
            {
                SaveBadge();
                //0 gelme urumundaki patlama sorunu fix

            }

        }

        async Task checkSqliteState()
        {
            if (sqliteManager.ItemCount() != SaveUserData.userBadge && SaveUserData.userBadge != 0)
            {
                var list = (await getAnnounData(OrderChild.key.ToString(), "", OrderChild.key, SaveUserData.userBadge)).ToList();
                sqliteManager.Insert(list);
            }
        }

        private void newBadgeCount()
        {
            //buradaki method listeden son okunana göre notification badge ayarlamak için
            try
            {
                var index = AnnList.ToList().FindIndex(x => x.Key == SaveUserData.userLastRead);
                if (index == 0) return;
                SaveUserData.userBadge += index;
                SaveUserData.userLastRead = AnnList.First().Key;
                SaveBadge();
            }
            catch (Exception ex)
            {
                Console.Write("err" + ex.ToString());
            }

        }



        public void OnDisappearing()
        {
            searchBarText = string.Empty;
        }

        public void listView_ItemClicked(SelectedItemChangedEventArgs list)
        {
            //item selected event

            if (list.SelectedItem == null || list == null) return;

            var data = (FirebaseObject<Announcements>)list.SelectedItem;
            if (data.Object.type == null)
            {
                Navigation.PushAsync(new ReadPage(), true);
                MessagingCenter.Send<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.dataAnnouncementsKey.ToString(), data);
            }
            else
            {
                var section = (AnnCategory)Convert.ToInt32(data.Object.type);
                switch (section)
                {
                    case AnnCategory.activities:
                        Navigation.PushAsync(new ReadPage(), true);
                        MessagingCenter.Send<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.dataActivityAnnounKey.ToString(), data);
                        break;
                    case AnnCategory.survey:
                        Navigation.PushModalAsync(new NavigationPage(new SurveyPage()) { BarTextColor = Color.White }, true);
                        MessagingCenter.Send<NewsFeedViewModel, string>(this, "keySurveyAnnc", data.Key);
                        break;
                    case AnnCategory.user_interaction:
                        Navigation.PushAsync(new EventPage(), true);
                        MessagingCenter.Send<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.annUserInteraction.ToString(), data);
                        break;
                    case AnnCategory.multiple_choice:
                        Navigation.PushAsync(new EventPage(), true);
                        MessagingCenter.Send<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.annMultipleChoice.ToString(), data);
                        break;

                    default:
                        break;
                }
            }
            //yan sayfalara geçerken searchbarı temizleme
            searchBarText = String.Empty;
        }
        async void searchBar_TextChanged()
        {
            isEmptyList = false;
            searchNotFound = false;

            if (string.IsNullOrEmpty(searchBarText))
            {
                isVisibleFeedView = true;
                searchListView = false;
                SearchList.Clear();
                //setNavigationBar(true);
            }
            //searchbar daki text 3 den fazla ise armaya başlıyoruz
            else
            {
                //setNavigationBar(false);

                if (searchBarText.Length >= 3)
                {
                    isPageLoading = true;
                    searchListView = true;
                    isVisibleFeedView = false;


                    var lists = await service.GET_list<Announcements>();
                    var result = lists.Where(x => x.Object.title.ToLower().Contains(searchBarText.ToLower())).OrderByDescending(x => x.Object.createdDate).ToList().convertObservable();

                    isPageLoading = false;
                    if (result.Count == 0)
                    {
                        searchNotFound = true;
                        SearchList = result;
                    }
                    else
                    {
                        SearchList = result;
                    }
                }
                else
                {
                    SearchList.Clear();
                }

            }

        }


        //sayfanın navigation bilgisini çekiyoruz
        public NewsFeedViewModel(Page pageNavigation) : base(pageNavigation)
        {
            service = new BoshokuService();
            new_AnnList = new ObservableCollection<FirebaseObject<Announcements>>();
            _annList = new ObservableCollection<FirebaseObject<Announcements>>();
            isEmptyList = false;
            isPageLoading = true;
            subscribeFastMenu();
            subscribeNotification();
            sqliteManager = new SqliteManager();
            notifyList = new List<Announcements>();



        }

        private void subscribeNotification()
        {
            MessagingCenter.Subscribe<App, string>(this, MCenter.dataNotify.ToString(), (arg1, arg2) =>
            {
                if (!string.IsNullOrEmpty(arg2))
                {
                    Navigation.PushAsync(new ReadPage(), true);
                    MessagingCenter.Send<NewsFeedViewModel, string>(this, MCenter.dataNotify.ToString(), arg2);
                }
                MessagingCenter.Unsubscribe<App, string>(this, MCenter.dataNotify.ToString());
            });
            MessagingCenter.Subscribe<App, bool>(this, MCenter.notificationReceived.ToString(), (arg1, key) =>
            {
                notifyList.Add(new Announcements());
                newNotifyButtonText = notifyList.Count + "";
                newNotifyButtonVisible = true;
            });

            MessagingCenter.Subscribe<App, bool>(this, MCenter.onResume.ToString(), (arg1, arg2) =>
            {
                RefreshCommand.Execute(null);
            });


        }

        private void subscribeFastMenu()
        {
            MessagingCenter.Subscribe<DrawerPage, Search>(this, "drawerAnn", async (page, data) =>
            {
                isEmptyList = false;
                isVisibleIndicator = true;
                categoryData = data;

                try
                {
                    await categoryList((int)ListCount.mid);


                }
                catch (Exception ex)
                {
                    isVisibleIndicator = false;
                    isEmptyList = true;
                    upIconVisible = false;
                    AnnList.Clear();
                    Console.WriteLine("err {0}", ex.Message);
                }

            });

        }

        private async Task categoryList(int count)
        {
            var tempList = (await App.hardwareService.GET_list<Announcements>("category", categoryData.categoryKey, OrderChild.startAndEndAt, count)).OrderByDescending(x => x.Object.createdDate).ToList();
            //servisten yeni data gelmemesi durumundaki aksiyon
            //if (tempList.Last().Key == AnnList.Last().Key) return;
            isVisibleIndicator = false;
            if (tempList.Count == 0)
            {
                //eğer arama sonucu data gelmediyse
                isEmptyList = true;
                AnnList.Clear();
            }
            else
            {
                //gelen data -1 ise aylık bir arama olmaması kontrolü
                if (categoryData.monthNumbE != -1)
                {
                    //ocak şubat mart gibi bir arama
                    tempList = tempList.Where(x => x.Object.employeesOfTheMonthDate != null && Convert.ToInt32(x.Object.employeesOfTheMonthDate.Split('/')[0]) == categoryData.monthNumbS + 1).ToList();
                    if (tempList.Count == 0)
                    {
                        //gelen liste sonucunda o ay yoksa içinde 0 döndür
                        isEmptyList = true;
                        AnnList.Clear();
                        return;
                    }
                }
                var convertList = new ObservableCollection<FirebaseObject<Announcements>>();
                foreach (var item in tempList)
                {
                    convertList.Add(correct_Item(item));
                }
                AnnList = convertList;

            }


        }


        //page  listeleme

        async public Task ListViewItem_ApperingCommand(FirebaseObject<Announcements> item)
        {
           
            var itemIndex = AnnList.IndexOf(item);

            if (AnnList.Count - 1 == itemIndex)
            {
               

                isPageLoading = true;
                //eğer o anki liste categorik bir arama içinden gelmişse ona göre altına elamanları getirme
                if (categoryData != null)
                {
                    await categoryList(AnnList.Count + (int)ListCount.mid);
                }
                else
                {
                    var newData = await getAnnounData(OrderChild.key.ToString(), AnnList.Last().Key, OrderChild.keyAndEndAt, (int)ListCount.mid);

                    foreach (var items in newData)
                    {
                        if (!AnnList.Any(x => x.Key == items.Key))
                        {
                            AnnList.Add(items);
                        }
                    }

                }

                isPageLoading = false;
            }
       

            if (itemIndex >= 4)
            {
                upIcon_NavBar(true);
            }

            else
            {
                upIcon_NavBar(false);
            }

            //listede up iconu gösteriken count tercihi




        }

        void upIcon_NavBar(bool res)
        {
            upIconVisible = res;
            //setNavigationBar(!res);
        }

        //ListView scrool fonksiyon
        public void scroolListView(ListView listView)
        {
            listView.ScrollTo(AnnList.First(), ScrollToPosition.Start, true);
            upIconVisible = false;


        }
        public async Task<ObservableCollection<FirebaseObject<Announcements>>> getAnnounData(string order, string val, OrderChild param, int count)
        {
            try
            {

                var collection = new ObservableCollection<FirebaseObject<Announcements>>();
                var data_list = (await service.GET_list<Announcements>(order, val, param, count));

                var convertList = new ObservableCollection<FirebaseObject<Announcements>>();
                foreach (var item in data_list)
                {
                    convertList.Add(correct_Item(item));
                }
                return convertList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AnnMVVM error" + ex.Message);
                return AnnList;
            }

        }

        private FirebaseObject<Announcements> correct_Item(FirebaseObject<Announcements> item)
        {
            item.Object.category = getCategory(LocalCategory.getListMain(), item.Object.category);
            //item.Object.dateCreator = item.Object.createdDate.ToShortDateString();
            item.Object.newsCreateDate = getCreatedDate(item.Object.createdDate);
            //item.Object.widthx = (Resolver.Resolve<IDevice>()).Display.Width;
            //item.Object.desc = item.Object.content.HtmlToPlainText();
            item.Object.imgVisible = true;
            item.Object.image = string.IsNullOrEmpty(item.Object.image) ? "promotion" : item.Object.image;
            return item;




        }
        private Announcements correct_Item(Announcements item)
        {
            item.category = getCategory(LocalCategory.getListMain(), item.category);
            item.newsCreateDate = getCreatedDate(item.createdDate);
            return item;
        }
        private string getCreatedDate(DateTime createdDate)
        {
            int year = DateTime.Now.Year - createdDate.Year;
            if (year >= 1) return year + " yıl önce";
            //yıl
            int month = DateTime.Now.Month - createdDate.Month;
            if (month >= 1) return month + " ay önce";

            int day = DateTime.Now.Day - createdDate.Day;
            if (day != 0) return day > 7 ? (day / 7) + " hafta önce" : (day) + " gün önce";

            int hour = DateTime.Now.Hour - createdDate.Hour;
            if (hour > 0) return hour + " saat önce";
            int minute = DateTime.Now.Minute - createdDate.Minute;
            if (minute > 0) return minute + " dakika önce";
            else return "Şuan";

        }

        public string getCategory(List<LocalCategory> list, string key)
        {
            try
            {
                var category = "";
                foreach (var item in list)
                {
                    if (item.key == key) return item.val;
                    else if (item.isSub == true)
                    {
                        foreach (var subitem in item.sub)
                        {
                            if (subitem.key == key) return subitem.val;
                        }
                    }

                }

                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Category hatası:  msg" + ex.ToString());
                return string.Empty;

            }

        }






        #region boolDatalar
        //listview pull to refresh
        bool _loadingComplate = false;

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
        //sayfa boş iken gelen
        bool _isEmptyList = false;

        public bool isEmptyList
        {
            get
            {
                return _isEmptyList;
            }

            set
            {
                _isEmptyList = value;
                OnPropertyChanged();
            }
        }


        //listview yeni page geçiş
        bool _isPageLoading = false;

        public bool isPageLoading
        {
            get
            {
                return _isPageLoading;
            }

            set
            {
                _isPageLoading = value;
                OnPropertyChanged();
            }
        }
        //yeni bilidirim gelince uygulamadayken üste bildirim butonu
        bool _newNotifyButtonVisible = false;

        public bool newNotifyButtonVisible
        {
            get
            {
                return _newNotifyButtonVisible;
            }

            set
            {
                _newNotifyButtonVisible = value;
                OnPropertyChanged();
            }
        }
        //yen bildirim count
        string _newNotifyButtonText;

        public string newNotifyButtonText
        {
            get
            {
                return _newNotifyButtonText + " Yeni Haber";
            }

            set
            {
                _newNotifyButtonText = value;
                OnPropertyChanged();
            }
        }

        //search Text
        string _searchBarText = String.Empty;

        public string searchBarText
        {
            get
            {
                return _searchBarText;
            }

            set
            {
                _searchBarText = value;
                OnPropertyChanged("searchBarText");
                searchBar_TextChanged();

            }
        }
        //listview search
        bool _searchListView = false;

        public bool searchListView
        {
            get
            {
                return _searchListView;
            }

            set
            {
                _searchListView = value;
                OnPropertyChanged();
            }
        }

        //giriş 
        bool _isVisibleIndicator = false;

        public bool isVisibleIndicator
        {
            get
            {
                return _isVisibleIndicator;
            }

            set
            {
                _isVisibleIndicator = value;
                OnPropertyChanged();
            }
        }
        //arama bulunamadı uyarsı
        bool _searchNotFound = false;

        public bool searchNotFound
        {
            get
            {
                return _searchNotFound;
            }

            set
            {
                _searchNotFound = value;
                OnPropertyChanged();
            }
        }


        //yukarı çıkma butonu görünür kılma
        bool _upIconVisible = false;

        public bool upIconVisible
        {
            get
            {
                return _upIconVisible;
            }

            set
            {
                _upIconVisible = value;
                OnPropertyChanged();
            }
        }
        bool _isVisibleFeedView = false;

        //search yaparken arkadaki listview görünmez hale getirme
        public bool isVisibleFeedView
        {
            get
            {
                return _isVisibleFeedView;
            }

            set
            {
                _isVisibleFeedView = value;
                OnPropertyChanged();
            }
        }
        bool _notifyEnabled = true;

        //notification ciftTıklamafix
        public bool notifyEnabled
        {
            get
            {
                return _notifyEnabled;
            }

            set
            {
                _notifyEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}