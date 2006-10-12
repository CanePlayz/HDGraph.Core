using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace HDGraph
{
    public partial class WaitForm : Form
    {
        #region Constructeur
        /// <summary>
        /// Constructeur. Inutile dans la plupart des cas : utiliser les méthodes statiques ShowWaitForm et HideWaitForm.
        /// </summary>
        public WaitForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Méthodes liées à l'utilisation statique

        public void SetMessage(string message)
        {
            labelInformation.Text = message;
        }

        private static Thread myThread = null;

        // Evènement de signal de fin de thread
        private static AutoResetEvent _endThreadCalculsEvent = new AutoResetEvent(false);
        private static string formMessage = "";
        private static CultureInfo threadCulture = null;
        private static WaitForm myWaitForm;

        public static CultureInfo ThreadCulture
        {
            get { return WaitForm.threadCulture; }
            set { WaitForm.threadCulture = value; }
        }

        private static void LoadFormIfNecessary()
        {
            if (threadCulture != null)
                Thread.CurrentThread.CurrentUICulture = threadCulture;
            if (myWaitForm == null)
            {
                myWaitForm = new WaitForm();
                myWaitForm.labelInformation.Text = formMessage;
                myWaitForm.Show();
                //myWaitForm.CenterToParent();
                Application.DoEvents();
                while (!_endThreadCalculsEvent.WaitOne(50, false))
                {
                    if (myWaitForm.labelInformation.Text != formMessage)
                        myWaitForm.labelInformation.Text = formMessage;
                    Application.DoEvents();
                }
                myWaitForm.Close();
                myWaitForm = null;
            }
        }


        /// <summary>
        /// Affiche une fenêtre pour faire patienter l'utilisateur (dans un thread à part, pour ne pas bloquer l'exécution).
        /// </summary>
        /// <param name="parent">Non utilisé pour le moment.</param>
        /// <param name="message"></param>
        public static void ShowWaitForm(string message)
        {
            ShowWaitForm(null, message);
        }

        /// <summary>
        /// Affiche une fenêtre pour faire patienter l'utilisateur (dans un thread à part, pour ne pas bloquer l'exécution).
        /// </summary>
        /// <param name="parent">Non utilisé pour le moment.</param>
        /// <param name="message"></param>
        public static void ShowWaitForm(Form parent, string message)
        {

            formMessage = message;
            if (myThread == null || myThread.ThreadState == ThreadState.Stopped || myThread.ThreadState == ThreadState.Unstarted)
            {
                // LoadFormIfNecessary est la fonction exécutée par le thread.
                myThread = new Thread(new ThreadStart(LoadFormIfNecessary));
                myThread.Start();
            }
        }

        /// <summary>
        /// Ferme la fenêtre d'attente affichée avec la méthode ShowWaitForm (si elle existe).
        /// </summary>
        public static void HideWaitForm()
        {
            if (myThread != null)
            {
                // L'evenement passe à l'état signalé
                _endThreadCalculsEvent.Set();
                // On attend la fin du thread.
                myThread.Join();
            }
        }

        #endregion


        #region Méthodes liées à l'exécution modale

        private MoteurGraphiqueur moteur = null;
        private string path;
        private int nbNiveaux;

        private void WaitForm_Load(object sender, EventArgs e)
        {
            if (!buttonCancel.Visible)
            {
                int heightDiff = 40;
                this.Height -= heightDiff;
                labelInformation.Height += heightDiff;
            }
            if (moteur == null)
                return;
        }

        public void ShowDialogAndStartScan(MoteurGraphiqueur moteur, string path, int nbNiveaux)
        {
            this.moteur = moteur;
            this.path = path;
            this.nbNiveaux = nbNiveaux;
            this.buttonCancel.Visible = true;
            this.ShowDialog();
        }

        private string message;

        private void UpdateMessage(string message)
        {
            this.message = message;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (moteur != null)
            {
                try
                {
                    moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(this.UpdateMessage);
                    moteur.ConstruireArborescence(path, nbNiveaux);
                }
                catch (ArgumentException ex)
                {
                    System.Diagnostics.Trace.TraceError("Invalid path (" + path + "): " + HDGTools.PrintError(ex));
                    if (ex.ParamName == "path")
                    {
                        MessageBox.Show(Resources.ApplicationMessages.InvalidPathError,
                                Resources.ApplicationMessages.Error,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.ApplicationMessages.UnexpectedErrorDuringAnalysis,
                                Resources.ApplicationMessages.Error,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Trace.TraceError("Error while scanning " + path+ ": " + HDGTools.PrintError(ex));
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Close(); // instruction moved to WaitForm_Shown
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            if (moteur != null)
            {
                backgroundWorker1.RunWorkerAsync();
                Application.DoEvents();
                while (backgroundWorker1.IsBusy)
                {
                    Thread.Sleep(50);
                    if (message != null && message != labelInformation.Text)
                        labelInformation.Text = message;
                    Application.DoEvents();
                }
                this.Close();
            }
        }

        #endregion

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            moteur.PleaseCancelCurrentWork = true;
            this.buttonCancel.Enabled = false;
        }
    }
}
