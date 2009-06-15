using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.WpfDrawEngine
{
    public class WpfDrawEngineContract : IDrawEngineContract
    {

        #region IDrawEngineContract Members

        public DrawEngineType EngineType
        {
            get { return DrawEngineType.ControlDrawEngine; }
        }

        public string Name
        {
            get { return "Evolved WPF Draw Engine"; }
        }

        public string Description
        {
            get { return "Evolved draw engine. Renders vectorial content. Requires .NET Framework version 3.0 or higher."; }
        }

        public IControlTypeEngine GetNewControlTypeEngine()
        {
            return new DrawEngine();
        }

        public IBitmapTypeEngine GetNewBitmapTypeEngine()
        {
            return null;
        }

        #endregion
    }
}
