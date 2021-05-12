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
        public Action DisplayProfileUpdateError;
        public Action DisplayFoodDeleted;
        public Action DisplayFoodDeletedError;
        public Action DisplayErrorOnUpdate;
        public Action DisplayFatalError;
        public Action DisplayUpdatedFood;
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ICommand PublicProfileCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
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
            Foods = new ObservableCollection<Food>();
            SelectedChangedFood = new Command(OnSelectedFood);
            PublicProfileCommand = new Command(OnPublicProfile);
            UpdateProfileCommand = new Command(OnUpdateProfile);
            BackCommand = new Command(OnBackToMainPage);
            GetFoods();
        }
        public async void GetFoods()
        {
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            List<Food> listFoods = await myFoodManager.GetTasksAsync();
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodsharingimages;AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;EndpointSuffix=core.windows.net";
            // Create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //The name of the container
            string containerName = "foodpicsblobs";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach (var item in listFoods)
            {
                if (user.Id == item.UserID)
                {
                    // Get a reference to a blob
                    BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);
                    item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
                    Foods.Add(item);
                }
            }
        }
        //public async void OnMoreClicked()
        //{
        //    var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
        //    RestService restSevice = new RestService();
        //    FoodManager myFoodManager = new FoodManager(restSevice);
        //    Response response = await myFoodManager.UpdateTaskAsync(SelectedFood);
        //    switch (response.Status)
        //    {
        //        case Constants.Status.Error:
        //            {
        //                switch (response.Message)
        //                {
        //                    case Constants.APIMessages.ErrorOnDeletion:
        //                        {
        //                            DisplayErrorOnUpdate();
        //                            break;
        //                        }
        //                    default:
        //                        {
        //                            DisplayFatalError();
        //                            break;
        //                        }
        //                }
        //            }
        //            break;
        //        case Constants.Status.Success:
        //            {
        //                DisplayUpdatedFood();
        //                await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        //                break;
        //            }
        //        default:
        //            {
        //                DisplayFatalError();
        //                break;
        //            }
        //    }
        //}

        public async void OnBackToMainPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
        public async void OnDeleteClicked()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            Response response = await myFoodManager.DeleteTaskAsync(selectedFood);
            switch (response.Status)
            {
                case Constants.Status.Error:
                    {
                        switch (response.Message)
                        {
                            case Constants.APIMessages.ErrorOnDeletion:
                                {
                                    DisplayFoodDeletedError();
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
                        DisplayFoodDeleted();
                        //TO DO refresh?
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
