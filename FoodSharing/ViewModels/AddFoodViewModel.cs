using Android;
using Android.Content.PM;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Azure.Storage;
using Azure.Storage.Blobs;
using System.IO;
using System.Runtime.CompilerServices;

namespace FoodSharing.ViewModels
{
    public class AddFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action DisplayCompleteFields;
        public Action DisplayFoodCreated;
        public Action DisplayFatalError;
        public ICommand CreateProductCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand TakePicCommand { get; set; }
        public ICommand ImageTapped { get; set; }

        private byte[] photo;
        private string foodDetails;
        public string FoodDetails
        {
            get { return foodDetails; }
            set
            {
                foodDetails = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FoodDetails"));
            }
        }

        private string foodName;
        public string FoodName
        {
            get { return foodName; }
            set
            {
                foodName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FoodName"));
            }
        }
        private object typeSelection;
        public object TypeSelection
        {
            get { return typeSelection; }
            set
            {
                typeSelection = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TypeSelection"));
            }
        }
        
        private ImageSource takePhoto;
        public ImageSource TakePhoto
        {
            get { return takePhoto; }
            set
            {
                takePhoto = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TakePhoto"));
            }
        }
        public AddFoodViewModel()
        {
            CreateProductCommand = new Command(OnCreateProduct);
            HomeCommand = new Command(OnHome);
            TakePicCommand = new Command(OnTakePic);
            ImageTapped = new Command(OnImageTapped);

            
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
        public async void OnCreateProduct()
        {
            if (string.IsNullOrEmpty(FoodName) || string.IsNullOrEmpty(FoodDetails) || TypeSelection == null || TakePhoto == null)
            {
                DisplayCompleteFields();
                IsBusy = false;
            }
            else
            {
                IsBusy = true;
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodsharingimages;AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;EndpointSuffix=core.windows.net";
               
                // Create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                string containerName = "foodpicsblobs";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                containerClient.CreateIfNotExists();
                string fileURL = Guid.NewGuid().ToString();

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(fileURL);
                await blobClient.UploadAsync(new MemoryStream(photo), true);

                var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
                RestService restSevice = new RestService();
                FoodManager myFoodManager = new FoodManager(restSevice);
                Response response = await myFoodManager.SaveFoodAsync(new Food {
                    ImageUrl = fileURL,
                    Name = FoodName,
                    Details = FoodDetails,
                    FoodType = (TypeOfFood)Enum.Parse(typeof(TypeOfFood),
                    (string)TypeSelection),
                    AnnouncementAvailability = Availability.Available,
                    UserID = user.Id,
                    FoodLocationLatitude = user.UserLocLatitude,
                    FoodLocationLongitude = user.UserLocLongitude });
                switch (response.Status)
                {
                    case Constants.Status.Error:
                        {
                            DisplayFatalError();
                            break;
                        }
                    case Constants.Status.Success:
                        {
                            DisplayFoodCreated();
                            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                            break;
                        }
                    default:
                        {
                            DisplayFatalError();
                            break;
                        }
                }
                //TO DO take location from the user device   
                IsBusy = false;
            }
        }

        public async void OnHome()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
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

            //Save photo as a byte array to store later in azure
            photo = new byte[file.GetStream().Length];
            file.GetStream().Read(photo, 0, (int)file.GetStream().Length);
            MemoryStream test = new MemoryStream(photo);
            //file.GetStream().CopyTo(test);
            TakePhoto = ImageSource.FromStream(() => test);
            //{
            //  var stream = file.GetStream();
            //    return stream;
            //});
        }
        private async void OnImageTapped()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                // open the maps app directly
                await Launcher.OpenAsync("geo:0,0?q=394+Pacific+Ave+San+Francisco+CA");
            }
            //Code to execute on tapped event
            //    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
            //    {
            //    StartRequestingLocationUpdates();
            //    isRequestingLocationUpdates = true;
            //    }
            //    else
            //    {
            //    // The app does not have permission ACCESS_FINE_LOCATION 
            //}

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
