using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.WpfDrawEngine
{
    public class WpfDrawEngineContract : IDrawEngineContract
    {

        #region IDrawEngineContract Members


        public string Name
        {
            get { return "WPF Draw Engine"; }
        }

        public string Description
        {
            get { return "Evolved draw engine, but requires .NET Framework version 3.5 or higher. Renders vectorial content with full support for graph manipulations and printing."; }
        }

        public IDrawEngine GetNewEngine()
        {
            return new DrawEngine();
        }

        private static Guid guid = new Guid("{9053B2B2-E6E3-4ee7-BDAC-FCCA15C9E3BE}");

        public Guid Guid
        {
            get { return guid; }
        }

        #endregion
    }
}
