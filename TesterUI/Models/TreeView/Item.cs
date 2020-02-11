using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace TesterUI.Models.TreeView
{
    public class Item
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public XmlNode node { get; set; }
    }
}
