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

        private static Guid guid = new Guid("{0959A259-3900-4ffe-AE6F-A9E3A63D2C6B}");

        public Guid Guid
        {
            get { return guid; }
        }

        public bool PrintIsAvailable
        {
            get { return false; }
        }

        public bool PrintPreviewIsAvailable
        {
            get { return false; }
        }

        #endregion
    }
}
