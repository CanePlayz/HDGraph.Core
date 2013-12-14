//#define GENERATE_VERSION_INFO     // comment for standard application use.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace HDGraph
{
    static class Program
    {
        private static bool launchForm = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            HDGTools.mySwitch = new TraceSwitch("traceLevelSwitch", "HDG TraceSwitch");
            Trace.Listeners.Add(new TextWriterTraceListener(GetLogFilename()) { TraceOutputOptions = TraceOptions.DateTime });
            // TODO :
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
#if (!DEBUG)
            try
            {
#endif
            Trace.WriteLineIf(HDGTools.mySwitch.TraceInfo, "Application started.");

#if GENERATE_VERSION_INFO
            VersionInfo vInfo = new VersionInfo()
            {
                ChangeLogUrl = "http://hdgraph.com/index.php?option=com_content&view=category&layout=blog&id=38&Itemid=64",
                DownloadPageUrl = "http://hdgraph.com/index.php?option=com_content&view=article&id=51&Itemid=56",
                VersionNumber = typeof(Program).Assembly.GetName().Version.ToString(),
                ReleaseDate = DateTime.Now,
            };
            string filename = "versionInfo.xml";
            File.WriteAllText(filename, vInfo.SerializeToString(), System.Text.Encoding.Default);
            Process.Start(filename);
            return;
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            ProcessCommandLineArgs(form);
            if (launchForm)
                Application.Run(form);
#if (!DEBUG)    
        }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Trace.TraceError(HDGTools.PrintError(ex));
                System.Resources.ResourceManager res = new System.Resources.ResourceManager(typeof(Program).Assembly.GetName().Name + ".Resources.ApplicationMessages", typeof(Program).Assembly);
                MessageBox.Show(string.Format(res.GetString("CriticalError"), GetLogFilename()),
                                res.GetString("Error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        private const string LogFilename = "HDGraph.log";

        public static string GetLogFilename()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LogFilename);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Console.Error.WriteLine(e.Exception.ToString());
            Trace.TraceError(HDGTools.PrintError(e.Exception));
            System.Resources.ResourceManager res = new System.Resources.ResourceManager(typeof(Program).Assembly.GetName().Name + ".Resources.ApplicationMessages", typeof(Program).Assembly);
            MessageBox.Show(string.Format(res.GetString("CriticalError"), GetLogFilename()),
                            res.GetString("Error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Analyse and process the command lines arguments.
        /// </summary>
        /// <param name="form"></param>
        private static void ProcessCommandLineArgs(MainForm form)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string path = args[1]; // args[0] correspond à l'exécutable, args[1] au premier argument
                Trace.WriteLineIf(HDGTools.mySwitch.TraceInfo, "Path argument received: " + path);

                if (path == "/" + HDGTools.RestartAction.addToExplorerContextMenu.ToString())
                {
                    HDGTools.AddMeToExplorerContextMenu(true);
                    launchForm = false;
                }
                else if (path == "/" + HDGTools.RestartAction.removeFromExplorerContextMenu.ToString())
                {
                    HDGTools.RemoveMeFromExplorerContextMenu(true);
                    launchForm = false;
                }
                else if (path.StartsWith("/") || path.StartsWith("::"))
                {
                    // path startsWith "::" is a Window internal shortcut ; exemple : "::{GUID_here}" for MyComputer.
                    path = "";
                }
                else
                {   // chemin pour lequel lancer le scan au démarrage
                    path = RemoveDoubleQuoteIfNecessary(path);
                    if (File.Exists(path) && Path.GetExtension(path) == ".hdg")
                    {
                        // le 1er argument est un fichier HDG à charger
                        form.LoadGraphFromFile(path);
                    }
                    else
                    {   // le 1er argument est un répertoire: il faut lancer le scan.
                        path = (new DirectoryInfo(path)).FullName;
                        form.comboBoxPath.Text = path;
                        form.SavePathHistory();
                        form.LaunchScanOnStartup = true;
                    }
                }
            }
            for (int i = 2; i < args.Length; i++)
            {
                string commandLineOption = args[i];
                ProcessArg(form, commandLineOption);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string MakeFileNameUnique(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            string rootFileName = fileInfo.FullName.Remove(fileInfo.FullName.Length - fileInfo.Extension.Length);
            string newPath = path;
            int i = 2;
            while (File.Exists(newPath))
            {
                newPath = rootFileName + "_" + i + fileInfo.Extension;
                i++;
            }
            return newPath;
        }

        private static void ProcessArg(MainForm form, string arg)
        {
            if (arg.StartsWith(OUTPUT_IMG_CMD_LINE_OPTION_PREFIX))
            {
                arg = arg.Substring(OUTPUT_IMG_CMD_LINE_OPTION_PREFIX.Length);
                string outputImgFilePath = RemoveDoubleQuoteIfNecessary(arg);

                if (!outputImgFilePath.ToLower().EndsWith(".png"))
                    outputImgFilePath += ".png";
                outputImgFilePath = MakeFileNameUnique(outputImgFilePath);
                form.OutputImgFilePath = outputImgFilePath;

            }
            if (arg.StartsWith(OUTPUT_GRAPH_CMD_LINE_OPTION_PREFIX))
            {
                arg = arg.Substring(OUTPUT_GRAPH_CMD_LINE_OPTION_PREFIX.Length);
                form.OutputGraphFilePath = RemoveDoubleQuoteIfNecessary(arg);
                if (!form.OutputGraphFilePath.ToLower().EndsWith(".hdg"))
                    form.OutputGraphFilePath += ".hdg";
                form.OutputGraphFilePath = MakeFileNameUnique(form.OutputGraphFilePath);
            }
            if (arg.StartsWith(OUTPUT_IMG_SIZE_CMD_LINE_OPTION_PREFIX))
            {
                arg = arg.Substring(OUTPUT_IMG_SIZE_CMD_LINE_OPTION_PREFIX.Length);
                try
                {
                    string[] size = arg.Split('x');
                    form.OutputImgSize = new System.Drawing.Size(Int32.Parse(size[0]), Int32.Parse(size[1]));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Wrong syntax for argument \"imgOutputSize\". Check the documentation.", ex);
                }
            }
        }



        private const string OUTPUT_IMG_CMD_LINE_OPTION_PREFIX = "/imgOutput:";
        private const string OUTPUT_GRAPH_CMD_LINE_OPTION_PREFIX = "/graphOutput:";
        private const string OUTPUT_IMG_SIZE_CMD_LINE_OPTION_PREFIX = "/imgOutputSize:";

        private static string RemoveDoubleQuoteIfNecessary(string path)
        {
            if (path.EndsWith("\""))
                path = path.Substring(0, path.Length - 1);
            if (path.StartsWith("\""))
                path = path.Substring(1);
            return path;
        }

    }
}