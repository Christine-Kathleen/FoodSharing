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
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(51, 153, 137);
            vm.DisplayCompleteFields += () => DisplayAlert("Error", "All fields must be completed!", "OK");
            vm.DisplayFatalError += () => DisplayAlert("Error", "Fatal Error!", "OK");
            vm.DisplayFoodCreated += () => DisplayAlert("Success", "Food announcement was created!", "OK");
            vm.DisplayNotSupportedOnDevice += () => DisplayAlert("Error", "Location is not supported on this device, please change device. The location will be set as default to Alba Iulia.", "OK");
            vm.DisplayUnableToGetLocation += () => DisplayAlert("Error", "Unable to get location. The location will be set as default to Alba Iulia.", "OK");
            vm.DisplayNotEnabledOnDevice += () => DisplayAlert("Error", "Location not enabled on device. The location will be set as default to Alba Iulia.", "OK");
            vm.DisplayPermissionException += () => DisplayAlert("Error", "There was a permission exception. The location will be set as default to Alba Iulia.", "OK");

            InitializeComponent();
        }
    }
}