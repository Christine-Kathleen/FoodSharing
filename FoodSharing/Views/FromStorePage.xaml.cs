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
        }
        protected override bool OnBackButtonPressed()
        {
            return true ;
        }
    }
}