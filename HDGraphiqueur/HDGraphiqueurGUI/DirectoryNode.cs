using System;
using System.Collections.Generic;
using System.Text;

namespace HDGraphiqueurGUI
{
    [Serializable()]
    public class DirectoryNode
    {
        private long totalSize;

        public long TotalSize
        {
            get { return totalSize; }
            set { totalSize = value; }
        }

        private long filesSize;

        public long FilesSize
        {
            get { return filesSize; }
            set { filesSize = value; }
        }


        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private DirectoryNode parent;

        [System.Xml.Serialization.XmlIgnore()]
        public DirectoryNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private List<DirectoryNode> children = new List<DirectoryNode>();

        public List<DirectoryNode> Children
        {
            get { return children; }
            set { children = value; }
        }

        private int profondeurMax = 1;

        public int ProfondeurMax
        {
            get { return profondeurMax; }
            set { profondeurMax = value; }
        }


        public DirectoryNode(string path)
        {
            this.path = path;
            this.name = System.IO.Path.GetFileName(path);
            if (this.name == null || this.name.Length == 0)
                this.name = path;
        }

        public override string ToString()
        {
            return base.ToString() + ": "+ name;
        }
    }
}
