using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace HDGraphiqueurGUI
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
        MoteurGraphiqueur moteur = new MoteurGraphiqueur();

        #endregion

        #region Constructeur(s) et initialisation

        public MainForm()
        {
            bool changeLangIsSuccess = LoadLanguage();

            // LeResourceManager prend en paramètre : nom_du_namespace.nom_de_la_ressource_principale
            //resManager = new System.Resources.ResourceManager("HDGraphiqueurGUI.MainForm", System.Reflection.Assembly.GetExecutingAssembly());
            //resManager = new System.Resources.ResourceManager(this.GetType().Assembly.GetName().Name + ".ApplicationMessages", this.GetType().Assembly);
            resManager = new System.Resources.ResourceManager(this.GetType().Assembly.GetName().Name + ".Resources.ApplicationMessages", this.GetType().Assembly);
            if (!changeLangIsSuccess)
                MessageBox.Show(resManager.GetString("ErrorInConfigLanguage"),
                                resManager.GetString("ErrorInConfigLanguageTitle"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

            InitializeComponent();
            this.Text = AboutBox.AssemblyTitle;
            EnableHelpIfAvailable();
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                comboBoxPath.Text = args[1]; // args[0] correspond à l'exécutable !
                launchScanOnStartup = true;
            }
        }

        private void EnableHelpIfAvailable()
        {
            string helpFile = GetHelpFile();
            if (helpFile != null && helpFile.Length > 0 && System.IO.File.Exists(helpFile))
            {
                contentsToolStripMenuItem.Enabled = true;
                contentsToolStripMenuItem.Visible = true;
                indexToolStripMenuItem.Enabled = true;
                indexToolStripMenuItem.Visible = true;
                searchToolStripMenuItem.Enabled = true;
                searchToolStripMenuItem.Visible = true;
                toolStripSeparator8.Visible = true;
                helpToolStripButton.Enabled = true;
                helpToolStripButton.Visible = true;
            }
        }

        /// <summary>
        /// Charge la langue souhaitée depuis le fichier de config (dans le cas où l'utilisateur souhaite overrider le choix du framework).
        /// </summary>
        /// <returns></returns>
        private bool LoadLanguage()
        {
            string lang = HDGraphiqueurGUI.Properties.Settings.Default.Language;
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
                    // TODO: log erreur;
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Méthodes liées aux menus

        private void ShowNewForm(object sender, EventArgs e)
        {
            // Create a new instance of the child form.
            Form childForm = new Form();
            // Make it a child of this MDI form before showing it.
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                // TODO: Add code here to open the file.
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                // TODO: Add code here to save the current contents of the form to a file.
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

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

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PrintStatus(resManager.GetString("statusReady"));
            if (launchScanOnStartup)
            {
                LaunchScan();
                launchScanOnStartup = false;
            }
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new OptionsForm()).ShowDialog();
        }

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
            (new LanguageForm(resManager)).ShowDialog();
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

        #region Méthodes

        public void PrintStatus(string message)
        {
            if (message != null && message.Length > 0)
                toolStripStatusLabel.Text = DateTime.Now.ToString() + " : " + message;
            Application.DoEvents();
        }

        /// <summary>
        /// Renvoi le chemin du fichier d'aide.
        /// </summary>
        /// <returns>Null ou chaine vide si aucun fichier d'aide.</returns>
        private string GetHelpFile()
        {
            return null;
            // Exemple :
            // return @"C:\WINDOWS\Help\notepad.chm"; // TODO: fichier d'aide. Eventuellement, gérer un fichier par langue et donc renvoyer le fichier approprié à la langue en cours.
        }

        #endregion

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                comboBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            LaunchScan();
        }

        /// <summary>
        /// Lance le scan et le graphiquage.
        /// </summary>
        private void LaunchScan()
        {
            int nbNiveaux = (int)numUpDownNbNivx.Value;
            moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(PrintStatus);
            moteur.ConstruireArborescence(comboBoxPath.Text, nbNiveaux);
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.Moteur = moteur;
            treeGraph1.Refresh();
            PrintStatus("Terminé !");
        }

        private void numUpDownNbNivxAffich_ValueChanged(object sender, EventArgs e)
        {
            int nbNiveaux = (int)numUpDownNbNivxAffich.Value;
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.Refresh();
            PrintStatus("Terminé !");
        }

        private void checkBoxPrintSizes_CheckedChanged(object sender, EventArgs e)
        {
            treeGraph1.OptionShowSize = checkBoxPrintSizes.Checked;
            treeGraph1.Refresh();
            PrintStatus("Terminé !");
        }

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
                // TODO: log erreur
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
                // TODO: log erreur
            }
        }







    }
}
