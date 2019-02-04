using System;
using System.ComponentModel;
using BoshokuDemo1.Droid.Dependency;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly : ExportRenderer(typeof(ViewCell),typeof(ListCellRenderer))]
namespace BoshokuDemo1.Droid.Dependency
{
    public class ListCellRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
        {
            var cell =  base.GetCellCore(item, convertView, parent, context);
            cell.SetBackgroundColor(Android.Graphics.Color.Transparent);
            return cell;
        }
  //      private Android.Views.View _cellCore;
  //      private bool _selected;
		//protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		//{
  //          base.OnCellPropertyChanged(sender, e);
  //          if (e.PropertyName == "IsSelected")
  //          {
  //              _selected = !_selected;
  //              var extendedViewCell = sender as ViewCell;
  //              if (_selected)
  //                  _cellCore.SetBackgroundColor(Android.Graphics.Color.WhiteSmoke);
  //              else
  //                  _cellCore.SetBackgroundColor(Android.Graphics.Color.Transparent);
  //          }
		//}
	}
}
