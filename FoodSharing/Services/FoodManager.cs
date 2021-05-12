using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class FoodManager
    {
        IRestService restService;

        public FoodManager(IRestService service)
        {
            restService = service;
        }
        public List<Food> Foods { get; set; }
        public Task<List<Food>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task<ApplicationUser> GetUser(string username, string password)
        {
            return restService.GetUser(username, password);
        } 

        public Task<Response> UpdateTaskAsync(Food food)
        {
            return restService.SaveFoodAsync(food, false);
        }
        public Task<Response> SaveTaskAsync(Food food)
        {
            return restService.SaveFoodAsync(food, true);
        }

        public Task<Response> DeleteTaskAsync(Food food)
        {
            return restService.DeleteFoodAsync(food.FoodId);
        }
    }
}
