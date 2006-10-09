using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;

namespace HDGraph
{
    public class DirectoryNode : IXmlSerializable
    {
        #region Variables et propriétés 

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

        #endregion

        #region Constructeur(s)

        public DirectoryNode(string path)
        {
            this.path = path;
            this.name = GetNameFromPath(path);
        }

        internal DirectoryNode()
        {

        }

        #endregion

        #region Méthodes
        
        public override string ToString()
        {
            return base.ToString() + ": " + name;
        }

        private string GetNameFromPath(string path)
        {
            string theName = System.IO.Path.GetFileName(path);
            if (theName == null || theName.Length == 0)
                theName = path;
            return theName;
        }

        /// <summary>
        /// Se base sur le nom du node courant et sur le path du père pour mettre à jour le path courant.
        /// </summary>
        private void UpdatePathFromNameAndParent()
        {
            if (parent != null)
                this.path = parent.path + System.IO.Path.DirectorySeparatorChar + name;
            foreach (DirectoryNode node in children)
            {
                node.UpdatePathFromNameAndParent();
            }
        }

        #endregion

        #region IXmlSerializable Membres

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            // Début élément DirectoryNode
            reader.ReadStartElement();

            name = reader.ReadElementContentAsString();
            totalSize = reader.ReadElementContentAsLong();
            filesSize = reader.ReadElementContentAsLong();
            profondeurMax = reader.ReadElementContentAsInt();

            // Début élément Children
            reader.ReadStartElement("Children");
            XmlSerializer serializer = new XmlSerializer(typeof(List<DirectoryNode>));
            children = (List<DirectoryNode>)serializer.Deserialize(reader);
            // Fin élément Children
            reader.ReadEndElement();

            // Mise à jour du parent
            foreach (DirectoryNode node in children)
            {
                node.parent = this;
            }

            // Mise à jour du path
            if (name.Contains(":"))
            {
                path = name;
                name = GetNameFromPath(path);
                UpdatePathFromNameAndParent();
            }

            // Fin élément DirectoryNode
            reader.ReadEndElement();
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<DirectoryNode>));
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
