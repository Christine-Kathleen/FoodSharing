using System.Collections.Generic;
using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class BaseFoodViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        object selectedFood;

        private TypeOfFood FoodType;
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

        public BaseFoodViewModel(TypeOfFood type)
        {
            FoodType = type;
            AddCommand = new Command(OnAdd);
            SelectedChangedFood = new Command(OnSelectedFood);
            Foods = new ObservableCollection<Food>();
            GetFoods(FoodType);
        }

        public async void GetFoods(TypeOfFood foodtype)
        {
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            List<Food> listFoods = await myFoodManager.GetFoodssAsync();
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodsharingimages;AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;EndpointSuffix=core.windows.net";
            // Create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //The name of the container
            string containerName = "foodpicsblobs";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach (var item in listFoods)
            {
                if (item.FoodType != foodtype)
                    continue;
                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);
                item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
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


