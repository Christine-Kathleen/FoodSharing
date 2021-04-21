using FoodSharing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public interface IRestService
    {
        Task<List<Food>> RefreshDataAsync();

        Task SaveFoodAsync(Food food, bool isNewItem);

        Task DeleteFoodAsync(int id);
    }
}