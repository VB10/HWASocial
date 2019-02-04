
using BoshokuDemo1.ViewModel;
using Xamarin.Forms;

namespace BoshokuDemo1.Views.DrawerMenu
{
    public partial class EventInteractionPage : ContentPage
    {
        InteractionViewModel interactionViewModel;
        public EventInteractionPage()
        {
            InitializeComponent();
            BindingContext = interactionViewModel = new InteractionViewModel(this);
        }
    }
}
