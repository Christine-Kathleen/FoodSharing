using FoodSharing.Models;
using FoodSharing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAnnouncementPage : ContentPage
    {
        public EditAnnouncementPage(Food donatedfood)
        {
            var vm = new EditAnnouncementViewModel(donatedfood);
            this.BindingContext = vm;
            vm.DisplayFoodDeleted += () => DisplayAlert("Success", "Your announcement has been deleted!", "OK");
            vm.DisplayFoodDeletedError += () => DisplayAlert("Error", "Could not delete!", "OK");
            vm.DisplayUpdatedFood += () => DisplayAlert("Success", "Your announcement has been updated!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal error!", "OK");
            vm.DisplayErrorOnUpdate += () => DisplayAlert("Error", "There was an error on announcement update!", "OK");
            vm.DisplayApplicationError += () => DisplayAlert("Error", "Application error, contact the administrator!", "OK");
            InitializeComponent();
        }
    }
}