using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    // A collection of object to help create the view for adding a message.
    public class AddMessage
    {
        public ViewTicket viewTicket {
            get; set;
        }

        public IEnumerable<User> users {
            get; set;
        }

        public Message message {
            get; set;
        }
    }
}
