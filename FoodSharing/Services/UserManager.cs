using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class UserManager
    {
        IRestService restService;

        public UserManager(IRestService service)
        {
            restService = service;
        }
        public Task<ApplicationUser> GetUser(string username, string password)
        {
            return restService.GetUser(username, password);
        }
        public Task DeleteUserAsync(string id)
        {
            return restService.DeleteUserAsync(id);
        }

        public Task UpdateUserAsync(ApplicationUser user)
        {
            return restService.SaveUserAsync(user, false);
        }
        public Task SaveUserAsync(ApplicationUser user)
        {
            return restService.SaveUserAsync(user, true);
        }
        public Task<Response> RegisterUserAsync(RegisterModel model)
        {
            return restService.RegisterUserAsync(model);
        }
    }
}
