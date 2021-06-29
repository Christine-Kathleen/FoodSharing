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
    public partial class HomeMadePage : ContentPage
    {
        public HomeMadePage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            var vm = new HomeMadeViewModel(Models.TypeOfFood.HomeMade);
            this.BindingContext = vm;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}