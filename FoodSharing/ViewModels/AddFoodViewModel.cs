using FoodSharing.Pages;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class AddFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand CreateProductCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand TakePicCommand { get; set; }
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
            
        }

        public void OnCreateProduct()
        {
            //TO DO ADD product to DB
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
                App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
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
    }
}
