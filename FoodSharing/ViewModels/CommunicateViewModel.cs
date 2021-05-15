using FoodSharing.Models;
using FoodSharing.Services;
using Newtonsoft.Json;
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
    public class CommunicateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand UserClickedCommand { get; set; }
        public ICommand SendMessageCommand { protected set; get; }

        public ObservableCollection<Message> Messages { get; set; }

        public ObservableCollection<ApplicationUser> Users { get; set; }

        ApplicationUser user;
        public CommunicateViewModel()
        {
            SendMessageCommand = new Command(OnSendMessageClicked);
            UserClickedCommand = new Command(OnUserNameClicked);
            user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            Messages = new ObservableCollection<Message>();
            Users = new ObservableCollection<ApplicationUser>();
            GetMessages();
        }

        public async void GetMessages()
        {
            RestService restSevice = new RestService();
            MessageManager myMessageManager = new MessageManager(restSevice);
            List<Message> listMessages = await myMessageManager.GetMessagesAsync(user.Id);
            Messages.Clear();
            foreach (var item in listMessages)
            {
                Messages.Add(item);
                if (!Users.Contains(item.ReceiverId))
                {
                    Users.Add(item.ReceiverId);
                }
                if (!Users.Contains(item.SenderId))
                {
                    Users.Add(item.SenderId);
                }
            }
        }

        public async void OnUserNameClicked()
        {
        }
        public async void OnSendMessageClicked()
        {
        }

    }
}
