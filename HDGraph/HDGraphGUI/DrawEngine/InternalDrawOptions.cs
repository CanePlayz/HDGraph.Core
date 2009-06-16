using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.DrawEngine
{
    public class InternalDrawOptions : DrawOptions
    {

        public Size BitmapSize { get; set; }

        public DrawType DrawStyle { get; set; }
        
        public InternalDrawOptions Clone()
        {
            return (InternalDrawOptions)this.MemberwiseClone();
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
