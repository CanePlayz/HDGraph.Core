using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using System.Drawing;

namespace HDGraph.Interfaces.DrawEngines
{

    public interface IDrawEngineContract
    {
        string Name { get; }

        string Description { get; }

        IDrawEngine GetNewEngine();

        Guid Guid { get; }
    }
}
