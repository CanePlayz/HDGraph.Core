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
    }
}