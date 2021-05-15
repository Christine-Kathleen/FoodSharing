using FoodSharing.Models;
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
    public class CommunicateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand UserClickedCommand { get; set; }
        public ICommand SendMessageCommand { protected set; get; }
        
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
        public CommunicateViewModel()
        {
            SendMessageCommand = new Command(OnSendMessageClicked);
            UserClickedCommand = new Command(OnUserNameClicked);
            var user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            UserName = user.UserName;
        }

        public async void OnUserNameClicked()
        {
        }
        public async void OnSendMessageClicked()
        {
        }

    }
}
