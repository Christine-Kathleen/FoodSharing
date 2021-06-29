using System.Collections.Generic;
using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace FoodSharing.ViewModels
{
    public class BaseFoodViewModel : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        public ICommand SelectedChangedFood { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private readonly TypeOfFood FoodType;
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

        readonly ApplicationUser user;

        public BaseFoodViewModel(TypeOfFood type)
        {
            user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
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
            List<Food> listFoods = await myFoodManager.GetFoodsAsync();

            string connectionString = Constants.connectionString;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "foodpicsblobs";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach (var item in listFoods)
            {
                if (item.FoodType != foodtype)
                    continue;
                item.SetUserLoc(new Location(user.UserLocLatitude, user.UserLocLongitude));
                BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);
                item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
                Foods.Add(item);
            }
        }
        public async void OnSelectedFood()
        {
            if (selectedFood != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new SelectedFoodPage(selectedFood));
                SelectedFood = null;
            }
        }

        public async void OnAdd()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddFoodPage());
        }
    }
}


