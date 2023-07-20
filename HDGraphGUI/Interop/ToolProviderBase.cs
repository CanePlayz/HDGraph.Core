using System;
using System.Collections.Generic;
using System.Drawing;

namespace HDGraph.Interop
{
    public abstract class ToolProviderBase
    {
        #region Current OS (Win, Linux, Mac, etc).

        private static EnvironmentTarget? currentEnvironment = null;

        public static EnvironmentTarget GetEnvironmentType()
        {
            if (!currentEnvironment.HasValue)
            {
                switch (Environment.OSVersion.Platform)
                {
                    // From 3.5 SP1 only ???
                    //case PlatformID.MacOSX:
                    //    currentEnvironment = EnvironmentTarget.Mac;
                    //    break;
                    case PlatformID.Unix:
                        currentEnvironment = EnvironmentTarget.Unix;
                        break;
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                        if (HDGraph.Interop.Windows.VistaTools.IsReallyVista())
                            currentEnvironment = EnvironmentTarget.WindowsVista;
                        else
                            currentEnvironment = EnvironmentTarget.WindowsXp;
                        break;
                    // From 3.5 SP1 only ???
                    //case PlatformID.Xbox:
                    default:
                        currentEnvironment = EnvironmentTarget.Unknown;
                        break;
                }
            }
            return currentEnvironment.Value;
        }

        public static bool CurrentOsIsWindows()
        {
            EnvironmentTarget env = GetEnvironmentType();
            return (env == EnvironmentTarget.WindowsVista
                    || env == EnvironmentTarget.WindowsXp);
        }

        #endregion

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
                        case EnvironmentTarget.Unix:
                        case EnvironmentTarget.Mac:
                            current = new Unknown.UnknownEnvToolProvider();
                            break;
                        case EnvironmentTarget.Unknown:
                            current = new Unknown.UnknownEnvToolProvider();
                            break;
                        default:
                            break;
                    }
                }
                return current;
            }
        }

        /// <summary>
        /// List the user favorit paths, which will be displayed in the "shortcuts" bar.
        /// For example, in Windows, list all available drives.
        /// </summary>
        /// <returns></returns>
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
