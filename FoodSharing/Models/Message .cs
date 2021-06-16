using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text;
using Xamarin.Forms;

namespace FoodSharing.Models
{

    public class Message
    {
        public Message()
        {
            this.SendTime = DateTime.UtcNow;
        }
        [Key]
        public int MessageId { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        [Required]
        public string SenderUserId { get; set; }
        public ApplicationUser SenderId { get; set; }
        [Required]
        public string ReceiverUserId { get; set; }
        public ApplicationUser ReceiverId { get; set; }

        [Required(ErrorMessage = "Must have a message state")]
        public MessageState State { get; set; }
        [NotMapped]
        public System.Drawing.Color ColorOfMessage { get; set; }
        [NotMapped]
        public LayoutOptions MessageAlignment { get; set; }
        [NotMapped]
        public string CompareMessages { get; set; }
    }
    public enum MessageState
    {
        Sent = 1,
        Seen = 2,
        Replied = 3
    }
}
