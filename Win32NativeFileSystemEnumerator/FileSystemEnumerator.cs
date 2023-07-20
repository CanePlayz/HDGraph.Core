using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using System.IO;
using HDGraph.Interfaces;
using System.Text.RegularExpressions;

/*=============================================================================
    FileSystemEnumerator.cs: Lazy enumerator for finding files in subdirectories.

    Copyright (c) 2006 Carl Daniel. Distributed under the Boost
    Software License, Version 1.0. (See accompanying file
    LICENSE.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
=============================================================================*/

// ---------------------------------------------------------------------------
// FileSystemEnumerator implementation
// ---------------------------------------------------------------------------
namespace HDGraph.Win32NativeFileSystemEnumerator
{
    /// <summary>
    /// File system enumerator.  This class provides an easy to use, efficient mechanism for searching a list of
    /// directories for files matching a list of file specifications.  The search is done incrementally as matches
    /// are consumed, so the overhead before processing the first match is always kept to a minimum.
    /// </summary>
    public class FileSystemEnumerator : IFileSystemEnumerator
    {
        /// <summary>
        /// Information that's kept in our stack for simulated recursion
        /// </summary>
        private struct SearchInfo
        {
            /// <summary>
            /// Find handle returned by FindFirstFile
            /// </summary>
            public Win32.SafeFindHandle Handle;

            /// <summary>
            /// Path that was searched to yield the find handle.
            /// </summary>
            public string Path;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="h">Find handle returned by FindFirstFile.</param>
            /// <param name="p">Path corresponding to find handle.</param>
            public SearchInfo(Win32.SafeFindHandle h, string p)
            {
                Handle = h;
                Path = p;
            }
        }

        /// <summary>
        /// Stack of open scopes.  This is a member (instead of a local variable)
        /// to allow Dispose to close any open find handles if the object is disposed
        /// before the enumeration is completed.
        /// </summary>
        private Stack<SearchInfo> m_scopes;

        /// <summary>
        /// Array of paths to be searched.
        /// </summary>
        private string[] m_paths;

        /// <summary>
        /// Array of regular expressions that will detect matching files.
        /// </summary>
        private List<Regex> m_fileSpecs;

        /// <summary>
        /// If true, sub-directories are searched.
        /// </summary>
        private bool m_includeSubDirs;

        /// <summary>
        /// If true, analyse directories and ignore it if it's a symLink/hardLink/junctionPoint.
        /// </summary>
        private bool ignoreDirectoryLinks;

        #region IDisposable implementation

        /// <summary>
        /// IDisposable.Dispose
        /// </summary>
        public void Dispose()
        {
            while (m_scopes.Count > 0)
            {
                SearchInfo si = m_scopes.Pop();
                si.Handle.Close();
            }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pathsToSearch">path to search.</param>
        /// <param name="fileTypesToMatch">Semicolon- or comma-delimitted list of wildcard filespecs to match.</param>
        /// <param name="includeSubDirs">If true, subdirectories are searched.</param>
        public FileSystemEnumerator(string pathsToSearch, string fileTypesToMatch, bool includeSubDirs, bool ignoreDirectoryLinks)
        {
            m_scopes = new Stack<SearchInfo>();
            this.ignoreDirectoryLinks = ignoreDirectoryLinks;
            // check for nulls
            if (null == pathsToSearch)
                throw new ArgumentNullException("pathsToSearch");

            // m_paths = pathsToSearch.Split(new char[] { ';', ',' }); // a single folder can contains ";" or "," ==> so don't split !
            m_paths = new string[] { pathsToSearch };
            m_fileSpecs = null;
            m_includeSubDirs = includeSubDirs;

            if (fileTypesToMatch != null)
            {

                // make sure spec doesn't contain invalid characters
                if (fileTypesToMatch.IndexOfAny(new char[] { ':', '<', '>', '/', '\\' }) >= 0)
                    throw new ArgumentException("invalid cahracters in wildcard pattern", "fileTypesToMatch");

                string[] specs = fileTypesToMatch.Split(new char[] { ';', ',' });
                m_fileSpecs = new List<Regex>(specs.Length);
                foreach (string spec in specs)
                {

                    // trim whitespace off file spec and convert Win32 wildcards to regular expressions
                    string pattern = spec
                      .Trim()
                      .Replace(".", @"\.")
                      .Replace("*", @".*")
                      .Replace("?", @".?")
                      ;
                    m_fileSpecs.Add(
                      new Regex("^" + pattern + "$", RegexOptions.IgnoreCase)
                      );
                }
            }
        }


        public IEnumerable<IExtendedFileInfo> Matches()
        {
            lastErrors = new List<string>();
            ignoredLinks = new List<string>();
            Stack<string> pathsToSearch = new Stack<string>(m_paths);
            Win32.FindData findData = new Win32.FindData();
            string path, fileName;

            while (0 != pathsToSearch.Count)
            {
                path = pathsToSearch.Pop(); //.Trim()

                using (Win32.SafeFindHandle handle = Win32.SafeNativeMethods.FindFirstFile(
                    Path.Combine(path, "*"), findData))
                {
                    if (!handle.IsInvalid)
                    {
                        do
                        {
                            fileName = findData.fileName;
                            if (string.IsNullOrEmpty(fileName)) continue;
                            if (string.Equals(fileName, ".", StringComparison.Ordinal)) continue;
                            if (string.Equals(fileName, "..", StringComparison.Ordinal)) continue;

                            if (0 != ((int)FileAttributes.Directory & findData.fileAttributes))
                            {
                                if (m_includeSubDirs)
                                {
                                    if (ignoreDirectoryLinks && (0 != ((int)FileAttributes.ReparsePoint & findData.fileAttributes)))
                                    {
                                        ignoredLinks.Add(path);
                                        continue;
                                    }

                                    pathsToSearch.Push(Path.Combine(path, fileName));
                                    lastRootHasSubdir = true;
                                }
                            }
                            else
                            {
                                bool fileMatch = true;
                                if (m_fileSpecs != null)
                                {
                                    fileMatch = false;
                                    foreach (Regex fileSpec in m_fileSpecs)
                                    {
                                        if (fileSpec.IsMatch(fileName))
                                        {
                                            fileMatch = true;
                                            break;
                                        }
                                    }
                                }
                                if (fileMatch)
                                {
                                    ExtendedFileInfo extInfo = new ExtendedFileInfo()
                                    {
                                        FileName = fileName,
                                        Size = GetSize(findData),
                                        FolderPath = path,
                                    };
                                    yield return extInfo;
                                }
                            }

                        }
                        while (Win32.SafeNativeMethods.FindNextFile(handle, findData));
                    }
                    else
                    {
                        lastErrors.Add(path);
                    }
                }
            }
        }

        private long GetSize(Win32.FindData findData)
        {
            return (long)findData.nFileSizeLow + (long)findData.nFileSizeHigh * 4294967296;
            //return ((findData.nFileSizeHigh << 0x20)
            //        | (findData.nFileSizeLow & ((long)0xffffffffL)));
        }

        private IList<string> lastErrors;

        public IList<string> LastErrors
        {
            get
            {
                return lastErrors;
            }
        }

        private IList<string> ignoredLinks;

        public IList<string> IgnoredLinks
        {
            get
            {
                return ignoredLinks;
            }
        }

        private bool lastRootHasSubdir;

        public bool LastRootHasSubDir
        {
            get { return lastRootHasSubdir; }
        }

    }
}
