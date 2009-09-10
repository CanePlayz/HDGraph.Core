﻿using System;
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
            get { return "Evolved draw engine. Renders vectorial content. Requires .NET Framework version 3.5 or higher."; }
        }

        public IDrawEngine GetNewEngine()
        {
            return new DrawEngine();
        }

        #endregion
    }
}
