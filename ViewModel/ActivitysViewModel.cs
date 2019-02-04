using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BoshokuDemo1.Model;
using BoshokuDemo1.Service;
using Firebase.Xamarin.Database;
using Xamarin.Forms;
using BoshokuDemo1.Helper;
using System.Linq;
using BoshokuDemo1.Views.DrawerMenu;
using BoshokuDemo1.Views.Tab;

namespace BoshokuDemo1.ViewModel
{
  public class ActivitysViewModel : BaseViewModel
  {

    public BoshokuService service;
    #region Listeler
    Page currentPage;
    public ObservableCollection<FirebaseObject<Activities>> _activityList = new ObservableCollection<FirebaseObject<Activities>>();
    public ObservableCollection<FirebaseObject<Activities>> activityList
    {
      get
      {
        return _activityList;
      }
      set
      {

        _activityList = value;
        OnPropertyChanged();

      }
    }

    #endregion


    #region command

    public ICommand RefreshCommand
    {
      get
      {
        return new Command(async () =>
        {
          if (isEmptyList) isEmptyList = false;
          loadingComplate = true;
          await getActivitys("", ActivityCategory.startAt);
          loadingComplate = false;
          check();
        });
      }
    }

    void check()
    {
      if (activityList.Count == 0)
      {
        isEmptyList = true;
      }
      else
      {
        isEmptyList = false;
      }
    }

    internal async Task OnAppearing()
    {
      if (activityList.Count == 0)
      {
        await getActivitys("", ActivityCategory.startAt);
        isVisibleIndicator = false;
        check();
      }
    }

    async public void activityForCategory(string key, string orderVal)
    {
      loadingComplate = true;
      await getActivitys(key, ActivityCategory.category);
      loadingComplate = false;
    }

    #endregion

    public ActivitysViewModel(Page page) : base(page)
    {
      currentPage = page;
      service = new BoshokuService();
      activityList = new ObservableCollection<FirebaseObject<Activities>>();
      subscribe();

    }

    void subscribe()
    {
      MessagingCenter.Subscribe<App, string>(this, MCenter.dataActivity.ToString(), (arg1, arg2) =>
      {
        if (!string.IsNullOrEmpty(arg2))
        {
          currentPage.Navigation.PushAsync(new ReadPage(), true);
          MessagingCenter.Send<ActivitysViewModel, string>(this, "dataActivity_View", arg2);
        }
        MessagingCenter.Send<ActivitysViewModel, CurrentTabPageChange>(this, "changeCurrentPage", CurrentTabPageChange.pageActivity);
        MessagingCenter.Unsubscribe<App>(this, MCenter.dataActivity.ToString());
      });

      MessagingCenter.Subscribe<DrawerPage, bool>(this, "drawerActivity", async (page, data) =>
      {
        switch (data)
        {
          case true:
            await getActivitys("", ActivityCategory.startAt);
            break;
          case false:
            await getActivitys("", ActivityCategory.endAt);
            break;
        }
        MessagingCenter.Unsubscribe<DrawerPage>(this, "drawerActivity");
      });



    }
    public void Handle_ItemSelected(SelectedItemChangedEventArgs list)
    {
      if (list == null || list.SelectedItem == null) return;
      var model = list.SelectedItem as FirebaseObject<Activities>;
      currentPage.Navigation.PushAsync(new ReadPage(), true);
      MessagingCenter.Send<ActivitysViewModel, string>(this, "dataActivity_View", model.Key);
    }

    public async Task getActivitys(string key, ActivityCategory category)
    {
      try
      {
        switch (category)
        {
          case ActivityCategory.startAt:
            activityList = (await service.GET_Tlist<Activities>(key, category)).Where(x => x.Object.endDate >= DateTime.Now).ToList().convertObservable();
            break;
          case ActivityCategory.endAt:
            activityList = (await service.GET_Tlist<Activities>(key, category)).Where(x => x.Object.endDate < DateTime.Now).ToList().convertObservable();
            break;
          default:
            break;
        }
        check();
      }
      catch (Exception ex)
      {
        Console.WriteLine("ActivityError Desc:" + ex.Message);
        isVisibleIndicator = false;

      }

    }
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
    bool _isVisibleIndicator = true;

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

