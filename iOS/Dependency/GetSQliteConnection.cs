using System;
using System.IO;
using BoshokuDemo1.Helper;
using BoshokuDemo1.iOS.Dependency;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetSQliteConnection))]
namespace BoshokuDemo1.iOS.Dependency
{
    public class GetSQliteConnection : ISqliteConnection
    {
        public SQLiteConnection GetConnection()
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(docPath, App.DbName);
            var connection = new SQLiteConnection(path);
            return connection;

        }
    }
}
