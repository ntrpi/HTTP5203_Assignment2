using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using HTTP5203_Assignment2.Models;
using HTTP5203_Assignment2.Models.ViewModels;

namespace HTTP5203_Assignment2.Controllers
{
    public class TicketDataController: XmlDataController
    {
        private static int maxTicketId;
        private static int maxProductId;
        private static int maxMessageId;
        private static Dictionary<int, Product> products = new Dictionary<int, Product>();
        private static UserDataController userData = UserDataController.getUserDataController();
        private static TicketDataController ticketData;

        private TicketDataController() : base( "\\App_Data", "\\support.xml" )
        {
            foreach( XElement e in getElementsWithName( "ticket" ) ) {
                int ticketId = getTicketId( e );
                if( ticketId > maxTicketId ) {
                    maxTicketId = ticketId;
                }
                Product product = getProductFromXml( e );
                if( product.productId > maxProductId ) {
                    maxProductId = product.productId;
                }
                products.Add( product.productId, product );
            }

            foreach( XElement e in getElementsWithName( "messageId" ) ) {
                int messageId = (int) e;
                if( messageId > maxMessageId ) {
                    maxMessageId = messageId;
                }
            }
        }

        public static TicketDataController getTicketDataController()
        {
            if( ticketData == null ) {
                ticketData = new TicketDataController();
            }
            return ticketData;
        }

        public static IEnumerable<Product> getProducts()
        {
            return products.Values;
        }

        private int getTicketId( XElement element )
        {
            return (int) element.Element( "ticketId" );
        }

        private int getProductId( XElement element )
        {
            XElement productId = element.Descendants( "productId" ).First();
            if( productId != null ) {
                return (int) productId;
            }
            return 0;
        }

        private Product getProductFromXml( XElement element )
        {
            XElement product = element.Element( "product" );
            Product newProduct = new Product {
                name = product.Element( "name" ).Value,
                productId = (int) product.Element( "productId" )
            };
            return newProduct;
        }

        private XElement getXmlFromProduct( Product product )
        {
            XElement element = new XElement( "product" );
            XElement productId = addElement( "productId" );
            productId.SetValue( product.productId.ToString() );
            element.Add( productId );
            XElement name = addElement( "name" );
            name.SetValue( product.name );
            element.Add( name );
            return element;
        }

        public Message getMessageFromXml( XElement messageXml )
        {
            return new Message {
                messageId = (int) messageXml.Element( "messageId" ),
                timestamp = (DateTime) messageXml.Element( "timestamp" ),
                userId = (int) messageXml.Element( "userId" ),
                content = messageXml.Element( "content" ).Value
            };
        }

        public IEnumerable<Message> getMessagesFromXml( XElement parent )
        {
            int ticketId = (int) parent.Element( "ticketId" );
            IEnumerable<XElement> messages = getElementsWithName( parent, "message" );
            List<Message> newMessages = new List<Message>();
            foreach( XElement e in messages ) {
                Message message = getMessageFromXml( e );
                message.ticketId = ticketId;
                newMessages.Add( message );
            }
            return newMessages;
        }

        Ticket getTicketFromXml( XElement ticketXml )
        {
            return new Ticket {
                ticketId = getTicketId( ticketXml ),
                userId = userData.getUserId( ticketXml ),
                subject = ticketXml.Element( "subject" ).Value,
                productId = getProductId( ticketXml ),
                timestamp = (DateTime) ticketXml.Element( "timestamp" ),
                status = Ticket.getStatus( ticketXml.Element( "status" ).Value ),
                messages = getMessagesFromXml( ticketXml )
            };
        }

        public IEnumerable<Ticket> getTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            foreach( XElement e in getElementsWithName( "ticket" ) ) {
                tickets.Add( getTicketFromXml( e ) );
            }
            return tickets;
        }

        public Ticket getTicket( int ticketId )
        {
            return getTicketFromXml( getTicketElement( ticketId ) );
        }

        private XElement getTicketElement( int ticketId )
        {
            return getElementWithChildValue( "ticket", "ticketId", ticketId.ToString() );
        }

        private void modifyXml( Ticket ticket, XElement element )
        {
            // TODO: ID shouldn't change.
            element.SetElementValue( "ticketId", ticket.ticketId );

            if( ticket.timestamp != null ) {
                element.SetElementValue( "timestamp", ticket.timestamp.ToString( "g", DateTimeFormatInfo.InvariantInfo ) );
            }

            if( ticket.userId != 0 ) {
                element.SetElementValue( "userId", ticket.userId );
            }

            if( ticket.subject != null ) {
                element.SetElementValue( "subject", ticket.subject );
            }

            if( ticket.productId != 0 ) {
                Product product = products.GetValueOrDefault( ticket.productId );
                element.Add( getXmlFromProduct( product ) );
            }

            element.SetElementValue( "status", Ticket.getStatus( ticket.status) );
        }

        public void updateTicket( Ticket ticket )
        {
            XElement element = getTicketElement( ticket.ticketId );
            modifyXml( ticket, element );
            updateFile();
        }

        public int addTicket( Ticket ticket )
        {
            XElement element = addElement( "ticket" );
            maxTicketId++;
            ticket.ticketId = maxTicketId;
            modifyXml( ticket, element );
            updateFile();
            return ticket.ticketId;
        }

        public void deleteTicket( int ticketId )
        {
            XElement element = getElementWithChildValue( "ticket", "ticketId", ticketId.ToString() );
            element.Remove();
            updateFile();
        }
    }
}
