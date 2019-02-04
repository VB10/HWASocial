using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.ViewModel;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Popup;
using Firebase.Xamarin.Database;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public partial class SurveyPage : ContentPage
    {
        SurveyMVVM surveyMVVM;

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await surveyMVVM.onAppering();
            Title = "Anket";

        }
        async void Handle_BackClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
        public SurveyPage()
        {
            InitializeComponent();
            this.BindingContext = surveyMVVM = new SurveyMVVM(this);
            //NavigationPage.SetTitleIcon(this, "navIcon");


            //TODO MVVM  DUZELT TAM OLARAK !
            MessagingCenter.Subscribe<NewsFeedViewModel, string>(this, "keySurveyAnnc", async (arg1, key) =>
             {
                 await getQuestionDetail(key);
                 MessagingCenter.Instance.Unsubscribe<NewsFeedViewModel, string>(this, "keySurveyAnnc");

             });
            MessagingCenter.Subscribe<NotifyViewModel, string>(this, "keySurveyAnnc", async (arg1, key) =>
            {
                await getQuestionDetail(key);
                MessagingCenter.Instance.Unsubscribe<NotifyViewModel, string>(this, "keySurveyAnnc");

            });

            MessagingCenter.Subscribe<App, string>(this, MCenter.dataSurvey.ToString(), async (arg1, key) =>
            {
                MessagingCenter.Unsubscribe<App, string>(this, MCenter.dataSurvey.ToString());
                await getQuestionDetail(key);
            });

        }
        async Task getQuestionDetail(string key)
        {
            MessagingCenter.Instance.Unsubscribe<NewsFeedViewModel, string>(this, "keySurveyAnnc");
            MessagingCenter.Instance.Unsubscribe<App, string>(this, MCenter.dataSurvey.ToString());

            var surveyDetail = await surveyMVVM.getSurveyObject(key);
            if (surveyDetail != null)
            {
                await getSurveyQuestionPage(key, surveyDetail);
            }
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (_lst.SelectedItem != null)
            {
                var data = e.Item as FirebaseObject<Surveys>;
                await getSurveyQuestionPage(data.Key, data.Object);
                _lst.SelectedItem = null;
            }
        }

        public async Task getSurveyQuestionPage(string key, Surveys data)
        {
            var result = true;
            await Navigation.PushPopupAsync(new LoadingPopPage(), true);
            result = await surveyMVVM.checkUserResponse(key);
            await Navigation.PopPopupAsync(true);

            if (!result)
            {

                await Navigation.PushAsync(new SurveyQuestionList() { Title = data.surveyName }, true);
                MessagingCenter.Send<SurveyPage, List<Question>>(this, MCenter.question.ToString(), data.questions);
                MessagingCenter.Send<SurveyPage, string>(this, MCenter.questionKey.ToString(), key);
            }
            else
            {
                await DisplayAlert("Uyarı", "Bu ankete daha önceden cevap verilmiş", "Tamam");
            }
            await Navigation.PopAllPopupAsync();

        }
        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            _lst.IsRefreshing = false;
            await surveyMVVM.getSurveyList("");
        }
    }
}