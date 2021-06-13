using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class EditAnnouncementViewModel : INotifyPropertyChanged
    {
        private byte[] photo;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand TakePicCommand { get; set; }

        public Action DisplayFoodDeleted;
        public Action DisplayFoodDeletedError;
        public Action DisplayErrorOnUpdate;
        public Action DisplayUpdatedFood;
        public Action DisplayApplicationError;
        public Action DisplayFatalError;

        private bool IsImageChanged = false;
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImageSource"));
            }
        }
        private object foodType;
        public object FoodType
        {
            get { return foodType; }
            set
            {
                foodType = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FoodType"));
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
             set
            {
                name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        private string details;
        public string Details
        {
            get { return details; }
             set
            {
                details = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Details"));
            }
        }
        private Location foodLoc;
        public Location FoodLoc
        {
            get { return foodLoc; }
             set
            {
                foodLoc = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FoodLoc"));
            }
        }
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }
        private string distance;
        public string Distance
        {
            get { return distance; }
             set
            {
                distance = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Distance"));
            }
        }

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
        public EditAnnouncementViewModel(Food _donatedfood)
        {
            UpdateProductCommand = new Command(OnUpdateClicked);
            DeleteProductCommand = new Command(OnDeleteClicked);
            CancelCommand = new Command(OnCancelClicked);
            TakePicCommand = new Command(OnTakePic);

            selectedFood = _donatedfood;
            ImageSource = selectedFood.ImageSource;
            FoodType = selectedFood.FoodType.ToString();
            Name = selectedFood.Name;
            Details = selectedFood.Details;
            UserName = selectedFood.User.UserName;
        }
        public async void OnDeleteClicked()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            Response response = await myFoodManager.DeleteFoodAsync(selectedFood.FoodId);
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
                        await App.Current.MainPage.Navigation.PushAsync(new MyProfile());
                        break;
                    }
                default:
                    {
                        DisplayFatalError();
                        break;
                    }
            }
        }
        public async void OnUpdateClicked()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            selectedFood.Details = Details;
            selectedFood.FoodType = (TypeOfFood)Enum.Parse(typeof(TypeOfFood),(string)FoodType);
            selectedFood.Name = Name;
            if (IsImageChanged)
            {
                string connectionString = Constants.connectionString;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                string containerName = "foodpicsblobs";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                containerClient.CreateIfNotExists();

                BlobClient blobClient = containerClient.GetBlobClient(selectedFood.ImageUrl);
                await blobClient.UploadAsync(new MemoryStream(photo), true);
            }
                
            Response response = await myFoodManager.UpdateFoodAsync(SelectedFood);
            switch (response.Status)
            {
                case Constants.Status.Error:
                    {
                        switch (response.Message)
                        {
                            case Constants.APIMessages.ErrorOnDeletion:
                                {
                                    DisplayErrorOnUpdate();
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
                        DisplayUpdatedFood();
                        await App.Current.MainPage.Navigation.PushAsync(new MyProfile());
                        break;
                    }
                default:
                    {
                        DisplayFatalError();
                        break;
                    }
            }
        }
        public async void OnCancelClicked()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MyProfile());
        }

        public async void OnTakePic()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            IsImageChanged = true;

            photo = new byte[file.GetStream().Length];
            file.GetStream().Read(photo, 0, (int)file.GetStream().Length);
            MemoryStream test = new MemoryStream(photo);
            ImageSource = ImageSource.FromStream(() => test);
        }
    }
}


