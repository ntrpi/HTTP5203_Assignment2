using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTP5203_Assignment2.Models;
using User = HTTP5203_Assignment2.Models.User;
using UserType = HTTP5203_Assignment2.Models.User.UserType;

namespace HTTP5203_Assignment2.Models
{
    // There are too many conflicts with the VS User class, so this is required.
    public class UserHelper
    {
        // Utility functions to convert the user type back and forth between int, type, and string.
        public static UserType getType( string type )
        {
            return (UserType) Enum.Parse( typeof( UserType ), type, true );
        }

        public static string getType( UserType userType )
        {
            return Enum.GetName( typeof( UserType ), userType );
        }

        public static string getType( int userType )
        {
            return getType( (UserType) userType );
        }

        // Syntactic sugar to get the correct object.
        public static User getUserOfType( UserType userType )
        {
            if( userType == UserType.customer ) {
                return new Customer { userType = userType };
            } else {
                return new User { userType = userType };
            }
        }

        // Yeah, that email property became quite a pain in the butt.
        public static void addEmail( User user, string email )
        {
            if( user is Customer ) {
                ( user as Customer ).email = email;
            }
        }

        public static string getEmail( User user )
        {
            if( user is Customer ) {
                return ( user as Customer ).email;
            }
            return null;
        }

        // Syntactic sugar.
        public static Customer getCustomer( User user )
        {
            if( user is Customer ) {
                return user as Customer;
            }
            return null;
        }

        public static bool isCustomer( User user )
        {
            return user.userType == UserType.customer;
        }

        public static bool isCustomer( UserType userType )
        {
            return userType == UserType.customer;
        }
    }
}
