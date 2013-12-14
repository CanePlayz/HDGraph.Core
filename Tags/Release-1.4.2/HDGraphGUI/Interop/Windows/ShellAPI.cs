using System;
using System.Text;
using System.Runtime.InteropServices;

namespace HDGraph.Interop.Windows
{
    internal class ShellAPI
    {
        #region DLL Imports

        /// <summary>
        /// Provides access to function required to delete handle.
        /// </summary>
        /// <param name="hIcon">Pointer to icon handle.</param>
        /// <returns>N/A</returns>
        [DllImport("User32.dll")]
        internal static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);


        #endregion

        #region Enumerations

        [Flags]
        internal enum SHGFI
        {
            SHGFI_ICON = 0x000000100,
            SHGFI_DISPLAYNAME = 0x000000200,
            SHGFI_TYPENAME = 0x000000400,
            SHGFI_ATTRIBUTES = 0x000000800,
            SHGFI_ICONLOCATION = 0x000001000,
            SHGFI_EXETYPE = 0x000002000,
            SHGFI_SYSICONINDEX = 0x000004000,
            SHGFI_LINKOVERLAY = 0x000008000,
            SHGFI_SELECTED = 0x000010000,
            SHGFI_ATTR_SPECIFIED = 0x000020000,
            SHGFI_LARGEICON = 0x000000000,
            SHGFI_SMALLICON = 0x000000001,
            SHGFI_OPENICON = 0x000000002,
            SHGFI_SHELLICONSIZE = 0x000000004,
            SHGFI_PIDL = 0x000000008,
            SHGFI_USEFILEATTRIBUTES = 0x000000010,
            SHGFI_ADDOVERLAYS = 0x000000020,
            SHGFI_OVERLAYINDEX = 0x000000040
        }

        [Flags]
        internal enum CSIDL : uint
        {
            CSIDL_DESKTOP = 0x0000,
            CSIDL_WINDOWS = 0x0024
        }

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHFILEINFO
        {
            internal const int NAMESIZE = 80;
            internal IntPtr hIcon;
            internal int iIcon;
            internal uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            internal string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            internal string szTypeName;
        };

        #endregion

        internal const int MAX_PATH = 256;
        [StructLayout(LayoutKind.Sequential)]
        internal struct SHITEMID
        {
            internal ushort cb;
            [MarshalAs(UnmanagedType.LPArray)]
            internal byte[] abID;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ITEMIDLIST
        {
            internal SHITEMID mkid;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BROWSEINFO
        {
            internal IntPtr hwndOwner;
            internal IntPtr pidlRoot;
            internal IntPtr pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            internal string lpszTitle;
            internal uint ulFlags;
            internal IntPtr lpfn;
            internal int lParam;
            internal IntPtr iImage;
        }

        // Browsing for directory.
        internal const uint BIF_RETURNONLYFSDIRS = 0x0001;
        internal const uint BIF_DONTGOBELOWDOMAIN = 0x0002;
        internal const uint BIF_STATUSTEXT = 0x0004;
        internal const uint BIF_RETURNFSANCESTORS = 0x0008;
        internal const uint BIF_EDITBOX = 0x0010;
        internal const uint BIF_VALIDATE = 0x0020;
        internal const uint BIF_NEWDIALOGSTYLE = 0x0040;
        internal const uint BIF_USENEWUI = (BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
        internal const uint BIF_BROWSEINCLUDEURLS = 0x0080;
        internal const uint BIF_BROWSEFORCOMPUTER = 0x1000;
        internal const uint BIF_BROWSEFORPRINTER = 0x2000;
        internal const uint BIF_BROWSEINCLUDEFILES = 0x4000;
        internal const uint BIF_SHAREABLE = 0x8000;



        internal const uint SHGFI_ICON = 0x000000100;     // get icon
        internal const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
        internal const uint SHGFI_TYPENAME = 0x000000400;     // get type name
        internal const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
        internal const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
        internal const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
        internal const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
        internal const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
        internal const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
        internal const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
        internal const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
        internal const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
        internal const uint SHGFI_OPENICON = 0x000000002;     // get open icon
        internal const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
        internal const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
        internal const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
        internal const uint SHGFI_ADDOVERLAYS = 0x000000020;     // apply the appropriate overlays
        internal const uint SHGFI_OVERLAYINDEX = 0x000000040;     // Get the index of the overlay

        internal const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        internal const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        [DllImport("Shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags
            );
    }
}
