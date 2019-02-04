using System.Collections.Generic;
using BoshokuDemo1.Helper;
using BoshokuDemo1.Model;
using Firebase.Xamarin.Database;
using SQLite;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;

namespace BoshokuDemo1.Service
{
    public class SqliteManager
    {
        SQLiteConnection sqliteConnection;
        List<LocalCategory> categoryList;

        public SqliteManager()
        {
            sqliteConnection = DependencyService.Get<ISqliteConnection>().GetConnection();
            categoryList = LocalCategory.getListMain();
            sqliteConnection.CreateTable<Announcements>();

            //userin son okuduğu veriyi tutmak için
            sqliteConnection.CreateTable<SimpleNews>();
        }

        #region crud user bar kayıt işlemleri

        public void Insert(List<FirebaseObject<Announcements>> listNews)
        {

            foreach (var item in listNews)
            {
                //eğer dbde o veri varsa yazmaması için
                if (!sqliteConnection.Table<Announcements>().Any(x => x.key == item.Key))
                {
                    item.Object.key = item.Key;
                    if (item.Object.type == null)
                    {
                        //item.Object.category = categoryList[0].sub.FirstOrDefault(x => x.key == item.Object.category).val;
                    }
                    else if (item.Object.type == AnnCategory.activities.ToString())
                    {
                        item.Object.category = "Etkinlikler";
                    }
                    else
                    {
                        item.Object.category = "Anket ve Yarışma";
                    }
                    sqliteConnection.Insert(item.Object);
                }
                else
                {
                    Debug.WriteLine(item.Key + " db'de bulunuyor.");
                }

            }


        }
        public void Insert(FirebaseObject<Announcements> item)
        {
            //eğer dbde o veri varsa yazmaması için
            if (!sqliteConnection.Table<Announcements>().Any(x => x.key == item.Key))
            {
                item.Object.key = item.Key;
                sqliteConnection.Insert(item.Object);
            }

        }
        public void Insert(Announcements item)
        {
            sqliteConnection.Insert(item);

        }
        public void Insert(string key)
        {
            //geçiçi userin okuduğu notficationdan datalar
            sqliteConnection.Insert(new SimpleNews { content = key });
        }
        //liste içindekileri silme ortadan veya sondan seçilme durumları için
        public void Delete(string _key)
        {
            sqliteConnection.Table<Announcements>().Delete(x => x.key == _key);
        }

        public void Delete()
        {
            sqliteConnection.DeleteAll<SimpleNews>();
        }
        public void Update(){
            
        }
        public void DeleteData()
        {
            sqliteConnection.DeleteAll<Announcements>();
        }
        public void DeleteSimpe()
        {
            sqliteConnection.DeleteAll<SimpleNews>();
        }

        public List<Announcements> GetAll()
        {
            var userNotificationComplateReadList = sqliteConnection.Table<SimpleNews>().ToList();
            var userNotificationBeginReadList = sqliteConnection.Table<Announcements>().OrderByDescending(x => x.createdDate).ToList();
            if (userNotificationComplateReadList.Count > 0)
            {
                foreach (var item in userNotificationComplateReadList)
                {
                    userNotificationBeginReadList.Remove(userNotificationBeginReadList.FirstOrDefault(x => x.key == item.content));
                }
            }

            return userNotificationBeginReadList;

        }

        //eğer yana geçerken badgeCount ile item count aynı ise yeni bir item gelmediğinden ekleme yapma işlemi
        public int ItemCount()
        {
            return sqliteConnection.Table<Announcements>().Count();
        }

        #endregion
    }
}
