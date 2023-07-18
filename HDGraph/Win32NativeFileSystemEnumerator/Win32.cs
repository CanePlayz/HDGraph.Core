using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.IO;

namespace HDGraph.Win32NativeFileSystemEnumerator.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal sealed class FindData
    {
        public int fileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string fileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string alternateFileName;
    }

    /// <summary>
    /// SafeHandle class for holding find handles
    /// </summary>
    internal sealed class SafeFindHandle : Microsoft.Win32.SafeHandles.SafeHandleMinusOneIsInvalid
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SafeFindHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Release the find handle
        /// </summary>
        /// <returns>true if the handle was released</returns>
        protected override bool ReleaseHandle()
        {
            return SafeNativeMethods.FindClose(handle);
        }
    }


    /// <summary>
    /// Wrapper for P/Invoke methods used by FileSystemEnumerator
    /// </summary>
    internal static class SafeNativeMethods
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern SafeFindHandle FindFirstFile(String fileName, [In, Out] FindData findFileData);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FindNextFile(SafeFindHandle hFindFile, [In, Out] FindData lpFindFileData);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FindClose(IntPtr hFindFile);
    }
}
