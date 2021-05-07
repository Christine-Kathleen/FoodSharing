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
    public partial class AddFoodPage : ContentPage
    {
        public AddFoodPage()
        {
            var vm = new AddFoodViewModel();
            this.BindingContext = vm;
            vm.DisplayCompleteFields += () => DisplayAlert("Error", "All fields must be completed!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal Error!", "OK");
            vm.DisplayFoodCreated += () => DisplayAlert("Success", "Food announcement was created!", "OK");

            InitializeComponent();
        }
    }
}