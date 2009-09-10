using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.DrawEngine
{
    public class SimpleDrawEngineContract : IDrawEngineContract
    {
        #region IDrawEngineContract Members

        public string Name
        {
            get { return "Standard"; }
        }

        public string Description
        {
            get { return "The first HDGraph draw engine. Based on Bitmap generation. Slower dans less powerfull than WPF engine (if you have a good graphic card)."; }
        }

        public IDrawEngine GetNewEngine()
        {
            return new SimpleDrawEngine();
        }

        #endregion
    }
}
