using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using BoshokuDemo1.Helper;
using System.IO;
using BoshokuDemo1.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace BoshokuDemo1.Service
{
  public class BoshokuService
  {

    #region CRUD
    public BoshokuService() { }

    async Task<bool> globalTryCatch(Exception ex)
    {
      switch (ex.Message)
      {
        case "401 (Unauthorized)":
          await authUser();
          return true;
        case "An error occurred while sending the request":
          //await App.Current.MainPage.Navigation.PushPopupAsync(new ErrorPopPage(), true);
          //MessagingCenter.Send<BoshokuService,string>(this,MCenter.errorPagePopKey.ToString(), "İnternet bağlantınızda sorun var");
          return false;
        default:
          return false;
      }

    }
    //Read List
    public async Task<List<FirebaseObject<T>>> GET_list<T>(string key)
    {
      try
      {
        var lists = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderByKey().EndAt(key).LimitToLast(10).WithAuth(SaveUserData.userToken).OnceAsync<T>()).ToList();
        return lists;
      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await GET_list<T>(key);
        }
        else
        {
          Console.WriteLine("Get list error" + ex.Message);
          return new List<FirebaseObject<T>>();
        }

      }

    }

    //all item
    public async Task<List<FirebaseObject<T>>> GET_list<T>()
    {
      try
      {
        var lists = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderByKey().WithAuth(SaveUserData.userToken).OnceAsync<T>()).ToList();
        return lists;
      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await GET_list<T>();
        }
        else
        {
          Console.WriteLine("Get list error" + ex.Message);
          return new List<FirebaseObject<T>>();
        }

      }

    }
    //Read List order
    public async Task<List<FirebaseObject<T>>> GET_list<T>(string orderVal, string val, OrderChild firstOrlast, int count)
    {
      try
      {
        var newsFeedList = new List<FirebaseObject<T>>();
        var newsFeedObservableList = new ObservableCollection<FirebaseObject<T>>();
        switch (firstOrlast)
        {
          case OrderChild.startAt:
            newsFeedList = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(orderVal).StartAt(val).LimitToLast(count).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            break;
          case OrderChild.startAndEndAt:
            newsFeedList = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(orderVal).StartAt(val).EndAt(val).LimitToLast(count).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            break;
          case OrderChild.key:
            newsFeedList = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderByKey().LimitToLast(count).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            break;
          case OrderChild.keyAndEndAt:
            newsFeedList = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderByKey().EndAt(val).LimitToLast(count).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            break;
          case OrderChild.keyAndStartAt:
            newsFeedList = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderByKey().StartAt(val).LimitToLast(count).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            break;
          default:
            newsFeedList = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(orderVal).EndAt(val).LimitToFirst(count).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            break;
        }
        return newsFeedList;
      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await GET_list<T>(orderVal, val, firstOrlast, (int)ListCount.mid);
        }
        else
        {
          Console.WriteLine("Get list error" + ex.Message);
          return new List<FirebaseObject<T>>();
        }
      }

    }

    //Read List Time
    public async Task<List<FirebaseObject<T>>> GET_Tlist<T>(string key, ActivityCategory orderVal)
    {
      try
      {
        //iki kere çağırıyor kontrol et

        switch (orderVal)
        {
          case ActivityCategory.startAt:
            var date = DateTime.Now.convertDateFire();
            return (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(ActivityCategory.endDate.ToString()).StartAt(date).LimitToFirst((int)ListCount.high).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
          case ActivityCategory.category:
            var list = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(ActivityCategory.category.ToString()).StartAt(key).EndAt(key).LimitToFirst((int)ListCount.high).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            return list;
          case ActivityCategory.endAt:
            var dateS = DateTime.Now.convertDateFire();
            //page mantığı gelecek
            var listS = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(ActivityCategory.endDate.ToString()).EndAt(dateS).LimitToFirst(50).WithAuth(SaveUserData.userToken).OnceAsync<T>()).OrderByDescending(x => x.Key).ToList();
            return listS;
          default:
            return null;
        }


      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await GET_Tlist<T>(key, orderVal);
        }
        else
        {
          Console.WriteLine("Get list error" + ex.Message);
          return new List<FirebaseObject<T>>();
        }
      }
    }

    //Zaman kaybını düzenle
    async Task authUser()
    {
      try
      {
        //await AUTO_LOGIN();

        var user = await App.authProvider.SignInWithEmailAndPasswordAsync(SaveUserData.userRegNumber, SaveUserData.userPassword.Base64Decode());
        SaveUserData.deleteToken();
        SaveUserData.userToken = user.FirebaseToken;
      }
      catch (Exception ex)
      {
        Debug.WriteLine("auth error" + ex);
      }


    }

    //Read Object
    public async Task<T> GET_object<T>(string key)
    {

      try
      {
        var val = (await App.fireClient.Child(typeof(T).Name.ToLower() + "/" + key).WithAuth(SaveUserData.userToken).OnceSingleAsync<T>());
        return val;

      }
      catch (Exception ex)
      {

        if (await globalTryCatch(ex))
        {
          return await GET_object<T>(key);

        }
        else
        {
          Console.WriteLine("Object error" + ex.ToString());
          return default(T);
        }

      }

    }
    public async Task<T> GET_object_AUTH<T>(string value, string authKey)
    {

      try
      {
        //gelen sicil numaraısna göre dbde arama
        var val = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy("regNumber").EqualTo(value.ToLower()).WithAuth(authKey).OnceAsync<T>());
        return val.First().Object;

      }
      catch (Exception ex)
      {
        if (ex.Message == "401 (Unauthorized)")
        {
          return await GET_object_AUTH<T>(value, authKey);
        }
        else
        {
          Console.WriteLine("Object error" + ex.ToString());
          return default(T);
        }

      }

    }
    //ifcheck iki metod cakışması bug fix
    public async Task<string> GET_object_AUTH<T>(string child, string authKey, bool ifCheck)
    {

      try
      {
        //gelen sicil numaraısna göre dbde arama
        var val = (await App.fireClient.Child(typeof(T).Name.ToLower() + "/" + child).WithAuth(authKey).OnceSingleAsync<string>());
        return val;

      }
      catch (Exception ex)
      {

        if (await globalTryCatch(ex))
        {
          return await GET_object_AUTH<T>(child, authKey, ifCheck);

        }
        else
        {
          Console.WriteLine("Object error" + ex.ToString());
          return string.Empty;
        }

      }

    }


    //Read Object farklı childer aynı ana okuma
    public async Task<T> GET_OTHER_object<T>(string key, string child)
    {

      try
      {
        var val = (await App.fireClient.Child(child + "/" + key).WithAuth(SaveUserData.userToken).OnceSingleAsync<T>());
        return val;

      }
      catch (Exception ex)
      {

        if (await globalTryCatch(ex))
        {
          return await GET_object<T>(key, child);

        }
        else
        {
          Console.WriteLine("Object error" + ex.ToString());
          return default(T);
        }

      }

    }
    //Read Object
    public async Task<List<T>> GET_ListObject<T>()
    {

      try
      {
        var val = (await App.fireClient.Child("securityquestions").WithAuth(SaveUserData.userToken).OnceAsync<T>());

        return val.Select(a => a.Object).ToList();


      }
      catch (Exception ex)
      {

        if (await globalTryCatch(ex))
        {
          return await GET_ListObject<T>();

        }
        else
        {
          Console.WriteLine("Object error" + ex.ToString());
          return default(List<T>);
        }

      }

    }

    public async Task<T> GET_object<T>(string key, string child)
    {

      try
      {
        var val = (await App.fireClient.Child(child + "/" + key).WithAuth(SaveUserData.userToken).OnceSingleAsync<T>());
        return val;

      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await GET_object<T>(key, child);

        }
        else
        {
          Console.WriteLine("Object error" + ex.ToString());
          return default(T);
        }

      }

    }


    //read one query object
    public async Task<FirebaseObject<T>> GET_object<T>(string order, string _startAt, string _endAt)
    {
      try
      {

        var obj = (await App.fireClient.Child(typeof(T).Name.ToLower()).OrderBy(order).StartAt(_startAt).EndAt(_endAt).WithAuth(SaveUserData.userToken).OnceAsync<T>()).First();
        return obj;

      }
      catch (Exception ex)
      {

        if (await globalTryCatch(ex))
        {
          return await GET_object<T>(order, _startAt, _endAt);

        }
        else
        {
          Console.WriteLine("Get object error" + ex.Message);
          return null;
        }
      }

    }



    //Post Data
    public async Task<FirebaseObject<T>> POST_object<T>(T data)
    {
      try
      {

        var obj = (await App.fireClient.Child("feedbacks").WithAuth(SaveUserData.userToken)
         .PostAsync(data, false));
        return obj;
      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await POST_object<T>(data);

        }
        else
        {
          Console.WriteLine("Post hata" + ex.Message);
          return default(FirebaseObject<T>);
        }

      }

    }


    //Put Data
    public async Task PUT_object<T>(T data, string key)
    {

      try
      {

        await App.fireClient.Child(typeof(T).Name.ToLower() + "/" + key).WithAuth(SaveUserData.userToken).PutAsync(data);

      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          await PUT_object<T>(data, key);

        }
        else
        {
          Console.WriteLine("Post hata" + ex.Message);
        }

      }
    }

    public async Task PUT_object<T>(T data, string key, string token)
    {

      try
      {

        await App.fireClient.Child(typeof(T).Name.ToLower() + "/" + key).WithAuth(token).PutAsync(data);

      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          await PUT_object<T>(data, key);

        }
        else
        {
          Console.WriteLine("Post hata" + ex.Message);
        }

      }
    }

    //Post Image
    public async Task<String> POST_image(string mainKey, string key, Stream data)
    {
      try

      {
        return (await App.fireStorage.Child(mainKey).Child(key).PutAsync(data));

      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await POST_image(mainKey, key, data);

        }
        else
        {
          Console.WriteLine("Image upload error" + ex.Message);
          return string.Empty;

        }


      }

    }

    public async Task<bool> POST_updateUser(string password)
    {

      try
      {
        using (var httpClient = new HttpClient())
        {
          var post = new PasswordRequest { idToken = SaveUserData.userToken, password = password };
          var json = JsonConvert.SerializeObject(post);

          httpClient.BaseAddress = new Uri("https://www.googleapis.com");

          var content = new StringContent(json, Encoding.UTF8, "application/json");


          var request = await httpClient.PostAsync("/identitytoolkit/v3/relyingparty/setAccountInfo?key=" + App.firebaseConfig, content);

          var result = await request.Content.ReadAsStringAsync();
          JObject jObject = JObject.Parse(result);
          JToken key = jObject["idToken"];
          if (key == null) return false;
          //SaveUserData.deleteToken();

          //SaveUserData.userToken = key.ToString();
          return true;



        }


      }
      catch (Exception ex)
      {

        Console.WriteLine("Update User Error" + ex.ToString());
        return false;

      }

    }
    public async Task<bool> AUTO_LOGIN()
    {
      try
      {
        var user = await App.authProvider.SignInWithEmailAndPasswordAsync(SaveUserData.userRegNumber, SaveUserData.userPassword.Base64Decode());
        SaveUserData.deleteToken();
        SaveUserData.userToken = user.FirebaseToken;
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message != "An error occurred while sending the request")
        {
          SaveUserData.deleteData();


        }
        Console.WriteLine("Auth Login  Error" + ex.ToString());

        //await App.Current.MainPage.Navigation.PushPopupAsync(new ErrorPopPage(), true);
        //MessagingCenter.Send<BoshokuService,string>(this,MCenter.errorPagePopKey.ToString(), "İnternet bağlantınızda sorun var");
        return false;

      }

    }

    //firebase token
    public async Task<bool> DELETE_USER(string key)
    {
      try
      {
        using (var httpClient = new HttpClient())
        {
          const string URL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/" + "deleteAccount?key=" + App.firebaseConfig;
          var dict = new Dictionary<string, string>();
          dict.Add("idToken", key);
          var req = new HttpRequestMessage(HttpMethod.Post, URL) { Content = new FormUrlEncodedContent(dict) };
          var res = await httpClient.SendAsync(req);
          var result = await res.Content.ReadAsStringAsync();
          JObject jObject = JObject.Parse(result);
          JToken kind = jObject["kind"];
          if (kind == null) return false;
          else return true;

        }


      }
      catch (Exception ex)
      {

        Console.WriteLine("User delete error" + ex.ToString());
        return false;

      }

    }




    #region düzelitecek!!

    //child event ve survey için ayırmaya özel 
    async public Task<string> saveUserReply<T>(List<T> responseList, string key, string child)
    {
      try
      {
        var index = 0;
        string customKey = "";
        foreach (var item in responseList)
        {
          //TODO stupid CODE
          var data = await App.fireClient.Child(child + "/" + key + "/questions/" + index + "/userResponses/").WithAuth(SaveUserData.userToken).PostAsync(item, false);
          customKey = data.Key;
          index++;
        }

        await App.fireClient.Child(child + "/" + key + "/users/" + SaveUserData.userKey).WithAuth(SaveUserData.userToken).PutAsync(true);
        return customKey;
      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await saveUserReply(responseList, key, child);

        }
        else
        {
          Console.WriteLine("User Response Err: err msg" + ex.ToString());
          return string.Empty;

        }
      }
    }

    async public Task<bool> saveUserReply(List<eventUserResponse> responseList, string key, string child)
    {
      try
      {
        var index = 0;

        foreach (var item in responseList)
        {
          var objData = new { createdDate = item.createdDate, userKey = item.userKey };
          var data = await App.fireClient.Child(child + "/" + key + "/questions/" + index + "/replies/" + item.index + "/users").WithAuth(SaveUserData.userToken).PostAsync(objData, false);
          //data key tek bir data için
          index++;
        }

        await App.fireClient.Child(child + "/" + key + "/users/" + SaveUserData.userKey).WithAuth(SaveUserData.userToken).PutAsync(true);
        return true;
      }
      catch (Exception ex)
      {
        if (await globalTryCatch(ex))
        {
          return await saveUserReply(responseList, key, child);

        }
        else
        {
          Console.WriteLine("User Response Err: err msg" + ex.ToString());
          return false;

        }
      }
    }

    #endregion




    #endregion

  }
}