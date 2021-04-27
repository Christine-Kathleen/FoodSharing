using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Xamarin.Essentials;
using FoodSharing.Models;

namespace WebAPI.Authentication
{
    //public class ApplicationUser : IdentityUser
    //{
    //    //[Key]
    //    //public override string Id { get; set; }
    //    [ProtectedPersonalData]
    //    public virtual string FirstName { get; set; }

    //    [ProtectedPersonalData]
    //    public virtual string LastName { get; set; }

    //    [ProtectedPersonalData]
    //    public float RatingGrade { get; set; }
    //    public List<Food> Foods { get; set; }
    //    // public Location UserLoc { get; set; }
    //}

    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //public override string Id { get; set; }
        public ApplicationUser()
        {
            Foods = new List<Food>();
        }
        [ProtectedPersonalData]
        [Required]
        public virtual string FirstName { get; set; }

        [ProtectedPersonalData]
        [Required]
        public virtual string LastName { get; set; }
        public float RatingGrade { get; set; }

        public ICollection<Food> Foods { get; set; }

        public ICollection<Review> Revieweds { get; set; }
        public ICollection<Review> Reviewers { get; set; }

        public ICollection<Message> Senders { get; set; }
        public ICollection<Message> Receivers { get; set; }
        // public Location UserLoc { get; set; }
    }
}
