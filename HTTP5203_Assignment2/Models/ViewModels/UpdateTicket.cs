using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    // A collection of object to help create the view for updating a ticket.
    public class UpdateTicket
    {
        public Ticket ticket {
            get; set;
        }

        public IEnumerable<User> users {
            get; set;
        }

        public IEnumerable<Product> products {
            get; set;
        }

        public IEnumerable<Message> messages {
            get; set;
        }
    }
}
