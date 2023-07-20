using System;
using System.Collections.Generic;
using System.Text;

namespace HDGraph.Interfaces.ScanEngines
{

    public enum SpecialDirTypes : short
    {
        /// <summary>
        /// An ordinary folder.
        /// </summary>
        NotSpecial,
        /// <summary>
        /// A list of folders, which are too small to appear individualy on the graph.
        /// </summary>
        SubDirectoryCollection,
        /// <summary>
        /// Free space of the drive, which has been included in the total size of the root.
        /// </summary>
        FreeSpaceAndShow,
        /// <summary>
        /// Free space of the drive, which has NOT been included in the total size of the root.
        /// </summary>
        FreeSpaceAndHide,
        /// <summary>
        /// All space which couldn't be scanned (because of scan errors or lost space due to cluster size, etc).
        /// </summary>
        UnknownPart,
        /// <summary>
        /// One single directory whith a scan error : Size is unknown or approximate.
        /// </summary>
        ScanError
    }
}