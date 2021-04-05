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

        // Return the first element if the enumerable is not empty, otherwise null.
        public XElement getFirst( IEnumerable<XElement> elements )
        {
            if( elements.Count() > 0 ) {
                return elements.First();
            }
            return null;
        }

        // Get all the decendants with a given name from the root.
        public IEnumerable<XElement> getElementsWithName( string name )
        {
            return getElementsWithName( root, name );
        }

        // Get all the decendants with a given name from the given parent element.
        public IEnumerable<XElement> getElementsWithName( XElement parent, string name )
        {
            return parent.Descendants( name );
        }

        // Get the first decendant with a given name from the root or null.
        public XElement getElementWithName( string name )
        {
            return getFirst( getElementsWithName( name ) );
        }

        // Get the first decendant with a given name from the given parent element or null.
        public XElement getElementWithName( XElement parent, string name )
        {
            return getFirst( getElementsWithName( parent, name ) );
        }

        // Get all the decendents of the root that have a child with the given name and value if passed in.
        public IEnumerable<XElement> getElementsWithChild( string parentName, string childName, string childValue = null )
        {
            return getElementsWithChild( root, parentName, childName, childValue );
        }

        // Get all the decendents of the element that have a child with the given name and value if passed in.
        public IEnumerable<XElement> getElementsWithChild( XElement ancestor, string parentName, string childName, string childValue = null )
        {
            IEnumerable<XElement> parents = ancestor.Descendants( parentName );
            IEnumerable<XElement> children;
            if( childValue == null ) {
                children = parents.Elements( childName );
            } else {
                children = parents.Elements( childName ).Where( c => c.Value == childValue );
            }
            return children
                .SelectMany( c => c.Ancestors( parentName ) )
                .ToList();
        }

        public IEnumerable<XElement> getGrandparents( string grandparentName, string parentName, string childName, string childValue = null )
        {
            IEnumerable<XElement> parents = root.Descendants( grandparentName )
                .Descendants( parentName );
            IEnumerable<XElement> children;
            if( childValue == null ) {
                children = parents.Descendants( childName );
            } else {
                children = parents.Descendants( childName ).Where( c => c.Value == childValue );
            }
            return children
                .SelectMany( c => c.Ancestors( parentName ) )
                .SelectMany( c => c.Ancestors( grandparentName ) )
                .Distinct()
                .ToList();
        }

        public IEnumerable<XElement> getElementsWithDescendant( string parentName, string descendantName, string descendantValue = null )
        {
            return getElementsWithDescendant( root, parentName, descendantName, descendantValue );
        }

        public IEnumerable<XElement> getElementsWithDescendant( XElement ancestor, string parentName, string descendantName, string descendantValue = null )
        {
            IEnumerable<XElement> parents = ancestor.Descendants( parentName );
            IEnumerable<XElement> children;
            if( descendantValue == null ) {
                children = parents.Descendants( descendantName );
            } else {
                children = parents.Descendants( descendantName ).Where( c => c.Value == descendantValue );
            }
            return children
                .SelectMany( c => c.Ancestors( parentName ) )
                .ToList();
        }


        // Get the first decendent of the root that has a child with the given name and value if passed in.
        public XElement getElementWithChild( string parentName, string childName, string childValue = null )
        {
            return getFirst( getElementsWithChild( root, parentName, childName, childValue ) );
        }

        // Get the first decendent of the element that has a child with the given name and value if passed in.
        public XElement getElementWithChild( XElement ancestor, string parentName, string childName, string childValue = null )
        {
            return getFirst( getElementsWithChild( ancestor, parentName, childName, childValue ) );
        }


        // Create an element and add it to the root.
        public XElement addElement( string name )
        {
            XElement element = new XElement( name );
            root.Add( element );
            return element;
        }

        // Create an element with a given value and add it to the root.
        public XElement addElement( string name, string value )
        {
            XElement element = new XElement( name );
            element.SetValue( value );
            root.Add( element );
            return element;
        }

        // Get the first element with parentName that has a child with childName and childValue, or null.
        public XElement getElementWithChildValue( string parentName, string childName, string childValue )
        {
            IEnumerable<XElement> children = getElementsWithChild( parentName, childName, childValue );
            if( children.Count() > 0 ) {
                return children.First();
            }
            return null;
        }

        // Save the tree to the file.
        public void updateFile()
        {
            root.Save( fullPath );
        }
    }
}
