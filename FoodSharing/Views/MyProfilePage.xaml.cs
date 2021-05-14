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
            vm.DisplayProfileUpdateError += () => DisplayAlert("Error", "Fatal Error, could not update!", "OK");

            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal Error!", "OK");
            vm.DisplayErrorOnUpdate += () => DisplayAlert("Error", "Could not update!", "OK");
            vm.DisplayApplicationError += () => DisplayAlert("Error", "Fatal application error, contact the administrator!", "OK");

            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(112, 174, 152);
        }
    }
}