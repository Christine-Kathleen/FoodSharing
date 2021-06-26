using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class CreateUserViewModel : INotifyPropertyChanged
    {
        public Action DisplayTakenEmail;
        public Action DisplayFatalError;
        public Action DisplayCheckboxNotChecked;
        public Action DisplayTakenUserName;
        public Action DisplayNoPassword;
        public Action DisplayInvalidEmail;
        public Action DisplayUserCreated;
        public Action DisplayPhoneNrErr;
        public Action DisplayCompleteFields;
        public Action DisplayPasswordMismatch;
        public Action DisplayCheckTheCheckBox;
        public Action DisplayPasswordHasNoNumber;
        public Action DisplayPasswordHasNoMinLength;
        public Action DisplayPasswordHasNoLowerCase;
        public Action DisplayPasswordHasNoUpperCase;
        public Action DisplayPasswordHasNoNonalphanumeric;
        public Action DisplayPasswordHasNoOneUniqueCharacter;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private readonly Regex regexTelephoneNr = new Regex(@"^07\d{8}$");
        private readonly Regex regexEmail = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$");
        private readonly Regex regexPasswordHasNumber = new Regex("[0-9]+");
        private readonly Regex regexPasswordHasMinLength = new Regex("^.{6,}$");
        private readonly Regex regexPasswordHasLowerCase = new Regex("[a-z]");
        private readonly Regex regexPasswordHasUpperCase = new Regex("[A-Z]");
        private readonly Regex regexPasswordHasNonalphanumeric = new Regex(@"\W");
        private readonly Regex regexPasswordHasOneUniqueCharacter = new Regex(@"(.)(?<!\1.+)(?!.*\1)");
        bool agreeOnTerms;
        public bool AgreeOnTerms
        {
            get { return agreeOnTerms; }
            set
            {
                agreeOnTerms = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AgreeOnTerms"));
            }
        }

        private string firstName = "user";
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            }
        }
        private string lastName = "test";
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            }
        }
        private string telephone = "0756345343";
        public string Telephone
        {
            get { return telephone; }
            set
            {
                telephone = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Telephone"));
            }
        }
        private string email = "user3@yahoo.com";
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string username = "User3";
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
        private string password = "Christine23.";
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        private string checkPassword = "Christine23.";
        public string CheckPassword
        {
            get { return checkPassword; }
            set
            {
                checkPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CheckPassword"));
            }
        }
        bool isBusy = false;
        bool isNotBusy = true;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
                IsNotBusy = !value;
            }

        }
        private bool checkBoxCheckedChanged;
        public bool CheckBoxCheckedChanged
        {
            get { return checkBoxCheckedChanged; }
            set
            {
                SetProperty(ref checkBoxCheckedChanged, value);
                SetProperty(ref checkBoxCheckedChanged, value, nameof(CheckBoxCheckedChanged));
            }

        }
        public bool IsNotBusy
        {
            get { return isNotBusy; }
            set { SetProperty(ref isNotBusy, value); }
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
            IsBusy = true;
            if (AgreeOnTerms.Equals(false)) 
            {
                DisplayCheckboxNotChecked();
                IsBusy = false;
            }
            else if (!regexEmail.IsMatch(email.ToUpper()))
            {
                DisplayInvalidEmail();
                IsBusy = false;
                return;
            }
            else if (!regexPasswordHasNumber.IsMatch(password))
            {
                DisplayPasswordHasNoNumber();
                IsBusy = false;
                return;
            }
            else if (!regexPasswordHasMinLength.IsMatch(password))
            {
                DisplayPasswordHasNoMinLength();
                IsBusy = false;
                return;
            }
            else if (!regexPasswordHasLowerCase.IsMatch(password))
            {
                DisplayPasswordHasNoLowerCase();
                IsBusy = false;
                return;
            }
            else if (!regexPasswordHasUpperCase.IsMatch(password))
            {
                DisplayPasswordHasNoUpperCase();
                IsBusy = false;
                return;
            }
            else if (!regexPasswordHasNonalphanumeric.IsMatch(password))
            {
                DisplayPasswordHasNoNonalphanumeric();
                IsBusy = false;
                return;
            }
            else if (!regexPasswordHasOneUniqueCharacter.IsMatch(password))
            {
                DisplayPasswordHasNoOneUniqueCharacter();
                IsBusy = false;
                return;
            }
            else if (!regexTelephoneNr.IsMatch(Telephone))
            {
                DisplayPhoneNrErr();
                IsBusy = false;
            }
            else if (checkPassword != password)
            {
                DisplayPasswordMismatch();
                IsBusy = false;
            }
            else if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Username))
            {
                DisplayCompleteFields();
                IsBusy = false;
            }
            else
            {
                RestService restSevice = new RestService();
                UserManager myUserManager = new UserManager(restSevice);
                Response response = await myUserManager.RegisterUserAsync(new RegisterModel { 
                    Email = Email, FirstName = FirstName, LastName = LastName, UserName = Username, 
                    Telephone = Telephone, Password = Password 
                });
                switch (response.Status)
                {
                    case Constants.Status.Error:
                        {
                            switch (response.Message)
                            {
                                case Constants.APIMessages.ErrorRegisterName:
                                    {
                                        DisplayTakenUserName();
                                        IsBusy = false;
                                        break;
                                    }

                                case Constants.APIMessages.ErrorRegisterEmail:
                                    {
                                        DisplayTakenEmail();
                                        IsBusy = false;
                                        break;
                                    }
                                case Constants.APIMessages.ErrorOnRegisterFailed:
                                case Constants.APIMessages.ErrorOnDeletion:
                                case Constants.APIMessages.Success:
                                default:
                                    {
                                        DisplayFatalError();
                                        IsBusy = false;
                                        break;
                                    }
                            }
                        }
                        break;
                    case Constants.Status.Success:
                        {
                            DisplayUserCreated();
                            IsBusy = false;
                            break;
                        }
                    default:
                        {
                            DisplayFatalError();
                            IsBusy = false;
                            break;
                        }

                }
            }
        }
        public async void OnChangeToSignIn()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
          [CallerMemberName] string propertyName = "",
          Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


