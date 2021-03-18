using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using HTTP5203_Assignment2.Models;

namespace HTTP5203_Assignment2.Controllers
{
    public class ControllerHelper: Controller
    {
        public XmlDocument getXmlDocument( string pathBase, string path )
        {
            path = pathBase + path;
            if( !System.IO.File.Exists( path ) ) {
                return null;
            }
            XmlDocument xml = new XmlDocument();
            xml.Load( path );
            return xml;
        }

        public XmlElement getFirstXmlElement( XmlElement parent, string tagName )
        {
            XmlNodeList elements = parent.GetElementsByTagName( tagName );
            if( elements == null || elements.Count == 0 ) {
                return null;
            }

            XmlNode node = elements[ 0 ];
            return (XmlElement) node;
        }

        public string getXmlElementText( XmlElement element )
        {
            return element.InnerText;
        }

        public string getFirstXmlElementText( XmlElement parent, string tagName )
        {
            XmlElement element = getFirstXmlElement( parent, tagName );
            if( element == null ) {
                return null;
            }
            return getXmlElementText( element );
        }

        public void loadUserData( User user, XmlElement userElement )
        {
            user.userId = Int32.Parse( getFirstXmlElementText( userElement, "userId" ) );
            user.name = getFirstXmlElementText( userElement, "name" );
            user.userName = getFirstXmlElementText( userElement, "username" );
            user.password = getFirstXmlElementText( userElement, "password" );
        }

        public Customer getCustomerFromXml( XmlElement userElement )
        {
            Customer customer = new Customer {
                email = getFirstXmlElementText( userElement, "email" )
            };
            loadUserData( customer, userElement );
            return customer;
        }

        public Support getSupportFromXml( XmlElement userElement )
        {
            Support support = new Support();
            string type = getFirstXmlElementText( userElement, "userType" );
            support.type = type == "admin" ? Support.Type.Admin : Support.Type.Service;
            loadUserData( support, userElement );
            return support;
        }

        public User getUserFromXml( XmlElement userElement )
        {
            User user = new User();
            loadUserData( user, userElement );
            return user;
        }
    }

}
