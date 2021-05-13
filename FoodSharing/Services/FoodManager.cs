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
        public Task<List<Food>> GetFoodssAsync()
        {
            return restService.RefreshFoodDataAsync();
        }

        public Task<ApplicationUser> GetUser(string username, string password)
        {
            return restService.GetUser(username, password);
        } 

        public Task<Response> UpdateFoodAsync(Food food)
        {
            return restService.SaveFoodAsync(food, false);
        }
        public Task<Response> SaveFoodAsync(Food food)
        {
            return restService.SaveFoodAsync(food, true);
        }

        public Task<Response> DeleteFoodAsync(Food food)
        {
            return restService.DeleteFoodAsync(food.FoodId);
        }
    }
}
