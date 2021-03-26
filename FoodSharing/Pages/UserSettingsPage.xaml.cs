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
            vm.DisplayDeletedAccount += () => DisplayAlert("Deletion", "Your account was deleted!", "OK");
            vm.DisplayNoPassword += () => DisplayAlert("Error", "Password cannot be empty!", "OK");
            vm.DisplayPasswordChanged += () => DisplayAlert("Success", "Password changed!", "OK");
            InitializeComponent();

        }
    }
}