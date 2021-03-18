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
        private ControllerHelper helper = new ControllerHelper();
        private XmlDocument xmlDocument;

        private XmlDocument getXml()
        {
            if( xmlDocument == null ) {
                xmlDocument = helper.getXmlDocument( Request.PathBase, "App_Data/users.xml" );
            }
            return xmlDocument;
        }


             
        // GET: UserController
        public ActionResult Index()
        {
            XmlDocument xml = getXml();
            if( xml == null ) {
                return View();
            }

            IList<User> users = new List<User>();
            XmlNodeList userNodes = xml.GetElementsByTagName( "user" );
            foreach( XmlElement element in userNodes ) {
                User user = helper.getUserFromXml( element );
                users.Add( user );
            }

            return View( users ); //pass the users to view
        }

        // GET: UserController/Details/5
        public ActionResult Details( int id )
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit( int id )
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id, IFormCollection collection )
        {
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete( int id )
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
