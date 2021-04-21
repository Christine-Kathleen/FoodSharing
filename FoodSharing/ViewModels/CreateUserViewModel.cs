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
        public class CreateUserViewModel : INotifyPropertyChanged
        {
            public Action DisplayTakenEmail;
            public Action DisplayNoPassword;
            public Action DisplayInvalidEmail;
            public Action DisplayUserCreated;
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
            private string email;
            private Regex regexemail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$");

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

        public ICommand RegisterCommand { protected set; get; }
        public ICommand ChangeToSignIn { protected set; get; }
        public CreateUserViewModel()
            {
                RegisterCommand = new Command(OnRegister);
                ChangeToSignIn = new Command(OnChangeToSignIn);
                
            }

        public async void OnRegister()
            {
                //if (!regexemail.IsMatch(email.ToUpper()))
                //{
                //    DisplayInvalidEmail(); 
                //    return;
                //}
                //User user = await App.Database.GetUserAsync(email);
                //if (user != null)
                //{
                //    DisplayTakenEmail(); 
                //}
                //else
                //if (!string.IsNullOrEmpty(Password)) 
                //{
                //    await App.Database.SaveUpdateUserAsync(new User() { Email = email, Password = UserHelper.CreateMD5(password) });
                //    DisplayUserCreated();
                //}
                //else
                //{
                //    DisplayNoPassword();
                //}
            }
        public async void OnChangeToSignIn()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
        //TO DO checkbox must be checked 
    }
}


