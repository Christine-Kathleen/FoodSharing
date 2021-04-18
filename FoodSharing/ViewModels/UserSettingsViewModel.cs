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
    public class UserSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        //private string email;
        //private readonly User user;
        public Action DisplayDeletedAccount;
        public Action DisplayNoPassword;
        public Action DisplayPasswordChanged;

        //public string Email
        //{
        //    get { return user.Email; }
        //}
        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NewPassword"));
            }
        }

        public ICommand BackToMainPageCommand { protected set; get; }
        public ICommand DeleteAccountCommand { protected set; get; }
        public ICommand PasswordChangeCommand { protected set; get; }



        public UserSettingsViewModel()
        {
            //email = user.Email;
            BackToMainPageCommand = new Command(OnBackToMainPage);
            DeleteAccountCommand = new Command(OnDeleteAccount);
            PasswordChangeCommand = new Command(OnPasswordChange);
        }
        async public void OnBackToMainPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
            //MainPage Page = new MainPage();
            //Application.Current.MainPage = Page;
        }
        async public void OnDeleteAccount()
        {
            //await App.Database.DeleteUserAsync(user);
            DisplayDeletedAccount();
            LoginPage Page = new LoginPage();
            Application.Current.MainPage = Page;
        }

        async public void OnPasswordChange()
        {
            if (!string.IsNullOrEmpty(NewPassword))
            {
                //User.Instance.Password = UserHelper.CreateMD5(NewPassword);
               // await App.Database.SaveUpdateUserAsync(user);
                DisplayPasswordChanged();
            }
            else
            {
                DisplayNoPassword();
            }
        }

    }
}
