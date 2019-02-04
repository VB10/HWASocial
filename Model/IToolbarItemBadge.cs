using System;
using Xamarin.Forms;

namespace BoshokuDemo1.Model
{
    public interface IToolbarItemBadge
    {
        void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor);
    }
}
