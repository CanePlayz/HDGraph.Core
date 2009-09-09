using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using System.Drawing;

namespace HDGraph.Interfaces.DrawEngines
{
    public enum DrawEngineType
    {
        BitmapDrawEngine,
        ControlDrawEngine
    }

    public interface IDrawEngineContract
    {
        DrawEngineType EngineType { get; }

        string Name { get; }

        string Description { get; }

        IControlTypeEngine GetNewControlTypeEngine();

        IBitmapTypeEngine GetNewBitmapTypeEngine();
    }

    public interface IControlTypeEngine
    {
        System.Windows.Forms.Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options);

        event EventHandler<NodeContextEventArgs> ContextMenuRequired;
    }

    public interface IBitmapTypeEngine
    {
        Object GenerateBitmapFromNode(IDirectoryNode node);
    }

    public class NodeContextEventArgs : EventArgs
    {
        public IDirectoryNode Node { get; set; }

        public PointF Position { get; set; }
    }
}
