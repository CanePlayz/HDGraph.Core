namespace HDGraph.Interfaces.ScanEngines
{
    public interface IExtendedFileInfo
    {
        string FileName { get; }

        long Size { get; }

        string FolderPath { get; }
    }
}
