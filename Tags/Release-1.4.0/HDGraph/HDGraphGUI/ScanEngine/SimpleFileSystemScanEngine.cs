using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.ScanEngine
{
    public class SimpleFileSystemScanEngine : HDGraphScanEngineBase
    {

        /// <summary>
        /// Méthode récursive construisant l'arborescence de DirectoryNode.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="maxLevel"></param>
        protected override void BuildTreeInternal(IDirectoryNode dir, int maxLevel)
        {
            if (pleaseCancelCurrentWork)
            {
                workCanceled = true;
                return;
            }
            try
            {
                if (NotifyForNewInfo != null)
                    NotifyForNewInfo(scanningMessage + dir.Path + "...");
                if (maxLevel <= 0)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir.Path);
                    dir.ExistsUncalcSubDir = HasSubdir(dir.Path);
                    FileInfo[] fis = dirInfo.GetFiles("*", SearchOption.AllDirectories);
                    dir.DirectoryFilesNumber = fis.LongLength;
                    foreach (FileInfo fi in fis)
                    {
                        if (pleaseCancelCurrentWork)
                        {
                            workCanceled = true;
                            break;
                        }
                        else
                        {
                            try
                            {
                                // TODO : détecter hardlink/junction points.
                                //if ((fi.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                                //{
                                //    string path = JunctionPoint.GetTargetOrNull(fi.FullName);
                                //    // TODO
                                //}
                                dir.FilesSize += fi.Length;
                            }
                            catch (Exception ex)
                            {
                                // Une erreur de type FileNotFoundException peut survenir.
                                // Elle peut être due à une PathTooLongException.
                                Trace.TraceError("Error during file analysis (" + dir.Path +
                                    "\\" + fi.Name + "). Details: " + HDGTools.PrintError(ex));
                                ErrorList.Add(new ScanError()
                                {
                                    FileOrDirPath = fi.FullName,
                                    Exception = ex
                                });
                            }
                        }
                    }
                    dir.TotalSize = dir.FilesSize;
                }
                else
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir.Path);
                    // Add file sizes.
                    FileInfo[] fis = dirInfo.GetFiles();
                    dir.DirectoryFilesNumber = fis.LongLength;
                    foreach (FileInfo fi in fis)
                    {
                        if (pleaseCancelCurrentWork)
                        {
                            workCanceled = true;
                            break;
                        }
                        try
                        {
                            // TODO : détecter hardlink/junction points.
                            //if ((fi.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                            //{
                            //    string path = JunctionPoint.GetTargetOrNull(fi.FullName);
                            //    // TODO
                            //}
                            dir.FilesSize += fi.Length;
                        }
                        catch (Exception ex)
                        {
                            // Une erreur de type FileNotFoundException peut survenir.
                            // Elle peut être due à une PathTooLongException.
                            Trace.TraceError("Error during file analysis (" + dir.Path +
                                "\\" + fi.Name + "). Details: " + HDGTools.PrintError(ex));
                            ErrorList.Add(new ScanError()
                            {
                                FileOrDirPath = fi.FullName,
                                Exception = ex
                            });

                        }
                    }
                    if (pleaseCancelCurrentWork)
                    {
                        workCanceled = true;
                    }
                    else
                    {
                        dir.TotalSize += dir.FilesSize;

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

                                BuildTreeInternal(dirNode, maxLevel - 1);
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
                }
            }
            catch (Exception ex)
            {
                HandleAnalysisException(dir, ex);
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
