using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HTTP5203_Assignment2.Models
{
    // Represents a product element in the xml.
    public class Product
    {
        [Display( Name = "Product ID" )]
        public int productId {
            get; set;
        }

        [Required(ErrorMessage = "Please enter a name for the product.")]
        [Display( Name = "Product" )]
        public string name {
            get; set;
        }
    }
}
