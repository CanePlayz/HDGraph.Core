using System;
using System.Collections.Generic;
using System.Text;

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
