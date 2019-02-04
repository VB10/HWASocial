using System;
using System.Diagnostics;
using BoshokuDemo1.Droid.Dependency;
using BoshokuDemo1.Helper;
using SQLite;
using Xamarin.Forms;

[assembly : Dependency(typeof(GetSQliteConnection))]
namespace BoshokuDemo1.Droid.Dependency
{
	public class GetSQliteConnection : ISqliteConnection
    {
        public SQLiteConnection GetConnection()
        {
            try
            {
                var docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = System.IO.Path.Combine(docPath, App.DbName);
                var connection = new SQLiteConnection(path);
                return connection;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sqlite Android path error" + ex.Message);
                return null;
            }
        }
    }
}
