using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace HDGraph.DrawEngine
{
    public class DrawOptions
    {
        public Font TextFont { get; set; }

        public bool ShowSize { get; set; }

        public Size BitmapSize { get; set; }

        public int ShownLevelsCount { get; set; }

        public ModeAffichageCouleurs ColorStyleChoice { get; set; }

        public int ImageRotation { get; set; }

        /// <summary>
        /// Angle min to enable text print.
        /// </summary>
        public int TextDensity { get; set; }

        public DrawOptions Clone()
        {
            return (DrawOptions)this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            if (obj == null
                || !(obj is DrawOptions))
                return false;
            if ((object)this == obj)
                return true;
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (!property.GetValue(this, null).Equals(property.GetValue(obj, null)))
                    return false;
            }
            return true;
        }
    }
}
