using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class MessageManager
    {
        readonly IRestService restService;

        public MessageManager(IRestService service)
        {
            restService = service;
        }
        public List<Message> Messages { get; set; }
        public Task<List<Message>> GetMessagesAsync(string userId)
        {
            return restService.GetMessageDataAsync(userId);
        }
        public Task<Response> SaveMessageAsync(Message message)
        {
            return restService.SaveMessageAsync(message, true);
        }
        public Task<Response> UpdateMessageAsync(int id)
        {
            return restService.UpdateMessageAsync(id);
        }
    }
}

