using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xamarin.Essentials;
using NetTopologySuite.Geometries;
using Location = Xamarin.Essentials.Location;
using Xamarin.Forms;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FoodSharing.Models
{
    public class Food:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Food()
        {
            this.TimePosted = DateTime.UtcNow;
        }

        [Key]
        public int FoodId { get; set; }
        public double FoodLocationLatitude { get; set; }
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
        public string Distance { get { return Math.Round(Location.CalculateDistance(new Location(FoodLocationLatitude, FoodLocationLongitude), userLoc, DistanceUnits.Kilometers), 2).ToString() + " km"; } }
        [Required(ErrorMessage = "The food type is required")]
        public TypeOfFood FoodType { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string UserID { get; set; }
        public Availability AnnouncementAvailability { get; set; }

        private ImageSource imageSource;

        [NotMapped]
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImageSource"));
            }
        }
    }
    public enum Availability
    {
        Available = 0,
        Reserved = 1,
        Unavailable = 2
    }
    public enum TypeOfFood
    {
        FromStore = 0,
        HomeMade = 1
    }
}
