using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FoodSharing.Models
{
    public class Food
    {
        private Location userLoc;
        public void SetUserLoc(Location location)
        {
            userLoc = location;
        }
        public string Name { get; set; }
        public Location FoodLoc { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public string Distance { get { return Math.Round(Location.CalculateDistance(FoodLoc, userLoc, DistanceUnits.Kilometers), 2).ToString() + "km"; } }
        public TypeOfFood FoodType { get; set; } 
        public string UserName { get; set; }
    }

    public enum TypeOfFood
    {
        fromStore = 0,
        homeMade = 1
    }
}
