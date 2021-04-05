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
    public class ProductController: Controller
    {
        private static ProductDataController data = ProductDataController.getProductDataController();

        // GET: ProductController
        public ActionResult Index()
        {
            return View( data.getProducts() );
        }

        // GET: ProductController/Details/5
        public ActionResult Details( int id )
        {
            return View( data.getProduct( id ) );
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // A utility function to create a Product object using information from the collection.
        private Product getProductFromCollection( IFormCollection collection )
        {
            Product product = new Product();
            if( collection.ContainsKey( "productId" ) ) {
                product.productId = Int32.Parse( collection[ "productId" ] );
            }
            if( collection.ContainsKey( "name" ) ) {
                product.name = collection[ "name" ];
            }
            return product;
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection )
        {
            Product product = getProductFromCollection( collection );
            product.productId = data.addProduct( product );
            try {
                return RedirectToAction( nameof( Details ), new {
                    id = product.productId
                } );
            } catch {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit( int id )
        {
            return View( data.getProduct( id ) );
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id, IFormCollection collection )
        {
            Product product = getProductFromCollection( collection );
            product.productId = data.updateProduct( product );
            try {
                return RedirectToAction( nameof( Details ), new {
                    id = product.productId
                } );
            } catch {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult DeleteConfirm( int id )
        {
            return View( data.getProduct( id ) );
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( int id, IFormCollection collection )
        {
            data.deleteProduct( id );
            try {
                return RedirectToAction( nameof( Index ) );
            } catch {
                return View();
            }
        }
    }
}
