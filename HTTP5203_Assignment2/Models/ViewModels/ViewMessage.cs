using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    // A collection of object to help create the view for viewing a particular message.
    public class ViewMessage
    {
        public Message message {
            get; set;
        }

        public User user {
            get; set;
        }
    }
}
