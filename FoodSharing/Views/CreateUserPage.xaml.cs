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
    public partial class CreateUserPage : ContentPage
    {
        public CreateUserPage()
        {
            var vm = new CreateUserViewModel();
            this.BindingContext = vm;
            vm.DisplayTakenEmail += () => DisplayAlert("Error", "This email address is already taken!", "OK"); //error alerts
            vm.DisplayInvalidEmail += () => DisplayAlert("Error", "This email address format is invalid!", "OK");
            vm.DisplayNoPassword += () => DisplayAlert("Error", "Password cannot be empty!", "OK");
            vm.DisplayUserCreated += () => DisplayAlert("Success", "The user was created!", "OK");
            InitializeComponent();
        }
    }
}