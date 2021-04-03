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

        // GET: TicketController
        public ActionResult Index()
        {
            return View( data.getTickets() );
        }

        // GET: TicketController/Details/5
        public ActionResult Details( int id )
        {
            return View( data.getTicket( id ) );
        }

        // GET: TicketController/Create
        public ActionResult Create()
        {
            UpdateTicket updateTicket = new UpdateTicket();
            updateTicket.products = TicketDataController.getProducts();
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
            updateTicket.products = TicketDataController.getProducts();
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
    }
}
