using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    public class ViewCustomerTicket
    {
        public Ticket ticket {
            get; set;
        }

        public Customer customer {
            get; set;
        }
    }
}
