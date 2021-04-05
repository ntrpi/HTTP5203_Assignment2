using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using HTTP5203_Assignment2.Models;
using HTTP5203_Assignment2.Models.ViewModels;
using User = HTTP5203_Assignment2.Models.User;
using UserType = HTTP5203_Assignment2.Models.User.UserType;

namespace HTTP5203_Assignment2.Controllers
{
    public class UserController: Controller
    {
        // Use these controllers to interact with the XML.
        private UserDataController data = UserDataController.getUserDataController();
        private TicketDataController ticketData = TicketDataController.getTicketDataController();
        private ProductDataController productData = ProductDataController.getProductDataController();

        // GET: User
        public ActionResult Index()
        {
            return View( data.getUsers() );
        }

        // A utility function to create a list of ViewTickets for a given user.
        private IEnumerable<ViewTicket> getViewTickets( User user )
        {
            List<ViewTicket> viewTickets = new List<ViewTicket>();
            IEnumerable<Ticket> tickets = ticketData.getTicketsForUser( user.userId, user.userType );
            foreach( Ticket t in tickets ) {
                viewTickets.Add( new ViewTicket {
                    ticket = t,
                    product = productData.getProduct( t.productId )
                } );
            }
            return viewTickets;
        }



        // GET: User/Details/5
        public ActionResult Details( int id )
        {
            User user = data.getUser( id );
            ViewUser viewUser = ViewUser.getViewUser( user );
            viewUser.tickets = getViewTickets( user );
            return View( viewUser );
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // A utility function to create a User object using the information in the collection.
        private User getUserFromCollection( IFormCollection collection )
        {
            User user = null;
            if( collection.ContainsKey( "userType" ) ) {
                UserType userType = UserHelper.getType( collection[ "userType" ] );
                user = UserHelper.getUserOfType( userType );
                if( UserHelper.isCustomer( user ) ) {
                    if( collection.ContainsKey( "email" ) ) {
                        UserHelper.addEmail( user, collection[ "email" ] );
                    }
                }
            } else {
                user = new User();
            }

            if( collection.ContainsKey( "userId" ) ) {
                user.userId = Int32.Parse( collection[ "userId" ] );
            }
            if( collection.ContainsKey( "userName" ) ) {
                user.userName = collection[ "userName" ];
            }
            if( collection.ContainsKey( "name" ) ) {
                user.name = collection[ "name" ];
            }
            if( collection.ContainsKey( "password" ) ) {
                user.password = UserDataController.getHashed( collection[ "password" ] );
            }
            return user;
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection )
        {
            User user = getUserFromCollection( collection );
            user.userId = data.addUser( user );
            
            try {
                if( UserHelper.isCustomer( user ) ) {
                    return RedirectToAction( nameof( AddEmail ), new {
                        id = user.userId
                    } );
                }
                return RedirectToAction( nameof( Details ), new {
                id = user.userId } );
            } catch {
                return View();
            }
        }

        // GET: User/AddEmail/5
        public ActionResult AddEmail( int id )
        {
            User user = data.getUser( id );
            return View( user as Customer );
        }

        // GET: User/Edit/5
        public ActionResult Edit( int id )
        {
            return View( data.getUser( id ) );
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id, IFormCollection collection )
        {
            User user = getUserFromCollection( collection );
            data.updateUser( user );
            try {
                if( UserHelper.isCustomer( user ) ) {
                    return RedirectToAction( nameof( AddEmail ), new {
                        id = user.userId
                    } );
                }

                return RedirectToAction( nameof( Details ), new {
                    id = user.userId
                } );
            } catch {
                return View();
            }
        }


        // GET: User/DeleteConfirm/5
        public ActionResult DeleteConfirm( int id )
        {
            return View( ViewUser.getViewUser( data.getUser( id ) ) );
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( int id, IFormCollection collection )
        {
            data.deleteUser( id );
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }
    }
}
