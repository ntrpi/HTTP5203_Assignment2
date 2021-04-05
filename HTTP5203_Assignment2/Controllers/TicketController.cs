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

        // GET: TicketController
        public ActionResult Index()
        {
            return View( data.getTickets() );
        }

        private ViewTicket getViewTicket( int id )
        {
            Ticket ticket = data.getTicket( id );
            return new ViewTicket {
                ticket = ticket,
                user = userData.getUser( ticket.userId ),
                messages = getViewMessages( id ),
                product = productData.getProduct( ticket.productId )
            };
        }

        // GET: TicketController/Details/5
        public ActionResult Details( int id )
        {
            return View( getViewTicket( id ) );
        }

        // GET: TicketController/Create
        public ActionResult Create()
        {
            UpdateTicket updateTicket = new UpdateTicket();
            updateTicket.products = productData.getProducts();
            updateTicket.users = userData.getUsers();
            return View( updateTicket );
        }

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


        // POST: TicketController/Create
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

        // GET: TicketController/Edit/5
        public ActionResult Edit( int id )
        {
            UpdateTicket updateTicket = new UpdateTicket();
            updateTicket.ticket = data.getTicket( id );
            updateTicket.products = productData.getProducts();
            updateTicket.users = userData.getUsers();
            return View( updateTicket );
        }

        // POST: TicketController/Edit/5
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

        // GET: TicketController/Delete/5
        public ActionResult DeleteConfirm( int id )
        {
            return View( data.getTicket( id ) );
        }

        // POST: TicketController/Delete/5
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

        private IEnumerable<ViewMessage> getViewMessages( int id )
        {
            IEnumerable<Message> messages = data.getMessages( id );
            List<ViewMessage> viewMessages = new List<ViewMessage>();
            foreach( Message m in messages ) {
                viewMessages.Add( new ViewMessage {
                    message = m,
                    user = userData.getUser( m.userId )
                } );
            }
            return viewMessages;
        }

        // id = ticket id
        public ActionResult Messages( int id )
        {
            return View( data.getMessages( id ) );
        }

        public ActionResult MessageDetails( int id, int ticketId )
        {
            return View( data.getMessage( id, ticketId ) );
        }

        public ActionResult CreateMessage( int id )
        {
            AddMessage addMessage = new AddMessage();
            addMessage.viewTicket = getViewTicket( id );
            addMessage.users = userData.getUsersByType( (Models.User.UserType) 3 );
            return View( addMessage );
        }

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

        public ActionResult EditMessage( int id, int ticketId )
        {
            return View( data.getMessage( id, ticketId ) );
        }

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

        public ActionResult DeleteMessageConfirm( int id )
        {
            return View( data.getMessage( id ) );
        }

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
