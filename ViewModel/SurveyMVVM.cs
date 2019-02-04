using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using BoshokuDemo1.Views.Tab;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class SurveyMVVM : INotifyPropertyChanged
    {
        BoshokuService service;
        Page page;
        ObservableCollection<FirebaseObject<Surveys>> _surveyList;

        public ObservableCollection<FirebaseObject<Surveys>> surveyList
        {
            get
            {
                return _surveyList;
            }

            set
            {
                _surveyList = value;
                OnPropertyChanged();
            }
        }


        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    loadingComplate = true;

                    await getSurveyList("");
                    check();
                    loadingComplate = false;
                });
            }
        }
        private void check()
        {
            if (_surveyList.Count == 0)
            {
                isEmptyList = true;
            }
            else
            {
                isEmptyList = false;

            }
        }


        public SurveyMVVM(Page page)
        {
            service = new BoshokuService();
            this.page = page;
            _surveyList = new ObservableCollection<FirebaseObject<Surveys>>();
            subsPage();

        }

        private void subsPage()
        {
            MessagingCenter.Subscribe<SurveyQuestionList, string>(this, MCenter.deleteSurveyKey.ToString(), (pg, data) =>
            {
                surveyList.Remove(surveyList.FirstOrDefault(x => x.Key == data));
                MessagingCenter.Unsubscribe<SurveyQuestionList>(this, MCenter.deleteSurveyKey.ToString());

            });
        }

        public async Task onAppering()
        {
            if (surveyList.Count == 0)
            {
                isVisibleIndicator = true;
                await getSurveyList("");
                check();
                isVisibleIndicator = false;
            }
        }



        public async Task<Surveys> getSurveyObject(string key)
        {
            try
            {
                return (await service.GET_object<Surveys>(key));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Survey Object Error  " + ex.Message);
                return null;
            }
        }
        public async Task<bool> getSurveyList(string key)
        {
            try
            {

                var datalist = (await service.GET_list<Surveys>("endDate", DateTime.Now.convertDateFire(), OrderChild.startAt, (int)ListCount.mid)).OrderBy(x => x.Object.endDate);
                var emptyList = new ObservableCollection<FirebaseObject<Surveys>>();

                foreach (var item in datalist)
                {

                    if (item.Object.endDate > DateTime.Now)
                    {
                        var result = await checkUserResponse(item.Key);
                        if (!result)
                        {
                            surveyList.Add(item);
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SurveyError " + ex.Message);
                return false;
            }
        }

        //TODO MVVM  service düzelt!!
        public async Task<bool> checkUserResponse(string key)
        {
            try
            {

                var result = await App.fireClient.Child("surveys/" + key + "/users/" + SaveUserData.userKey).WithAuth(SaveUserData.userToken).OnceSingleAsync<bool>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("user response errror" + ex.StackTrace);
                return false;
            }


        }

        #region Değişiklikleri yakalama
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        #region boolDatalar
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
        bool _isVisibleIndicator;

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

        #endregion

    }
}