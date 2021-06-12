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
        public Action DisplayFatalError;
        public Action DisplayMessageAlreadySent;
        public Action DisplayErrorOnSending;

        public ObservableCollection<Message> Messages { get; set; }

        public ObservableCollection<ApplicationUser> Users { get; set; }
        bool newMessage;
        public bool NewMessage //TO DO
        {
            get { return newMessage; }
            set
            {
                newMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NewMessage"));
            }
        }
        private string textToSend;
        public string TextToSend
        {
            get { return textToSend; }
            set
            {
                textToSend = value;
                PropertyChanged(this, new PropertyChangedEventArgs("textToSend"));
            }
        }
        private string selectedUser;
        public string SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = user.UserName;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
            }
        }
        private string sentContent;
        public string SentContent
        {
            get { return sentContent; }
            set
            {
                sentContent = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SentContent"));
            }
        }
        private string receivedContent;
        public string ReceivedContent
        {
            get { return receivedContent; }
            set
            {
                receivedContent = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ReceivedContent"));
            }
        }
        private MessageState sent;
        public MessageState Sent
        {
            get { return sent; }
            set
            {
                sent = Sent;
                PropertyChanged(this, new PropertyChangedEventArgs("Sent"));
            }
        }


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
                    //Message.Equals(sentContent);
                }
                if (!Users.Contains(item.SenderId))
                {
                    Users.Add(item.SenderId);
                }
            }
        }

        public async void OnUserNameClicked()
        {//TO DO
        }
        public async void OnSendMessageClicked()
        {//TO DO
            RestService restSevice = new RestService();
            MessageManager myMessageManager = new MessageManager(restSevice);
            Response response = await myMessageManager.SaveMessageAsync(new Message { Content = TextToSend, SenderUserId = user.Id, ReceiverUserId = selectedUser, State = Sent });
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
