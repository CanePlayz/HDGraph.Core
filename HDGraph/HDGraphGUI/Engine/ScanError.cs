using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Resources;

namespace HDGraph.Engine
{
    public class ScanError
    {
        public string FileOrDirPath { get; set; }

        public Exception Exception { get; set; }

        public string Message
        {
            get { return Exception.Message; }
        }

    }
}
