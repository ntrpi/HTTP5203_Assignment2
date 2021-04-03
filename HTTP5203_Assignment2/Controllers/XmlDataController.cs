using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
            fullPath = Directory.GetCurrentDirectory() + path + fileName;
            if( System.IO.File.Exists( fullPath ) ) {
                root = XElement.Load( fullPath );
            }
        }

        public IEnumerable<XElement> getElementsWithName( string name )
        {
            return root.Descendants( name );
        }

        public IEnumerable<XElement> getElementsWithName( XElement parent, string name )
        {
            return parent.Descendants( name );
        }

        public XElement getElementWithName( string name )
        {
            return getElementsWithName( name ).First();
        }

        public XElement getElementWithName( XElement parent, string name )
        {
            return getElementsWithName( parent, name ).First();
        }

        public IEnumerable<XElement> getElementsWithChild( string parent, string child, string value )
        {
            return root
                .Descendants( child )
                .Where( c => c.Value == value )
                .SelectMany( c => c.Ancestors( parent ) )
                .ToList();
        }

        public IEnumerable<XElement> getElementsWithChild( string parent, string child )
        {
            return root
                .Descendants( child )
                .SelectMany( c => c.Ancestors( parent ) )
                .ToList();
        }

        public XElement addElement( string name )
        {
            XElement element = new XElement( name );
            root.Add( element );
            return element;
        }

        public XElement getElementWithChildValue( string parentName, string elementName, string elementValue )
        {
            return root.Descendants( parentName ).Elements( elementName ).Where( x => x.Value == elementValue ).First().Parent;
        }

        public void updateFile()
        {
            root.Save( fullPath );
        }
    }
}
