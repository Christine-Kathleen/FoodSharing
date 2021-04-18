using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebAPI.Authentication;

namespace WebAPI.Models
{
    public class Message
    {
        [Key]
        public string MessageId { get; set; }
        [Required(ErrorMessage = "Food Name is required")]
        public string Content { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "Must have a sender")]
        public ApplicationUser Sender { get; set; }

        [Required(ErrorMessage = "Must have a receiver")]
        public ApplicationUser Receiver { get; set; }
        [Required]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Must have a message state")]
        public MessageState State { get; set; }
    }
    public enum MessageState
    {
        Sent=1,
        Seen=2,
        Replied=3
    }
}
