using FoodSharing.Models;
using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class TabbedViewModel : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Items { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        object selectedFood;
        public object SelectedFood
        {
            get
            {
                return selectedFood;
            }
            set
            {
                if (selectedFood != value)
                {
                    selectedFood = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFood"));
                }
            }
        }

        public TabbedViewModel()
        {
            AddCommand = new Command(OnAdd);
            SelectedChangedFood = new Command(OnSelectedFood);
            Items = new ObservableCollection<Food>();
            Items.Add(new Food() { Name = "oranges", Details = "fresh", FoodLoc = new Location(46.0667, 23.5833), ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore, UserID = 1}); ;
            Items.Add(new Food() { Name = "cake", Details = "vanilla", FoodLoc = new Location(46.114912810335994, 23.6575937791188), ImageUrl = "loginpic.jpeg", FoodType = TypeOfFood.HomeMade });
            Items.Add(new Food() { Name = "apples", Details = "red & green", FoodLoc = new Location(46.770439, 23.591423), ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            Items.Add(new Food() { Name = "lime", Details = "fresh", FoodLoc = new Location(46.03333, 23.56667), ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            Items.Add(new Food() { Name = "tomato", Details = "red", FoodLoc = new Location(46.0667, 23.5833), ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            Items.Add(new Food() { Name = "onion", Details = "white", FoodLoc = new Location(46.0667, 23.5833), ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            foreach (var food in Items)
            {
                food.SetUserLoc(User.Instance.UserLoc);
            }

        }
        public async void OnSelectedFood()
        {
            if (selectedFood != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new SelectedFoodPage((Food)selectedFood));
                SelectedFood = null;
            }
        }

        public async void OnAdd()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddFoodPage());
        }
    }
}

