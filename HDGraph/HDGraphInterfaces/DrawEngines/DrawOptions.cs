using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using System.Xml.Serialization;

namespace HDGraph.Interfaces.DrawEngines
{
    public enum ModeAffichageCouleurs
    {
        RandomNeutral,
        RandomBright,
        Linear,
        Linear2,
        ImprovedLinear
    }

    [Serializable]
    public class DrawOptions : INotifyPropertyChanged
    {
        public DrawOptions()
        {
        }

        #region INotifyPropertyChanged Members

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private Font textFont;
        [XmlIgnore]
        public Font TextFont
        {
            get { return textFont; }
            set
            {
                if (textFont != value)
                {
                    textFont = value;
                    RaisePropertyChanged("TextFont");
                }
            }
        }

        private bool showSize;
        public bool ShowSize
        {
            get { return showSize; }
            set
            {
                if (showSize != value)
                {
                    showSize = value;
                    RaisePropertyChanged("ShowSize");
                }
            }
        }

        private int shownLevelsCount;
        public int ShownLevelsCount 
        {
            get { return shownLevelsCount; }
            set
            {
                if (shownLevelsCount != value)
                {
                    shownLevelsCount = value;
                    RaisePropertyChanged("ShownLevelsCount");
                }
            }
        }

        private ModeAffichageCouleurs colorStyleChoice;
        public ModeAffichageCouleurs ColorStyleChoice
        {
            get { return colorStyleChoice; }
            set
            {
                if (colorStyleChoice != value)
                {
                    colorStyleChoice = value;
                    RaisePropertyChanged("ColorStyleChoice");
                }
            }
        }

        private int imageRotation;
        public int ImageRotation
        {
            get { return imageRotation; }
            set
            {
                if (imageRotation != value)
                {
                    imageRotation = value;
                    RaisePropertyChanged("ImageRotation");
                }
            }
        }

        private int textDensity;
        /// <summary>
        /// Angle min to enable text print.
        /// </summary>
        public int TextDensity
        {
            get { return textDensity; }
            set
            {
                if (textDensity != value)
                {
                    textDensity = value;
                    RaisePropertyChanged("TextDensity");
                }
            }
        }

        private Size targetSize;
        /// <summary>
        /// Used to define the target size of the graph. May be useless sometimes (if
        /// the graph automatically determine the size it should use).
        /// For exemple, is used when the user manually set the final size of the graph (for 
        /// exemple to export a bitmap).
        /// </summary>
        public Size TargetSize
        {
            get { return targetSize; }
            set
            {
                if (targetSize != value)
                {
                    targetSize = value;
                    RaisePropertyChanged("TargetSize");
                }
            }
        }

        private bool showTooltip = true;
        /// <summary>
        /// Activate or deactivate tooltips on the graph.
        /// </summary>
        public bool ShowTooltip
        {
            get { return showTooltip; }
            set
            {
                if (showTooltip != value)
                {
                    showTooltip = value;
                    RaisePropertyChanged("ShowTooltip");
                }
            }
        }

        public virtual DrawOptions Clone()
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
