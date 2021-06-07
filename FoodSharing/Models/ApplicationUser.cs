using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Xamarin.Essentials;
using FoodSharing.Models;
using FoodSharing.Services;

namespace FoodSharing.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ProtectedPersonalData]
        [Required]
        public virtual string FirstName { get; set; }
        [ProtectedPersonalData]
        [Required]
        public virtual string LastName { get; set; }
        public virtual string Description { get; set; }
        public ICollection<Food> Foods { get; set; }

        public ICollection<Review> Revieweds { get; set; }
        public ICollection<Review> Reviewers { get; set; }

        public ICollection<Message> Senders { get; set; }
        public ICollection<Message> Receivers { get; set; }

        public double UserLocLatitude { get; set; }
        public double UserLocLongitude { get; set; }
    }
}
