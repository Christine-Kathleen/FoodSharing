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
    public partial class SelectedFoodPage : ContentPage
    {
       
        public SelectedFoodPage(Food donatedfood)
        {
            var vm = new SelectedFoodViewModel(donatedfood);
            this.BindingContext = vm;
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(112, 174, 152);
        }
    }
}