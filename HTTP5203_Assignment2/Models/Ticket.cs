using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models
{
    public class Ticket
    {
        public enum Status
        {
            Open,
            Investigating,
            Closed
        }

        public int ticketId {
            get; set;
        }

        public int userId {
            get; set;
        }

        public string subject {
            get; set;
        }
        
        public Status status {
            get; set;
        }

        public IEnumerable<Message> messages {
            get; set;
        }
    }
}
