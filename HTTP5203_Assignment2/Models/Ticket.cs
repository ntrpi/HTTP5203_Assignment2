using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HTTP5203_Assignment2.Models
{
    // Represents a ticket element in the xml.
    public class Ticket
    {
        public enum Status
        {
            [Display(Name = "Open")]
            open = 0,
            [Display( Name = "Assigned" )]
            investigating = 1,
            [Display( Name = "Closed" )]
            closed = 2
        }

        // Utility functions to convert the status type back and forth between int, type, and string.
        public static Status getStatus( string status )
        {
            return (Status) Enum.Parse( typeof( Status ), status, true );
        }

        public static string getStatus( Status status )
        {
            return Enum.GetName( typeof( Status ), status );
        }

        public static string getStatus( int status )
        {
            return getStatus( (Status) status );
        }


        [Display(Name = "Ticket ID")]
        public int ticketId {
            get; set;
        }

        [Required]
        [Display( Name = "Created" )]
        public DateTime timestamp {
            get; set; 
        }

        [Required(ErrorMessage = "Please select a user")]
        [Display( Name = "User ID" )]
        public int userId {
            get; set;
        }

        [Required(ErrorMessage = "Please enter a subject.")]
        [Display( Name = "Subject" )]
        public string subject {
            get; set;
        }

        [Required]
        [Display( Name = "Status" )]
        public Status status {
            get; set;
        }

        [Required(ErrorMessage = "Please select a product.")]
        [Display( Name = "Product" )]
        public int productId {
            get; set;
        }

        [Display( Name = "Messages" )]
        public IEnumerable<Message> messages {
            get; set;
        }
    }
}
