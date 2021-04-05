using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using HTTP5203_Assignment2.Models;
using HTTP5203_Assignment2.Models.ViewModels;
using User = HTTP5203_Assignment2.Models.User;
using UserType = HTTP5203_Assignment2.Models.User.UserType;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace HTTP5203_Assignment2.Controllers
{
    public class UserDataController: XmlDataController
    {
        // Copied ferom https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        // on 2021/04/02
        public static string getHashed( string rawData )
        {
            // Create a SHA256   
            using( SHA256 sha256Hash = SHA256.Create() ) {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash( Encoding.UTF8.GetBytes( rawData ) );

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for( int i = 0; i < bytes.Length; i++ ) {
                    builder.Append( bytes[ i ].ToString( "x2" ) );
                }
                return builder.ToString();
            }
        }

        private static int maxUserId;
        private static UserDataController userDataController;

        private UserDataController() : base( "\\App_Data", "\\users.xml")
        {
            foreach( XElement e in getElementsWithName( "user" ) ) {
                int userId = getUserId( e );
                if( userId > maxUserId ) {
                    maxUserId = userId;
                }
            }
        }

        public static UserDataController getUserDataController()
        {
            if( userDataController == null ) {
                userDataController = new UserDataController();
            }
            return userDataController;
        }

        public int getUserId( XElement element )
        {
            return (int) element.Element( "userId" );
        }

        User getUserFromXml( XElement userXml )
        {
            UserType userType = UserHelper.getType( userXml.Element( "userType" ).Value );

            User user = UserHelper.getUserOfType( userType ) ;

            user.userId = (int) userXml.Element( "userId" );
            user.name = userXml.Element( "name" ).Value;
            user.userName = userXml.Element( "userName" ).Value;
            user.password = userXml.Element( "password" ).Value;
            user.userType = userType;

            XElement email = userXml.Element( "email" );
            string emailString = email != null ? email.Value : "";
            UserHelper.addEmail( user, emailString );
            return user;
        }

        public IEnumerable<User> getUsers()
        {
            List<User> users = new List<User>();
            foreach( XElement e in getElementsWithName( "user" ) ) {
                users.Add( getUserFromXml( e ) );
            }
            return users;
        }


        public IEnumerable<User> getUsersByType( User.UserType type )
        {
            List<User> users = new List<User>();
            // Use 3 to indicate both admin and service as "support". It is not a
            // value in the enum.
            if( (int) type == 3 ) {
                foreach( XElement e in getElementsWithChild( "user", "userType", "admin" ) ) {
                    users.Add( getUserFromXml( e ) );
                }
                foreach( XElement e in getElementsWithChild( "user", "userType", "service" ) ) {
                    users.Add( getUserFromXml( e ) );
                }
            } else {
                string userType = Enum.GetName( typeof( User.UserType ), type );
                foreach( XElement e in getElementsWithChild( "user", "userType", userType ) ) {
                    users.Add( getUserFromXml( e ) );
                }
            }
            return users;
        }

        public Customer getCustomer( int userId )
        {
            return getUser( userId ) as Customer;
        }

        public User getUser( int userId )
        {
            return getUserFromXml( getUserElement( userId ) );
        }

        private XElement getUserElement( int userId )
        {
            return getElementWithChildValue( "user", "userId", userId.ToString() );
        }

        private void modifyXml( User user, XElement element )
        {
            // TODO: ID shouldn't change.
            element.SetElementValue( "userId", user.userId );

            if( user.name != null ) {
                element.SetElementValue( "name", user.name );
            }

            if( user.userName != null ) {
                // TODO: check unique.
                element.SetElementValue( "userName", user.userName );
            }

            if( user.password != null ) {
                element.SetElementValue( "password", user.password );
            }
            
            element.SetElementValue( "userType", UserHelper.getType( user.userType ) );

            string email = UserHelper.getEmail( user );
            if( email != null && email.Length > 0 ) {
                element.SetElementValue( "email", email );
            }
        }

        public void updateUser( User user )
        {
            XElement element = getUserElement( user.userId );
            modifyXml( user, element );
            updateFile();
        }

        public int addUser( User user )
        {
            XElement element = addElement( "user" );
            maxUserId++;
            user.userId = maxUserId;
            modifyXml( user, element );
            updateFile();
            return user.userId;
        }

        public void deleteUser( int userId )
        {
            XElement element = getElementWithChildValue( "user", "userId", userId.ToString() );
            element.Remove();
            updateFile();
        }
    }
}
