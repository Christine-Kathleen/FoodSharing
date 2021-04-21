using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xamarin.Essentials;
using WebAPI.Authentication;

namespace FoodSharing.Models
{
    public class Food
    {

        [Key]
        public int FoodId { get; set; }

        private Location userLoc= new Location (46.0667, 23.5833);
        public void SetUserLoc(Location location)
        {
            userLoc = location;
        }
        [Required]
        public string Name { get; set; }
        //public Location FoodLoc { get; set; }
        //time posted!!!!!
        [Required]
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        //TODO Distance Calculation has to be changed, as FoodLoc has to be the first parameter
        public string Distance { get { return Math.Round(Location.CalculateDistance(userLoc, userLoc, DistanceUnits.Kilometers), 2).ToString() + "km"; } }
        public TypeOfFood FoodType { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }

    public enum TypeOfFood
    {
        FromStore = 0,
        HomeMade = 1
    }
}
