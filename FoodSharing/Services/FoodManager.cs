using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class FoodManager
    {
        readonly IRestService restService;

        public FoodManager(IRestService service)
        {
            restService = service;
        }
        public Task<List<Food>> GetFoodsAsync()
        {
            return restService.RefreshFoodDataAsync();
        }
        public Task<Response> UpdateFoodAsync(Food food)
        {
            return restService.SaveFoodAsync(food, false);
        }
        public Task<Response> SaveFoodAsync(Food food)
        {
            return restService.SaveFoodAsync(food, true);
        }
        public Task<Response> DeleteFoodAsync(int foodId)
        {
            return restService.DeleteFoodAsync(foodId);
        }
    }
}
