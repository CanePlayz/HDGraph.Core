using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

namespace HDGraphiqueurGUI
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
            CultureInfo newLang = (CultureInfo)comboBoxLanguage.SelectedItem;
            System.Threading.Thread.CurrentThread.CurrentUICulture = newLang;
            HDGraphiqueurGUI.Properties.Settings.Default.Language = newLang.Name;
            HDGraphiqueurGUI.Properties.Settings.Default.Save();
            UpdateApplyBtnStatus();
            MessageBox.Show(resManager.GetString("languageApplied"),
                            resManager.GetString("languageAppliedTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void LanguageForm_Load(object sender, EventArgs e)
        {
            //BringToFront();
        }

        private void LoadLanguagesCombo()
        {
            List<CultureInfo> cultureList = new List<CultureInfo>();
            foreach (CultureInfo culture in CultureInfo.GetCultures(System.Globalization.CultureTypes.FrameworkCultures))
            {
                if (resManager.GetString("languageId", culture) == culture.Name)
                    cultureList.Add(culture);
            }
            comboBoxLanguage.DataSource = cultureList;
            comboBoxLanguage.DisplayMember = "NativeName";

            comboBoxLanguage.SelectedItem = CultureInfo.CurrentUICulture;
        }

        private void comboBoxLanguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateApplyBtnStatus();
        }

        private void UpdateApplyBtnStatus()
        {
            bool canApply = (comboBoxLanguage.SelectedItem is CultureInfo)
                            &&
                            (!(System.Threading.Thread.CurrentThread.CurrentUICulture.Equals((CultureInfo)comboBoxLanguage.SelectedItem))
                            );
            buttonApply.Enabled = canApply;
        }

        private void LanguageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //resManager.ReleaseAllResources();
        }
    }
}