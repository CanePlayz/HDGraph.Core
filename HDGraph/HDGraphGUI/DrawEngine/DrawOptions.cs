using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HDGraph.DrawEngine
{
    internal class DrawOptions
    {
        public Font TextFont { get; set; }

        public bool ShowSize { get; set; }

        public Size BitmapSize { get; set; }

        public int ShownLevelsCount { get; set; }

        public ModeAffichageCouleurs ColorStyleChoice { get; set; }

        public int ImageRotation { get; set; }
    }
}
