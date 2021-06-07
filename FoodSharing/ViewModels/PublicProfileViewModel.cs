using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class PublicProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action DisplayReviewError;
        public Action DisplayReviewAdded;
        public Action DisplayFatalError;
        public Action DisplayCompleteFields;
        public ObservableCollection<Review> Reviews { get; set; }
        public ICommand SelectedChangedFood { get; set; }
        public ICommand PostReviewCommand { get; set; }
        public ObservableCollection<Food> Foods { get; set; }

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
        private string review;
        public string Review
        {
            get { return review; }
            set
            {
                review = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Review"));
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

        private ApplicationUser reviewedUser;
        public PublicProfileViewModel(ApplicationUser user)
        {
            reviewedUser = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Description = user.Description;
            Foods = new ObservableCollection<Food>();
            Reviews = new ObservableCollection<Review>();
            SelectedChangedFood = new Command(OnSelectedFood);
            PostReviewCommand = new Command(OnPostReview);
            GetFoods();
            GetReviews();
        }
        public async void OnSelectedFood()
        {
            if (selectedFood != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new SelectedFoodPage((Food)selectedFood));
                SelectedFood = null;
            }
        }

        public async void OnPostReview()
        {
            if (string.IsNullOrEmpty(Review))
            {
                DisplayCompleteFields();
                IsBusy = false;
            }
            else
            {
                IsBusy = true;
                var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
                RestService restSevice = new RestService();
                ReviewManager myReviewManager = new ReviewManager(restSevice);
                Response response = await myReviewManager.SaveReviewAsync(new Review { ReviewContent = Review, ReviewerUserId = user.Id, ReviewedUserId=reviewedUser.Id });
                switch (response.Status)
                {
                    case Constants.Status.Error:
                        {
                            DisplayReviewError();
                            break;
                        }
                    case Constants.Status.Success:
                        {
                            DisplayReviewAdded();
                            GetReviews();
                            break;
                        }
                    default:
                        {
                            DisplayFatalError();
                            break;
                        }
                }
                IsBusy = false;
            }
        }
        public async void GetReviews()
        {
            RestService restSevice = new RestService();
            ReviewManager myReviewManager = new ReviewManager(restSevice);
            List<Review> listReviewss = await myReviewManager.GetReviewsAsync(reviewedUser.Id);
            Reviews.Clear();
            foreach (var item in listReviewss)
            { 
                    Reviews.Add(item);
            }
        }
        public async void GetFoods()
        {
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            List<Food> listFoods = await myFoodManager.GetFoodsAsync();
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodsharingimages;AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;EndpointSuffix=core.windows.net";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "foodpicsblobs";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            foreach (var item in listFoods)
            {
                if (user.Id == item.UserID)
                {
                    BlobClient blobClient = containerClient.GetBlobClient(item.ImageUrl);
                    item.ImageSource = ImageSource.FromStream(() => { var stream = blobClient.OpenRead(); return stream; });
                    Foods.Add(item);
                }
            }
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
