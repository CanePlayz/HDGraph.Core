using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HDGraphiqueur
{
    public class MoteurGraphiqueur
    {
        private DirectoryNode root = null;

        internal DirectoryNode Root
        {
            get { return root; }
        }

        public delegate void PrintInfoDelegate(string message);

        private PrintInfoDelegate printInfoDeleg = null;

        public PrintInfoDelegate PrintInfoDeleg
        {
            get { return printInfoDeleg; }
            set { printInfoDeleg = value; }
        }


        public void ConstruireArborescence(string path, int maxLevel) {
            if (maxLevel < 1)
                throw new ArgumentOutOfRangeException("maxLevel", "Il faut afficher au moins 1 niveau !");
            root = new DirectoryNode(path);
            
            ConstruireArborescence(root, maxLevel-1);
        }

        private void ConstruireArborescence(DirectoryNode dir, int maxLevel)
        {
            try
            {
                if (printInfoDeleg != null)
                    printInfoDeleg("Scanning " + dir.Path + "...");
                DirectoryInfo dirInfo = new DirectoryInfo(dir.Path);
                if (maxLevel <= 0)
                {
                    FileInfo[] fis = dirInfo.GetFiles("*", SearchOption.AllDirectories);
                    foreach (FileInfo fi in fis)
                    {
                        dir.TotalSize += fi.Length;
                    }
                }
                else
                {
                    // Add file sizes.
                    FileInfo[] fis = dirInfo.GetFiles();
                    foreach (FileInfo fi in fis)
                    {
                        dir.FilesSize += fi.Length;
                    }
                    dir.TotalSize += dir.FilesSize;

                    // Add subdirectory sizes.
                    DirectoryInfo[] dis = dirInfo.GetDirectories();
                    foreach (DirectoryInfo di in dis)
                    {
                        DirectoryNode dirNode = new DirectoryNode(di.FullName);
                        ConstruireArborescence(dirNode, maxLevel - 1);
                        dirNode.Parent = dir;
                        dir.Children.Add(dirNode);
                        dir.TotalSize += dirNode.TotalSize;
                        if (dir.ProfondeurMax < dirNode.ProfondeurMax + 1)
                            dir.ProfondeurMax = dirNode.ProfondeurMax + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: autre msg ?
                dir.Name += "Erreur lors du chargement de " + dir.Name + ": " + ex.Message;
            }
        }
    }
}
