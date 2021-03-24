using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using HTTP5203_Assignment2.Models;

namespace HTTP5203_Assignment2.Controllers
{
    public class UserController: Controller
    {
        private UserDataController data = new UserDataController();

        // GET: UserController
        public ActionResult Index()
        {
            IList<User> users = new List<User>();
            XmlNodeList userNodes = xml.GetElementsByTagName( "user" );
            foreach( XmlElement element in userNodes ) {
                User user = helper.getUserFromXml( element );
                users.Add( user );
            }

            return View( users ); //pass the users to view
        }

        public ActionResult ListCustomers()
        {
            return View();
        }

        public ActionResult ListSupport( int id )
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult DetailsCustomer( int id )
        {
            return View();
        }

        public ActionResult DetailsSupport( int id )
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        public ActionResult CreateSupport()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer( IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSupport( IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult EditCustomer( int id )
        {
            return View();
        }

        // GET: UserController/Edit/5
        public ActionResult EditSupport( int id )
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer( int id, IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSupport( int id, IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult DeleteConfirm( int id )
        {
            return View();
        }

        // POST: UserController/Delete/5
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
