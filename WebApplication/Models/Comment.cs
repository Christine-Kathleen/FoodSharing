using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Authentication;

namespace WebAPI.Models
{
    public class Comment
    {
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string Subject { get; set; }
    }
}
