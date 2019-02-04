using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace BoshokuDemo1.ViewModel
{
    public class EventQuestionViewModel : BaseViewModel
    {
        List<Tuple<string, List<String>>> _questionList;

        public List<Tuple<string, List<String>>> questionList
        {
            get
            {
                return _questionList;
            }

            set
            {
                _questionList = value;
                OnPropertyChanged();
            }
        }
        List<eventUserResponse> responseList;

        string child = "competitions";
        string questionKey;

        public EventQuestionViewModel(Page page) : base(page)
        {
            questionList = new List<Tuple<string, List<string>>>();
            responseList = new List<eventUserResponse>();
            //questionListObservable = new ObservableCollection<Tuple<string, List<string>>>()
            // Yarışma sayfası gelen data
            MessagingCenter.Subscribe<EventViewModel, List<Question>>(this, MCenter.question.ToString(), (pg, data) =>
            {
                MessagingCenter.Unsubscribe<EventViewModel, List<Question>>(this, MCenter.question.ToString());
                listQuestions(data);
            });


            MessagingCenter.Subscribe<EventViewModel, string>(this, MCenter.questionKey.ToString(), (pg, data) =>
            {
                questionKey = data;
                MessagingCenter.Unsubscribe<EventViewModel, string>(this, MCenter.questionKey.ToString());

            });

            #region Yarışma sayfası gelen data
            MessagingCenter.Subscribe<EventViewModel, List<Question>>(this, MCenter.question.ToString(), (pg, data) =>
            {
                MessagingCenter.Unsubscribe<EventViewModel, List<Question>>(this, MCenter.question.ToString());
                listQuestions(data);
            });

            MessagingCenter.Subscribe<EventViewModel, string>(this, MCenter.questionKey.ToString(), (pg, data) =>
            {
                questionKey = data;
                MessagingCenter.Unsubscribe<EventViewModel, string>(this, MCenter.questionKey.ToString());

            });
            #endregion

        }
        public void checkboxChanged(object sender,int _index)
        {
            var bindable = (CustomRadioButton)sender;
            var data = bindable.BindingContext as Tuple<string, List<String>>;
            var indis = questionList. FindIndex(x => x.Item1 == data.Item1);
            var response = new eventUserResponse(){index = _index};
            if (responseList.Count > indis)
            {
                responseList[indis].index = _index;
            }
            else responseList.Insert(indis, response);
 

        }

        void listQuestions(List<Question> questions)
        {
            var tempList = new List<Tuple<string, List<string>>>();
            foreach (var question in questions)
            {
                var replyStringList = question.replies.ConvertAll((input) =>
                {
                    return input.reply;
                });

                tempList.Add(new Tuple<string, List<string>>(question.questionTitle, replyStringList));

            }
            foreach (var item in tempList)
            {
                responseList.Add(new eventUserResponse());
            }
            questionList = tempList;

        }

        public ICommand saveCommand
        {
            get
            {
                return new Command(async () =>
               {
                    if (!responseList.Any(x => x.index == -1))
                   {
                       await saveUserResponse();
                   }
                   else
                   {
                       DisplayError("Lütfen soruların tümüne yanıt veriniz..");
                   }
               });
            }
        }

        async public Task saveUserResponse()
        {

            //loading bar eklenecek
            var resp = false;
            await startAnimation();
            resp = await service.saveUserReply(responseList, questionKey, child);
            await stopAnimation();

            if (resp)
            {
                await ToastMessage(Show.SHORT, "Başarılı", AnimationType.Success);

                DisplaySuccess("Cevaplarınız alındı.Sonuçlar yakında açıklanacak.");


                //Y
                MessagingCenter.Send<EventQuestionViewModel, string>(this, MCenter.deleteSurveyKey.ToString(), questionKey);
                await Navigation.PopAsync();

            }
            else
            {
                var chooseBtn = await base.currentPage.DisplayAlert("Uyarı", "Bir problemle karşılaşıldı", "Tekrar dene", "Geri Dön");

                if (chooseBtn)
                {
                    await saveUserResponse();
                }
                else
                {
                    await Navigation.PopAsync();
                }

            }

        }



    }
}

