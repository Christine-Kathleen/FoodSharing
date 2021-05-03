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
    public class UserSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action DisplayDeletedAccount;
        public Action DisplayNoPassword;
        public Action DisplayPasswordChanged;
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
            BackToMainPageCommand = new Command(OnBackToMainPage);
            DeleteAccountCommand = new Command(OnDeleteAccount);
            PasswordChangeCommand = new Command(OnPasswordChange);
        }
        async public void OnBackToMainPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
        async public void OnDeleteAccount()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            UserManager myUserManager = new UserManager(restSevice);
            await myUserManager.DeleteUserAsync(user.Id);
            DisplayDeletedAccount();
            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        async public void OnPasswordChange()
        {
            if (!string.IsNullOrEmpty(NewPassword))
            {
                var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
                RestService restSevice = new RestService();
                UserManager myUserManager = new UserManager(restSevice);
                //TODO update password
                await myUserManager.UpdateUserAsync(user);
                DisplayPasswordChanged();
            }
            else
            {
                DisplayNoPassword();
            }
        }

    }
}
