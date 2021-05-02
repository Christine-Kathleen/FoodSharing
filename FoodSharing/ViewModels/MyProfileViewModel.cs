using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
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
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand PublicProfileCommand { protected set; get; }
        public ICommand UpdateProfileCommand { protected set; get; }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            private set
            {
                firstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            private set
            {
                lastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            private set
            {
                userName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            private set
            {
                description = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }
        public MyProfileViewModel()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            UserManager myUserManager = new UserManager(restSevice);
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.UserName = UserName;
            user.Description = Description;
            PublicProfileCommand = new Command(OnPublicProfile);
            UpdateProfileCommand = new Command(OnUpdateProfile);
        }
        public async void OnPublicProfile()
        {
            await App.Current.MainPage.Navigation.PushAsync(new PublicProfilePage());
        }
        public async void OnUpdateProfile()
        {
           //TO DO update
        }
    }
}
