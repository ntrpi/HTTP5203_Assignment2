using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Models
{
    public class Support: User
    {
        public enum Type
        {
            Service,
            Admin
        }

        public Type type {
            get; set;
        }
    }
}
