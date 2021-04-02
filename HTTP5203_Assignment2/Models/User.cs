using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace HTTP5203_Assignment2.Models
{
    public class User
    {
        public enum UserType
        {
            [Display( Name = "Admin" )]
            admin = 0,
            [Display( Name = "Service" )]
            service = 1,
            [Display( Name = "Customer" )]
            customer = 2
        }

        [HiddenInput]
        [Display( Name = "User ID")]
        public int userId {
            get; set;
        }

        [Required]
        [Display( Name = "Name" )]
        public string name {
            get; set;
        }

        [Required]
        [Display( Name = "User Name" )]
        public string userName {
            get; set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display( Name = "Password" )]
        public string password {
            get; set;
        }

        [Required]
        [Display( Name = "User Type" )]
        public UserType userType {
            get; set;
        }

    }
}
