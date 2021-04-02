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
        private UserDataController data = new UserDataController();

        // GET: UserController
        public ActionResult Index()
        {
            return View( data.getUsers() );
        }

        public ActionResult List( int id )
        {
            IEnumerable<Customer> customers = (IEnumerable<Customer>) data.getUsersByType( (User.UserType) id );
            return View( customers );
        }

        // GET: UserController/Details/5
        public ActionResult Details( int id )
        {
            User user = data.getUser( id );
            ViewUser viewUser = new ViewUser {
                user = user
            };
            
            if( user.userType ==  UserType.customer ) {
                viewUser.email = ( user as Customer ).email;
            }
            return View( viewUser );
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        private User getUserFromCollection( IFormCollection collection )
        {
            User user = null;
            if( collection.ContainsKey( "userType" ) ) {
                UserType userType = UserHelper.getType( collection[ "userType" ] );
                user = UserHelper.getUserOfType( userType );
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
            if( collection.ContainsKey( "email" ) ) {
                UserHelper.addEmail( user, collection[ "email" ] );
            }
            return user;
        }

        // POST: UserController/Create
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

        // GET: UserController/Edit/5
        public ActionResult AddEmail( int id )
        {
            User user = data.getUser( id );
            return View( user as Customer );
        }

        // GET: UserController/Edit/5
        public ActionResult Edit( int id )
        {
            return View( data.getUser( id ) );
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id, IFormCollection collection )
        {
            User user = getUserFromCollection( collection );
            data.updateUser( user );
            try {
                return RedirectToAction( nameof( Details ), new {
                    id = user.userId
                } );
            } catch {
                return View();
            }
        }


        // GET: UserController/Delete/5
        public ActionResult DeleteConfirm( int id )
        {
            return View( ViewUser.getViewUser( data.getUser( id ) ) );
        }

        // POST: UserController/Delete/5
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
