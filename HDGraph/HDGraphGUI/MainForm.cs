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
using WilsonProgramming;
using HDGraph.DrawEngine;

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
        HDGraphScanEngine moteur;

        /// <summary>
        /// Liste des nodes parcours, pour les boutons "back" et "next".
        /// </summary>
        List<DirectoryNode> graphViewHistory = new List<DirectoryNode>();

        /// <summary>
        /// Index du node (de la liste graphViewHistory) qui actuellement affiché
        /// </summary>
        int currentNodeIndex = 0;

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

        #endregion

        #region Constructeur(s) et initialisation

        public MainForm()
        {
            bool changeLangIsSuccess = LoadLanguage();

            // LeResourceManager prend en paramètre : nom_du_namespace.nom_de_la_ressource_principale
            resManager = new System.Resources.ResourceManager(this.GetType().Assembly.GetName().Name + ".Resources.ApplicationMessages", this.GetType().Assembly);
            HDGTools.resManager = resManager;
            moteur = new HDGraphScanEngine();
            moteur.ShowDiskFreeSpace = Properties.Settings.Default.OptionShowFreeSpace;
            if (!changeLangIsSuccess)
                MessageBox.Show(resManager.GetString("ErrorInConfigLanguage"),
                                resManager.GetString("ErrorInConfigLanguageTitle"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

            InitializeComponent();

            this.Text = AboutBox.AssemblyTitle;
            this.WindowState = HDGraph.Properties.Settings.Default.OptionMainWindowOpenState;
            this.ClientSize = HDGraph.Properties.Settings.Default.OptionMainWindowSize;

            EnableHelpIfAvailable();
            comboBoxPath.DataSource = HDGraph.Properties.Settings.Default.PathHistory;
            checkBoxAutoRecalc.Checked = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
            SetTrackBarZoomValueFromNumUpDown();
            try
            {
                moteur.AutoRefreshAllowed = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
                ModeAffichageCouleurs modeCouleurs = (ModeAffichageCouleurs)Enum.Parse(typeof(ModeAffichageCouleurs), HDGraph.Properties.Settings.Default.OptionColorStyle);
                comboBoxColorStyle.SelectedIndex = (int)modeCouleurs;
            }
            catch (Exception ex)
            {
                Trace.TraceError(HDGTools.PrintError(ex));
            }
            PopulateAnalyseShortcuts();
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
            ShellAPI.SHFILEINFO shInfo = new ShellAPI.SHFILEINFO();
            ShellAPI.SHGFI dwAttribs =
                ShellAPI.SHGFI.SHGFI_ICON |
                ShellAPI.SHGFI.SHGFI_SMALLICON |
                ShellAPI.SHGFI.SHGFI_SYSICONINDEX |
                ShellAPI.SHGFI.SHGFI_DISPLAYNAME;

            Dictionary<int, Icon> iconList = new Dictionary<int, Icon>();
            foreach (string drive in System.IO.Directory.GetLogicalDrives())
            {
                ToolStripButton button = new ToolStripButton(drive);
                IntPtr m_pHandle = ShellAPI.SHGetFileInfo(drive, ShellAPI.FILE_ATTRIBUTE_NORMAL, out shInfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(shInfo), dwAttribs);

                if (!m_pHandle.Equals(IntPtr.Zero))
                {
                    if (!iconList.ContainsKey(shInfo.iIcon))
                    {
                        iconList.Add(shInfo.iIcon, Icon.FromHandle(shInfo.hIcon).Clone() as Icon);
                        User32API.DestroyIcon(shInfo.hIcon);
                    }

                    button.Text = shInfo.szDisplayName;
                    button.Image = iconList[shInfo.iIcon].ToBitmap();
                    button.Tag = drive;
                    button.Click += new EventHandler(shortcutButton_Click);
                    toolStripShortcuts.Items.Add(button);
                }
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
            toolStripContainer1.Location = new Point(0, menuStrip.Size.Height);
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
                XmlSerializer serializer = new XmlSerializer(typeof(HDGraphScanEngine));
                moteur = (HDGraphScanEngine)serializer.Deserialize(reader);
                reader.Close();
                moteur.PrintInfoDeleg = new HDGraphScanEngine.PrintInfoDelegate(PrintStatus);
                treeGraph1.Moteur = moteur;
                treeGraph1.UpdateHoverNode = new TreeGraph.NodeNotificationDelegate(PrintNodeHoverCursor);
                treeGraph1.NotifyNewRootNode = new TreeGraph.NodeNotificationDelegate(UpdateCurrentNodeRoot);

                if (moteur.Root != null)
                {
                    comboBoxPath.Text = moteur.Root.Path;
                    numUpDownNbNivx.Value = moteur.Root.ProfondeurMax;
                    numUpDownNbNivxAffich.Value = moteur.Root.ProfondeurMax;
                    treeGraph1.NbNiveaux = moteur.Root.ProfondeurMax;
                }
                treeGraph1.ForceRefresh();
                UpdateNodeHistory(moteur.Root);
                PrintStatus(String.Format(resManager.GetString("GraphLoadedFromDate"), moteur.AnalyzeDate.ToString()));
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
            XmlSerializer serializer = new XmlSerializer(typeof(HDGraphScanEngine));
            serializer.Serialize(writer, moteur);
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
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
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
                treeGraph1.NbNiveaux = nbNiveaux;
                treeGraph1.ForceRefresh();
                PrintStatus(Resources.ApplicationMessages.GraphRefreshed, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    HDGTools.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Trace.TraceError(HDGTools.PrintError(ex));
            }
        }

        private void checkBoxPrintSizes_CheckedChanged(object sender, EventArgs e)
        {
            treeGraph1.OptionShowSize = checkBoxPrintSizes.Checked;
            treeGraph1.TextChangeInProgress = true;
            treeGraph1.ForceRefresh();
            treeGraph1.TextChangeInProgress = false;
            PrintStatus(Resources.ApplicationMessages.GraphRefreshed);
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
            form.ShowDialogAndStartScan(moteur, comboBoxPath.Text, nbNiveaux);

            //watch.Stop();
            //MessageBox.Show(watch.Elapsed.ToString());

            // // moteur.ConstruireArborescence(comboBoxPath.Text, nbNiveaux); // OBSOLETE
            // // moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(WaitForm.ShowWaitForm); // OBSOLETE

            moteur.PrintInfoDeleg = new HDGraphScanEngine.PrintInfoDelegate(PrintStatus);
            numUpDownNbNivxAffich.Value = nbNiveaux;
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.Moteur = moteur;
            treeGraph1.ForceRefresh();
            UpdateNodeHistory(moteur.Root);
            //PrintStatus("Terminé !");
            treeGraph1.UpdateHoverNode = new TreeGraph.NodeNotificationDelegate(PrintNodeHoverCursor);
            treeGraph1.NotifyNewRootNode = new TreeGraph.NodeNotificationDelegate(UpdateCurrentNodeRoot);
            errorStatus1.Update(moteur.ErrorList);

            buttonScan.Enabled = true;
        }

        /// <summary>
        /// Affiche les informations du répertoire "node" dans la barre de status.
        /// </summary>
        /// <param name="node"></param>
        private void PrintNodeHoverCursor(DirectoryNode node)
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
        private void UpdateCurrentNodeRoot(DirectoryNode node)
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
        private void UpdateNodeHistory(DirectoryNode node)
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

        private void MainForm_Activated(object sender, EventArgs e)
        {

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
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

                        treeGraph1.ImageBuffer.Save(outputImgFilePath);
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
            moteur.AutoRefreshAllowed = checkBoxAutoRecalc.Checked;
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

        private void button1_Click(object sender, EventArgs e)
        {
            (new PickColorForm()).Show();
        }

        private void comboBoxColorStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModeAffichageCouleurs modeCouleurs = (ModeAffichageCouleurs)comboBoxColorStyle.SelectedIndex;
            HDGraph.Properties.Settings.Default.OptionColorStyle = modeCouleurs.ToString();
            HDGraph.Properties.Settings.Default.Save();
            treeGraph1.ModeCouleur = modeCouleurs;
            treeGraph1.ForceRefresh();
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
            HDGraph.Properties.Settings.Default.Save();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripShortcuts.Visible = shortcutsToolStripMenuItem.Checked;
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


        private void tipsMonitor1_HideTipsWanted(object sender, EventArgs e)
        {
            applicationTipsToolStripMenuItem.Checked = false;
        }

        private void applicationTipsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            splitContainerGraphAndTips.Panel2Collapsed = !applicationTipsToolStripMenuItem.Checked;
        }

        private void imageRotationTrackBar_ValueChanged(object sender, EventArgs e)
        {

            treeGraph1.DrawOptions.ImageRotation = imageRotationTrackBar.Value;
            labelValeurRotation.Text = treeGraph1.DrawOptions.ImageRotation.ToString() + " °";
            treeGraph1.ForceRefresh();
        }

        private void trackBarTextDensity_ValueChanged(object sender, EventArgs e)
        {
            treeGraph1.DrawOptions.TextDensity = trackBarTextDensity.Maximum - trackBarTextDensity.Value + trackBarTextDensity.Minimum;
            treeGraph1.ForceRefresh();
        }

        private void trackBarTextDensity_Scroll(object sender, EventArgs e)
        {
            
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
    }
}
