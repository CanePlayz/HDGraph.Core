using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using Microsoft.Win32;

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

        private const string HDG_REG_KEY = "HDGraphiqueur";


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


    }
}
