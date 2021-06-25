using FoodSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodSharing.Services
{
    public class ReviewManager
    {

        readonly IRestService restService;

        public ReviewManager(IRestService service)
        {
            restService = service;
        }
        public Task<List<Review>> GetReviewsAsync(string ReviewedUserId)
        {
            return restService.GetReviewDataAsync(ReviewedUserId);
        }
        public Task<Response> SaveReviewAsync(Review review)
        {
            return restService.SaveReviewAsync(review, true);
        }
    }
}
