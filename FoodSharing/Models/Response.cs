using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FoodSharing.Constants;

namespace FoodSharing.Models
{
    public class Response
    {
        public Status Status { get; set; }
        public APIMessages Message { get; set; }
    }
}
