using FoodSharing.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public interface IRestService
    {
        Task<List<Food>> RefreshDataAsync();

        Task SaveFoodAsync(Food food, bool isNewItem);

        Task DeleteFoodAsync(int id);

        Task<ApplicationUser> GetUser(string username, string password);
        Task DeleteUserAsync(string id);
        Task SaveUserAsync(ApplicationUser user, bool isNewUser);
        Task<Response> RegisterUserAsync(RegisterModel model);
    }
}