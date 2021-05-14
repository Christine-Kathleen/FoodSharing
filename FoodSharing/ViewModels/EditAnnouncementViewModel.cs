using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class EditAnnouncementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private readonly Food donatedfood;

        public Action DisplayFoodDeleted;
        public Action DisplayFoodDeletedError;
        public Action DisplayErrorOnUpdate;
        public Action DisplayUpdatedFood;
        public Action DisplayApplicationError;
        public Action DisplayFatalError;

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            private set
            {
                imageSource = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImageSource"));
            }
        }
        private TypeOfFood foodType;
        public TypeOfFood FoodType
        {
            get { return foodType; }
            private set
            {
                foodType = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FoodType"));
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            private set
            {
                name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        private string details;
        public string Details
        {
            get { return details; }
            private set
            {
                details = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Details"));
            }
        }
        private Location foodLoc;
        public Location FoodLoc
        {
            get { return foodLoc; }
            private set
            {
                foodLoc = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FoodLoc"));
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
        private string distance;
        public string Distance
        {
            get { return distance; }
            private set
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
            donatedfood = _donatedfood;
            ImageSource = donatedfood.ImageSource;
            FoodType = donatedfood.FoodType;
            Name = donatedfood.Name;
            Details = donatedfood.Details;
            UserName = donatedfood.User.UserName;
        }
        public async void OnDeleteClicked()
        {
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            RestService restSevice = new RestService();
            FoodManager myFoodManager = new FoodManager(restSevice);
            Response response = await myFoodManager.DeleteFoodAsync(selectedFood);
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
    }
}


