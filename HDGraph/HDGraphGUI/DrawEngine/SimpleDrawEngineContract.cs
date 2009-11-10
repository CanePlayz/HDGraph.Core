using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.DrawEngines;
using HDGraph.Resources;

namespace HDGraph.DrawEngine
{
    public class SimpleDrawEngineContract : IDrawEngineContract
    {
        #region IDrawEngineContract Members

        public string Name
        {
            get { return ApplicationMessages.SimpleDrawEngineName; }
        }

        public string Description
        {
            get { return ApplicationMessages.SimpleDrawEngineDescription; }
        }

        public IDrawEngine GetNewEngine()
        {
            return new SimpleDrawEngine();
        }

        #endregion
    }
}
