using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;

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
    }

    public interface IBitmapTypeEngine
    {
        Object GenerateBitmapFromNode(IDirectoryNode node);
    }
}
