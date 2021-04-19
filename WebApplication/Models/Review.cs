using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Authentication;

namespace WebAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string ReviewContent { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        public string ReviewedUserId { get; set; }
        public ApplicationUser ReviewedId { get; set; }

        public string ReviewerUserId { get; set; }
        public ApplicationUser ReviewerId { get; set; }

    }
}
