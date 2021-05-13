using FoodSharing.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public interface IRestService
    {
        Task<List<Food>> RefreshFoodDataAsync();
        Task<Response> SaveFoodAsync(Food food, bool isNewItem);
        Task<Response> DeleteFoodAsync(int id);

        Task<ApplicationUser> GetUser(string username, string password);
        Task<Response> DeleteUserAsync(string id);
        Task<Response> UpdatePasswordAsync(UpdatePasswordModel model);
        Task<Response> UpdateUserAsync(UpdateUserModel model);
        Task SaveUserAsync(ApplicationUser user, bool isNewUser);
        Task<Response> RegisterUserAsync(RegisterModel model);

        Task<List<Review>> RefreshReviewDataAsync();
        Task<Response> SaveReviewAsync(Review review, bool isNewItem);

        Task<Response> SaveMessageAsync(Message message, bool isNewItem);
        Task<List<Message>> RefreshMessageDataAsync();
        Task<Response> DeleteMessageAsync(int id);
    }
}