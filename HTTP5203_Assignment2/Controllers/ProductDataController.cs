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
    public class ProductDataController: XmlDataController
    {
        // Use this to determine the product ID for any new products.
        private static int maxProductId;

        // Use this to interact with the XML.
        private static ProductDataController productData;

        // Make this a singleton.
        private ProductDataController() : base( "\\App_Data", "\\products.xml" )
        {
            foreach( XElement e in getElementsWithName( "product" ) ) {
                int productId = getProductId( e );
                if( productId > maxProductId ) {
                    maxProductId = productId;
                }
            }
        }

        // Get the instance.
        public static ProductDataController getProductDataController()
        {
            if( productData == null ) {
                productData = new ProductDataController();
            }
            return productData;
        }

        // Get the product id from the element.
        public int getProductId( XElement element )
        {
            XElement productId = element.Element( "productId" );
            if( productId != null ) {
                return (int) productId;
            }
            return 0;
        }

        // Get the product XElement with the given id.
        private XElement getProductElement( int productId )
        {
            return getElementWithChildValue( "product", "productId", productId.ToString() );
        }

        // Get Product object by id.
        public Product getProduct( int productId )
        {
            return getProductFromXml( getProductElement( productId ) );
        }

        // Create a Product object from the element.
        private Product getProductFromXml( XElement product )
        {
            Product newProduct = new Product {
                name = product.Element( "name" ).Value,
                productId = (int) product.Element( "productId" )
            };
            return newProduct;
        }

        // Create a product XML element from the Product object.
        private XElement getXmlFromProduct( Product product )
        {
            XElement element = new XElement( "product" );
            element.SetElementValue( "productId", product.productId.ToString() );
            element.SetElementValue( "name", product.name );
            return element;
        }

        // Get a list of all the Product objects.
        public IEnumerable<Product> getProducts()
        {
            List<Product> products = new List<Product>();
            foreach( XElement e in getElementsWithName( "product" ) ) {
                products.Add( getProductFromXml( e ) );
            }
            return products;
        }

        // Modify the XElement based on what is in the Product object.
        private void modifyXml( Product product, XElement element )
        {
            if( product.productId != 0 ) {
                element.SetElementValue( "productId", product.productId );
            }

            if( product.name != null ) {
                element.SetElementValue( "name", product.name );
            }
        }

        // Add product to XML and Product collection.
        public int addProduct( Product product )
        {
            maxProductId++;
            product.productId = maxProductId;
            XElement element = addElement( "product" );
            modifyXml( product, element );
            updateFile();
            return product.productId;
        }

        // Update product in XML and Product collection.
        public int updateProduct( Product product )
        {
            XElement element = getProductElement( product.productId );
            modifyXml( product, element );
            updateFile();
            return product.productId;
        }


        // Delete product from XML and Product collection.
        public void deleteProduct( int productId )
        {
            XElement element = getElementWithChildValue( "product", "productId", productId.ToString() );
            element.Remove();
            updateFile();
        }
    }
}
