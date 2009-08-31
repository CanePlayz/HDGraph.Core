using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HDGraph.Interfaces.DrawEngines;
using System.Windows.Forms;
using HDGraph.Interfaces.ScanEngines;
using System.IO;
using System.Diagnostics;

namespace HDGraph.PlugIn
{
    public class PlugInsManager
    {
        private const string PlugInRelativePath = "Plugins";

        /// <summary>
        /// List all available Draw Engine Plugins.
        /// </summary>
        /// <returns></returns>
        public static List<IDrawEngineContract> GetDrawEnginePlugins()
        {
            if (!Directory.Exists(PlugInRelativePath))
                return new List<IDrawEngineContract>();

            List<IDrawEngineContract> plugInsList = new List<IDrawEngineContract>();
            foreach (string fileName in Directory.GetFiles(PlugInRelativePath, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(new FileInfo(fileName).FullName);
                    foreach (Type type in assembly.GetTypes())
                    {
                        try
                        {
                            if (type.IsPublic && typeof(IDrawEngineContract).IsAssignableFrom(type))
                            {
                                IDrawEngineContract engineContract = (IDrawEngineContract)Activator.CreateInstance(type);
                                plugInsList.Add(engineContract);
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(HDGTools.PrintError(ex));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(HDGTools.PrintError(ex));
                }
            }
            return plugInsList;
        }


        public static void Test(IDirectoryNode node, DrawOptions options)
        {
            List<IDrawEngineContract> plugins = GetDrawEnginePlugins();
            if (plugins == null
                || plugins.Count == 0)
                return;

            IDrawEngineContract engineContract = plugins[0];
            IControlTypeEngine engine = engineContract.GetNewControlTypeEngine();
            Control control = engine.GenerateControlFromNode(node, options);
            Form form = new Form();
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(10);
            control.Padding = new Padding(10);
            form.Show();
            return;
        }

    }
}
