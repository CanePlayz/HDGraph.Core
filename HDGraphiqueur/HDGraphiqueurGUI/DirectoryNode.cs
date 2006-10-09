using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;

namespace HDGraph
{
    public class DirectoryNode : IXmlSerializable
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

        internal DirectoryNode()
        {

        }

        public override string ToString()
        {
            return base.ToString() + ": " + name;
        }

        #region IXmlSerializable Membres

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();

            name = reader.ReadElementContentAsString();
            totalSize = reader.ReadElementContentAsLong();
            filesSize = reader.ReadElementContentAsLong();
            profondeurMax = reader.ReadElementContentAsInt();

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            //string ns = "http://HDGraphiqueur.tools.laugel.fr/DirectoryNode.xsd";
            if (parent == null)
                writer.WriteElementString("Name", path);
            else
                writer.WriteElementString("Name", name);
            writer.WriteElementString("TotalSize", totalSize.ToString());
            writer.WriteElementString("FilesSize", filesSize.ToString());
            writer.WriteElementString("ProfondeurMax", profondeurMax.ToString());

            writer.WriteStartElement("Children");
            XmlSerializer serializer = new XmlSerializer(children.GetType());
            serializer.Serialize(writer, children);
            writer.WriteEndElement();

            // Pour sérialiser génériquement les propriétés d'un  objet :
            //foreach (PropertyInfo prop in this.GetType().GetProperties())
            //{
            //    if (prop.GetAccessors().Length > 1  // il faut un get et un set
            //        && prop.GetAccessors()[0].IsPublic   // chacun des 2 doivent être publiques
            //        && prop.GetAccessors()[1].IsPublic   // chacun des 2 doivent être publiques
            //        && prop.Name != "Parent"
            //        && prop.Name != "Path"
            //        && prop.Name != "Name"
            //        && prop.Name != "ProfondeurMax")
            //    {
            //        writer.WriteStartElement(prop.Name);
            //        XmlSerializer serializer = new XmlSerializer(prop.PropertyType);
            //        serializer.Serialize(writer, prop.GetValue(this, null));
            //        writer.WriteEndElement();
            //    }
            //}
        }

        #endregion
    }
}
