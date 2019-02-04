using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using BoshokuDemo1.ViewModel;
using BoshokuDemo1.Views.Popup;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace BoshokuDemo1.Views.Tab
{
    public partial class SurveyQuestionList : ContentPage
    {
        //TODO mvmm taşı !
        List<Tuple<string, List<String>>> questionList = new List<Tuple<string, List<String>>>();
        List<surveyUserResponse> responseList;


        string child;
        string questionKey;

        //Silinecek TODO
        BoshokuService service = new BoshokuService();

        public SurveyQuestionList()
        {
            InitializeComponent();
            responseList = new List<surveyUserResponse>();
            MessagingCenter.Subscribe<SurveyPage, List<Question>>(this, MCenter.question.ToString(), (page, data) =>
            {
                MessagingCenter.Unsubscribe<SurveyPage, List<Question>>(this, MCenter.question.ToString());
                List<string> replyList = new List<string>();
                child = "surveys";
                foreach (var question in data)
                {
                    var replyStringList = question.replies.ConvertAll((input) =>
                         {
                             return input.reply;
                         });

                    questionList.Add(new Tuple<string, List<string>>(question.questionTitle, replyStringList));

                }
                foreach (var item in questionList)
                {
                    responseList.Add(new surveyUserResponse());
                }
                _ls.BindingContext = questionList;
            });

            MessagingCenter.Subscribe<SurveyPage, string>(this, MCenter.questionKey.ToString(), (page, data) =>
            {
                questionKey = data;
                MessagingCenter.Unsubscribe<SurveyPage, string>(this, MCenter.questionKey.ToString());

            });
     
        }



        void Handle_CheckedChanged(object sender, int e)
        {
            var bindable = (CustomRadioButton)sender;
            var data = bindable.BindingContext as Tuple<string, List<String>>;
            var indis = questionList.FindIndex(x => x.Item1 == data.Item1);
            var response = new surveyUserResponse() { reply = bindable.Text };

            if (responseList.Count > indis)
            {
                responseList[indis].reply = bindable.Text;
            }
            else responseList.Insert(indis, response);



        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (!responseList.Any(x => x.reply == null))
            {
                saveUserResponse();
            }
            else
            {
                await DisplayAlert("Eksik", "Lütfen soruların tümüne yanıt veriniz", "Tamam");
            }
        }


        async public void saveUserResponse()
        {

            var mvvmHelper = new SurveyMVVM(this);
            //loading bar eklenecek
            string resp = string.Empty;
            await Navigation.PushPopupAsync(new LoadingPopPage(), true);
            resp = await service.saveUserReply(responseList, questionKey, child);
            await Navigation.PopPopupAsync(true);

            await Navigation.PopAllPopupAsync(true);

            if (!string.IsNullOrEmpty(resp))
            {
                await DisplayAlert("Tamamlandı", "Cevaplarınız alındı teşekür ederiz.", "Kapat");

                MessagingCenter.Send<SurveyQuestionList, string>(this, MCenter.deleteSurveyKey.ToString(), questionKey);
                await Navigation.PopAsync();

            }
            else
            {
                var chooseBtn = await DisplayAlert("Hata", "Bir problemle karşılaşıldı", "Tekrar dene", "Geri Dön");

                if (chooseBtn)
                {
                    saveUserResponse();
                }
                else
                {
                    await Navigation.PopAsync();
                }

            }

        }


        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            _ls.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();

        }
    }
}

