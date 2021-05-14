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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            vm.DisplayInvalidEmail += () => DisplayAlert("Error", "This email address format is invalid!", "OK");
            vm.DisplayNoPassword += () => DisplayAlert("Error", "Password cannot be empty!", "OK");
            vm.DisplayFailedLogin += () => DisplayAlert("Error", "Login failed, please check your username and password!", "OK");
            vm.DisplayApplicationError += () => DisplayAlert("Error", "Fatal application error, contact the administrator!", "OK");
            vm.DisplayCompleteFields += () => DisplayAlert("Error", "All fields must be completed!", "OK");
            InitializeComponent();

            //Email.Completed += (object sender, EventArgs e) =>
            //{
            //    Password.Focus();
            //};

            //Password.Completed += (object sender, EventArgs e) =>
            //{
            //    vm.SubmitCommand.Execute(null);
            //};
        }
    }
}
