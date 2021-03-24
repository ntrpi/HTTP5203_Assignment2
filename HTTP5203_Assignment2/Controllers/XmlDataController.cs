using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace HTTP5203_Assignment2.Controllers
{
    public class XmlDataController: Controller
    {
        private string fullPath;
        private XElement root;
        
        public XmlDataController( string path, string fileName )
        {
            fullPath = Request.PathBase + path + fileName;
            if( System.IO.File.Exists( fullPath ) ) {
                root = XElement.Load( fullPath );
            }
        }

        public IEnumerable<XElement> getElementsWithName( string name )
        {
            return root.Descendants( name );
        }

        public XElement getElementWithName( string name )
        {
            return getElementsWithName( name ).First();
        }

        public XElement addElement( string name )
        {
            XElement element = new XElement( name );
            root.Add( element );
            return element;
        }

        public XElement getElementWithChildValue( string parentName, string elementName, string elementValue )
        {
            return root.Descendants( parentName ).Elements( elementName ).Where( x => x.Value == elementValue ).First();
        }

        public void updateFile()
        {
            root.Save( fullPath );
        }
    }
}
