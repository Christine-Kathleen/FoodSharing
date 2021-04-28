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


    //public sealed class Singleton
    //{
    //    private static ApplicationUser instance = null;
    //    private static readonly object padlock = new object();


    //    Singleton()
    //    {
    //    }

    //    public static ApplicationUser Instance
    //    {
    //        get
    //        {
    //            lock (padlock)
    //            {
    //                if (instance == null)
    //                {
    //                    RestService userRestService = new RestService();
    //                    var response = await userRestService.AuthWithCredentialsAsync(Username, Password);
    //                    ApplicationUser user = await userRestService.GetUser(Username, Password);
    //                    instance = new User();
    //                }
    //                return instance;
    //            }
    //        }
    //    }
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
        public double UserLocLatitude { get; set; }
        public double UserLocLongitude { get; set; }
    }
}
