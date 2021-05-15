﻿using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class SelectedFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private readonly Food donatedfood;
        public ICommand UserClickedCommand { get; set; }
        public ICommand SendMessageCommand { protected set; get; }

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
        
        public SelectedFoodViewModel(Food _donatedfood)
        {
            SendMessageCommand = new Command(OnSendMessageClicked);
            UserClickedCommand = new Command(OnUserNameClicked);
            donatedfood = _donatedfood;
            ImageSource = donatedfood.ImageSource;
            FoodType = donatedfood.FoodType;
            Name = donatedfood.Name;
            Details = donatedfood.Details;
            UserName = donatedfood.User.UserName;
        }
        public async void OnUserNameClicked()
        {
            await App.Current.MainPage.Navigation.PushAsync(new PublicProfilePage(donatedfood.User)); //TO DO who s profile???
        }
        public async void OnSendMessageClicked()
        {
           // await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
