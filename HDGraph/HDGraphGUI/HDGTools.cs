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
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace HDGraph
{
    public class IncompatibleVersionException : Exception
    {

    }


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

        #region Variables chaîne (utilisées en tant que cache du resourceManager)

        private static string abrevOctet;

        public static string AbrevOctet
        {
            get
            {
                if (abrevOctet == null)
                    abrevOctet = HDGTools.resManager.GetString("abreviationOctet");
                return abrevOctet;
            }
        }
        private static string abrevKo;

        public static string AbrevKo
        {
            get
            {
                if (abrevKo == null)
                    abrevKo = HDGTools.resManager.GetString("abreviationKOctet");
                return abrevKo;
            }
        }
        private static string abrevMo;

        public static string AbrevMo
        {
            get
            {
                if (abrevMo == null)
                    abrevMo = HDGTools.resManager.GetString("abreviationMOctet");
                return abrevMo;
            }
        }
        private static string abrevGo;

        public static string AbrevGo
        {
            get
            {
                if (abrevGo == null)
                    abrevGo = HDGTools.resManager.GetString("abreviationGOctet");
                return abrevGo;
            }
        }
        private static string abrevTo;

        public static string AbrevTo
        {
            get
            {
                if (abrevTo == null)
                    abrevTo = HDGTools.resManager.GetString("abreviationTOctet");
                return abrevTo;
            }
        }

        #endregion

        /// <summary>
        /// Format une taille en octets en chaine de caractères.
        /// </summary>
        /// <param name="sizeInOctet"></param>
        /// <returns></returns>
        public static string FormatSize(long sizeInOctet)
        {
            long unit = 1;
            if (sizeInOctet < unit * 1000)
                return sizeInOctet.ToString() + " " + AbrevOctet;
            unit *= 1024;
            if (sizeInOctet < unit * 1000)
                return String.Format("{0:F} " + AbrevKo, sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1000)
                return String.Format("{0:F} " + AbrevMo, sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1000)
                return String.Format("{0:F} " + AbrevGo, sizeInOctet / (double)unit);
            unit *= 1024;
            return String.Format("{0:F} " + AbrevTo, sizeInOctet / (double)unit);
        }


        public enum RestartAction
        {
            addToExplorerContextMenu,
            removeFromExplorerContextMenu
        }

        /// <summary>
        /// Restart the current process with administrator credentials
        /// </summary>
        public static void StartActionInAdminMode(RestartAction action)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Application.ExecutablePath;
            startInfo.Verb = "runas";
            startInfo.Arguments = "/" + action.ToString();
            try
            {
                Process p = new Process();
                p.StartInfo = startInfo;

                p.Start();
                //System.Threading.Thread.Sleep(500);

                //Process p2 = Process.GetProcessesByName("consent.exe")[0];
                //SetForegroundWindow(p2.MainWindowHandle);

                // Attente de la fin de la commande
                //p.WaitForExit();
                // Libération des ressources
                //p.Close();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Trace.TraceError(HDGTools.PrintError(ex));
                return; //If cancelled, do nothing
            }
        }

        public static bool IsInAdminMode()
        {
            if (Interop.ToolProviderBase.GetEnvironmentType() == HDGraph.Interop.EnvironmentTarget.WindowsVista)
            {
                // Vista
                return HDGraph.Interop.Windows.VistaTools.IsElevated();
            }
            else
            {
                // Windows XP ou inférieur
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(id);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void AddMeToExplorerContextMenu(bool skipAdminModeCheck)
        {
            if (!skipAdminModeCheck && !IsInAdminMode())
            {
                StartActionInAdminMode(RestartAction.addToExplorerContextMenu);
            }
            else
            {
                try
                {
                    if (AddMeToExplorerContextMenuViaRegistry())
                        MessageBox.Show(resManager.GetString("HdgCorrectlyIntegratedInExplorer"),
                                    resManager.GetString("OperationSuccessfullTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    else
                        MessageBox.Show(resManager.GetString("HdgAlreadyIntegratedInExplorer"),
                                    resManager.GetString("OperationFailedTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(resManager.GetString("UnableToIntegrateInExplorer"), ex.Message),
                                    resManager.GetString("OperationFailedTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    Trace.TraceError(HDGTools.PrintError(ex));
                }
            }
        }

        public static void RemoveMeFromExplorerContextMenu(bool skipAdminModeCheck)
        {
            if (!skipAdminModeCheck && !IsInAdminMode())
            {
                StartActionInAdminMode(RestartAction.removeFromExplorerContextMenu);
            }
            else
            {
                try
                {
                    if (RemoveMeFromExplorerContextMenuViaRegistry())
                        MessageBox.Show(resManager.GetString("HdgCorrectlyDesIntegratedInExplorer"),
                                    resManager.GetString("OperationSuccessfullTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    else
                        MessageBox.Show(resManager.GetString("HdgAlreadyDesIntegratedInExplorer"),
                                    resManager.GetString("OperationFailedTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(resManager.GetString("UnableToDesIntegrateInExplorer"), ex.Message),
                                    resManager.GetString("OperationFailedTitle"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    Trace.TraceError(HDGTools.PrintError(ex));
                }
            }
        }

        private static bool AddMeToExplorerContextMenuViaRegistry()
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

        private static bool RemoveMeFromExplorerContextMenuViaRegistry()
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
            //string errMsg = ex.Message + " - Source: " + ex.Source + " - Stack: " + ex.StackTrace;
            //if (ex.InnerException != null)
            //    return errMsg + " ==> " + PrintError(ex.InnerException);
            //else
            //    return errMsg;
            return ex.ToString();
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
                    object location = resources.GetObject(fieldInfos[index].Name + ".Location");
                    if (location != null)
                    {
                        point = (System.Drawing.Point)location;
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
