using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace HDGraph
{
    public class MoteurGraphiqueur : IXmlSerializable
    {
        #region Vatiables et propriétés

        #region Variables avec role de cache
        /// <summary>
        /// Cache messages localisés
        /// </summary>
        private static string scanningMessage = null;

        #endregion

        private DirectoryNode root = null;
        /// <summary>
        /// Répertoire racine du moteur.
        /// </summary>
        public DirectoryNode Root
        {
            get { return root; }
        }

        private DateTime analyzeDate;
        /// <summary>
        /// Date de la dernière analyse
        /// </summary>
        public DateTime AnalyzeDate
        {
            get { return analyzeDate; }
            set { analyzeDate = value; }
        }


        public delegate void PrintInfoDelegate(string message);

        private PrintInfoDelegate printInfoDeleg = null;

        /// <summary>
        /// Delegate appelé par le moteur lorsqu'une analyze est en cours.
        /// </summary>
        [XmlIgnore()]
        public PrintInfoDelegate PrintInfoDeleg
        {
            get { return printInfoDeleg; }
            set { printInfoDeleg = value; }
        }

        private bool pleaseCancelCurrentWork = false;
        /// <summary>
        /// Booléen indiquant s'il faut stopper l'analyse en cours.
        /// </summary>
        public bool PleaseCancelCurrentWork
        {
            get { return pleaseCancelCurrentWork; }
            set { pleaseCancelCurrentWork = value; }
        }

        private bool workCanceled = false;
        /// <summary>
        /// Indique si la précédente analyse a été stoppée en 
        /// raison d'une demande de l'utilisateur.
        /// </summary>
        public bool WorkCanceled
        {
            get { return workCanceled; }
            set { workCanceled = value; }
        }

        private bool autoRefreshAllowed;
        /// <summary>
        /// Booléen indiquant si le moteur a l'autorisation de compléter l'analyse
        /// lorsque le niveau de profondeur est insuffisant pour afficher la totalité du 
        /// graphique.
        /// </summary>
        public bool AutoRefreshAllowed
        {
            get { return autoRefreshAllowed; }
            set { autoRefreshAllowed = value; }
        }

        private bool showDiskFreeSpace;

        public bool ShowDiskFreeSpace
        {
            get { return showDiskFreeSpace; }
            set { showDiskFreeSpace = value; }
        }

        #endregion

        #region Contructeur(s)

        public MoteurGraphiqueur()
        {
            if (scanningMessage == null)
                scanningMessage = HDGTools.resManager.GetString("Scanning");
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Méthode à appeler pour lancer le scan.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxLevel"></param>
        public void ConstruireArborescence(string path, int maxLevel)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException("Invalid path.", "path");
            if (path.EndsWith(":"))
                path += @"\";
            pleaseCancelCurrentWork = false;
            workCanceled = false;
            if (maxLevel < 1)
                throw new ArgumentOutOfRangeException("maxLevel", "Il faut afficher au moins 1 niveau !");
            root = new DirectoryNode(path);

            ConstruireArborescence(root, maxLevel - 1);
            ApplySpecialRootOptions(root);

            analyzeDate = DateTime.Now;
            if (workCanceled)
                root = null;
        }

        private void ApplySpecialRootOptions(DirectoryNode root)
        {
            string path = root.Path;
            bool rootNodeIsDrive = PathIsDrive(path);
            if (rootNodeIsDrive)
            {
                // full drive scan, show disk free space if asked
                DriveInfo info = new DriveInfo(path);

                // Unknown files
                DirectoryNode dirNode = new DirectoryNode("");
                // Unknown files = taille du disque - espace libre - fichiers trouvés
                dirNode.TotalSize = info.TotalSize - info.TotalFreeSpace - root.TotalSize;
                dirNode.FilesSize = dirNode.TotalSize;
                dirNode.DirectoryType = SpecialDirTypes.UnknownPart;
                dirNode.ExistsUncalcSubDir = false;
                dirNode.Name = HDGTools.resManager.GetString("UnknownFiles");
                dirNode.Parent = root;
                root.Children.Add(dirNode);
                root.TotalSize += dirNode.TotalSize;


                // free disk space
                DirectoryNode dirNodeFS = new DirectoryNode("");
                dirNodeFS.TotalSize = info.TotalFreeSpace;
                if (showDiskFreeSpace)
                {
                    dirNodeFS.DirectoryType = SpecialDirTypes.FreeSpaceAndShow;
                    root.TotalSize += dirNodeFS.TotalSize;
                }
                else
                {
                    dirNodeFS.DirectoryType = SpecialDirTypes.FreeSpaceAndHide;
                }
                dirNodeFS.ExistsUncalcSubDir = false;
                dirNodeFS.Name = HDGTools.resManager.GetString("FreeSpace");
                dirNodeFS.Parent = root;
                root.Children.Add(dirNodeFS);
            }
        }

        /// <summary>
        /// Indique si le chemin passé en paramètre représente un lecteur ou non.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool PathIsDrive(string path)
        {
            bool rootNodeIsDrive = path.IndexOf('\\') == path.LastIndexOf('\\') // 1 seul '\' dans le chemin
                // le seul '\' du chemin est le dernier car du chemin ( en effet, c:\test n'est pas la racine)                   
                && path.IndexOf('\\') == path.Length - 1;
            return rootNodeIsDrive;
        }

        /// <summary>
        /// Méthode récursive construisant l'arborescence de DirectoryNode.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="maxLevel"></param>
        private void ConstruireArborescence(DirectoryNode dir, int maxLevel)
        {
            if (pleaseCancelCurrentWork)
            {
                workCanceled = true;
                return;
            }
            try
            {
                if (printInfoDeleg != null)
                    printInfoDeleg(scanningMessage + dir.Path + "...");
                DirectoryInfo dirInfo = new DirectoryInfo(dir.Path);
                if (maxLevel <= 0)
                {
                    dir.ExistsUncalcSubDir = (dirInfo.GetDirectories().Length > 0);
                    FileInfo[] fis = dirInfo.GetFiles("*", SearchOption.AllDirectories);
                    foreach (FileInfo fi in fis)
                    {
                        if (pleaseCancelCurrentWork)
                        {
                            workCanceled = true;
                            return;
                        }
                        try
                        {
                            dir.TotalSize += fi.Length;
                        }
                        catch (Exception ex)
                        {
                            // Une erreur de type FileNotFoundException peut survenir.
                            // Elle peut être due à une PathTooLongException.
                            Trace.TraceError("Error during file analysis (" + dir.Path +
                                "\\" + fi.Name + "). Details: " + HDGTools.PrintError(ex));
                        }
                    }
                }
                else
                {
                    // Add file sizes.
                    FileInfo[] fis = dirInfo.GetFiles();
                    foreach (FileInfo fi in fis)
                    {
                        if (pleaseCancelCurrentWork)
                        {
                            workCanceled = true;
                            return;
                        }
                        try
                        {
                            dir.FilesSize += fi.Length;
                        }
                        catch (Exception ex)
                        {
                            // Une erreur de type FileNotFoundException peut survenir.
                            // Elle peut être due à une PathTooLongException.
                            Trace.TraceError("Error during file analysis (" + dir.Path +
                                "\\" + fi.Name + "). Details: " + HDGTools.PrintError(ex));
                        }
                    }
                    dir.TotalSize += dir.FilesSize;

                    // Add subdirectory sizes.
                    DirectoryInfo[] dis = dirInfo.GetDirectories();
                    foreach (DirectoryInfo di in dis)
                    {
                        if (pleaseCancelCurrentWork)
                        {
                            workCanceled = true;
                            return;
                        }
                        DirectoryNode dirNode = new DirectoryNode(di.FullName);
                        ConstruireArborescence(dirNode, maxLevel - 1);
                        dirNode.Parent = dir;
                        dir.Children.Add(dirNode);
                        dir.TotalSize += dirNode.TotalSize;
                        if (dir.ProfondeurMax < dirNode.ProfondeurMax + 1)
                            dir.ProfondeurMax = dirNode.ProfondeurMax + 1;
                    }
                    dir.ExistsUncalcSubDir = false;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error during folder analysis (" + dir.Path + "). Folder skiped. Details: " + HDGTools.PrintError(ex));
                dir.Name += String.Format(HDGTools.resManager.GetString("ErrorLoading"), dir.Name, ex.Message);
            }
        }

        /// <summary>
        /// Complète une arborescence existante pour lui donner une profondeur max de maxLevel.
        /// I.e. si l'arborescence n'a été calculée que sur n niveaux et que maxLevel vaut n+4,
        /// la méthode va calculer les 4 niveaux manquant.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public bool CompleterArborescence(DirectoryNode node, int maxLevel)
        {
            if (!this.autoRefreshAllowed)
                return false;
            if (node.ProfondeurMax >= maxLevel)
                return true; // rien à faire !

            if (node.ProfondeurMax == 1 && node.ExistsUncalcSubDir)
            {
                long dirPreviousTotalSize = node.TotalSize;
                node.TotalSize = 0;
                node.FilesSize = 0;
                ConstruireArborescence(node, maxLevel - 1);
                if (dirPreviousTotalSize != node.TotalSize)
                    IncrementerTailleParents(node, node.TotalSize - dirPreviousTotalSize);
            }
            else
            {
                foreach (DirectoryNode fils in node.Children)
                {
                    CompleterArborescence(fils, maxLevel - 1);
                }
            }
            return true;
        }

        /// <summary>
        /// Incrémente la taille totale de tous les parents d'un noeud donné.
        /// </summary>
        /// <param name="node">Noeud dont les parents sont à mettre à jour.</param>
        /// <param name="tailleAjoutee">Montant à ajouter.</param>
        private void IncrementerTailleParents(DirectoryNode node, long tailleAjoutee)
        {
            if (node.Parent == null)
                return;
            node.Parent.TotalSize += tailleAjoutee;
            IncrementerTailleParents(node.Parent, tailleAjoutee);
        }


        public bool RafraichirArborescence(DirectoryNode node)
        {
            if (!this.autoRefreshAllowed)
                return false;

            long dirPreviousTotalSize = node.TotalSize;
            node.TotalSize = 0;
            node.FilesSize = 0;
            node.Children = new List<DirectoryNode>();
            if (!Directory.Exists(node.Path))
            {
                IncrementerTailleParents(node, -dirPreviousTotalSize);
                return true;
            }
            ConstruireArborescence(node, node.ProfondeurMax - 1);
            if (dirPreviousTotalSize != node.TotalSize)
                IncrementerTailleParents(node, node.TotalSize - dirPreviousTotalSize);
            RafraichirEspaceLibre(node);
            return true;
        }

        private void RafraichirEspaceLibre(DirectoryNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IXmlSerializable Membres

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private string[] compatibleVersionsList = new string[] {
                        "1.1.0.0"
                    };


        public void ReadXml(System.Xml.XmlReader reader)
        {
            // Début élément MoteurGraphiqueur
            reader.ReadStartElement();

            string version = reader.ReadElementContentAsString();

            if (version != AboutBox.AssemblyVersion
                && Array.IndexOf<string>(compatibleVersionsList, version) == -1)
                throw new IncompatibleVersionException();

            analyzeDate = reader.ReadElementContentAsDateTime();

            XmlSerializer serializer = new XmlSerializer(typeof(DirectoryNode));
            root = (DirectoryNode)serializer.Deserialize(reader);

            // Fin élément MoteurGraphiqueur
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            //string ns = "http://HDGraph.tools.laugel.fr/MoteurGraphiqueur.xsd";

            writer.WriteElementString("HdgVersion", AboutBox.AssemblyVersion);
            writer.WriteElementString("AnalyzeDate", analyzeDate.ToString("s"));

            XmlSerializer serializer = new XmlSerializer(typeof(DirectoryNode));//, ns);
            serializer.Serialize(writer, root);
        }

        #endregion

        public void ApplyFreeSpaceOption(DirectoryNode directoryNode)
        {
            if (directoryNode == null)
                return;
            // Rechercher le répertoire racine
            DirectoryNode root = directoryNode;
            while (root.Parent != null)
                root = root.Parent;
            if (PathIsDrive(root.Path))
            {
                DirectoryNode freeSpaceNode = null;
                foreach (DirectoryNode node in root.Children)
                {
                    if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndHide
                        || node.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
                    {
                        freeSpaceNode = node;
                        break; // sortir du foreach, car le node de l'espace libre est trouvé.
                    }
                }
                if (freeSpaceNode != null)
                {
                    if (this.showDiskFreeSpace
                        && freeSpaceNode.DirectoryType == SpecialDirTypes.FreeSpaceAndHide)
                    {
                        // afficher l'espace libre
                        freeSpaceNode.DirectoryType = SpecialDirTypes.FreeSpaceAndShow;
                        root.TotalSize += freeSpaceNode.TotalSize;
                    }
                    else if (!this.showDiskFreeSpace
                        && freeSpaceNode.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
                    {
                        // masquer l'espace libre
                        freeSpaceNode.DirectoryType = SpecialDirTypes.FreeSpaceAndHide;
                        root.TotalSize -= freeSpaceNode.TotalSize;
                    }
                }
            }
        }
    }
}
