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
    public partial class UserSettingsPage : ContentPage
    {
        public UserSettingsPage()
        {
            var vm = new UserSettingsViewModel();
            this.BindingContext = vm;
            vm.DisplayDeletedAccount += () => DisplayAlert("Success", "Your account was deleted!", "OK");
            vm.DisplayNoPassword += () => DisplayAlert("Error", "Password cannot be empty!", "OK");
            vm.DisplayPasswordChanged += () => DisplayAlert("Success", "Password changed!", "OK");
            vm.DisplayNoPassword += () => DisplayAlert("Error", "Password cannot be empty!", "OK");
            vm.DisplaySamePassword += () => DisplayAlert("Error", "Your new password must not match the old one!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal Error!", "OK");
            vm.DisplayPasswordHasNoNumber += () => DisplayAlert("Error", "The password must contain a number between 0-9!", "OK");
            vm.DisplayPasswordHasNoMinLength += () => DisplayAlert("Error", "The minimum length of the password must be 6!", "OK");
            vm.DisplayPasswordHasNoLowerCase += () => DisplayAlert("Error", "The password requires a lowercase character!", "OK");
            vm.DisplayPasswordHasNoUpperCase += () => DisplayAlert("Error", "The password requires a uppercase character!", "OK");
            vm.DisplayPasswordHasNoNonalphanumeric += () => DisplayAlert("Error", "The password requires a non-alphanumeric character!", "OK");
            vm.DisplayPasswordHasNoOneUniqueCharacter += () => DisplayAlert("Error", "The password requires at least one distinct character!", "OK");
            vm.DisplayCompletePasswordField += () => DisplayAlert("Error", "You must enter your current password!", "OK");
            vm.DisplayCompleteNewPasswordField += () => DisplayAlert("Error", "You must enter a new password!", "OK");
            vm.DisplayApplicationError += () => DisplayAlert("Error", "Application error, try again!", "OK");
            vm.DisplayFailedChange += () => DisplayAlert("Error", "Failed to change password!", "OK");
            vm.DisplayWrongPasswordEntered += () => DisplayAlert("Error", "You entered a wrong password!", "OK");
            InitializeComponent();

        }
    }
}