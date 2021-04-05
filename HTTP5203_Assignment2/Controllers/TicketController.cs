using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTP5203_Assignment2.Models;
using HTTP5203_Assignment2.Models.ViewModels;

namespace HTTP5203_Assignment2.Controllers
{
    public class TicketController: Controller
    {
        private TicketDataController data = TicketDataController.getTicketDataController();
        private static UserDataController userData = UserDataController.getUserDataController();
        private static ProductDataController productData = ProductDataController.getProductDataController();

        // GET: Ticket
        public ActionResult Index()
        {
            return View( data.getTickets() );
        }

        // A utility function to create a ViewTicket object for the ticket with 
        // the given id.
        private ViewTicket getViewTicket( int ticketId )
        {
            Ticket ticket = data.getTicket( ticketId );
            return new ViewTicket {
                ticket = ticket,
                user = userData.getUser( ticket.userId ),
                messages = getViewMessages( ticketId ),
                product = productData.getProduct( ticket.productId )
            };
        }

        // GET: Ticket/Details/5
        public ActionResult Details( int id )
        {
            return View( getViewTicket( id ) );
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            UpdateTicket updateTicket = new UpdateTicket();
            updateTicket.products = productData.getProducts();
            updateTicket.users = userData.getUsers();
            return View( updateTicket );
        }

        // A utility function to create a Ticket object with the information from the collection.
        private Ticket getTicketFromCollection( IFormCollection collection )
        {
            Ticket ticket = new Ticket();

            if( collection.ContainsKey( "ticketId" ) ) {
                ticket.ticketId = Int32.Parse( collection[ "ticketId" ] );
            }
            if( collection.ContainsKey( "timestamp" ) ) {
                ticket.timestamp = DateTime.Parse( collection[ "timestamp" ] );
            }
            if( collection.ContainsKey( "userId" ) ) {
                ticket.userId = Int32.Parse( collection[ "userId" ] );
            } else if( collection.ContainsKey( "User" ) ) {
                ticket.userId = Int32.Parse( collection[ "User" ] );
            }

            if( collection.ContainsKey( "productId" ) ) {
                ticket.productId = Int32.Parse( collection[ "productId" ] );
            }
            if( collection.ContainsKey( "subject" ) ) {
                ticket.subject = collection[ "subject" ];
            }
            if( collection.ContainsKey( "status" ) ) {
                ticket.status = Ticket.getStatus( collection[ "status" ] );
            }
            return ticket;
        }


        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection )
        {
            Ticket ticket = getTicketFromCollection( collection );
            ticket.ticketId = data.addTicket( ticket );

            try {
                return RedirectToAction( nameof( Details ), new {
                    id = ticket.ticketId
                } );
            } catch {
                return View();
            }
        }

        // GET: Ticket/Edit/5
        public ActionResult Edit( int id )
        {
            UpdateTicket updateTicket = new UpdateTicket();
            updateTicket.ticket = data.getTicket( id );
            updateTicket.products = productData.getProducts();
            updateTicket.users = userData.getUsers();
            return View( updateTicket );
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id, IFormCollection collection )
        {
            Ticket ticket = getTicketFromCollection( collection );
            data.updateTicket( ticket );
            try {
                return RedirectToAction( nameof( Details ), new {
                    id = ticket.ticketId
                } );
            } catch {
                return View();
            }
        }

        // GET: Ticket/DeleteConfirm/5
        public ActionResult DeleteConfirm( int id )
        {
            return View( data.getTicket( id ) );
        }

        // POST: Ticket/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( int id, IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        // A utility function to create a list of ViewMessage objects for the 
        // ticket with the given id.
        private IEnumerable<ViewMessage> getViewMessages( int ticketId )
        {
            IEnumerable<Message> messages = data.getMessages( ticketId );
            List<ViewMessage> viewMessages = new List<ViewMessage>();
            foreach( Message m in messages ) {
                viewMessages.Add( new ViewMessage {
                    message = m,
                    user = userData.getUser( m.userId )
                } );
            }
            return viewMessages;
        }

        // GET: Ticket/Messages/5
        // id = ticket id
        public ActionResult Messages( int id )
        {
            return View( data.getMessages( id ) );
        }

        // GET: Ticket/MessageDetails/?id=3&ticketId=5
        public ActionResult MessageDetails( int id, int ticketId )
        {
            return View( data.getMessage( id, ticketId ) );
        }

        // GET: Ticket/CreateMessage/5
        // id = ticket id
        public ActionResult CreateMessage( int id )
        {
            AddMessage addMessage = new AddMessage();
            addMessage.viewTicket = getViewTicket( id );
            addMessage.users = userData.getUsersByType( (Models.User.UserType) 3 );
            return View( addMessage );
        }

        // Utility function to create a Message object using the information from the collection.
        private Message getMessageFromCollection( IFormCollection collection )
        {
            Message message = new Message();
            if( collection.ContainsKey( "messageId" ) ) {
                message.messageId = Int32.Parse( collection[ "messageId" ] );
            }

            if( collection.ContainsKey( "timestamp" ) ) {
                message.timestamp = DateTime.Parse( collection[ "timestamp" ] );
            } else {
                message.timestamp = DateTime.Now;
            }

            if( collection.ContainsKey( "userId" ) ) {
                message.userId = Int32.Parse( collection[ "userId" ] );
            }

            if( collection.ContainsKey( "ticketId" ) ) {
                message.ticketId = Int32.Parse( collection[ "ticketId" ] );
            }

            if( collection.ContainsKey( "content" ) ) {
                message.content = collection[ "content" ];
            }
            return message;
        }

        // POST: Ticket/CreateMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMessage( IFormCollection collection )
        {
            Message message = getMessageFromCollection( collection );
            message.messageId = data.addMessage( message );
            try {
                return RedirectToAction( nameof( Details ), new {
                    id = message.ticketId
                } );
            } catch {
                return View();
            }
        }

        // POST: Ticket/EditMessage/?id=5&ticketId=9
        public ActionResult EditMessage( int id, int ticketId )
        {
            return View( data.getMessage( id, ticketId ) );
        }

        // POST: Ticket/EditMessage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMessage( int id, IFormCollection collection )
        {
            Message message = getMessageFromCollection( collection );
            message.messageId = data.updateMessage( message );
            try {
                return RedirectToAction( nameof( MessageDetails ), new {
                    id = message.messageId,
                    ticketId = message.ticketId
                } );
            } catch {
                return View();
            }
        }

        // POST: Ticket/DeleteMessageConfirm/5
        public ActionResult DeleteMessageConfirm( int id )
        {
            return View( data.getMessage( id ) );
        }

        // POST: Ticket/DeleteMessage/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMessage( int id, IFormCollection collection )
        {
            data.deleteMessage( id );
            try {
                return RedirectToAction( nameof( Messages ) );
            } catch {
                return View();
            }
        }

    }
}
