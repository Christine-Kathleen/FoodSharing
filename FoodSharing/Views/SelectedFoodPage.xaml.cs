using FoodSharing.Models;
using FoodSharing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedFoodPage : ContentPage
    {
       
        public SelectedFoodPage(Food donatedfood)
        {
            var vm = new SelectedFoodViewModel(donatedfood);
            this.BindingContext = vm;
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(51, 153, 137);
            vm.DisplayErrorOnSending += () => DisplayAlert("Error", "Error on creating! Message was not sent!", "OK");
            vm.DisplayMessageAlreadySent += () => DisplayAlert("Error", "There is a message with the same id that was been sent! Message was not sent!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal error! Message was not sent!", "OK");
        }
    }
}