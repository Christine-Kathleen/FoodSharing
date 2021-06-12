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
    public partial class FromStorePage : ContentPage
    {
        public FromStorePage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            var vm = new FromStoreViewModel(Models.TypeOfFood.FromStore);
            var navigationPage = Application.Current.MainPage as NavigationPage;
            this.BindingContext = vm;
            vm.DisplayNotSupportedOnDevice += () => DisplayAlert("Error", "Location is not supported on this device, please change device. The location will be set as default to Alba Iulia.", "OK");
            vm.DisplayUnableToGetLocation += () => DisplayAlert("Error", "Unable to get location. The location will be set as default to Alba Iulia.", "OK");
            vm.DisplayNotEnabledOnDevice += () => DisplayAlert("Error", "Location not enabled on device. The location will be set as default to Alba Iulia.", "OK");
            vm.DisplayPermissionException += () => DisplayAlert("Error", "There was a permission exception. The location will be set as default to Alba Iulia.", "OK");   
        }
        protected override bool OnBackButtonPressed()
        {
            return true ;
        }
    }
}