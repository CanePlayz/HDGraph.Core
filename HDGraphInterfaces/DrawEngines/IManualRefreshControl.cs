namespace HDGraph.Interfaces.DrawEngines
{
    public interface IManualRefreshControl
    {
        void ForceRefresh();

        bool RotationInProgress { get; set; }

        bool TextChangeInProgress { get; set; }

        bool Resizing { get; set; }
    }
}
