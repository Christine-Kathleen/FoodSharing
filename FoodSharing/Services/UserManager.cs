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
        readonly IRestService restService;

        public UserManager(IRestService service)
        {
            restService = service;
        }
        public Task<ApplicationUser> GetUser(string username, string password)
        {
            return restService.GetUser(username, password);
        }
        public Task<Response> DeleteUserAsync(string id)
        {
            return restService.DeleteUserAsync(id);
        }

        public Task<Response> UpdateUserAsync(UpdateUserModel model)
        {
            return restService.UpdateUserAsync(model);
        }
        public Task SaveUserAsync(ApplicationUser user)
        {
            return restService.SaveUserAsync(user, true);
        }
        public Task<Response> RegisterUserAsync(RegisterModel model)
        {
            return restService.RegisterUserAsync(model);
        }
        public Task<Response> UpdatePassword(UpdatePasswordModel model)
        {
            return restService.UpdatePasswordAsync(model);
        }
    }
}
