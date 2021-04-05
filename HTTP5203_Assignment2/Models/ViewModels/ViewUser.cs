using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    // A collection of object to help create the view for viewing the details of user.
    public class ViewUser
    {
        // Syntactic sugar to help with the whole email thing.
        public static ViewUser getViewUser( User user )
        {
            ViewUser viewUser = new ViewUser { user = user };
            UserHelper.addEmail( user, UserHelper.getEmail( user ) );
            return viewUser;
        }

        public User user {
            get; set;
        }

        [Display( Name = "Email" )]
        public string email {
            get; set;
        }

        [Display( Name = "Tickets" )]
        public IEnumerable<ViewTicket> tickets {
            get; set;
        }
    }
}
