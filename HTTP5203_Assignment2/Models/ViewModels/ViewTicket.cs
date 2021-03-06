using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    // A collection of object to help create the view for viewing the details of a ticket.
    public class ViewTicket
    {
        public Ticket ticket {
            get; set;
        }

        public User user {
            get; set;
        }

        public Product product {
            get; set;
        }

        public IEnumerable<ViewMessage> messages {
            get; set;
        }
    }
}
