using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HDGraph.Interop
{
    public abstract class ToolProviderBase
    {
        public static EnvironmentTarget GetEnvironmentType()
        {
            return EnvironmentTarget.WindowsXp; // TODO
        }

        private static ToolProviderBase current;

        public static ToolProviderBase Current
        {
            get
            {
                if (current == null)
                {
                    EnvironmentTarget env = GetEnvironmentType();
                    switch (env)
                    {
                        case EnvironmentTarget.WindowsXp:
                        case EnvironmentTarget.WindowsVista:
                            current = new Windows.WindowsToolProvider();
                            break;
                        case EnvironmentTarget.Linux:
                            throw new NotImplementedException();
                            break;
                        case EnvironmentTarget.Mac:
                            throw new NotImplementedException();
                            break;
                        case EnvironmentTarget.Unknown:
                            throw new NotImplementedException();
                            break;
                        default:
                            break;
                    }
                }
                return current;
            }
        }

        public abstract List<PathWithIcon> ListFavoritPath();
        
        /// <summary>
        /// Returns an icon for a given file - indicated by the "name" parameter.
        /// </summary>
        /// <param name="name">Pathname for file.</param>
        /// <param name="size">Large or small</param>
        /// <param name="linkOverlay">Whether to include the "link" icon</param>
        /// <returns>System.Drawing.Icon</returns>
        public abstract Icon GetFileIcon(string name, IconSize size, bool linkOverlay);

        /// <summary>
        /// Used to access system folder icons.
        /// </summary>
        /// <param name="size">Specify large or small icons.</param>
        /// <param name="folderType">Specify open or closed FolderType.</param>
        /// <returns>System.Drawing.Icon</returns>
        public abstract Icon GetFolderIcon(IconSize size, FolderType folderType);

    }




    /// <summary>
    /// Options to specify the size of icons to return.
    /// </summary>
    public enum IconSize
    {
        /// <summary>
        /// Specify large icon - 32 pixels by 32 pixels.
        /// </summary>
        Large = 0,
        /// <summary>
        /// Specify small icon - 16 pixels by 16 pixels.
        /// </summary>
        Small = 1
    }

    /// <summary>
    /// Options to specify whether folders should be in the open or closed state.
    /// </summary>
    public enum FolderType
    {
        /// <summary>
        /// Specify open folder.
        /// </summary>
        Open = 0,
        /// <summary>
        /// Specify closed folder.
        /// </summary>
        Closed = 1
    }
}
