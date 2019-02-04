using System;
using BoshokuDemo1.Views.DrawerMenu;
using SlideOverKit;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.Tab
{
    public class MenuCPage : MenuContainerPage
    {
        public MenuCPage()
        {
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
              
                Children = {
                    new Button{
                        Text ="Show Menu",
                        Command = new Command(()=>{
                            this.ShowMenu();
                        })
                    },
                    new Button{
                        Text ="Hide Menu",
                        Command = new Command(()=>{
                            this.HideMenu();
                        })
                    },
                }
            };

            this.BackgroundColor = Color.Red;
     
            this.SlideMenu = new DrawerPage();


        }
    }
}

