using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HDGraph.Interfaces.DrawEngines;
using System.Windows.Forms;

namespace HDGraph.PlugIn
{
    public class PlugInsManager
    {
        public static void Test()
        {
            Assembly assembly = Assembly.Load("WpfDrawEngine");
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IDrawEngineContract).IsAssignableFrom(type))
                {
                    IDrawEngineContract engineContract = (IDrawEngineContract)Activator.CreateInstance(type);
                    IControlTypeEngine engine = engineContract.GetNewControlTypeEngine();
                    Control control = engine.GenerateControlFromNode(null);
                    Form form = new Form();
                    form.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                    form.Show();
                    return;
                }
            }
        }

    }
}
