using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class ReviewManager
    {

        IRestService restService;

        public ReviewManager(IRestService service)
        {
            restService = service;
        }
        public List<Review> Reviews { get; set; }
        public Task<List<Review>> GetTasksAsync()
        {
            return restService.RefreshReviewDataAsync();
        }
        public Task<ApplicationUser> GetUser(string username, string password)
        {
            return restService.GetUser(username, password);
        }
        public Task<Response> SaveTaskAsync(Review review)
        {
            return restService.SaveReviewAsync(review, true);
        }
    }
}
