using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using Xamarin.Forms;
using BoshokuDemo1.Helper;
using Firebase.Xamarin.Database;
using System.Collections.Generic;
using BoshokuDemo1.Views.Tab;
using System.Windows.Input;

namespace BoshokuDemo1.ViewModel
{
    public class NotifyViewModel : INotifyPropertyChanged
    {
        Page page;
        BoshokuService service;
        ObservableCollection<Announcements> _newsList = new ObservableCollection<Announcements>();
        List<LocalCategory> categoryList;
        SqliteManager sqliteManager;

        public ObservableCollection<Announcements> newsList
        {
            get
            {
                return _newsList;
            }

            set
            {
                _newsList = value;
                OnPropertyChanged();
            }
        }

        public ICommand cleanList
        {
            get
            {
                return new Command(() =>
                {


                    sqliteManager.DeleteData();
                    newsList.Clear();
                    //user badge sıfırlama işlemi
                    if (SaveUserData.userBadge != 0)
                    {
                        SaveUserData.userBadge = 0;
                        var announPage = page.Navigation.NavigationStack.First();
                        DependencyService.Get<IToolbarItemBadge>().SetBadge(announPage, announPage.ToolbarItems.First(), $"" + SaveUserData.userBadge, Color.Red, Color.White);
                    }

                    isEmptyList = true;
                });
            }
        }

        public NotifyViewModel(Page page)
        {
            this.page = page;
            service = new BoshokuService();
            categoryList = LocalCategory.getListMain();
            sqliteManager = new SqliteManager();
        }


        internal void onAppearing()
        {
            isEmptyList = false;
            var list = sqliteManager.GetAll();

            if (list.Count == 0)
            {
                isEmptyList = true;
                if (SaveUserData.userBadge > 0)
                {
                    SaveUserData.userBadge = 0;
                    SaveUserBadge();
                }

            }
            else
            {
                newsList = list.convertObservable();
                SaveUserData.userBadge = newsList.Count;
                isEmptyList = false;
                SaveUserBadge();
            }

        }
        void SaveUserBadge()
        {
            var announPage = page.Navigation.NavigationStack.First();
            DependencyService.Get<IToolbarItemBadge>().SetBadge(announPage, announPage.ToolbarItems.First(), $"" + SaveUserData.userBadge, Color.Red, Color.White);
        }

        internal void listView_ItemClicked(SelectedItemChangedEventArgs list)
        {
            //item selected event

            //dokununca duyuruyu yok etme

            if (list.SelectedItem == null || list == null) return;

            var data = (Announcements)list.SelectedItem;
            sqliteManager.Delete(data.key);
            if (SaveUserData.userBadge != 0) SaveUserData.userBadge -= 1;
            //page notifaction count düşürmek için
            SaveUserBadge();

            if (data.type == null)
            {
                page.Navigation.PushAsync(new ReadPage(), true);
                MessagingCenter.Send<NotifyViewModel, Announcements>(this, "dataAnnouncements", data);
            }
            else
            {
                var section = (AnnCategory)Convert.ToInt32(data.type);
                switch (section)
                {
                    case AnnCategory.activities:
                        page.Navigation.PushAsync(new ReadPage(), true);
                        MessagingCenter.Send<NotifyViewModel, Announcements>(this, "dataActivity_announ", data);
                        break;
                    case AnnCategory.survey:
                        page.Navigation.PushModalAsync(new NavigationPage(new SurveyPage()), true);
                        MessagingCenter.Send<NotifyViewModel, string>(this, "keySurveyAnnc", data.key);
                        break;
                    default:
                        break;
                }
            }

        }
        bool _isEmptyList;

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
