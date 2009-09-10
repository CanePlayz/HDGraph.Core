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
}
