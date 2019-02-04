using System;
using System.Collections.Generic;
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace BoshokuDemo1.Views.DrawerMenu
{
    public partial class EventQuestionPage : ContentPage
    {
        EventQuestionViewModel eventQuestionViewModel;
        public EventQuestionPage()
        {
            InitializeComponent();
            BindingContext = eventQuestionViewModel = new EventQuestionViewModel(this);
        }
        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            _list.SelectedItem = null;
        }

     
        int oldIndex=0;
        void Handle_CheckedChanged(object sender, int e)
        {
            if (oldIndex == 0) {
                var checkBox = sender as CustomRadioButton;
                eventQuestionViewModel.checkboxChanged(sender, e);
                oldIndex++;

            }
            else
            {
                oldIndex = 0;
            }



        }
    }
}
