using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSharing.Models
{
    public class AuthResponse
    {
        public AuthResponse()
        {
        }

        public string Token { get; set; }

        public string Expiration { get; set; }

        //public bool Success { get; set; }

    }
}
