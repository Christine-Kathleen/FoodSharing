using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public Action DisplayProfileUpdateFatalError;
        public Action DisplayFoodUpdateFatalError;
        public Action DisplayFoodUpdateMade;
        public Action DisplayFoodDeleted;
        public Action DisplayFoodDeletedFatalError;
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ICommand PublicProfileCommand { protected set; get; }
        public ICommand UpdateProfileCommand { protected set; get; }
        object selectedFood;
        public object SelectedFood
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
            private set
            {
                description = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }
        public MyProfileViewModel()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Description = user.Description;
            PublicProfileCommand = new Command(OnPublicProfile);
            UpdateProfileCommand = new Command(OnUpdateProfile);
            //GetFoods();
        }
        //public async void GetFoods()
        //{
        //    RestService restSevice = new RestService();
        //    FoodManager myFoodManager = new FoodManager(restSevice);
        //    List<Food> listFoods = await myFoodManager.GetTasksAsync();
        //    string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodsharingimages;AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;EndpointSuffix=core.windows.net";
        //    // Create a container client
        //    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        //    //The name of the container
        //    string containerName = "foodpicsblobs";
        //    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        //    foreach (var item in listFoods)
        //    {
        //        // Get a reference to a blob
        //        BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);
        //        item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
        //        Foods.Add(item);
        //    }
        //}
        public async void OnSelectedFood()
        {
            if (selectedFood != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new SelectedFoodPage((Food)selectedFood));
                SelectedFood = null;
            }
        }
        public async void OnPublicProfile()
        {
            await App.Current.MainPage.Navigation.PushAsync(new PublicProfilePage());
        }
        public async void OnUpdateProfile()
        {
           //TO DO update
        }
    }
}
