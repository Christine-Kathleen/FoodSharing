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

namespace FoodSharing.ViewModels
{
    public class AddFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand CreateProductCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand TakePicCommand { get; set; }
        public ICommand ImageTapped { get; set; }


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

        public async void OnCreateProduct()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            await myFoodManager.SaveTaskAsync(new Food { Name = FoodName,  Details = FoodDetails, FoodType = (TypeOfFood)Enum.Parse(typeof(TypeOfFood),(string)TypeSelection), AnnouncementAvailability=Availability.Available, UserID=user.Id, FoodLocationLatitude=user.UserLocLatitude, FoodLocationLongitude=user.UserLocLongitude });
            //must add location&Pic
            //TO DO take location from the user device
            //TO DO alert
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
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
                //not really mvvm conform!!!!
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

            //not really mvvm conform!!!!
            await App.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            TakePhoto = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
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
    }
}
