using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models
{
    public class Message
    {
        public int messageId {
            get; set;
        }

        public DateTime timestamp {
            get; set;
        }

        public int userId {
            get; set;
        }

        public string content {
            get; set;
        }
    }
}
