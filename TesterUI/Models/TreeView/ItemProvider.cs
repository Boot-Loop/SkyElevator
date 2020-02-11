using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace TesterUI.Models.TreeView
{
    public class ItemProvider
    {
        private List<Item> build(XmlNode par_node, string path)
        {
            List<Item> items = new List<Item>();
            foreach (XmlNode node in par_node)
            {
                if (node.Name == "file") {
                    items.Add( new FileItem { Name = node.Attributes["name"].Value, Path = path + node.Attributes["name"].Value, node = node } );
                }
                else if ( node.Name=="folder") {
                    items.Add(
                        new DirectoryItem { Name = node.Attributes["name"].Value, Path = path + node.Attributes["name"].Value + "/", Items = build(node,  path + node.Attributes["name"].Value + "/" ), node = node }
                    );
                }
            }
            return items;
        }

        public List<Item> GetItems(string path)
        {

            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load(path);
            
            var root = xmldoc.FirstChild;
            List<Item> items = build(root, root.Attributes["path"].Value);
            
            //var dirInfo = new DirectoryInfo(path);
            //
            //foreach (var directory in dirInfo.GetDirectories())
            //{
            //    var item = new DirectoryItem
            //    {
            //        Name = directory.Name,
            //        Path = directory.FullName,
            //        Items = GetItems(directory.FullName)
            //    };
            //
            //    items.Add(item);
            //}
            //
            //foreach (var file in dirInfo.GetFiles())
            //{
            //    var item = new FileItem
            //    {
            //        Name = file.Name,
            //        Path = file.FullName
            //    };
            //
            //    items.Add(item);
            //}

            return items;
        }
    }
}
