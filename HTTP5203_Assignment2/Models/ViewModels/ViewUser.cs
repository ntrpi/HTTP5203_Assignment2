using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HTTP5203_Assignment2.Models.ViewModels
{
    public class ViewUser
    {
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
    }
}
