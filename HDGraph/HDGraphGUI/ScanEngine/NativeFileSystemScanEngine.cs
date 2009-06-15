using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using HDGraph.Interfaces;
using HDGraph.Win32NativeFileSystemEnumerator;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.ScanEngine
{
    public class NativeFileSystemScanEngine : HDGraphScanEngineBase
    {

        /// <summary>
        /// Méthode récursive construisant l'arborescence de DirectoryNode.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="maxLevel"></param>
        protected override void ConstruireArborescence(IDirectoryNode dir, int maxLevel)
        {
            if (pleaseCancelCurrentWork)
            {
                workCanceled = true;
                return;
            }
            try
            {
                if (PrintInfoDeleg != null)
                    PrintInfoDeleg(scanningMessage + dir.Path + "...");
                if (maxLevel <= 0)
                {
                    // Scanning in one time all files of the current directory AND its sub-directories.
                    ScanFilesOfDir(dir, true);
                    dir.TotalSize = dir.FilesSize;
                }
                else
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir.Path);
                    
                    // Add sub dir.
                    if (pleaseCancelCurrentWork)
                    {
                        workCanceled = true;
                    }
                    else
                    {
                        // Add subdirectory sizes.
                        DirectoryInfo[] dis = dirInfo.GetDirectories();
                        foreach (DirectoryInfo di in dis)
                        {
                            if (pleaseCancelCurrentWork)
                            {
                                workCanceled = true;
                                break;
                            }
                            DirectoryNode dirNode = new DirectoryNode(di.FullName);
                            try
                            {

                                // TODO : détecter hardlink/junction points.
                                //if ((di.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                                //{
                                //    string path = JunctionPoint.GetTargetOrNull(di.FullName);
                                //    // TODO
                                //}

                                ConstruireArborescence(dirNode, maxLevel - 1);
                                dirNode.Parent = dir;
                                dir.Children.Add(dirNode);
                                dir.TotalSize += dirNode.TotalSize;
                                if (dir.DepthMaxLevel < dirNode.DepthMaxLevel + 1)
                                    dir.DepthMaxLevel = dirNode.DepthMaxLevel + 1;
                            }
                            catch (Exception ex)
                            {
                                HandleAnalysisException(dirNode, ex);
                            }
                        }
                        dir.ExistsUncalcSubDir = false;
                    }
                    // Scanning all files of the current directory BUT no its sub-directories.
                    ScanFilesOfDir(dir, false);
                    dir.TotalSize += dir.FilesSize;

                }
            }
            catch (Exception ex)
            {
                HandleAnalysisException(dir, ex);
            }
        }

        private void ScanFilesOfDir(IDirectoryNode dir, bool includeFilesOfSubdir)
        {
            FileSystemEnumerator enumerator = new FileSystemEnumerator(dir.Path, null, includeFilesOfSubdir);
            foreach (IExtendedFileInfo fi in enumerator.Matches())
            {
                if (pleaseCancelCurrentWork)
                {
                    workCanceled = true;
                    break;
                }
                else
                {
                    dir.DirectoryFilesNumber++;
                    try
                    {
                        // TODO : détecter hardlink/junction points.
                        //if ((fi.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                        //{
                        //    string path = JunctionPoint.GetTargetOrNull(fi.FullName);
                        //    // TODO
                        //}
                        dir.FilesSize += fi.Size;
                    }
                    catch (Exception ex)
                    {
                        // Une erreur de type FileNotFoundException peut survenir.
                        // Elle peut être due à une PathTooLongException.
                        Trace.TraceError("Error during file analysis (" + dir.Path +
                            "\\" + fi.FileName + "). Details: " + HDGTools.PrintError(ex));
                        ErrorList.Add(new ScanError()
                        {
                            FileOrDirPath = fi.FileName,
                            Exception = ex
                        });
                    }
                }
            }
            dir.ExistsUncalcSubDir = enumerator.LastRootHasSubDir;
            foreach (string error in enumerator.LastErrors)
            {
                ErrorList.Add(new ScanError()
                {
                    FileOrDirPath = error,
                    Exception = new InvalidOperationException()
                });
            }
        }


        protected bool HasSubdir(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            bool existsUncalcSubDir = (dirInfo.GetDirectories().Length > 0);
            return existsUncalcSubDir;
        }
    }
}
