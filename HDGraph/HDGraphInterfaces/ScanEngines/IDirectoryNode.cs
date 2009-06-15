using System;
using System.Collections.Generic;

namespace HDGraph.Interfaces.ScanEngines
{
    public interface IDirectoryNode
    {
        List<IDirectoryNode> Children { get; set; }

        int DepthMaxLevel { get; set; }

        long DirectoryFilesNumber { get; set; }

        SpecialDirTypes DirectoryType { get; set; }

        bool ExistsUncalcSubDir { get; set; }

        long FilesSize { get; set; }

        string Name { get; set; }

        IDirectoryNode Parent { get; set; }

        string Path { get; set; }

        IDirectoryNode Root { get; }

        long TotalRecursiveFilesNumber { get; }

        long TotalSize { get; set; }

        bool HasMoreChildrenThan(long threshold);

        string HumanReadableTotalSize { get; }
    }
}
