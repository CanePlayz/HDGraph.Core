using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Diagnostics;
using HDGraph.DrawEngine;
using HDGraph.ScanEngine;
using HDGraph.Interop;
using HDGraph.Resources;
using HDGraph.Interfaces.ScanEngines;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph
{
    public partial class MainForm : Form
    {
        #region Variables et propriétés

        /// <summary>
        /// Nombre de fenêtres ouvertes, dans le cas d'une utilisation en MDI.
        /// </summary>
        private int childFormNumber = 0;

        /// <summary>
        /// Ressource manager, pour la localisation.
        /// </summary>
        private System.Resources.ResourceManager resManager = null;

        /// <summary>
        /// Indique si le scan doit automatiquement débuter au chargement de la fenêtre.
        /// </summary>
        private bool launchScanOnStartup = false;

        public bool LaunchScanOnStartup
        {
            get { return launchScanOnStartup; }
            set { launchScanOnStartup = value; }
        }

        /// <summary>
        /// Moteur de scan.
        /// </summary>
        private HDGraphScanEngineBase scanEngine;

        /// <summary>
        /// Liste des nodes parcours, pour les boutons "back" et "next".
        /// </summary>
        private List<IDirectoryNode> graphViewHistory = new List<IDirectoryNode>();

        /// <summary>
        /// Index du node (de la liste graphViewHistory) qui actuellement affiché
        /// </summary>
        private int currentNodeIndex = 0;

        /// <summary>
        /// Fichier dans lequel stocker l'image du graph lorsque le scan est fini (param de ligne de commande).
        /// Est null si ce chemin ne figure pas dans la ligne de commande.
        /// </summary>
        private string outputImgFilePath = null;

        public string OutputImgFilePath
        {
            get { return outputImgFilePath; }
            set { outputImgFilePath = value; }
        }
        /// <summary>
        /// Fichier dans lequel stocker le graph lorsque le scan est fini (param de ligne de commande).
        /// Est null si ce chemin ne figure pas dans la ligne de commande.
        /// </summary>
        private string outputGraphFilePath;

        public string OutputGraphFilePath
        {
            get { return outputGraphFilePath; }
            set { outputGraphFilePath = value; }
        }

        public DrawType DrawType { get; set; }

        #endregion

        #region Constructeur(s) et initialisation

        public MainForm()
        {
            bool changeLangIsSuccess = LoadLanguage();

            // LeResourceManager prend en paramètre : nom_du_namespace.nom_de_la_ressource_principale
            resManager = new System.Resources.ResourceManager(this.GetType().Assembly.GetName().Name + ".Resources.ApplicationMessages", this.GetType().Assembly);
            HDGTools.resManager = resManager;
            CreateScanEngine();
            scanEngine.ShowDiskFreeSpace = Properties.Settings.Default.OptionShowFreeSpace;
            if (!changeLangIsSuccess)
                MessageBox.Show(resManager.GetString("ErrorInConfigLanguage"),
                                resManager.GetString("ErrorInConfigLanguageTitle"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

            InitializeComponent();

            ApplyIcon();
            this.Text = AboutBox.AssemblyTitle;
            this.WindowState = HDGraph.Properties.Settings.Default.OptionMainWindowOpenState;
            this.ClientSize = HDGraph.Properties.Settings.Default.OptionMainWindowSize;

            if (Properties.Settings.Default.MyDrawOptions == null)
                Properties.Settings.Default.MyDrawOptions = new DrawOptions();
            treeGraph1.DrawOptions = Properties.Settings.Default.MyDrawOptions;
            drawOptionsBindingSource.DataSource = treeGraph1.DrawOptions;
            explorerIntegrationToolStripMenuItem.Enabled = ToolProviderBase.CurrentOsIsWindows();
            EnableHelpIfAvailable();
            comboBoxPath.DataSource = HDGraph.Properties.Settings.Default.PathHistory;
            checkBoxAutoRecalc.Checked = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
            SetTrackBarZoomValueFromNumUpDown();
            try
            {
                scanEngine.AutoRefreshAllowed = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
                ModeAffichageCouleurs modeCouleurs = (ModeAffichageCouleurs)Enum.Parse(typeof(ModeAffichageCouleurs), HDGraph.Properties.Settings.Default.OptionColorStyle);
                comboBoxColorStyle.SelectedIndex = (int)modeCouleurs;
            }
            catch (Exception ex)
            {
                Trace.TraceError(HDGTools.PrintError(ex));
            }
            PopulateAnalyseShortcuts();
            splitContainerGraphAndOptions.Panel2Collapsed = true;
        }

        private void CreateScanEngine()
        {
            if (Properties.Settings.Default.OptionUseSimpleScanEngine
                || !ToolProviderBase.CurrentOsIsWindows())
                scanEngine = new SimpleFileSystemScanEngine();
            else
                scanEngine = new NativeFileSystemScanEngine();
        }


        private void ApplyIcon()
        {
            if (ToolProviderBase.CurrentOsIsWindows())
                // errors on other os !
                this.Icon = Properties.Resources.HDGraph_ico;
        }

        private void SetTrackBarZoomValueFromNumUpDown()
        {
            int trackBarValue = Math.Min((int)numUpDownNbNivxAffich.Value, trackBarZoom.Maximum);
            trackBarValue = Math.Max(trackBarZoom.Minimum, trackBarValue);
            trackBarZoom.Value = trackBarValue;
        }

        private void PopulateAnalyseShortcuts()
        {
            toolStripSplitButtonModel.Visible = false;

            List<PathWithIcon> pathList = ToolProviderBase.Current.ListFavoritPath();

            foreach (PathWithIcon pathWithIco in pathList)
            {
                ToolStripButton button = new ToolStripButton(pathWithIco.Path);
                button.Text = pathWithIco.Name;
                if (pathWithIco.Icon != null)
                    button.Image = pathWithIco.Icon.ToBitmap();
                button.Tag = pathWithIco.Path;
                button.Click += new EventHandler(shortcutButton_Click);
                toolStripShortcuts.Items.Add(button);
            }
        }

        void shortcutButton_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn != null)
            {
                comboBoxPath.Text = btn.Tag as string;
                LaunchScan();
            }
        }

        private void ShowError(string msg, Exception ex)
        {
            Trace.TraceError(HDGTools.PrintError(ex));
            ShowError(msg);
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, resManager.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Check si un fichier d'aide est présent et si oui, active les boutons d'aide de l'IHM.
        /// </summary>
        private void EnableHelpIfAvailable()
        {
            string helpFile = GetHelpFile();
            bool activeHelp = (helpFile != null && helpFile.Length > 0 && System.IO.File.Exists(helpFile));
            if (activeHelp)
            {
                helpToolStripButton.Visible = true;
            }
            contentsToolStripMenuItem.Enabled = activeHelp;
            contentsToolStripMenuItem.Visible = activeHelp;
            indexToolStripMenuItem.Enabled = activeHelp;
            indexToolStripMenuItem.Visible = activeHelp;
            searchToolStripMenuItem.Enabled = activeHelp;
            searchToolStripMenuItem.Visible = activeHelp;
            toolStripSeparator8.Visible = activeHelp;
            helpToolStripButton.Enabled = activeHelp;
            linkLabelHelpGraph.Enabled = activeHelp;

        }

        /// <summary>
        /// Charge la langue souhaitée depuis le fichier de config (dans le cas où l'utilisateur souhaite overrider le choix du framework).
        /// </summary>
        /// <returns></returns>
        private bool LoadLanguage()
        {

            try
            {
                System.Globalization.CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                string lang = HDGraph.Properties.Settings.Default.Language;
                if (lang != null && lang.Length > 0)
                {
                    culture = new System.Globalization.CultureInfo(lang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    Application.CurrentCulture = culture;
                }
                WaitForm.ThreadCulture = culture;
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(HDGTools.PrintError(ex));
                return false;
            }
        }

        /// <summary>
        /// Evènement Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //toolStripContainer1.Location = new Point(0, menuStrip.Size.Height);
            PrintStatus(resManager.GetString("statusReady"));
        }

        #endregion

        #region Méthodes liées aux menus

        #region Sauvegardes et chargements de graphs

        /// <summary>
        /// Lance la boite de dialogue d'ouverture de fichier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            openFileDialog.Filter = resManager.GetString("HDGFiles") +
                                    " (*.hdg)|*.hdg|" +
                                    resManager.GetString("AllFiles") +
                                    "(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                LoadGraphFromFile(fileName);
            }
        }

        /// <summary>
        /// Charge un graph sauvegardé.
        /// </summary>
        /// <param name="fileName"></param>
        internal void LoadGraphFromFile(string fileName)
        {
            checkBoxAutoRecalc.Checked = false; // Autorecalc désactivé sur un graph enregistré !

            try
            {
                XmlReader reader = new XmlTextReader(fileName);
                XmlSerializer serializer = new XmlSerializer(scanEngine.GetType());
                scanEngine = (HDGraphScanEngineBase)serializer.Deserialize(reader);
                reader.Close();
                scanEngine.NotifyForNewInfo = new HDGraphScanEngineBase.PrintInfoDelegate(PrintStatus);
                treeGraph1.ScanEngine = scanEngine;
                treeGraph1.UpdateHoverNode = new TreeGraph.NodeNotificationDelegate(PrintNodeHoverCursor);
                treeGraph1.NotifyNewRootNode = new TreeGraph.NodeNotificationDelegate(UpdateCurrentNodeRoot);

                if (scanEngine.Root != null)
                {
                    comboBoxPath.Text = scanEngine.Root.Path;
                    numUpDownNbNivx.Value = scanEngine.Root.DepthMaxLevel;
                    numUpDownNbNivxAffich.Value = scanEngine.Root.DepthMaxLevel;
                    treeGraph1.DrawOptions.ShownLevelsCount = scanEngine.Root.DepthMaxLevel;
                }
                treeGraph1.ForceRefresh();
                UpdateNodeHistory(scanEngine.Root);
                PrintStatus(String.Format(resManager.GetString("GraphLoadedFromDate"), scanEngine.AnalyzeDate.ToString()));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is IncompatibleVersionException)
                    ShowError(Resources.ApplicationMessages.IncompatibleVersionError, ex.InnerException);
                else
                    ShowError(String.Format(resManager.GetString("ErrorLoadingFile"), fileName) + ex.Message, ex);
            }
        }


        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchSaveAsDialog();
        }

        /// <summary>
        /// Lance la boite de dialogue "Enregistrer sous".
        /// </summary>
        private void LaunchSaveAsDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = resManager.GetString("HDGFiles") +
                                    " (*.hdg)|*.hdg|" +
                                    resManager.GetString("AllFiles") +
                                    "(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                SaveGraphToFile(fileName);
            }
        }

        /// <summary>
        /// Sauvegarde le graph actuel dans un fichier HDG.
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveGraphToFile(string fileName)
        {
            XmlWriter writer = new XmlTextWriter(fileName, Encoding.Default);
            XmlSerializer serializer = new XmlSerializer(scanEngine.GetType());
            serializer.Serialize(writer, scanEngine);
            writer.Close();
        }

        private void exportAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Portable Network Graphics (PNG)" +
                                    " (*.png)|*.png|" +
                                    resManager.GetString("AllFiles") +
                                    "(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                treeGraph1.ImageBuffer.Save(fileName);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            LaunchSaveAsDialog();
        }

        #endregion


        /// <summary>
        /// Bouton "quitter".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Boutons couper/copier/coller

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        }

        #endregion

        #region Gestion d'Affichage des la statusBar et la toolBar

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolBarToolStrip.Visible = toolBarToolStripMenuItem.Checked;
            UpdateToolZoneVisibility();
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
            splitContainerGraphAndStatusBar.Panel2Collapsed = !statusBarToolStripMenuItem.Checked;
        }

        #endregion

        #region Gestion des fenêtres en MDI

        /// <summary>
        /// Affiche une nouvelle fenêtre (appli MDI seulement).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowNewForm(object sender, EventArgs e)
        {
            // Create a new instance of the child form.
            Form childForm = new Form();
            // Make it a child of this MDI form before showing it.
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        #endregion

        #region Méthodes diverses

        /// <summary>
        /// Affichage fenêtre "A propos".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }


        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /// <summary>
        /// Affichage fenêtre options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new OptionsForm()).ShowDialog();
        }

        /// <summary>
        /// Affichage fenêtre de sélection de langue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void languageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (!LanguageForm.IsLoaded)
            {
                // Premier chargement long
                WaitForm.ShowWaitForm(this, Resources.ApplicationMessages.loadingLanguageList);
            }
            DialogResult res = (new LanguageForm(resManager)).ShowDialog();
        }

        #endregion

        #region Affichages fichier d'aide


        /// <summary>
        /// Renvoi le chemin du fichier d'aide.
        /// </summary>
        /// <returns>Null ou chaine vide si aucun fichier d'aide.</returns>
        private string GetHelpFile()
        {
            return Application.StartupPath
                + Path.DirectorySeparatorChar
                + "Doc"
                + Path.DirectorySeparatorChar
                + Resources.ApplicationMessages.HelpFilename;
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, GetHelpFile(), HelpNavigator.TableOfContents);
        }

        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelpIndex(this, GetHelpFile());
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, GetHelpFile(), HelpNavigator.Find, "");
        }

        #endregion

        /// <summary>
        /// Clic btn "browse"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.Description = resManager.GetString("PleaseChooseDirectory");
            dialog.SelectedPath = comboBoxPath.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                comboBoxPath.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Clic débuter scan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScan_Click(object sender, EventArgs e)
        {
            LaunchScan();
        }

        #region Gestion de l'historique chemins entrés manuellement

        public void SavePathHistory()
        {
            if (HDGraph.Properties.Settings.Default.PathHistory == null)
                HDGraph.Properties.Settings.Default.PathHistory = new StringCollection();
            StringCollection history = HDGraph.Properties.Settings.Default.PathHistory;
            if (history.Count == 0 || comboBoxPath.Text != history[0])
            {
                history.Remove(comboBoxPath.Text);
                history.Insert(0, comboBoxPath.Text);
            }
            HDGraph.Properties.Settings.Default.Save();
            comboBoxPath.DataSource = null;
            comboBoxPath.DataSource = history;
            comboBoxPath.SelectedIndex = 0;
        }


        private void clearHistroryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentPath = comboBoxPath.Text;
            if (HDGraph.Properties.Settings.Default.PathHistory == null)
                HDGraph.Properties.Settings.Default.PathHistory = new StringCollection();
            HDGraph.Properties.Settings.Default.PathHistory.Clear();
            HDGraph.Properties.Settings.Default.Save();
            MessageBox.Show(resManager.GetString("HistorySuccessfullyCleared"),
                            resManager.GetString("OperationSuccessfullTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
            comboBoxPath.DataSource = null;
            comboBoxPath.DataSource = HDGraph.Properties.Settings.Default.PathHistory;
            comboBoxPath.Text = currentPath;
        }

        #endregion

        private void numUpDownNbNivxAffich_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int nbNiveaux = (int)numUpDownNbNivxAffich.Value;
                if (nbNiveaux >= trackBarZoom.Minimum
                    && nbNiveaux <= trackBarZoom.Maximum
                    && !currentlyScrollingZoom)
                    trackBarZoom.Value = nbNiveaux;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    HDGTools.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Trace.TraceError(HDGTools.PrintError(ex));
            }
        }

        #region Intégration à l'explorateur

        private void addMeToTheExplorerConToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDGTools.AddMeToExplorerContextMenu(false);
        }

        private void removeMeFromTheExplorerContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDGTools.RemoveMeFromExplorerContextMenu(false);
        }

        #endregion

        public void PrintStatus(string message)
        {
            PrintStatus(message, true);
        }

        public void PrintStatus(string message, bool doEvents)
        {
            if (message != null && message.Length > 0)
                toolStripStatusLabel.Text = DateTime.Now.ToString() + " : " + message;
            if (doEvents)
                Application.DoEvents();
        }


        /// <summary>
        /// Lance le scan et le graphiquage.
        /// </summary>
        private void LaunchScan()
        {
            buttonScan.Enabled = false;
            checkBoxAutoRecalc.Checked = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
            SavePathHistory();

            int nbNiveaux = (int)numUpDownNbNivx.Value;
            WaitForm form = new WaitForm();

            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            form.ShowDialogAndStartScan(scanEngine, comboBoxPath.Text, nbNiveaux);

            //watch.Stop();
            //MessageBox.Show(watch.Elapsed.ToString());

            // // moteur.ConstruireArborescence(comboBoxPath.Text, nbNiveaux); // OBSOLETE
            // // moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(WaitForm.ShowWaitForm); // OBSOLETE

            scanEngine.NotifyForNewInfo = new HDGraphScanEngineBase.PrintInfoDelegate(PrintStatus);
            treeGraph1.ScanEngine = scanEngine;
            numUpDownNbNivxAffich.Value = nbNiveaux;
            treeGraph1.DrawOptions.ShownLevelsCount = nbNiveaux;
            treeGraph1.ForceRefresh();
            UpdateNodeHistory(scanEngine.Root);
            //PrintStatus("Terminé !");
            treeGraph1.UpdateHoverNode = new TreeGraph.NodeNotificationDelegate(PrintNodeHoverCursor);
            treeGraph1.NotifyNewRootNode = new TreeGraph.NodeNotificationDelegate(UpdateCurrentNodeRoot);
            errorStatus1.Update(scanEngine.ErrorList);
            buttonScan.Enabled = true;
            TimeSpan executionTime = scanEngine.AnalyzeDate.Subtract(scanEngine.AnalyseStartDate);
            PrintStatus(String.Format(ApplicationMessages.ScanCompletedIn, executionTime));
        }

        /// <summary>
        /// Affiche les informations du répertoire "node" dans la barre de status.
        /// </summary>
        /// <param name="node"></param>
        private void PrintNodeHoverCursor(IDirectoryNode node)
        {
            if (node == null)
            {
                PrintStatus(resManager.GetString("CursorHoverNoDirectory"));
                groupBoxHoverInfo.Visible = false;
            }
            else
            {
                if (node.DirectoryType == SpecialDirTypes.UnknownPart)
                {
                    PrintStatus(resManager.GetString("CursorHoverUnknownPart"));
                }
                else if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
                {
                    PrintStatus(resManager.GetString("CursorHoverFreeSpace"));
                }
                else
                {
                    PrintStatus(String.Format(resManager.GetString("CursorHoverDirectory"), node.Path));
                }
                //MessageBox.Show("Cursor hover directory " + node.Path);
                labelDirName.Text = node.Name;
                labelDirTotalSize.Text = HDGTools.FormatSize(node.TotalSize);
                if (node.TotalSize > 0)
                    labelFilesSize.Text = HDGTools.FormatSize(node.FilesSize) + " (" + node.FilesSize * 100 / node.TotalSize + "%)";
                else
                    labelFilesSize.Text = " - ";
                groupBoxHoverInfo.Visible = true;
            }
        }

        /// <summary>
        /// Affiche les informations du répertoire "node" dans la barre de status.
        /// </summary>
        /// <param name="node"></param>
        private void UpdateCurrentNodeRoot(IDirectoryNode node)
        {
            if (node != null)
            {
                comboBoxPath.Text = node.Path;
                UpdateNodeHistory(node);
            }
        }

        /// <summary>
        /// Ajoute le node à l'historique et met à jour l'état des boutons de navigation.
        /// </summary>
        /// <param name="node"></param>
        private void UpdateNodeHistory(IDirectoryNode node)
        {

            if (currentNodeIndex < graphViewHistory.Count - 1)
            {
                graphViewHistory.RemoveRange(currentNodeIndex + 1, graphViewHistory.Count - 1 - currentNodeIndex);
            }

            this.graphViewHistory.Add(node);
            currentNodeIndex = graphViewHistory.Count - 1;
            UpdateNavigateButtonsAvailability();
        }

        /// <summary>
        /// Active ou desactive les boutons "suivant" et "précédent" en fonction de la position
        /// du node actuellement vis à vis de la liste de l'historique des nodes parcourus.
        /// </summary>
        private void UpdateNavigateButtonsAvailability()
        {
            bool nextAvailable = (currentNodeIndex < graphViewHistory.Count - 1);
            bool previousAvailable = currentNodeIndex > 0;

            navigateBackwardToolStripMenuItem.Enabled = previousAvailable;
            navigateForwardToolStripMenuItem.Enabled = nextAvailable;

            toolStripButtonNavBack.Enabled = previousAvailable;
            toolStripButtonNavForward.Enabled = nextAvailable;
        }

        #endregion

        private void licenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new LicenceForm()).ShowDialog();
        }

        public Size? OutputImgSize { get; set; }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            comboBoxPath.Focus();
            if (launchScanOnStartup)
            {
                try
                {
                    LaunchScan();
                    launchScanOnStartup = false;
                    bool exitApp = false;
                    if (outputImgFilePath != null
                        && outputImgFilePath.Length > 0)
                    {
                        ImageGraphGeneratorBase generator = ImageGraphGeneratorFactory.CreateGenerator(this.DrawType, scanEngine.Root, scanEngine);
                        DrawOptions outputDrawOptions = treeGraph1.DrawOptions.Clone();
                        if (OutputImgSize.HasValue)
                            outputDrawOptions.TargetSize = OutputImgSize.Value;
                        Bitmap bmp = generator.Draw(true, true, outputDrawOptions).Obj1;
                        //if (File.GetAccessControl(outputImgFilePath).
                        bmp.Save(outputImgFilePath);
                        exitApp = true;
                    }
                    if (outputGraphFilePath != null
                        && outputGraphFilePath.Length > 0)
                    {
                        SaveGraphToFile(outputGraphFilePath);
                        exitApp = true;
                    }
                    if (exitApp)
                        Application.Exit();
                }
                catch (Exception ex)
                {
                    throw new FatalHdgraphException(ex.Message, ex);
                }
            }
        }

        private void checkBoxAutoRecalc_CheckedChanged(object sender, EventArgs e)
        {
            scanEngine.AutoRefreshAllowed = checkBoxAutoRecalc.Checked;
        }

        private void comboBoxPath_TextUpdate(object sender, EventArgs e)
        {
            // Lorsque le chemin à calculé est changé par l'utilisateur,
            // on rédéfinit l'autoRecalc du moteur par la valeur par défaut du fichier de conf.
            checkBoxAutoRecalc.Enabled = true;
            checkBoxAutoRecalc.Checked = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
        }

        private void toolStripButtonNavForward_Click(object sender, EventArgs e)
        {
            if (currentNodeIndex < graphViewHistory.Count - 1)
            {
                currentNodeIndex++;
                treeGraph1.Root = graphViewHistory[currentNodeIndex];
                if (treeGraph1.Root != null)
                    comboBoxPath.Text = treeGraph1.Root.Path;
                treeGraph1.ForceRefresh();
            }
            UpdateNavigateButtonsAvailability();
        }

        private void toolStripButtonNavBack_Click(object sender, EventArgs e)
        {
            if (currentNodeIndex > 0)
            {
                currentNodeIndex--;
                treeGraph1.Root = graphViewHistory[currentNodeIndex];
                if (treeGraph1.Root != null)
                    comboBoxPath.Text = treeGraph1.Root.Path;
                treeGraph1.ForceRefresh();
            }
            UpdateNavigateButtonsAvailability();
        }

        private void linkLabelHelpGraph_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help.ShowHelp(this, GetHelpFile(), HelpNavigator.TopicId, "5");
        }

        private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\HDGraph.log");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    HDGTools.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Trace.TraceError(HDGTools.PrintError(ex));
            }
        }

        private void comboBoxColorStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModeAffichageCouleurs modeCouleurs = (ModeAffichageCouleurs)comboBoxColorStyle.SelectedIndex;
            treeGraph1.DrawOptions.ColorStyleChoice = modeCouleurs;
            PrintStatus(Resources.ApplicationMessages.GraphRefreshed);
        }

        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            numUpDownNbNivxAffich.Value = trackBarZoom.Value;
        }

        private void toolsMenu_DropDownOpening(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HDGraph.Properties.Settings.Default.MyDrawOptions = treeGraph1.DrawOptions;
            HDGraph.Properties.Settings.Default.Save();
        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripShortcuts.Visible = shortcutsToolStripMenuItem.Checked;
            UpdateToolZoneVisibility();
        }

        private void UpdateToolZoneVisibility()
        {
            splitContainerGraphAndToolBar.Panel1Collapsed = (!toolBarToolStripMenuItem.Checked && !shortcutsToolStripMenuItem.Checked);
        }

        private void checkBoxShowFreeSpace_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowFreeSpace.Checked)
                treeGraph1.ShowFreeSpace();
            else
                treeGraph1.HideFreeSpace();
            PrintStatus(Resources.ApplicationMessages.GraphRefreshed);
        }

        private bool currentlyScrollingZoom;

        private void trackBarZoom_MouseDown(object sender, MouseEventArgs e)
        {
            currentlyScrollingZoom = true;
        }

        private void trackBarZoom_MouseUp(object sender, MouseEventArgs e)
        {
            currentlyScrollingZoom = false;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            HDGraph.Properties.Settings.Default.OptionMainWindowOpenState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
                HDGraph.Properties.Settings.Default.OptionMainWindowSize = this.ClientSize;
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            treeGraph1.Resizing = true;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            treeGraph1.Resizing = false;
            treeGraph1.Refresh();
        }

        private void checkBoxShowTooltip_CheckedChanged(object sender, EventArgs e)
        {
            treeGraph1.ShowTooltip = checkBoxShowTooltip.Checked;
        }


        private void trackBarTextDensity_MouseDown(object sender, MouseEventArgs e)
        {
            treeGraph1.TextChangeInProgress = true;
        }

        private void trackBarTextDensity_MouseUp(object sender, MouseEventArgs e)
        {
            treeGraph1.TextChangeInProgress = false;
        }

        private void imageRotationTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            treeGraph1.RotationInProgress = true;
        }

        private void imageRotationTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            treeGraph1.RotationInProgress = false;
            //treeGraph1.ForceRefresh();
        }

        private void radioButtonEngineCircular_CheckedChanged(object sender, EventArgs e)
        {
            this.DrawType = (radioButtonEngineCircular.Checked) ? DrawType.Circular : DrawType.Rectangular;
            this.treeGraph1.DrawType = this.DrawType;
            treeGraph1.ForceRefresh();
        }

        private void buttonAdvanced_Click(object sender, EventArgs e)
        {
            splitContainerGraphAndOptions.Panel2Collapsed = !splitContainerGraphAndOptions.Panel2Collapsed;
            splitContainerGraphAndOptions.SplitterDistance = splitContainerGraphAndOptions.Size.Width - 166;

            #region Force locations for some controls in order to correct a Mono bug on Linux
            if (!ToolProviderBase.CurrentOsIsWindows())
            {
                groupBox1.Location = new Point(0, 5);
                groupBoxHoverInfo.Location = new Point(0, 424);
                errorStatus1.Location = new Point(4, 594);
            }
            #endregion

            if (splitContainerGraphAndOptions.Panel2Collapsed)
                buttonAdvanced.Image = Properties.Resources.FillLeftHS;
            else
                buttonAdvanced.Image = Properties.Resources.FillRightHS;
        }

        #region Drag&Drop

        /// <summary>
        /// DragEnter event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerAdressBarAndGraph_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                string path = GetFolderPathFromDrag(e);
                if (!string.IsNullOrEmpty(path))
                {
                    e.Effect = DragDropEffects.All;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("DragAndDrop Error (DragEnter) : " + HDGTools.PrintError(ex));
            }
        }

        /// <summary>
        /// Get the path of the directory of the drag event.
        /// Return null if the path is not a directory (for exemple for a file path instead
        /// of a folder path).
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string GetFolderPathFromDrag(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Object o = e.Data.GetData(DataFormats.FileDrop);
                string[] files = o as string[];
                if (files != null)
                {
                    string firstFile = files[0];
                    if (Directory.Exists(firstFile))
                    {
                        return firstFile;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Drop event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerAdressBarAndGraph_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string path = GetFolderPathFromDrag(e);
                if (!string.IsNullOrEmpty(path))
                {
                    comboBoxPath.Text = path;
                    LaunchScan();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("DragAndDrop Error (Drop) : " + HDGTools.PrintError(ex));
            }
        }

        #endregion

        private void buttonTestWpf_Click(object sender, EventArgs e)
        {
            PlugIn.PlugInsManager.Test(this.scanEngine.Root, this.treeGraph1.DrawOptions);
        }

        private void drawOptionsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

    }
}
