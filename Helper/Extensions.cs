using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoshokuDemo1.Helper
{
    public static class Extensions
    {

        public static ObservableCollection<T> convertObservable<T>(this List<T> list)
        {
            var collection = new ObservableCollection<T>();
            list.ForEach((obj) =>
            {
                collection.Add(obj);
            });

            return collection;
        }


        //string şifre açma
        public static string Base64Decode(this string text)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(text);
            var rs = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return rs;
        }


        //şifreleme
        public static string Base64Encode(this string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //html convert
        public static string convertDateFire(this DateTime nowDate)
        {
            var date = nowDate.ToString("MM/dd/yyyy");
            date = date.Replace('.', '/');
            return date;
        }



    }
}
