using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace HDGraph
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TraceSwitch mySwitch = new TraceSwitch("traceLevelSwitch", "HDG TraceSwitch");
            try
            {
                Trace.WriteLineIf(mySwitch.TraceInfo, "Application started.");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Trace.TraceError(PrintBug(ex));
                System.Resources.ResourceManager res = new System.Resources.ResourceManager(typeof(Program).Assembly.GetName().Name + ".Resources.ApplicationMessages", typeof(Program).Assembly);
                MessageBox.Show(res.GetString("CriticalError"),
                                res.GetString("Error"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string PrintBug(Exception ex) {
            string errMsg = ex.Message + " - Source: " + ex.Source + " - Stack: " + ex.StackTrace;
            if (ex.InnerException != null)
                return errMsg + " ==> " + PrintBug(ex.InnerException);
            else
                return errMsg;
        }
    }
}