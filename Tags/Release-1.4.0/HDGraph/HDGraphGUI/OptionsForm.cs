using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Icon = Owner.Icon;
        }


        private void buttonOk_Click(object sender, EventArgs e)
        {
            
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (optionsUserControl1.SaveValues() == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

    }
}