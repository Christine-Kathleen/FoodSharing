using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodSharing.Models
{
    public class DeleteUserModel
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
    }
}
