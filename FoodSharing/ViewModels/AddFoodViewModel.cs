using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class AddFoodViewModel
    {
        public ICommand CreateProductCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public AddFoodViewModel()
        {
            CreateProductCommand = new Command(OnCreateProduct);
            HomeCommand = new Command(OnHome);
        }

        public void OnCreateProduct()
        {
            //TO DO ADD product to DB
        }

        public async void OnHome()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
