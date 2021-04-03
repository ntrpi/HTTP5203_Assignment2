using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using HTTP5203_Assignment2.Models;

namespace HTTP5203_Assignment2.Controllers
{
    public class TicketDataController: XmlDataController
    {
        private static int maxTicketId;
        private static int maxMessageId;
        private static UserDataController userData = UserDataController.getUserDataController();
        private static ProductDataController productData = ProductDataController.getProductDataController();
        private static TicketDataController ticketData;

        private TicketDataController() : base( "\\App_Data", "\\support.xml" )
        {
            foreach( XElement e in getElementsWithName( "ticket" ) ) {
                int ticketId = getTicketId( e );
                if( ticketId > maxTicketId ) {
                    maxTicketId = ticketId;
                }
            }

            foreach( XElement e in getElementsWithName( "messageId" ) ) {
                int messageId = (int) e;
                if( messageId > maxMessageId ) {
                    maxMessageId = messageId;
                }
            }
        }

        // Get the instance.
        public static TicketDataController getTicketDataController()
        {
            if( ticketData == null ) {
                ticketData = new TicketDataController();
            }
            return ticketData;
        }

        // Get the message element.
        private XElement getMessageXml( int messageId, int ticketId = 0 )
        {
            if( ticketId != 0 ) {
                XElement ticket = getTicketElement( ticketId );
                return getElementWithChild( ticket, "message", "messageId", messageId.ToString() );
            } else {
                return getElementWithChild( "message", "messageId", messageId.ToString() );
            }
        }


        // Get the message id from the element.
        // Create a Message object from the element.
        public Message getMessageFromXml( XElement messageXml )
        {
            return new Message {
                messageId = (int) messageXml.Element( "messageId" ),
                timestamp = (DateTime) messageXml.Element( "timestamp" ),
                userId = (int) messageXml.Element( "userId" ),
                content = messageXml.Element( "content" ).Value
            };
        }

        private void modifyXml( XElement element, Message message )
        {
            if( message.messageId != 0 ) {
                element.SetElementValue( "messageId", message.messageId.ToString() );
            }
            if( message.userId != 0 ) {
                element.SetElementValue( "userId", message.userId );
            }
            if( message.timestamp != null ) {
                element.SetElementValue( "timestamp", message.timestamp.ToString() );
            }
            if( message.content != null ) {
                element.SetElementValue( "content", message.content );
            }
        }

        // Create a message XML element from the Message object.
        public XElement getXmlFromMessage( Message message )
        {
            XElement element = new XElement( "message" );
            modifyXml( element, message );
            return element;
        }

        // Get a list of all the Message objects from a given element.
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

        public IEnumerable<Message> getMessages( int ticketId )
        {
            XElement ticket = getTicketElement( ticketId );
            return getMessagesFromXml( ticket );
        }

        // Get Message object by id.
        public Message getMessage( int messageId, int ticketId = 0 )
        {
            XElement message = getMessageXml( messageId, ticketId );
            if( message != null ) {
                return getMessageFromXml( message );
            }
            return null;
        }

        // Add message to XML.
        public int addMessage( Message message )
        {
            XElement ticket = getTicketElement( message.ticketId );
            maxMessageId++;
            message.messageId = maxMessageId;
            ticket.Add( getXmlFromMessage( message ) );
            updateFile();
            return message.messageId;
        }

        // Update message in XML.
        public int updateMessage( Message message )
        {
            XElement element = getMessageXml( message.messageId, message.ticketId );
            modifyXml( element, message );
            updateFile();
            return message.messageId;
        }

        // Delete message from XML.
        public void deleteMessage( int messageId, int ticketId = 0 )
        {
            XElement element = getMessageXml( messageId, ticketId );
            element.Remove();
            updateFile();
        }



        // Get ticket id from the element.
        private int getTicketId( XElement element )
        {
            return (int) element.Element( "ticketId" );
        }

        // Create a Ticket object from the given element.
        Ticket getTicketFromXml( XElement ticketXml )
        {
            return new Ticket {
                ticketId = getTicketId( ticketXml ),
                userId = userData.getUserId( ticketXml ),
                subject = ticketXml.Element( "subject" ).Value,
                productId = productData.getProductId( ticketXml ),
                timestamp = (DateTime) ticketXml.Element( "timestamp" ),
                status = Ticket.getStatus( ticketXml.Element( "status" ).Value ),
                messages = getMessagesFromXml( ticketXml )
            };
        }

        // Get a list of Ticket objects from the xml.
        public IEnumerable<Ticket> getTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            foreach( XElement e in getElementsWithName( "ticket" ) ) {
                tickets.Add( getTicketFromXml( e ) );
            }
            return tickets;
        }

        // Get a Ticket object from the xml with the given id.
        public Ticket getTicket( int ticketId )
        {
            return getTicketFromXml( getTicketElement( ticketId ) );
        }

        // Get the ticket element with the given id.
        private XElement getTicketElement( int ticketId )
        {
            return getElementWithChildValue( "ticket", "ticketId", ticketId.ToString() );
        }

        // Modify the element with the properties of the Ticket object.
        private void modifyXml( XElement element, Ticket ticket )
        {
            if( ticket.ticketId != 0 ) {
                element.SetElementValue( "ticketId", ticket.ticketId );
            }

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
                element.SetElementValue( "productId", ticket.productId );
            }
            if( ticket.messages != null && ticket.messages.Count() > 0 ) {
                XElement messagesElement = element.Element( "messages" );
                if( messagesElement == null ) {
                    
                }
            }

            element.SetElementValue( "status", Ticket.getStatus( ticket.status) );
        }

        // Add a new ticket element to the xml.
        public int addTicket( Ticket ticket )
        {
            XElement element = addElement( "ticket" );
            maxTicketId++;
            ticket.ticketId = maxTicketId;
            modifyXml( element, ticket );
            updateFile();
            return ticket.ticketId;
        }

        // Update the corresponding ticket in the xml.
        public void updateTicket( Ticket ticket )
        {
            XElement element = getTicketElement( ticket.ticketId );
            modifyXml( element, ticket );
            updateFile();
        }

        // Delete the ticket in the xml with the given id.
        public void deleteTicket( int ticketId )
        {
            XElement element = getElementWithChildValue( "ticket", "ticketId", ticketId.ToString() );
            element.Remove();
            updateFile();
        }
    }
}
