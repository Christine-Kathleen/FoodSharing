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
using System.Threading.Tasks;
using System.Threading;

namespace FoodSharing.ViewModels
{
    public class AddFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action DisplayCompleteFields;
        public Action DisplayFoodCreated;
        public Action DisplayFatalError;
        public Action DisplayNotSupportedOnDevice;
        public Action DisplayPermissionException;
        public Action DisplayNotEnabledOnDevice;
        public Action DisplayUnableToGetLocation;

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
        private Task<Location> GetLoc;
        public AddFoodViewModel()
        {
            CreateProductCommand = new Command(OnCreateProduct);
            HomeCommand = new Command(OnHome);
            TakePicCommand = new Command(OnTakePic);
            GetLoc = GetCurrentLocation();
            

            
        }
        bool isBusy = false;
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
            Location loc=await GetLoc;
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            if (loc.Latitude!=0&&loc.Longitude!=0)
            {
                user.UserLocLatitude = loc.Latitude;
                user.UserLocLongitude = loc.Longitude;
                Preferences.Set("User", JsonConvert.SerializeObject(user));
            }
            if (string.IsNullOrEmpty(FoodName) || string.IsNullOrEmpty(FoodDetails) || TypeSelection == null || TakePhoto == null)
            {
                DisplayCompleteFields();
                IsBusy = false;
            }
            else
            {
                IsBusy = true;
                string connectionString = "DefaultEndpointsProtocol=https;" +
                    "AccountName=foodsharingimages;" +
                    "AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;" +
                    "EndpointSuffix=core.windows.net";
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                string containerName = "foodpicsblobs";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                containerClient.CreateIfNotExists();
                string fileURL = Guid.NewGuid().ToString();

                BlobClient blobClient = containerClient.GetBlobClient(fileURL);
                await blobClient.UploadAsync(new MemoryStream(photo), true);

                RestService restSevice = new RestService();
                FoodManager myFoodManager = new FoodManager(restSevice);
                Response response = await myFoodManager.SaveFoodAsync(new Food {
                    ImageUrl = fileURL,
                    Name = FoodName,
                    Details = FoodDetails,
                    FoodType = (TypeOfFood)Enum.Parse(typeof(TypeOfFood),
                    (string)TypeSelection),
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
                            IsBusy = true;
                            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                            DisplayFoodCreated();
                            IsBusy = false;
                            break;
                        }
                    default:
                        {
                            DisplayFatalError();
                            break;
                        }
                }
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

            photo = new byte[file.GetStream().Length];
            file.GetStream().Read(photo, 0, (int)file.GetStream().Length);
            MemoryStream test = new MemoryStream(photo);
            TakePhoto = ImageSource.FromStream(() => test);

        }

        CancellationTokenSource cts;

        async Task<Location> GetCurrentLocation()
        {
            var location = new Location();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
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
