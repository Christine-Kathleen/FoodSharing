using FoodSharing.Models;
using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string email; 
        public string Email
        {
            get { return email; }
            private set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        public ICommand LogOutUserCommand { protected set; get; }
        public ICommand AccountSettingsCommand { protected set; get; }
        public ICommand MyGamesCommand { protected set; get; }
        public ICommand GameCategoriesCommand { protected set; get; }
        public MainViewModel()
        {
            LogOutUserCommand = new Command(OnLogOut);
            AccountSettingsCommand = new Command(OnAccountSettings);
        }      
        public async void OnAccountSettings()
        {
            await App.Current.MainPage.Navigation.PushAsync(new UserSettingsPage());
        }
        public async void OnLogOut()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}

