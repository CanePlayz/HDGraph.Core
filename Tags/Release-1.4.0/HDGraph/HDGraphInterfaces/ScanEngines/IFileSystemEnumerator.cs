using System;
using System.Collections.Generic;
using System.Text;

namespace HDGraph.Interfaces.ScanEngines
{
    public interface IFileSystemEnumerator : IDisposable
    {
        System.Collections.Generic.IEnumerable<IExtendedFileInfo> Matches();

        IList<string> LastErrors
        {
            get;
        }

        bool LastRootHasSubDir
        {
            get;
        }
    }
}
