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
                Trace.TraceError(HDGTools.PrintError(ex));
                System.Resources.ResourceManager res = new System.Resources.ResourceManager(typeof(Program).Assembly.GetName().Name + ".Resources.ApplicationMessages", typeof(Program).Assembly);
                MessageBox.Show(res.GetString("CriticalError"),
                                res.GetString("Error"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}