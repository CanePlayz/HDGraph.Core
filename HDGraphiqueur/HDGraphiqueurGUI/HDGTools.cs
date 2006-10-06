using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using Microsoft.Win32;

namespace HDGraphiqueurGUI
{
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
            rk.SetValue("", "Graphiquer l'espace avec HDG"); // TODO
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


    }
}
