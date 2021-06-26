using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodSharing.Models
{
    public class Review
    {

        public Review()
        {
            this.SendTime = DateTime.UtcNow;
        }

        [Key]
        public int ReviewId { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string ReviewContent { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        [Required]
        public string ReviewedUserId { get; set; }
        public ApplicationUser ReviewedId { get; set; }
        [Required]
        public string ReviewerUserId { get; set; }
        public ApplicationUser ReviewerId { get; set; }

    }
}
