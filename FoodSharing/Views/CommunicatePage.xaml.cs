using FoodSharing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommunicatePage : ContentPage
    {
        public CommunicatePage()
        {
            NavigationPage.SetHasBackButton(this, false);
            var vm = new CommunicateViewModel();
            this.BindingContext = vm;
            var navigationPage = Application.Current.MainPage as NavigationPage;
            vm.DisplayErrorOnSending += () => DisplayAlert("Error", "Error on creating! Message was not sent!", "OK");
            vm.DisplayMessageAlreadySent += () => DisplayAlert("Error", "There is a message with the same id that was been sent! Message was not sent!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal error! Message was not sent!", "OK");

            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }    
}