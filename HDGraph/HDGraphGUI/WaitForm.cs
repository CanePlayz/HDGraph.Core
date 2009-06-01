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
    public enum AsyncActionType
    {
        None,
        Scan,
        AbstractAction
    }

    public partial class WaitForm : Form
    {
        #region Constructeur
        /// <summary>
        /// Constructeur.
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
        private AsyncActionType typeAction = AsyncActionType.None;

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
                myThread.CurrentUICulture = Application.CurrentCulture;
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

        private HDGraphScanEngineBase moteur = null;
        private string path;
        private int nbNiveaux;
        public Exception ActionError { get; set; }

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

        public void ShowDialogAndStartScan(HDGraphScanEngineBase moteur, string path, int nbNiveaux)
        {
            this.typeAction = AsyncActionType.Scan;
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
            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = WaitForm.ThreadCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = WaitForm.ThreadCulture;
                switch (typeAction)
                {
                    case AsyncActionType.None:
                        break;
                    case AsyncActionType.Scan:
                        if (moteur != null)
                        {
                            try
                            {
                                moteur.PrintInfoDeleg = new HDGraphScanEngineBase.PrintInfoDelegate(this.UpdateMessage);
                                moteur.BuildTree(path, nbNiveaux);
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
                                else
                                {
                                    MessageBox.Show(Resources.ApplicationMessages.UnexpectedErrorDuringAnalysis,
                                            Resources.ApplicationMessages.Error,
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    System.Diagnostics.Trace.TraceError("Error while scanning " + path + ": " + HDGTools.PrintError(ex));
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(Resources.ApplicationMessages.UnexpectedErrorDuringAnalysis,
                                            Resources.ApplicationMessages.Error,
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                System.Diagnostics.Trace.TraceError("Error while scanning " + path + ": " + HDGTools.PrintError(ex));
                            }
                        }
                        break;
                    case AsyncActionType.AbstractAction:
                        actionToDo();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                if (typeAction != AsyncActionType.AbstractAction)
                    MessageBox.Show(Resources.ApplicationMessages.UnexpectedErrorDuringAnalysis,
                                                Resources.ApplicationMessages.Error,
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Trace.TraceError("Error while scanning or during abstract action for " + path + ": " + HDGTools.PrintError(ex));
                this.ActionError = ex;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.typeAction = AsyncActionType.None;
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            if (moteur != null || typeAction == AsyncActionType.AbstractAction)
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
            this.typeAction = AsyncActionType.None;
            moteur.PleaseCancelCurrentWork = true;
            this.buttonCancel.Enabled = false;
        }

        public delegate void DoAction();
        private DoAction actionToDo = null;

        /// <summary>
        /// Lance la fenêtre d'attente en modal puis exécute l'action passée en paramètre.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionToDo"></param>
        public void ShowDialogAndStartAction(string message, DoAction actionToDo)
        {
            this.ActionError = null;
            this.typeAction = AsyncActionType.AbstractAction;
            this.actionToDo = actionToDo;
            this.message = message;
            this.ShowDialog();
        }
    }
}
