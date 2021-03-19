
using FoodSharing.Models;
using FoodSharing.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayNoPassword;
        public Action DisplayInvalidEmail;
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private Regex regexemail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$");
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

        public ICommand SubmitCommand { protected set; get; }
        public ICommand CreateUserCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
            CreateUserCommand = new Command(OnCreateUser);
        }

        public void OnSubmit()
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
                //}
                //else
                //{
                User user = new User(){ UserName="test",Email="test@test.com"};;
                //Shell.Current.GoToAsync(nameof(MainPage));
                //DO write shell to navigate
                //new NavigationPage(new MainPage(user));
                MainPage Page = new MainPage(user);
                Application.Current.MainPage = Page;
                //}
            }

        }
        public void OnCreateUser()
        {
            CreateUserPage Page = new CreateUserPage();
            Application.Current.MainPage = Page;
        }
    }
}
