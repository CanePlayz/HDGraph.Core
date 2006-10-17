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

        /// <summary>
        /// Moteur de scan.
        /// </summary>
        MoteurGraphiqueur moteur;

        /// <summary>
        /// Liste des nodes parcours, pour les boutons "back" et "next".
        /// </summary>
        List<DirectoryNode> graphViewHistory = new List<DirectoryNode>();
        int currentNodeIndex = 0;

        #endregion

        #region Constructeur(s) et initialisation

        public MainForm()
        {
            bool changeLangIsSuccess = LoadLanguage();

            // LeResourceManager prend en paramètre : nom_du_namespace.nom_de_la_ressource_principale
            resManager = new System.Resources.ResourceManager(this.GetType().Assembly.GetName().Name + ".Resources.ApplicationMessages", this.GetType().Assembly);
            HDGTools.resManager = resManager;
            moteur = new MoteurGraphiqueur();
            if (!changeLangIsSuccess)
                MessageBox.Show(resManager.GetString("ErrorInConfigLanguage"),
                                resManager.GetString("ErrorInConfigLanguageTitle"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

            InitializeComponent();
            this.Text = AboutBox.AssemblyTitle;
            EnableHelpIfAvailable();
            comboBoxPath.DataSource = HDGraph.Properties.Settings.Default.PathHistory;
            checkBoxAutoRecalc.Checked = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
            moteur.AutoRefreshAllowed = HDGraph.Properties.Settings.Default.OptionAutoCompleteGraph;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string path = args[1]; // args[0] correspond à l'exécutable, args[1] au premier argument
                Trace.WriteLineIf(HDGTools.mySwitch.TraceInfo, "Path argument received: " + path);

                // cas particulier de l'explorateur qui renvoie << x:" >> dans le cas du lecteur x
                if (path.EndsWith(":\""))
                    path = path.Substring(0, path.Length - 1);
                if (File.Exists(path) && Path.GetExtension(path) == ".hdg")
                {
                    // le 1er argument est un fichier HDG à charger
                    LoadGraphFromFile(path);
                }
                else
                {   // le 1er argument est un répertoire: il faut lancer le scan.
                    path = (new DirectoryInfo(path)).FullName;
                    comboBoxPath.Text = path;
                    SavePathHistory();
                    launchScanOnStartup = true;
                }
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
            string lang = HDGraph.Properties.Settings.Default.Language;
            if (lang != null && lang.Length > 0)
            {
                try
                {
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(lang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    WaitForm.ThreadCulture = culture;
                    return true;
                }
                catch (Exception ex)
                {
                    Trace.TraceError(HDGTools.PrintError(ex));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Evènement Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
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
        private void LoadGraphFromFile(string fileName)
        {
            checkBoxAutoRecalc.Checked = false; // Autorecalc interdit sur un graph enregistré !
            checkBoxAutoRecalc.Enabled = false;

            try
            {
                XmlReader reader = new XmlTextReader(fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(MoteurGraphiqueur));
                moteur = (MoteurGraphiqueur)serializer.Deserialize(reader);
                reader.Close();
                moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(PrintStatus);
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
            XmlSerializer serializer = new XmlSerializer(typeof(MoteurGraphiqueur));
            serializer.Serialize(writer, moteur);
            writer.Close();
        }

        private void exportAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Bitmap" +
                                    " (*.bmp)|*.bmp|" +
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
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
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
                WaitForm.ShowWaitForm(this, resManager.GetString("loadingLanguageList"));

                // Remarque: WaitForm.ShowWaitForm peux s'utiliser plusieurs fois consécutives pour mettre à jour le message. Exemple:

                //System.Threading.Thread.Sleep(1000);
                //WaitForm.ShowWaitForm(this, "Message 2 !!");
            }
            DialogResult res = (new LanguageForm(resManager)).ShowDialog();
            if (res == DialogResult.OK)
            {
                menuStrip.SuspendLayout();
                EnableHelpIfAvailable();
                HDGTools.ApplyCulture(this, System.Threading.Thread.CurrentThread.CurrentUICulture);
                this.Text = AboutBox.AssemblyTitle;
                treeGraph1.ForceRefresh();
                menuStrip.ResumeLayout(true);
                buttonScan.Refresh();
            }
        }

        #endregion

        #region Affichages fichier d'aide


        /// <summary>
        /// Renvoi le chemin du fichier d'aide.
        /// </summary>
        /// <returns>Null ou chaine vide si aucun fichier d'aide.</returns>
        private string GetHelpFile()
        {
            return Application.StartupPath + Path.DirectorySeparatorChar + Resources.ApplicationMessages.HelpFilename;
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
            //this.Cursor = Cursors.WaitCursor;
            //this.menuStrip.Enabled = false;
            buttonScan.Enabled = false;
            SavePathHistory();
            LaunchScan();
            //this.Cursor = this.DefaultCursor;
            //this.menuStrip.Enabled = true;
            buttonScan.Enabled = true;
        }

        #region Gestion de l'historique chemins entrés manuellement

        private void SavePathHistory()
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
            int nbNiveaux = (int)numUpDownNbNivxAffich.Value;
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.ForceRefresh();
            PrintStatus("Terminé !");
        }

        private void checkBoxPrintSizes_CheckedChanged(object sender, EventArgs e)
        {
            treeGraph1.OptionShowSize = checkBoxPrintSizes.Checked;
            treeGraph1.ForceRefresh();
            PrintStatus("Terminé !");
        }

        #region Intégration à l'explorateur

        private void addMeToTheExplorerConToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (HDGTools.AddMeToExplorerContextMenu())
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

        private void removeMeFromTheExplorerContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (HDGTools.RemoveMeFromExplorerContextMenu())
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

        #endregion


        public void PrintStatus(string message)
        {
            if (message != null && message.Length > 0)
                toolStripStatusLabel.Text = DateTime.Now.ToString() + " : " + message;
            Application.DoEvents();
        }


        /// <summary>
        /// Lance le scan et le graphiquage.
        /// </summary>
        private void LaunchScan()
        {

            int nbNiveaux = (int)numUpDownNbNivx.Value;

            WaitForm form = new WaitForm();

            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            form.ShowDialogAndStartScan(moteur, comboBoxPath.Text, nbNiveaux);

            //watch.Stop();
            //MessageBox.Show(watch.Elapsed.ToString());

            // // moteur.ConstruireArborescence(comboBoxPath.Text, nbNiveaux); // OBSOLETE
            // // moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(WaitForm.ShowWaitForm); // OBSOLETE

            moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(PrintStatus);
            numUpDownNbNivxAffich.Value = nbNiveaux;
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.Moteur = moteur;
            treeGraph1.ForceRefresh();
            UpdateNodeHistory(moteur.Root);
            //PrintStatus("Terminé !");
            treeGraph1.UpdateHoverNode = new TreeGraph.NodeNotificationDelegate(PrintNodeHoverCursor);
            treeGraph1.NotifyNewRootNode = new TreeGraph.NodeNotificationDelegate(UpdateCurrentNodeRoot);
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
                PrintStatus(String.Format(resManager.GetString("CursorHoverDirectory"), node.Path));
                //MessageBox.Show("Cursor hover directory " + node.Path);
                labelDirName.Text = node.Name;
                labelDirTotalSize.Text = treeGraph1.FormatSize(node.TotalSize);
                if (node.TotalSize > 0)
                    labelFilesSize.Text = treeGraph1.FormatSize(node.FilesSize) + " (" + node.FilesSize * 100 / node.TotalSize + "%)";
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

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //treeGraph1.Refresh();
        }

        #endregion

        private void licenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new LGPLLicenceForm()).ShowDialog();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (launchScanOnStartup)
            {
                LaunchScan();
                launchScanOnStartup = false;
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
            Help.ShowHelp(this, GetHelpFile(), "/html/fonctions_de_base.htm");
        }

    }
}
