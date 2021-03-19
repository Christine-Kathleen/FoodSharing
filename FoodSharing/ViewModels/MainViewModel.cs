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
        private readonly User user;

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


        public MainViewModel(User _user)
        {
            user = _user;
            email = user.Email;
            LogOutUserCommand = new Command(OnLogOut);
            AccountSettingsCommand = new Command(OnAccountSettings);
           
            //test();
            //Task t= App.Database.SaveCategoryAsync(new Category() { UserID = user.ID, CategoryName = "FPS" });

            // App.Database.SaveGameAsync(new Game() { UserID = user.ID, });
        }

        //public async void test()
        //{
        //   int Catid =await App.Database.SaveCategoryAsync(new Category() { UserID = user.ID, CategoryName = "FPS" });
        //    await App.Database.SaveGameAsync(new Game() { UserID = user.ID,CategoryID=Catid,Description="My fav FPS",Name="CallOfDuty",ReleaseDate=new System.DateTime(2012,01,02) });
        //    List<Game> g = await App.Database.GetGamesAsync();
        //}
       
        public void OnAccountSettings()
        {
            UserSettingsPage Page = new UserSettingsPage(user);
            Application.Current.MainPage = Page;
        }

        public void OnLogOut()
        {
            LoginPage Page = new LoginPage();
            Application.Current.MainPage = Page;
        }
    }
}

