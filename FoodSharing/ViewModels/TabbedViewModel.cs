using FoodSharing.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using FoodSharing.Models;
using FoodSharing.Services;
using System.Collections.Generic;
using Azure.Storage.Blobs;
using System.IO;

namespace FoodSharing.ViewModels
{
    public class TabbedViewModel : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
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

        public TabbedViewModel()
        {
            AddCommand = new Command(OnAdd);
            SelectedChangedFood = new Command(OnSelectedFood);
            Foods = new ObservableCollection<Food>();
            //Foods.Add(new Food() { Name = "oranges", Details = "fresh", /*FoodLoc = new Location(46.0667, 23.5833),*/ ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore, UserID = "1"}); ;
            //Foods.Add(new Food() { Name = "cake", Details = "vanilla", /*FoodLoc = new Location(46.114912810335994, 23.6575937791188),*/ ImageUrl = "loginpic.jpeg", FoodType = TypeOfFood.HomeMade });
            //Foods.Add(new Food() { Name = "apples", Details = "red & green", /*FoodLoc = new Location(46.770439, 23.591423),*/ ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            //Foods.Add(new Food() { Name = "lime", Details = "fresh", /*FoodLoc = new Location(46.03333, 23.56667),*/ ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            //Foods.Add(new Food() { Name = "tomato", Details = "red", /*FoodLoc = new Location(46.0667, 23.5833),*/ ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            //Foods.Add(new Food() { Name = "onion", Details = "white", /*FoodLoc = new Location(46.0667, 23.5833),*/ ImageUrl = "loginBG.jpeg", FoodType = TypeOfFood.FromStore });
            //foreach (var food in Items)
            //{
            //    food.SetUserLoc(User.Instance.UserLoc);
            //}
            GetFoods();
        }
        public async void GetFoods()
        {
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            List<Food> listFoods = await myFoodManager.GetTasksAsync();
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodsharingimages;AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;EndpointSuffix=core.windows.net";
            // Create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //      TakePhoto.

            //The name of the container
            string containerName = "foodpicsblobs";// + Guid.NewGuid().ToString();
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach (var item in listFoods)
            {
                //TODO to be removed , when no foods without imageurl!!!
                //if (!string.IsNullOrEmpty(item.ImageUrl))
                //{

                    // Get a reference to a blob
                    BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);

                    //var byteData = Encoding.UTF8.GetBytes(photo);
                    //MemoryStream imagestream;
                    //await blobClient.DownloadToAsync( MemoryStream image);
                    //item.ImageSource = ImageSource.FromStream(() =>imagestream);

                    /*MemoryStream downloadFileStream = new MemoryStream(1000000)*/;
                    
                        //await blobClient.DownloadToAsync(downloadFileStream);
                        item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
                    //downloadFileStream.Close();
                    
                //}
                Foods.Add(item);
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

        public async void OnAdd()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddFoodPage());
        }
    }
}

