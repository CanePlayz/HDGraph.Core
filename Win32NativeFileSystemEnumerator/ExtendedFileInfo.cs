using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.Win32NativeFileSystemEnumerator
{
    internal class ExtendedFileInfo : IExtendedFileInfo
    {
        public string FileName { get; set; }

        public long Size { get; set; }

        public string FolderPath { get; set; }
    }
}
