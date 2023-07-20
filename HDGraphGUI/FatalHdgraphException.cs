using System;

namespace HDGraph
{
    public class FatalHdgraphException : Exception
    {
        public FatalHdgraphException() : base()
        {
        }

        public FatalHdgraphException(string msg) : base(msg)
        {
        }

        public FatalHdgraphException(string msg, Exception innerEx) : base(msg, innerEx)
        {
        }
    }
}
