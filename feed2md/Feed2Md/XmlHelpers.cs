using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Feed2Md
{
    public static class XmlHelpers
    {
        private static XmlNamespaceManager ItunesNamespaceManager(XmlNameTable nametable)
        {
            var manager = new XmlNamespaceManager(nametable);
            manager.AddNamespace("itunes", "http://www.itunes.com/dtds/podcast-1.0.dtd");
            return manager;
        }

        private static XmlNamespaceManager PscNamespaceManager(XmlNameTable nametable)
        {
            var manager = new XmlNamespaceManager(nametable);
            manager.AddNamespace("psc", "http://podlove.org/simple-chapters");
            return manager;
        }

        public static string ITunes(this XmlNode xmlNode, string xpath)
        {
            if (!xpath.StartsWith("itunes:")) xpath = "itunes:" + xpath;
            return xmlNode.SelectSingleNode(xpath, ItunesNamespaceManager(xmlNode.OwnerDocument.NameTable))?.InnerText;
        }

        public static string ITunes(this XmlNode xmlNode, string xpath, string attribute)
        {
            if (!xpath.StartsWith("itunes:")) xpath = "itunes:" + xpath;
            return xmlNode.SelectSingleNode(xpath, ItunesNamespaceManager(xmlNode.OwnerDocument.NameTable))?.Attributes.GetNamedItem(attribute)?.InnerText;
        }

        public static string Psc(this XmlNode xmlNode, string xpath)
        {
            if (!xpath.StartsWith("psc:")) xpath = "psc:" + xpath;
            return xmlNode.SelectSingleNode(xpath, PscNamespaceManager(xmlNode.OwnerDocument.NameTable))?.InnerText;
        }

        public static XmlNodeList PscChildNodes(this XmlNode xmlNode, string xpath)
        {
            if (!xpath.StartsWith("psc:")) xpath = "psc:" + xpath;
            return xmlNode.SelectSingleNode(xpath, PscNamespaceManager(xmlNode.OwnerDocument.NameTable))?.ChildNodes;
        }

        public static string Psc(this XmlNode xmlNode, string xpath, string attribute)
        {
            if (!xpath.StartsWith("psc:")) xpath = "psc:" + xpath;
            return xmlNode.SelectSingleNode(xpath, PscNamespaceManager(xmlNode.OwnerDocument.NameTable))?.Attributes.GetNamedItem(attribute)?.InnerText;
        }
    }
}
