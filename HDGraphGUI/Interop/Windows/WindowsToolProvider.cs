using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HDGraph.Interop.Windows
{
    public class WindowsToolProvider : ToolProviderBase
    {
        public override List<PathWithIcon> ListFavoritPath()
        {
            List<PathWithIcon> result = new List<PathWithIcon>();
            Dictionary<int, Icon> iconList = new Dictionary<int, Icon>();

            ShellAPI.SHFILEINFO shInfo = new ShellAPI.SHFILEINFO();
            ShellAPI.SHGFI dwAttribs =
                ShellAPI.SHGFI.SHGFI_ICON |
                ShellAPI.SHGFI.SHGFI_SMALLICON |
                ShellAPI.SHGFI.SHGFI_SYSICONINDEX |
                ShellAPI.SHGFI.SHGFI_DISPLAYNAME;

            foreach (string drive in System.IO.Directory.GetLogicalDrives())
            {
                IntPtr m_pHandle = ShellAPI.SHGetFileInfo(drive, ShellAPI.FILE_ATTRIBUTE_NORMAL, out shInfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(shInfo), dwAttribs);
                if (!m_pHandle.Equals(IntPtr.Zero))
                {
                    if (!iconList.ContainsKey(shInfo.iIcon))
                    {
                        iconList.Add(shInfo.iIcon, Icon.FromHandle(shInfo.hIcon).Clone() as Icon);
                        ShellAPI.DestroyIcon(shInfo.hIcon);
                    }
                    PathWithIcon p = new PathWithIcon();
                    p.Name = shInfo.szDisplayName;
                    p.Icon = iconList[shInfo.iIcon];
                    p.Path = drive;
                    result.Add(p);
                }
            }
            return result;
        }


        /// <summary>
        /// Returns an icon for a given file - indicated by the name parameter.
        /// </summary>
        /// <remarks>Code from http://www.codeproject.com/cs/files/fileicon.asp </remarks>
        /// <param name="name">Pathname for file.</param>
        /// <param name="size">Large or small</param>
        /// <param name="linkOverlay">Whether to include the link icon</param>
        /// <returns>System.Drawing.Icon</returns>
        public override Icon GetFileIcon(string name, IconSize size, bool linkOverlay)
        {
            ShellAPI.SHFILEINFO shfi = new ShellAPI.SHFILEINFO();
            uint flags = ShellAPI.SHGFI_ICON | ShellAPI.SHGFI_USEFILEATTRIBUTES;

            if (true == linkOverlay) flags += ShellAPI.SHGFI_LINKOVERLAY;

            /* Check the size specified for return. */
            if (IconSize.Small == size)
            {
                flags += ShellAPI.SHGFI_SMALLICON;
            }
            else
            {
                flags += ShellAPI.SHGFI_LARGEICON;
            }

            ShellAPI.SHGetFileInfo(name,
                ShellAPI.FILE_ATTRIBUTE_NORMAL,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);

            // Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
            System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
            ShellAPI.DestroyIcon(shfi.hIcon);		// Cleanup
            return icon;
        }

        /// <summary>
        /// Used to access system folder icons.
        /// </summary>
        /// <remarks>Code from http://www.codeproject.com/cs/files/fileicon.asp </remarks>
        /// <param name="size">Specify large or small icons.</param>
        /// <param name="folderType">Specify open or closed FolderType.</param>
        /// <returns>System.Drawing.Icon</returns>
        public override Icon GetFolderIcon(IconSize size, FolderType folderType)
        {
            // Need to add size check, although errors generated at present!
            uint flags = ShellAPI.SHGFI_ICON | ShellAPI.SHGFI_USEFILEATTRIBUTES;

            if (FolderType.Open == folderType)
            {
                flags += ShellAPI.SHGFI_OPENICON;
            }

            if (IconSize.Small == size)
            {
                flags += ShellAPI.SHGFI_SMALLICON;
            }
            else
            {
                flags += ShellAPI.SHGFI_LARGEICON;
            }

            // Get the folder icon
            ShellAPI.SHFILEINFO shfi = new ShellAPI.SHFILEINFO();
            ShellAPI.SHGetFileInfo(null,
                ShellAPI.FILE_ATTRIBUTE_DIRECTORY,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);

            System.Drawing.Icon.FromHandle(shfi.hIcon);	// Load the icon from an HICON handle

            // Now clone the icon, so that it can be successfully stored in an ImageList
            System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();

            ShellAPI.DestroyIcon(shfi.hIcon);		// Cleanup
            return icon;
        }
    }
}
