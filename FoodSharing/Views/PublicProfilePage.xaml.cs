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
    public partial class PublicProfilePage : ContentPage
    {
        public PublicProfilePage(ApplicationUser user)
        {
            var vm = new PublicProfileViewModel(user);
            this.BindingContext = vm;
            vm.DisplayReviewError += () => DisplayAlert("Error", "Could not add review!", "OK");
            vm.DisplayCompleteFields += () => DisplayAlert("Error", "Empty review!", "OK");
            vm.DisplayReviewAdded += () => DisplayAlert("Success", "Your review has been added!", "OK");

            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(51, 153, 137);
        }
    }
}