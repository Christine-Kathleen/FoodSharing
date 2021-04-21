﻿using FoodSharing.Models;
using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using FoodSharing.Services;

namespace FoodSharing.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayNoPassword;
        public Action DisplayInvalidEmail;
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private Regex regexemail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$");
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
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
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        public ICommand LoginCommand { protected set; get; }
        public ICommand CreateUserCommand { protected set; get; }
        public LoginViewModel()
        {
            //create fake user to enter app easy
            ID = 1;
            Email = "belep@elek.hu";
            Password = "semmi";


            LoginCommand = new Command(OnLoginClicked);
            CreateUserCommand = new Command(OnCreateUserClicked);
        }

        public async void OnLoginClicked()
        {
            if (!regexemail.IsMatch(email.ToUpper()))
            {
                DisplayInvalidEmail();
            }
            else if (string.IsNullOrEmpty(password))
            {
                DisplayNoPassword();
            }
            else
            {
                //User user = await App.Database.GetUserAsync(email, UserHelper.CreateMD5(password));
                //if (user == null)
                //{
                //    DisplayInvalidLoginPrompt();

                //else
                //{

                //var newPage = new MainPage(user);
                //object p = await NavigationPage.PushAsync(newPage);


                //User.Instance.Email = Email;
                //User.Instance.Password = Password;
                //User.Instance.UserLoc = new Location(46.2, 23.68);
                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                //}
            }
        }
        public async void OnCreateUserClicked()
        {

            await App.Current.MainPage.Navigation.PushAsync(new CreateUserPage());
        }
    }
}
