using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;

namespace HDGraph
{
    /// <summary>
    /// Classe d'évènement avec message texte
    /// </summary>
    class TextEventArgs : EventArgs
    {
        private string myEventText = null;

        public TextEventArgs(string theEventText)
        {
            myEventText = theEventText;
        }

        public string EventText
        {
            get { return this.myEventText; }
        }
    }

    public class HDGTools
    {
        public static ResourceManager resManager;

        public static TraceSwitch mySwitch;

        private const string HDG_REG_KEY = "HDGraph";


        public static bool AddMeToExplorerContextMenu()
        {
            // Create a RegistryKey, which will access the HKEY_CLASSES_ROOT
            // key in the registry of this machine.
            RegistryKey rk = Registry.ClassesRoot;
            rk = rk.OpenSubKey("Folder");
            rk = rk.OpenSubKey("shell", true);
            if (rk.OpenSubKey(HDG_REG_KEY) != null)
            {
                return false;
            }
            rk = rk.CreateSubKey(HDG_REG_KEY);
            string cmdString = resManager.GetString("ExplorerContextMenuAction");
            rk.SetValue("", cmdString);
            rk = rk.CreateSubKey("command");
            rk.SetValue("", Environment.GetCommandLineArgs()[0] + " \"%1\"");
            return true;
        }

        public static bool RemoveMeFromExplorerContextMenu()
        {
            // Create a RegistryKey, which will access the HKEY_CLASSES_ROOT
            // key in the registry of this machine.
            RegistryKey rk = Registry.ClassesRoot;
            rk = rk.OpenSubKey("Folder");
            rk = rk.OpenSubKey("shell", true);
            if (rk.OpenSubKey(HDG_REG_KEY) == null)
            {
                return false;
            }
            rk.DeleteSubKeyTree(HDG_REG_KEY);
            return true;
        }


        public static string PrintError(Exception ex)
        {
            string errMsg = ex.Message + " - Source: " + ex.Source + " - Stack: " + ex.StackTrace;
            if (ex.InnerException != null)
                return errMsg + " ==> " + PrintError(ex.InnerException);
            else
                return errMsg;
        }

        public static void ApplyCulture(Form form, CultureInfo culture)
        {
            // Create a resource manager for this Form and determine its fields via reflection.
            ComponentResourceManager resources = new ComponentResourceManager(form.GetType());
            FieldInfo[] fieldInfos = form.GetType().GetFields(BindingFlags.Instance |
                BindingFlags.DeclaredOnly | BindingFlags.NonPublic);

            // Call SuspendLayout for Form and all fields derived from Control, so assignment of 
            //   localized text doesn't change layout immediately.
            form.SuspendLayout();
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                if (fieldInfos[index].FieldType.IsSubclassOf(typeof(Control)))
                {
                    fieldInfos[index].FieldType.InvokeMember("SuspendLayout",
                        BindingFlags.InvokeMethod, null,
                        fieldInfos[index].GetValue(form), null);
                }
            }

            // If available, assign localized text to Form and fields with Text property.
            // If available, assign localized Localtion and Size to fields
            System.Drawing.Point point;
            System.Drawing.Size? size = null;
            size = resources.GetObject("$this.ClientSize") as System.Drawing.Size?;
            if (size != null)
                form.ClientSize = size.Value;            
            String text = resources.GetString("$this.Text");
            if (text != null)
                form.Text = text;
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                if (fieldInfos[index].FieldType.GetProperty("Text", typeof(String)) != null)
                {
                    text = resources.GetString(fieldInfos[index].Name + ".Text");
                    if (text != null)
                    {
                        fieldInfos[index].FieldType.InvokeMember("Text",
                            BindingFlags.SetProperty, null,
                            fieldInfos[index].GetValue(form), new object[] { text });
                    }
                }
                if (fieldInfos[index].FieldType.GetProperty("Location", typeof(System.Drawing.Point)) != null)
                {
                    point = (System.Drawing.Point)resources.GetObject(fieldInfos[index].Name + ".Location");
                    if (point != null)
                    {
                        fieldInfos[index].FieldType.InvokeMember("Location",
                            BindingFlags.SetProperty, null,
                            fieldInfos[index].GetValue(form), new object[] { point });
                    }
                }
                if (fieldInfos[index].FieldType.GetProperty("Size", typeof(System.Drawing.Size)) != null)
                {
                    size = resources.GetObject(fieldInfos[index].Name + ".Size") as System.Drawing.Size?;
                    if (size != null)
                    {
                        fieldInfos[index].FieldType.InvokeMember("Size",
                            BindingFlags.SetProperty, null,
                            fieldInfos[index].GetValue(form), new object[] { size.Value });
                    }
                }
            }

            // Call ResumeLayout for Form and all fields derived from Control to resume layout logic.
            // Call PerformLayout, so layout changes due to assignment of localized text are performed.
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                if (fieldInfos[index].FieldType.IsSubclassOf(typeof(Control)))
                {
                    fieldInfos[index].FieldType.InvokeMember("ResumeLayout",
                            BindingFlags.InvokeMethod, null,
                            fieldInfos[index].GetValue(form), new object[] { true });
                }
            }
            form.ResumeLayout(false);
            form.PerformLayout();
        }


    }
}
