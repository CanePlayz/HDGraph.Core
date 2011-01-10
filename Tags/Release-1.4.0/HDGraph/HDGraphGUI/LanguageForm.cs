using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Reflection;

namespace HDGraph
{
    public partial class LanguageForm : Form
    {
        private static bool isLoaded = false;

        public static bool IsLoaded
        {
            get { return LanguageForm.isLoaded; }
            set { LanguageForm.isLoaded = value; }
        }


        private ResourceManager resManager;

        public ResourceManager ApplicationResourceManager
        {
            get { return resManager; }
            set { resManager = value; }
        }


        public LanguageForm()
        {
            InitializeComponent();
            LoadData();
        }

        public LanguageForm(ResourceManager applicationResManager)
        {
            this.resManager = applicationResManager;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            this.labelCurrentLanguage.Text = System.Threading.Thread.CurrentThread.CurrentUICulture.NativeName;
            LoadLanguagesCombo();
            UpdateApplyBtnStatus();
            WaitForm.HideWaitForm();
            isLoaded = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplyNewLangage();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (buttonApply.Enabled)
                ApplyNewLangage();
            this.Close();
        }

        private void ApplyNewLangage()
        {
            if (MessageBox.Show(Resources.ApplicationMessages.AppRestartRequiredToApplyLangage,
                                "HDGraph", MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question
                                ) == DialogResult.Yes)
            {
                CultureInfo newLang = ((CultureInfoWrapper)comboBoxLanguage.SelectedItem).Culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = newLang;
                //HDGTools.ApplyCulture(this, newLang);
                HDGraph.Properties.Settings.Default.Language = newLang.Name;
                HDGraph.Properties.Settings.Default.Save();
                UpdateApplyBtnStatus();
                this.labelCurrentLanguage.Text = System.Threading.Thread.CurrentThread.CurrentUICulture.NativeName;
                buttonCancel.DialogResult = DialogResult.OK;
                Application.Restart();
            }
        }

        private void LanguageForm_Load(object sender, EventArgs e)
        {
            //BringToFront();
        }

        private void LoadLanguagesCombo()
        {
            List<CultureInfoWrapper> cultureList = new List<CultureInfoWrapper>();
            foreach (string cultureId in Properties.Settings.Default.AvailableLanguages)
            {
                CultureInfo culture = CultureInfo.GetCultureInfo(cultureId);
                cultureList.Add(new CultureInfoWrapper(culture));
            }
            //foreach (CultureInfo culture in CultureInfo.GetCultures(System.Globalization.CultureTypes.FrameworkCultures))
            //{
            //    if (resManager.GetString("languageId", culture) == culture.Name)
            //        cultureList.Add(new CultureInfoWrapper(culture));
            //}
            comboBoxLanguage.DataSource = cultureList;
            comboBoxLanguage.SelectedItem = new CultureInfoWrapper(CultureInfo.CurrentUICulture);
        }

        private void comboBoxLanguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateApplyBtnStatus();
        }

        private void UpdateApplyBtnStatus()
        {
            bool canApply = (comboBoxLanguage.SelectedItem is CultureInfoWrapper);
            if (canApply)
            {
                CultureInfo cultureSelectionnee = ((CultureInfoWrapper)comboBoxLanguage.SelectedItem).Culture;
                canApply = !(Thread.CurrentThread.CurrentUICulture.Equals(cultureSelectionnee));
            }
            buttonApply.Enabled = canApply;
        }

        private void LanguageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //resManager.ReleaseAllResources();
        }

        class CultureInfoWrapper
        {
            private CultureInfo culture;

            public CultureInfo Culture
            {
                get { return culture; }
                set { culture = value; }
            }

            public CultureInfoWrapper(CultureInfo culture)
            {
                this.culture = culture;
            }

            public override string ToString()
            {
                return culture.NativeName.Substring(0, 1).ToUpper() + culture.NativeName.Substring(1);
            }
        }
    
    }
}