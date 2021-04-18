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
    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //public override string Id { get; set; }
        [ProtectedPersonalData]
        public virtual string FirstName { get; set; }

        [ProtectedPersonalData]
        public virtual string LastName { get; set; }

        [ProtectedPersonalData]
        public float RatingGrade { get; set; }
        public List<Food> Foods { get; set; }
        // public Location UserLoc { get; set; }
    }
}
