using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodSharing.Models
{
    class DeleteFoodModel
    {
        [Required(ErrorMessage = "FoodId is required")]
        public int FoodId { get; set; }
    }
}
