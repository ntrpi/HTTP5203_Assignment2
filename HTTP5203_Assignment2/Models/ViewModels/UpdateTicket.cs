using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models.ViewModels
{
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
    }
}
