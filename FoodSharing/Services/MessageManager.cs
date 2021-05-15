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
        public Task<List<Message>> GetMessagesAsync(string userId)
        {
            return restService.RefreshMessageDataAsync(userId);
        }

        public Task<Response> SaveMessageAsync(Message message)
        {
            return restService.SaveMessageAsync(message, true);
        }

        //public Task<Response> DeleteMessageAsync(Message message)
        //{
        //    return restService.DeleteMessageAsync(message.MessageId);
        //}
    }
}

