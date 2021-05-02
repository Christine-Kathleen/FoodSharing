﻿using FoodSharing.Models;
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
            public Task DeleteUserAsync(string id)
        {
            return restService.DeleteUserAsync(id);
        }

        public Task UpdateTaskAsync(Food food)
        {
            return restService.SaveFoodAsync(food, false);
        }
        public Task SaveTaskAsync(Food food)
        {
            return restService.SaveFoodAsync(food, true);
        }

        public Task DeleteTaskAsync(Food food)
        {
            return restService.DeleteFoodAsync(food.FoodId);
        }
    }
}
