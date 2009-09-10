using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.Interfaces.DrawEngines
{
    public interface IDrawEngineType
    {
        event EventHandler<NodeContextEventArgs> ContextMenuRequired;
    }

    public interface IControlTypeEngine : IDrawEngineType
    {
        System.Windows.Forms.Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options);
    }

    public interface IBitmapTypeEngine : IDrawEngineType
    {
        Object GenerateBitmapFromNode(IDirectoryNode node);
    }
}
