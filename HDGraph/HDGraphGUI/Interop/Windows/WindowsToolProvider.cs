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
    }
}
