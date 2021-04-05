using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSharing.Models
{
    public class Message
    {
        public string Content { get; set; }
        public DateTime SendTime {get; set;}
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Subject { get; set; }
        public MessageState State { get; set; }
    }
    public enum MessageState
    {
        Sent=1,
        Seen=2,
        Replied=3
    }
}
