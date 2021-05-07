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
    public partial class MyProfile : ContentPage
    {
        public MyProfile()
        {
            var vm = new MyProfileViewModel();
            this.BindingContext = vm;
            vm.DisplayProfileUpdateMade += () => DisplayAlert("Success", "Your profile has been updated!", "OK");
            vm.DisplayProfileUpdateFatalError += () => DisplayAlert("Error", "Fatal Error, could not update!", "OK");

            vm.DisplayFoodUpdateFatalError += () => DisplayAlert("Error", "Fatal Error, could not update!", "OK");
            vm.DisplayFoodUpdateMade += () => DisplayAlert("Success", "Your profile has been updated!", "OK");

            vm.DisplayFoodDeleted += () => DisplayAlert("Success", "Your profile has been updated!", "OK");
            vm.DisplayFoodDeletedFatalError += () => DisplayAlert("Error", "Fatal Error, could not update!", "OK");

            InitializeComponent();
        }
    }
}