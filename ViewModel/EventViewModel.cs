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
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace BoshokuDemo1.ViewModel
{
    public class EventViewModel : BaseViewModel
    {
        BoshokuService serivce;
        Page page;
        ObservableCollection<FirebaseObject<Competitions>> _eventList;

        public ObservableCollection<FirebaseObject<Competitions>> eventList
        {
            get
            {
                return _eventList;
            }

            set
            {
                _eventList = value;
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    isRefresh = true;
                    await getCompetitionsList();
                    check();
                    isRefresh = false;
                });
            }
        }
        public ICommand BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Navigation.PopModalAsync(true);

                    }
                    catch (Exception)
                    {
                        await Navigation.PopAsync(true);

                    }
                });
            }
        }
        public EventViewModel(Page page) : base(page)
        {
            serivce = new BoshokuService();
            eventList = new ObservableCollection<FirebaseObject<Competitions>>();
            this.page = page;

           

            MessagingCenter.Subscribe<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.annMultipleChoice.ToString(), async (pg, data) =>
            {
                MessagingCenter.Unsubscribe<NewsFeedViewModel>(this, MCenter.annMultipleChoice.ToString());

                await onItemSelected(AnnCategory.multiple_choice, data);
            });

            MessagingCenter.Subscribe<NewsFeedViewModel, FirebaseObject<Announcements>>(this, MCenter.annUserInteraction.ToString(), async (pg, data) =>
            {
                MessagingCenter.Unsubscribe<NewsFeedViewModel>(this, MCenter.annUserInteraction.ToString());
                await onItemSelected(AnnCategory.user_interaction, data);
            });

            MessagingCenter.Subscribe<App, string>(this, MCenter.dataUserInteraction.ToString(), async (arg1, key) =>
            {
                MessagingCenter.Unsubscribe<App, string>(this, MCenter.dataUserInteraction.ToString());
                await getEventDetail(AnnCategory.user_interaction, key);
            });
            MessagingCenter.Subscribe<App, string>(this, MCenter.dataMultipleChoice.ToString(), async (arg1, key) =>
            {
                MessagingCenter.Unsubscribe<App, string>(this, MCenter.dataMultipleChoice.ToString());
                await getEventDetail(AnnCategory.multiple_choice, key);
            });


            MessagingCenter.Subscribe<EventQuestionViewModel, string>(this, MCenter.deleteSurveyKey.ToString(), (pg, data) =>
            {
                eventList.Remove(eventList.FirstOrDefault(x => x.Key == data));
                MessagingCenter.Unsubscribe<EventQuestionViewModel,string>(this, MCenter.deleteSurveyKey.ToString());

            });

            MessagingCenter.Subscribe<InteractionViewModel, string>(this, MCenter.deleteSurveyKey.ToString(), (pg, data) =>
            {
                eventList.Remove(eventList.FirstOrDefault(x => x.Key == data));
                MessagingCenter.Unsubscribe<EventQuestionViewModel, string>(this, MCenter.deleteSurveyKey.ToString());

            });


        }

        public async Task onAppering()
        {
            //hiç item yoksa çalışması için
            if (eventList.Count != 0) return;
            isVisibleIndicator = true;
            await getCompetitionsList();
            check();

        }

        async Task getEventDetail(AnnCategory type, string key)
        {

            switch (type)
            {
                case AnnCategory.multiple_choice:
                    var data = (await serivce.GET_object<Competitions>(key));
                    if (data == null) return;

                    var dataSurvey = new Surveys()
                    {
                        surveyName = data.compName,
                        sKey = key
                    };
                    await getSurveyQuestionPage(key, dataSurvey);

                    break;
                case AnnCategory.user_interaction:
                    await startAnimation();
                    var dataCompt = (await serivce.GET_object<Competitions>(key));

                    var checkUser = await checkUserResponse(key);
                    if (!checkUser)
                    {
                        dataCompt.key = key;
                        await page.Navigation.PushAsync(new EventInteractionPage() { Title = dataCompt.compName }, true);
                        MessagingCenter.Send<EventViewModel, Competitions>(this, MCenter.competitionToInteractionKey.ToString(), dataCompt);

                    }
                    else
                    {
                        await page.DisplayAlert("Uyarı", "Bu yarışmaya önceden cevap vermişsiniz.", "Tamam");
                    }

                    await stopAnimation();
                    break;
                default:
                    return;
            }

        }


        public async Task SelectedItem(SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var item = (FirebaseObject<Competitions>)e.SelectedItem;
            var type = (Event)item.Object.type;
            switch (type)
            {
                case Event.multiple_choice:
                    var data = await getMultipleChoice(item.Key);
                    data.surveyName = item.Object.compName;
                    data.sKey = item.Key;
                    await getSurveyQuestionPage(item.Key, data);
                    break;
                case Event.user_interaction:
                    await startAnimation();
                    var checkUser = await checkUserResponse(item.Key);
                    if (!checkUser)
                    {
                        await page.Navigation.PushAsync(new EventInteractionPage() { Title = item.Object.compName }, true);
                        MessagingCenter.Send<EventViewModel, FirebaseObject<Competitions>>(this, MCenter.competitionToInteractionKey.ToString(), item);

                    }
                    else
                    {
                        await page.DisplayAlert("Uyarı", "Bu yarışmaya önceden cevap vermişsiniz.", "Tamam");
                    }

                    await stopAnimation();
                    break;
                default:
                    break;
            }

        }

        async Task onItemSelected(AnnCategory type, FirebaseObject<Announcements> item)
        {
            switch (type)
            {
                case AnnCategory.multiple_choice:
                    var data = await getMultipleChoice(item.Key);
                    if (data == null) return;

                    data.surveyName = item.Object.title;
                    data.sKey = item.Key;
                    await getSurveyQuestionPage(item.Key, data);
                    break;

                case AnnCategory.user_interaction:
                    await startAnimation();
                    var checkUser = await checkUserResponse(item.Key);
                    if (!checkUser)
                    {
                        await page.Navigation.PushAsync(new EventInteractionPage() { Title = item.Object.title }, true);
                        MessagingCenter.Send<EventViewModel, FirebaseObject<Announcements>>(this, MCenter.announToInteractionKey.ToString(), item);

                    }
                    else
                    {
                        await page.DisplayAlert("Uyarı", "Bu yarışmaya önceden cevap vermişsiniz.", "Tamam");
                    }

                    await stopAnimation();

                    break;

                default:
                    return;
            }

        }
        async Task<Surveys> getMultipleChoice(string key)
        {
            {
                try
                {
                    return (await serivce.GET_object<Surveys>(key, typeof(Competitions).Name.ToLower()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" error :" + ex.Message);
                    return new Surveys();
                }
            }
        }

        public async Task getSurveyQuestionPage(string key, Surveys data)
        {
            var result = true;
            await startAnimation();
            result = await checkUserResponse(key);
            await stopAnimation();

            if (!result)
            {
                await page.Navigation.PushAsync(new EventQuestionPage() { Title = data.surveyName }, true);
                MessagingCenter.Send<EventViewModel, List<Question>>(this, MCenter.question.ToString(), data.questions);
                MessagingCenter.Send<EventViewModel, string>(this, MCenter.questionKey.ToString(), key);
            }
            else
            {
                await page.DisplayAlert("Uyarı", "Bu yarışmaya önceden cevap vermişsiniz.", "Tamam");
            }

        }


        public async Task<bool> getCompetitionsList()
        {
            try
            {

                var datalist = (await serivce.GET_list<Competitions>("endDate", DateTime.Now.convertDateFire(), OrderChild.startAt, (int)ListCount.mid)).OrderBy(x => x.Object.endDate);


                foreach (var item in datalist)
                {

                    if (item.Object.endDate > DateTime.Now)
                    {
                        var result = await checkUserResponse(item.Key);
                        if (!result)
                        {
                            if (eventList.Any(x => x.Key == item.Key))
                            {
                                var listItem = eventList.FirstOrDefault(x => x.Key == item.Key);
                                eventList[eventList.IndexOf(listItem)] = item;
                            }
                            else
                            {
                                eventList.Add(item);

                            }
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

        public async Task<bool> checkUserResponse(string key)
        {
            try
            {

                var result = await App.fireClient.Child("competitions/" + key + "/users/" + SaveUserData.userKey).WithAuth(SaveUserData.userToken).OnceSingleAsync<bool>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("user response errror" + ex.StackTrace);
                return false;
            }


        }
        private void check()
        {
            if (eventList.Count == 0)
            {
                isEmptyList = true;
            }
            else
            {
                isEmptyList = false;

            }
            isVisibleIndicator = false;
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

        bool _isRefresh = false;

        public bool isRefresh
        {
            get
            {
                return _isRefresh;
            }

            set
            {
                _isRefresh = value;
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
    }
}

