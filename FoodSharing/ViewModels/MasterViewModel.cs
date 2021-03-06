using FoodSharing.Models;
using FoodSharing.Pages;
using Newtonsoft.Json;
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
        public ICommand PrivateProfileCommand { protected set; get; }
        public ICommand PublicProfileCommand { protected set; get; }

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
            AccountSettingsCommand = new Command(OnAccountSettings);
            HelpGrowCommand = new Command(OnHelpGrow);
            LogoutCommand = new Command(OnLogout);
            PrivateProfileCommand = new Command(OnPrivateProfile);
            PublicProfileCommand = new Command(OnPublicProfile);

        }

        public async void OnHome()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
        public async void OnPublicProfile()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            if (user is ApplicationUser)
            {
                await App.Current.MainPage.Navigation.PushAsync(new PublicProfilePage(user));
            }
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
        public async void OnPrivateProfile()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MyProfile());
        }

    }
}
