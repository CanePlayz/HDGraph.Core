using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Resources;

namespace HDGraph.ScanEngine
{
    public class ScanError
    {
        public string FileOrDirPath { get; set; }

        public Exception Exception { get; set; }

        public string ManualMessage { get; set; }

        public string Message
        {
            get
            {
                if (Exception != null)
                {
                    string msg = Exception.Message;
                    if (Exception.InnerException != null)
                    {
                        msg += msg.Trim().EndsWith(".") ? " " : " : ";
                        msg += Exception.InnerException.Message;
                    }
                    return msg;
                }
                return ManualMessage;
            }
        }

    }
}
