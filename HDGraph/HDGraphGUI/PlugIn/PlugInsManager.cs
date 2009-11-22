using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HDGraph.Interfaces.DrawEngines;
using System.Windows.Forms;
using HDGraph.Interfaces.ScanEngines;
using System.IO;
using System.Diagnostics;
using HDGraph.DrawEngine;

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
            List<IDrawEngineContract> plugInsList = new List<IDrawEngineContract>();
            plugInsList.Add(new SimpleDrawEngineContract());
            if (!Directory.Exists(PlugInRelativePath))
                return plugInsList;
            foreach (string fileName in Directory.GetFiles(PlugInRelativePath, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    foreach (Type type in assembly.GetTypes())
                    {
                        try
                        {
                            if (type.IsPublic && typeof(IDrawEngineContract).IsAssignableFrom(type)
                                && type != typeof(IDrawEngineContract))
                            {
                                IDrawEngineContract engineContract = (IDrawEngineContract)Activator.CreateInstance(type);
                                plugInsList.Add(engineContract);
                                MessageBox.Show("Class " + type.ToString() + " from Plugin (file " + fileName + ") is sucessfully loaded !", "Plugin loaded", MessageBoxButtons.OK, MessageBoxIcon.Information); // TODO : localize ?
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(HDGTools.PrintError(ex));
                            MessageBox.Show("Error loading class " + type.ToString() + " from Plugin (file " + fileName + ") : " + ex); // TODO : localize ?
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(HDGTools.PrintError(ex));
                    MessageBox.Show("Error loading Plugin from file " + fileName + " : " + ex, "Plugin failed to load.", MessageBoxButtons.OK, MessageBoxIcon.Warning); // TODO : localize ?
                }
            }
            return plugInsList;
        }


        public static void TestFirstPlugin(IDirectoryNode node, DrawOptions options, IActionExecutor actionExecutor)
        {
            List<IDrawEngineContract> plugins = GetDrawEnginePlugins();
            if (plugins == null
                || plugins.Count == 0)
                return;

            IDrawEngineContract engineContract = plugins[0];
            IDrawEngine engine = engineContract.GetNewEngine();
            Control control = engine.GenerateControlFromNode(node, options, actionExecutor);
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
