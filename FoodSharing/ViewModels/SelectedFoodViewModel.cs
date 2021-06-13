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
    public class SelectedFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action DisplayMessageAlreadySent;
        public Action DisplayErrorOnSending;
        public Action DisplayFatalError;
        private readonly Food donatedfood;
        public ICommand UserClickedCommand { get; set; }
        public ICommand SendMessageCommand { protected set; get; }

        bool isBusy = false;
        bool isNotBusy = true;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
                IsNotBusy = !value;
            }

        }
        public bool IsNotBusy
        {
            get { return isNotBusy; }
            set { SetProperty(ref isNotBusy, value); }
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
        private string textToSend;
        public string TextToSend
        {
            get { return textToSend; }
            set
            {
                textToSend = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TextToSend"));
            }
        }
        readonly ApplicationUser user;
        public SelectedFoodViewModel(Food _donatedfood)
        {
            SendMessageCommand = new Command(OnSendMessageClicked);
            UserClickedCommand = new Command(OnUserNameClicked);
            user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            donatedfood = _donatedfood;
            ImageSource = donatedfood.ImageSource;
            FoodType = donatedfood.FoodType;
            Name = donatedfood.Name;
            Details = donatedfood.Details;
            Distance = donatedfood.Distance;
            UserName = donatedfood.User.UserName;

        }
        public async void OnUserNameClicked()
        {
            await App.Current.MainPage.Navigation.PushAsync(new PublicProfilePage(donatedfood.User));
        }
        public async void OnSendMessageClicked()
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                RestService restSevice = new RestService();
                MessageManager myMessageManager = new MessageManager(restSevice);
                Message newMessage = new Message { Content = TextToSend, SenderUserId = user.Id, ReceiverUserId = donatedfood.UserID, State = MessageState.Sent };
                Response response = await myMessageManager.SaveMessageAsync(newMessage);
                switch (response.Status)
                {
                    case Constants.Status.Error:
                        {
                            switch (response.Message)
                            {
                                case Constants.APIMessages.ErrorAlreadyExists:
                                    {
                                        DisplayMessageAlreadySent();
                                        break;
                                    }

                                case Constants.APIMessages.ErrorOnCreating:
                                    {
                                        DisplayErrorOnSending();
                                        break;
                                    }
                                case Constants.APIMessages.Success:
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
                            Console.WriteLine("Message sent");
                            TextToSend ="";
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
    }
}
