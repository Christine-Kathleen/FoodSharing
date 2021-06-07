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
            vm.DisplayTakenEmail += () => DisplayAlert("Error", "This email address is already taken!", "OK");
            vm.DisplayInvalidEmail += () => DisplayAlert("Error", "This email address format is invalid!", "OK");
            vm.DisplayNoPassword += () => DisplayAlert("Error", "Password cannot be empty!", "OK");
            vm.DisplayPasswordMismatch += () => DisplayAlert("Error", "Password mismatch!", "OK");
            vm.DisplayCheckTheCheckBox += () => DisplayAlert("Error", "The checkbox must be checked!", "OK");
            vm.DisplayTakenUserName += () => DisplayAlert("Error", "This user name is already taken!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal Error!", "OK");
            vm.DisplayPasswordHasNoNumber += () => DisplayAlert("Error", "The password must contain a number between 0-9!", "OK");
            vm.DisplayPasswordHasNoMinLength += () => DisplayAlert("Error", "The minimum length of the password must be 6!", "OK");
            vm.DisplayPasswordHasNoLowerCase += () => DisplayAlert("Error", "The password requires a lowercase character!", "OK");
            vm.DisplayPasswordHasNoUpperCase += () => DisplayAlert("Error", "The password requires a uppercase character!", "OK");
            vm.DisplayPasswordHasNoNonalphanumeric += () => DisplayAlert("Error", "The password requires a non-alphanumeric character!", "OK");
            vm.DisplayPasswordHasNoOneUniqueCharacter += () => DisplayAlert("Error", "The password requires at least one distinct character!", "OK");
            vm.DisplayCompleteFields += () => DisplayAlert("Error", "All fields must be completed!", "OK");
            vm.DisplayPhoneNrErr += () => DisplayAlert("Error", "The phone number length must be of 10! Ex: 0754345221.", "OK");
            vm.DisplayUserCreated += () => DisplayAlert("Success", "The user was created!", "OK");
            InitializeComponent();
        }
    }
}