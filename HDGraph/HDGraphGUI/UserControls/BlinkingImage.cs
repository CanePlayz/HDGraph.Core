using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace HDGraph.UserControls
{
    public partial class BlinkingImage : PictureBox
    {
        public const int BLINK_INTERVAL_DEFAULT_VALUE = 300;

        [Description("Active or desactivate the \"blinking\" effect.")]
        [Category("Blinking effect")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool BlinkEnabled
        {
            get
            {
                return timer1.Enabled;
            }
            set
            {
                if (value != timer1.Enabled
                    && !value) // désactivation
                {
                    this.Visible = true;
                }
                timer1.Enabled = value;
            }
        }

        [Description("Duration (in ms) between visibility change.")]
        [Category("Blinking effect")]
        [Browsable(true)]
        [DefaultValue(BLINK_INTERVAL_DEFAULT_VALUE)]
        public int BlinkInterval
        {
            get { return timer1.Interval; }
            set { timer1.Interval = value; }
        }

        public BlinkingImage()
        {
            InitializeComponent();
            timer1.Interval = BLINK_INTERVAL_DEFAULT_VALUE;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }
    }
}
