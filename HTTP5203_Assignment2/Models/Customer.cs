using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HTTP5203_Assignment2.Models
{
    public class Customer: User
    {
        private const string errorMessage = "Please enter a valid email address.";

        [Required(ErrorMessage = errorMessage) ]
        [EmailAddress(ErrorMessage = errorMessage)]
        [Display(Name = "Email")]
        public string email {
            get; set;
        }
    }
}
