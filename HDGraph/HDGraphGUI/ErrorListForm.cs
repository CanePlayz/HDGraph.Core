using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDGraph.Engine;

namespace HDGraph
{
    public partial class ErrorListForm : Form
    {
        private static ErrorListForm currentErrorListForm;

        public static void ShowForm(BindingList<ScanError> errorList)
        {
            if (currentErrorListForm == null
                || currentErrorListForm.IsDisposed)
                currentErrorListForm = new ErrorListForm();
            currentErrorListForm.scanErrorBindingSource.DataSource = errorList;
            if (currentErrorListForm.Visible)
                currentErrorListForm.BringToFront();
            else
                currentErrorListForm.Show();
        }

        public ErrorListForm()
        {
            InitializeComponent();
            this.Owner = Application.OpenForms[0];
            this.Icon = Owner.Icon;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
