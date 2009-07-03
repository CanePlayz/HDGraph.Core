using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HDGraph.Interfaces.DrawEngines;
using System.Windows.Forms;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.PlugIn
{
    public class PlugInsManager
    {
        public static void Test(IDirectoryNode node, DrawOptions options)
        {
            Assembly assembly = Assembly.Load("WpfDrawEngine");
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IDrawEngineContract).IsAssignableFrom(type))
                {
                    IDrawEngineContract engineContract = (IDrawEngineContract)Activator.CreateInstance(type);
                    IControlTypeEngine engine = engineContract.GetNewControlTypeEngine();
                    Control control = engine.GenerateControlFromNode(node, options);
                    Form form = new Form();
                    form.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                    control.Margin = new Padding(10);
                    control.Padding = new Padding(10);
                    form.Show();
                    return;
                }
            }
        }

    }
}
