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

        private Location userLoc = new Location(46.0667, 23.5833);
        public void SetUserLoc(Location location)
        {
            userLoc = location;
        }
        [Required(ErrorMessage = "Food Name is required")]
        public string Name { get; set; }
        //public Location FoodLoc { get; set; }
        //time posted!!
        [Required(ErrorMessage = "Details are required")]
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        //TODO Distance Calculation has to be changed, as FoodLoc has to be the first parameter
        [Required]
        public string Distance { get { return Math.Round(Location.CalculateDistance(userLoc, userLoc, DistanceUnits.Kilometers), 2).ToString() + "km"; } }
        [Required(ErrorMessage = "The food type is required")]
        public TypeOfFood FoodType { get; set; }
        [Required]
        public Availability AnnouncementAvailability { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public string UserID { get; set; }
    }

    public enum TypeOfFood
    {
        FromStore = 0,
        HomeMade = 1
    }

    public enum Availability
    {
        Available = 0,
        Reserved = 1,
        Unavailable = 2
    }
}
