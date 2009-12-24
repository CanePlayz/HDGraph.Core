using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Reflection;
using HDGraph.Resources;

namespace HDGraph
{
    public partial class NewVersionAvailableForm : Form
    {
        private VersionInfo versionInfo;
        public VersionInfo VersionInfo
        {
            get { return versionInfo; }
            set
            {
                versionInfo = value;
                labelCurrentVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                labelNewerVersion.Text = value.VersionNumber;
                labelReleaseDate.Text = String.Format(ApplicationMessages.ReleasedOn, value.ReleaseDate);
            }
        }

        private WebClient webClient = new WebClient();

        public NewVersionAvailableForm()
        {
            InitializeComponent();
        }

        private void NewVersionAvailableForm_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.Icon = Owner.Icon;
            }
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            buttonNo.Enabled = false;
            buttonYes.Enabled = false;
            Process.Start(VersionInfo.DownloadPageUrl);
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(VersionInfo.ChangeLogUrl);
        }
    }
}
