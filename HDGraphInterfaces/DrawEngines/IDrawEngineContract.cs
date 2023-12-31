﻿using System;

namespace HDGraph.Interfaces.DrawEngines
{

    public interface IDrawEngineContract
    {
        string Name { get; }

        string Description { get; }

        IDrawEngine GetNewEngine();

        Guid Guid { get; }

        bool PrintIsAvailable { get; }

        bool PrintPreviewIsAvailable { get; }
    }
}
