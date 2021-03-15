using FoodSharing.Models;
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
            private Regex regexemail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$");//the email format

    //        public string Email
    //        {
    //            get { return email; }
    //            set
    //            {
    //                email = value;
    //                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
    //            }
    //        }
    //        private string password;
    //        public string Password
    //        {
    //            get { return password; }
    //            set
    //            {
    //                password = value;
    //                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
    //            }
    //        }

    //        public ICommand RegisterCommand { protected set; get; }
    //        public ICommand ChangeToSignIn { protected set; get; }
    //        public CreateUserViewModel()
    //        {
    //            RegisterCommand = new Command(OnRegister);//the constructor has 2 commands the register and signin
    //            ChangeToSignIn = new Command(OnChangeToSignIn);
    //        }
    //        async public void OnRegister()
    //        {
    //            if (!regexemail.IsMatch(email.ToUpper()))
    //            {
    //                DisplayInvalidEmail(); //if the email doesn't have the right format it displays this alert
    //                return;
    //            }
    //            //User user = await App.Database.GetUserAsync(email);
    //            //if (user != null)
    //            //{
    //              //  DisplayTakenEmail(); //if there is a user with that email in our database it displays this alert
    //            //}
    //           //else if (!string.IsNullOrEmpty(Password)) //if the password string is not empty
    //            //{
    //               // await App.Database.SaveUpdateUserAsync(new User() { Email = email, Password = UserHelper.CreateMD5(password) });//it creates a new user in the database with the given email and md5 password
    //              //  DisplayUserCreated();
    //            //}
    //            else
    //        //    {
    //        //        DisplayNoPassword();//if the password string is empty it displays this alert
    //        //    }
    //        //}
    //        public void OnChangeToSignIn()
    //        {
    //            LoginPage Page = new LoginPage();
    //            Application.Current.MainPage = Page;
    //        }
    }
}


