using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
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
        public Action DisplaySamePassword;
        public Action DisplayErrorOnDeletion;
        public Action DisplayFatalError;
        public Action DisplayFailedChange;
        public Action DisplayApplicationError;
        public Action DisplayWrongPasswordEntered;
        public Action DisplayPasswordChanged;
        public Action DisplayCompletePasswordField;
        public Action DisplayCompleteNewPasswordField;
        public Action DisplayPasswordHasNoNumber;
        public Action DisplayPasswordHasNoMinLength;
        public Action DisplayPasswordHasNoLowerCase;
        public Action DisplayPasswordHasNoUpperCase;
        public Action DisplayPasswordHasNoNonalphanumeric;
        public Action DisplayPasswordHasNoOneUniqueCharacter;
        private Regex regexTelephoneNr = new Regex(@"^07\d{8}$");
        private Regex regexPasswordHasNumber = new Regex("[0-9]+");
        private Regex regexPasswordHasMinLength = new Regex("^.{6,}$");
        private Regex regexPasswordHasLowerCase = new Regex("[a-z]");
        private Regex regexPasswordHasUpperCase = new Regex("[A-Z]");
        private Regex regexPasswordHasNonalphanumeric = new Regex(@"\W");
        private Regex regexPasswordHasOneUniqueCharacter = new Regex(@"(.)(?<!\1.+)(?!.*\1)");
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
            Response response = await myUserManager.DeleteUserAsync(user.Id);
            switch (response.Status)
            {
                case Constants.Status.Error:
                    {
                        switch (response.Message)
                        {
                            case Constants.APIMessages.ErrorOnDeletion:
                                {
                                    DisplayErrorOnDeletion();
                                    break;
                                }
                            default:
                                {
                                    DisplayFatalError();
                                    break;
                                }
                        }
                    }
                    break;
                case Constants.Status.Success:
                    {
                        DisplayDeletedAccount();
                        await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                        break;
                    }
                default:
                    {
                        DisplayFatalError();
                        break;
                    }
            }
        }

        async public void OnPasswordChange()
        {
            if (string.IsNullOrEmpty(NewPassword))
            {
                DisplayCompleteNewPasswordField();
            }
            else if (string.IsNullOrEmpty(Password))
            {
                DisplayCompletePasswordField();
            }
            else if (NewPassword == Password)
            {
                DisplaySamePassword();
            }
            else if (!regexPasswordHasNumber.IsMatch(newPassword))
            {
                DisplayPasswordHasNoNumber();
                return;
            }
            else if (!regexPasswordHasMinLength.IsMatch(newPassword))
            {
                DisplayPasswordHasNoMinLength();
                return;
            }
            else if (!regexPasswordHasLowerCase.IsMatch(newPassword))
            {
                DisplayPasswordHasNoLowerCase();
                return;
            }
            else if (!regexPasswordHasUpperCase.IsMatch(newPassword))
            {
                DisplayPasswordHasNoUpperCase();
                return;
            }
            else if (!regexPasswordHasNonalphanumeric.IsMatch(newPassword))
            {
                DisplayPasswordHasNoNonalphanumeric();
                return;
            }
            else if (!regexPasswordHasOneUniqueCharacter.IsMatch(newPassword))
            {
                DisplayPasswordHasNoOneUniqueCharacter();
                return;
            }
            else
            {
                var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
                if (user != null)
                {
                    UpdatePasswordModel model = new UpdatePasswordModel();
                    model.UserId = user.Id;
                    model.Password = Password;
                    model.NewPassword = NewPassword;
                    RestService restSevice = new RestService();
                    UserManager myUserManager = new UserManager(restSevice);
                    Response response = await myUserManager.UpdatePassword(model);
                    switch (response.Status)
                    {
                        case Constants.Status.Error:
                            {
                                switch (response.Message)
                                {
                                    case Constants.APIMessages.ErrorOnPasswordCheck:
                                        {
                                            DisplayWrongPasswordEntered();
                                            break;
                                        }
                                    default:
                                        {
                                            DisplayFatalError();
                                            break;
                                        }
                                }
                            }
                            break;
                        case Constants.Status.Success:
                            {
                                DisplayPasswordChanged();
                                break;
                            }
                        default:
                            {
                                DisplayFatalError();
                                break;
                            }
                    }
                }
                else
                {
                    DisplayApplicationError();
                }
            }
        }
    }
}
