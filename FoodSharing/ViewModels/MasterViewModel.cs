using FoodSharing.Models;
using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand HomeCommand { protected set; get; }
        public ICommand MyProfileCommand { protected set; get; }
        public ICommand AccountSettingsCommand { protected set; get; }
        public ICommand HelpGrowCommand { protected set; get; }
        public ICommand LogoutCommand { protected set; get; }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
        public MasterViewModel()
        {
            HomeCommand = new Command(OnHome);
            MyProfileCommand = new Command(OnMyProfile);
            AccountSettingsCommand = new Command(OnAccountSettings);
            HelpGrowCommand = new Command(OnHelpGrow);
            LogoutCommand = new Command(OnLogout);

        }

        public async void OnHome()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
        public async void OnMyProfile()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MyProfile());
        }

        public async void OnAccountSettings()
        {
            await App.Current.MainPage.Navigation.PushAsync(new UserSettingsPage());
        }
        public async void OnHelpGrow()
        {
            await App.Current.MainPage.Navigation.PushAsync(new HelpGrowPage());
        }
        public async void OnLogout()
        {
            Username = "";
            Password = "";
            Preferences.Remove("User");
            Preferences.Remove("BearerToken");
            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

    }
}
