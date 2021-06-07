using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using FoodSharing.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{

    public class MyProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action DisplayProfileUpdateMade;
        public Action DisplayProfileUpdateError;
        public Action DisplayErrorOnUpdate;
        public Action DisplayFatalError;
        public Action DisplayApplicationError;
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ICommand UpdateProfileCommand { protected set; get; }
        Food selectedFood;
        public Food SelectedFood
        {
            get
            {
                return selectedFood;
            }
            set
            {
                if (selectedFood != value)
                {
                    selectedFood = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFood"));
                }
            }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            private set
            {
                firstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            private set
            {
                lastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            private set
            {
                userName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
                SetProperty(ref isBusy, value, nameof(IsNotBusy));
            }

        }
        public bool IsNotBusy
        {
            get { return !IsBusy; }
        }
        public MyProfileViewModel()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Description = user.Description;
            Foods = new ObservableCollection<Food>();
            SelectedChangedFood = new Command(OnSelectedFood);
            UpdateProfileCommand = new Command(OnUpdateProfile);
            GetFoods();
        }
        public async void GetFoods()
        {
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            List<Food> listFoods = await myFoodManager.GetFoodsAsync();
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            string connectionString = "DefaultEndpointsProtocol=https;" +
                "AccountName=foodsharingimages;" +
                "AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;" +
                "EndpointSuffix=core.windows.net";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "foodpicsblobs";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach (var item in listFoods)
            {
                if (user.Id == item.UserID)
                {
                    BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);
                    item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
                    Foods.Add(item);
                }
            }
        }
        public async void OnSelectedFood()
        {
            if (selectedFood != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new EditAnnouncementPage(selectedFood));
                SelectedFood = null;
            }
        }
        public async void OnUpdateProfile()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            if (user != null)
            {
                UpdateUserModel model = new UpdateUserModel();
                model.UserId = user.Id;
                model.Description = Description;
                RestService restSevice = new RestService();
                UserManager myUserManager = new UserManager(restSevice);
                Response response = await myUserManager.UpdateUserAsync(model);
                switch (response.Status)
                {
                    case Constants.Status.Error:
                        {

                            DisplayFatalError();
                            break;
                        }
                    case Constants.Status.Success:
                        {
                            user.Description = Description;
                            Preferences.Set("User", JsonConvert.SerializeObject(user));
                            DisplayProfileUpdateMade();
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
            IsBusy = true;
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
