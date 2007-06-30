using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class DirectoryDetailUC : UserControl
    {
        public DirectoryDetailUC()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }
    }
}
