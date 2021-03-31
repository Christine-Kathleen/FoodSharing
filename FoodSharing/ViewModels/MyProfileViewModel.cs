using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand PublicProfileCommand { protected set; get; }
        public ICommand UpdateProfileCommand { protected set; get; }
        
        public MyProfileViewModel()
        {
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
