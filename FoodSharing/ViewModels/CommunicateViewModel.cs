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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FoodSharing.ViewModels
{
    public class CommunicateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand UserClickedCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public Action DisplayFatalError;
        public Action DisplayMessageAlreadySent;
        public Action DisplayErrorOnSending;
        public Action DisplayMessageNotFound;
        public Action DisplayErrorOnUpdate;

        readonly System.Drawing.Color ColorOfMessageSent;
        readonly System.Drawing.Color ColorOfMessageReceived;

        readonly LayoutOptions AlignmentForMessageSent;
        readonly LayoutOptions AlignmentForMessageReceived;

        private List<Message> AllMessagesReceivedandSentbyUser;
        public ObservableCollection<Message> Messages { get; set; }

        public ObservableCollection<ApplicationUser> Users { get; set; }
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
        private ApplicationUser selectedUser;
        public ApplicationUser SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
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

        readonly ApplicationUser user;
        public CommunicateViewModel()
        {
            ColorOfMessageSent = Color.FromRgb(51, 153, 137);
            ColorOfMessageReceived = Color.FromRgb(255, 250, 251);
            AlignmentForMessageSent = LayoutOptions.End;
            AlignmentForMessageReceived = LayoutOptions.Start;
            SendMessageCommand = new Command(OnSendMessageClicked);
            UserClickedCommand = new Command(OnUserNameClicked);
            RefreshCommand = new Command(UpdateMessages);
            user = JsonConvert.DeserializeObject<ApplicationUser>(Preferences.Get("User", "default_value"));
            Messages = new ObservableCollection<Message>();
            Users = new ObservableCollection<ApplicationUser>();
            UpdateMessages();
        }
        public async Task<List<Message>> GetMessages()
        {
            RestService restSevice = new RestService();
            MessageManager myMessageManager = new MessageManager(restSevice);
            return await myMessageManager.GetMessagesAsync(user.Id);
        }
        public async void UpdateMessages()
        {
            AllMessagesReceivedandSentbyUser = await GetMessages();
            Device.BeginInvokeOnMainThread(() =>
            {
                UpdateUserList();
                foreach (var item in AllMessagesReceivedandSentbyUser)
                {
                    if (Users.Any(x => x.Id == item.SenderId.Id))
                    {
                        if (item.State == MessageState.Sent)
                        {
                            Users.Where(x => x.Id == item.SenderId.Id).FirstOrDefault().HasUnseenMessages = true;
                            MessageSeen(item.MessageId);
                        }
                    }
                }
                if (Users.Count > 0 && SelectedUser == null) 
                    SelectedUser = Users[0];
                OnUserNameClicked();
            });

        }
        private void UpdateUserList()
        {
            Users.Clear();
            foreach (var item in AllMessagesReceivedandSentbyUser)
            {
                if (!Users.Any(x => x.Id == item.ReceiverId.Id) && item.ReceiverId.Id != user.Id)
                {
                    item.ReceiverId.HasUnseenMessages = false;
                    Users.Add(item.ReceiverId);
                }
                if (!Users.Any(x => x.Id == item.SenderId.Id) && item.SenderId.Id != user.Id)
                {
                    item.ReceiverId.HasUnseenMessages = false;
                    Users.Add(item.SenderId);
                }
            }
            //As Users list has new references, I need to set back the appropriate User selected before
            if (SelectedUser!=null)
            SelectedUser = Users.FirstOrDefault(x => x.Id == SelectedUser.Id);
        }

        public void OnUserNameClicked()
        {
            if (SelectedUser != null)
            {
                Messages.Clear();
                foreach (var item in AllMessagesReceivedandSentbyUser)
                {
                    if (item.SenderId.Id == SelectedUser.Id || item.ReceiverId.Id == SelectedUser.Id)
                    {
                        item.ColorOfMessage = item.SenderId.Id == user.Id ? ColorOfMessageSent : ColorOfMessageReceived;
                        item.MessageAlignment = item.SenderId.Id == user.Id ? AlignmentForMessageSent : AlignmentForMessageReceived;
                        Messages.Add(item);
                    }
                }
            }
        }
        public async void MessageSeen(int id)
        {
            RestService restSevice = new RestService();
            MessageManager myMessageManager = new MessageManager(restSevice);
            Response response = await myMessageManager.UpdateMessageAsync(id);
            //To be sure, we dont have crossthread call from timer
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (response.Status)
                {
                    case Constants.Status.Error:
                        {
                            switch (response.Message)
                            {
                                case Constants.APIMessages.ErrorOnNotFound:
                                    {
                                        DisplayMessageNotFound();
                                        break;
                                    }

                                case Constants.APIMessages.ErrorOnUpdate:
                                    {
                                        DisplayErrorOnUpdate();
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
                            Console.WriteLine("Message updated as seen");
                            break;
                        }
                    default:
                        {
                            DisplayFatalError();
                            break;
                        }
                }
            });
        }
        public async void OnSendMessageClicked()
        {
            if (!string.IsNullOrEmpty(TextToSend) && SelectedUser != null)
            {
                IsBusy = true;
                RestService restSevice = new RestService();
                MessageManager myMessageManager = new MessageManager(restSevice);
                Message newMessage = new Message
                {
                    Content = TextToSend,
                    SenderUserId = user.Id,
                    ReceiverUserId = SelectedUser.Id,
                    State = MessageState.Sent
                };
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
                            IsBusy = false;
                        }
                        break;
                    case Constants.Status.Success:
                        {
                            Console.WriteLine("Message sent");
                            newMessage.SenderId = user;
                            newMessage.ReceiverId = SelectedUser;
                            newMessage.ColorOfMessage = ColorOfMessageSent;
                            newMessage.MessageAlignment = AlignmentForMessageSent;
                            Messages.Add(newMessage);
                            AllMessagesReceivedandSentbyUser.Add(newMessage);
                            TextToSend = "";
                            IsBusy = false;
                            break;
                        }
                    default:
                        {
                            DisplayFatalError();
                            IsBusy = false;
                            break;
                        }
                }
            }
        }
    }
}

