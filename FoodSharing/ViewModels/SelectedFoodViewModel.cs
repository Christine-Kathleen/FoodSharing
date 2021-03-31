using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace FoodSharing.ViewModels
{
    public class SelectedFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private readonly Food donatedfood;
        private string imageURL; 
        public string ImageURL
        {
            get { return imageURL; }
            private set
            {
                imageURL = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImageURL"));
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

        public SelectedFoodViewModel(Food _donatedfood)
        {
            donatedfood = _donatedfood;
            ImageURL = donatedfood.ImageUrl;
            FoodType = donatedfood.FoodType;
            Name = donatedfood.Name;
            Details = donatedfood.Details;
            Distance = donatedfood.Distance;
        }
    }
}
