using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using HDGraph.Engine;
using HDGraph.Resources;

namespace HDGraph.UserControls
{
    public partial class ErrorStatus : UserControl
    {
        public ErrorStatus()
        {
            InitializeComponent();
        }

        private IList<ScanError> errorList;

        public void Update(IList<ScanError> errorList)
        {
            this.errorList = errorList;
            this.Visible = (errorList.Count > 0);
            if (errorList.Count == 1)
                labelErrors.Text = ApplicationMessages.OneElementSkipped;
            else
                labelErrors.Text = String.Format(ApplicationMessages.SomeElementsSkipped, errorList.Count);
        }

        private void linkLabelDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented yet.", "HDGraph", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
