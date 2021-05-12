using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class MessageManager
    {
        IRestService restService;

        public MessageManager(IRestService service)
        {
            restService = service;
        }
        public List<Message> Messages { get; set; }
        public Task<List<Message>> GetTasksAsync()
        {
            return restService.RefreshMessageDataAsync();
        }

        public Task<ApplicationUser> GetUser(string username, string password)
        {
            return restService.GetUser(username, password);
        }

        public Task<Response> SaveTaskAsync(Message message)
        {
            return restService.SaveMessageAsync(message, true);
        }

        public Task<Response> DeleteTaskAsync(Message message)
        {
            return restService.DeleteMessageAsync(message.MessageId);
        }
    }
}

