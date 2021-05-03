using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;  
  
namespace FoodSharing.Services
{
    public class DeleteUserModel
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

    }
}
