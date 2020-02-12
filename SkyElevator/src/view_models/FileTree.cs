using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyElevator.src.view_models
{

    public class FileTreeItem
    {
        public string name, path;
        public FileTreeItem(string name, string path) { this.name = name; this.path = path; }
    }

    public class FileItem : FileTreeItem
    {
        public FileItem(string name, string path) : base(name, path) { }
    }

    public class DirectoryItem : FileTreeItem
    {
        public List<FileTreeItem> items = new List<FileTreeItem>();
        public void Add(FileTreeItem item) { this.items.Add(item);  }
        public DirectoryItem(string name, string path) : base(name, path) { }
    }

    public class FileTree
    {
        public List<FileTreeItem> getItems() {

            List<FileTreeItem> ret = new List<FileTreeItem>();
            ret.Add(new FileItem("my file.txt", @"E:/my folder/my file.txt"));
            ret.Add(new FileItem("my file2.txt", @"E:/my folder/my file2.txt"));

            DirectoryItem folder = new DirectoryItem("my folder", "E:/my folder/another folder/");
            folder.Add(new FileItem("folder file.txt", @"E:/my folder/folder file.txt"));
            folder.Add(new FileItem("folder file2.txt", @"E:/my folder/folder file2.txt"));
            ret.Add(folder);

            return ret;
        }
    }

}
