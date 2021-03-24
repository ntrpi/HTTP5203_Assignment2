using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using HTTP5203_Assignment2.Models;
using HTTP5203_Assignment2.Models.ViewModels;

namespace HTTP5203_Assignment2.Controllers
{
    public class UserDataController: XmlDataController
    {
        private int maxUserId;
        public UserDataController() : base( "/App_Data", "users.xml")
        {
            foreach( XElement e in getElementsWithName( "user" ) ) {
                int userId = getUserId( e );
                if( userId > maxUserId ) {
                    maxUserId = userId;
                }
            }
        }

        private int getUserId( XElement element )
        {
            return (int) element.Element( "userId" );
        }

        User getUserFromXml( XElement userXml )
        {
            return new User {
                userId = (int) userXml.Element( "userId" ),
                name = userXml.Element( "name" ).ToString(),
                userName = userXml.Element( "userName" ).ToString(),
                password = userXml.Element( "password" ).ToString()
            };
        }

        public IEnumerable<User> getUsers()
        {
            List<User> users = new List<User>();
            foreach( XElement e in getElementsWithName( "user" ) ) {
                users.Add( getUserFromXml( e ) );
            }
            return users;
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
            element.SetElementValue( "userId", user.userId );
            element.SetElementValue( "name", user.name );
            element.SetElementValue( "userName", user.userName );
            element.SetElementValue( "password", user.password );

            if( user is Customer ) {
                element.SetElementValue( "email", (user as Customer).email );
            }

            if( user is Support ) {
                element.SetElementValue( "type", ( user as Support ).type );
            }
        }

        public void updateUser( User user )
        {
            XElement element = getUserElement( user.userId );
            modifyXml( user, element );
            updateFile();
        }

        public void addUser( User user )
        {
            XElement element = addElement( "user" );
            user.userId = maxUserId++;
            modifyXml( user, element );
            updateFile();
        }

        public void deleteUser( User user )
        {
            deleteElement( "user", "userId", user.userId.ToString() );
        }
    }
}
