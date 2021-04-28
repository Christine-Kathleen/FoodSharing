using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xamarin.Essentials;
using NetTopologySuite.Geometries;
using Location = Xamarin.Essentials.Location;
using System.Data.Entity.Spatial;

namespace FoodSharing.Models
{
    public class Food
    {
        public Food()
        {
            this.TimePosted = DateTime.UtcNow;
        }

        [Key]
        public int FoodId { get; set; }
        //[Required(ErrorMessage = "FoodLocationLatitude is required")]
        public double FoodLocationLatitude { get; set; }
        //[Required(ErrorMessage = "FoodLocationLongitude is required")]
        public double FoodLocationLongitude { get; set; }

        private Location userLoc = new Location(46.0667, 23.5833);
        public void SetUserLoc(Location location)
        {
            userLoc = location;
        }
        [Required(ErrorMessage = "Food Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The date and time is required")]
        public DateTime TimePosted { get; set; }
        [Required(ErrorMessage = "Details are required")]
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public string Distance { get { return Math.Round(Xamarin.Essentials.Location.CalculateDistance(new Location(FoodLocationLatitude, FoodLocationLongitude), userLoc, DistanceUnits.Kilometers), 2).ToString() + "km"; } }
        [Required(ErrorMessage = "The food type is required")]
        public TypeOfFood FoodType { get; set; }
        [Required]
        public Availability AnnouncementAvailability { get; set; }
        //[Required]
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
