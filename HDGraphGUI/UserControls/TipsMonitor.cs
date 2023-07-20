using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using HDGraph.Resources;
using System.Reflection;

namespace HDGraph.UserControls
{
    public partial class TipsMonitor : UserControl
    {
        public event EventHandler HideTipsWanted;

        private BindingList<ApplicationTip> tipList;

        public TipsMonitor()
        {
            InitializeComponent();
            CreateTipList();

        }

        private void CreateTipList()
        {
            tipList = new BindingList<ApplicationTip>();
            PropertyInfo[] properties = typeof(ApplicationMessages).GetProperties(BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            // sort properties by name
            Array.Sort(properties,
                            delegate(PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                            {
                                return propertyInfo1.Name.CompareTo(propertyInfo2.Name);
                            });
            List<PropertyInfo> propertiesList = new List<PropertyInfo>(properties);

            foreach (PropertyInfo propInfo in properties)
            {
                string propName = propInfo.Name;
                if (propName.StartsWith("Tip")
                    && propInfo.PropertyType == typeof(string))
                {
                    string message = (string)propInfo.GetValue(null, null);
                    Image image = null;

                    // Search of the corresponding image
                    int underscoreIndex = propName.IndexOf('_');
                    if (underscoreIndex >= 0)
                    {
                        string tipCode = propName.Substring(0, underscoreIndex);
                        PropertyInfo propInfoImage = propertiesList.Find(new Predicate<PropertyInfo>(
                            delegate(PropertyInfo target)
                            {
                                return target.Name.StartsWith(tipCode) && typeof(Image).IsAssignableFrom(target.PropertyType);
                            }));
                        if (propInfoImage != null)
                            image = (Image)propInfoImage.GetValue(null, null);
                    }

                    tipList.Add(new ApplicationTip()
                    {
                        Code = propName,
                        Message = message,
                        TipImage = image
                    });
                }
            }
            applicationTipBindingSource.DataSource = tipList;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (HideTipsWanted != null)
                HideTipsWanted(this, EventArgs.Empty);
        }
    }

    public class ApplicationTip
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public Image TipImage { get; set; }
    }
}
