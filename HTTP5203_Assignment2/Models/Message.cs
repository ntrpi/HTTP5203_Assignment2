using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HTTP5203_Assignment2.Models
{
    // Represents a message element in the xml, with a reference to the ticket to which 
    // it belongs.
    public class Message
    {
        [Display( Name = "Message ID" )]
        public int messageId {
            get; set;
        }

        [Required]
        [Display( Name = "Timestamp" )]
        public DateTime timestamp {
            get; set;
        }

        [Required]
        [Display( Name = "User ID" )]
        public int userId {
            get; set;
        }

        [Required]
        [Display( Name = "Content" )]
        public string content {
            get; set;
        }

        [Required]
        [Display( Name = "Ticket" )]
        public int ticketId {
            get; set;
        }
    }
}
