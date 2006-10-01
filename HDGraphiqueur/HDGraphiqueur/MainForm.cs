using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HDGraphiqueur
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        MoteurGraphiqueur moteur = new MoteurGraphiqueur();

        private void buttonScan_Click(object sender, EventArgs e)
        {
            int nbNiveaux = (int) numUpDownNbNivx.Value;
            moteur.PrintInfoDeleg = new MoteurGraphiqueur.PrintInfoDelegate(PrintStatus);
            moteur.ConstruireArborescence(comboBoxPath.Text, nbNiveaux);
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.Moteur = moteur;
            treeGraph1.Refresh();
            PrintStatus("Terminé !");
        }

        public void PrintStatus(string message)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " : " + message;
            Application.DoEvents();
        }

        private void numUpDownNbNivxAffich_ValueChanged(object sender, EventArgs e)
        {
            int nbNiveaux = (int)numUpDownNbNivxAffich.Value;
            treeGraph1.NbNiveaux = nbNiveaux;
            treeGraph1.Moteur = moteur;
            treeGraph1.Refresh();
            PrintStatus("Terminé !");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //treeGraph1.LimiteRemplissage = (int) numericUpDown1.Value;
            treeGraph1.Refresh();
        }
    }
}