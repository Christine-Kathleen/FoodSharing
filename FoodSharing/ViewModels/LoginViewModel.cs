using FoodSharing.Models;
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
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FoodSharing.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayNoPassword;
        public Action DisplayFailedLogin;
        public Action DisplayInvalidEmail;
        public Action DisplayCompleteFields;
        public Action DisplayApplicationError;
        public Action DisplayInvalidLoginPrompt;
        public Action DisplayNotSupportedOnDevice;
        public Action DisplayPermissionException;
        public Action DisplayNotEnabledOnDevice;
        public Action DisplayUnableToGetLocation;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string username = "Cristina";
        private string password = "Password@1234";
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
        public bool IsNotBusy
        {
            get { return isNotBusy; }
            set { SetProperty(ref isNotBusy, value); }
        }
       // private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
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
       // private string password;
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
            LoginCommand = new Command(OnLoginClicked);
            CreateUserCommand = new Command(OnCreateUserClicked);
        }

        public async void OnLoginClicked()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                DisplayCompleteFields();
            }
            IsBusy = true;
            RestService userRestService = new RestService();
            if (await userRestService.AuthWithCredentialsAsync(Username, Password))
            {
                ApplicationUser user = await userRestService.GetUser(Username, Password);
                if (user != null)
                {                   
                    Location loc=await GetCurrentLocation();
                    user.UserLocLatitude = loc.Latitude;
                    user.UserLocLongitude = loc.Longitude;
                    Preferences.Set("User", JsonConvert.SerializeObject(user));
                    await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                }
                else
                {
                    DisplayApplicationError();
                }
            }
            else
            {
                DisplayFailedLogin();
                IsBusy = false;
            }

        }

        async Task<Location> GetCurrentLocation()
        {
            var location = new Location();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                CancellationTokenSource cts = new CancellationTokenSource();
                location = await Geolocation.GetLocationAsync(request, cts.Token);
            }
            catch (FeatureNotSupportedException)
            {
                DisplayNotSupportedOnDevice();
            }
            catch (FeatureNotEnabledException)
            {
                DisplayNotEnabledOnDevice();
            }
            catch (PermissionException)
            {
                DisplayPermissionException();
            }
            catch (Exception)
            {
                DisplayUnableToGetLocation();
            }
            return location;
        }
        public async void OnCreateUserClicked()
        {

            await App.Current.MainPage.Navigation.PushAsync(new CreateUserPage());
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
