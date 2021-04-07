using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSharing.Models
{
    public class Comment
    {
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Subject { get; set; }
    }
}
