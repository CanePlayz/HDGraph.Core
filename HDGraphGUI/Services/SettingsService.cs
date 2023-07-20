using System;
using System.IO;
using Cogwheel;

namespace HDGraph.Services
{
    public class SettingsService : SettingsBase
    {
        public string Language { get; set; }

        public string PathHistory { get; set; }

        public decimal OptionCalculMaxDepth { get; set; } = 5;

        public decimal OptionDrawMaxDepth { get; set; } = 5;

        public bool OptionPrintSize { get; set; } = true;

        public bool OptionAutoCompleteGraph { get; set; } = true;

        public bool OptionAllowFolderDeletion { get; set; } = false;

        public bool OptionDeletionAsk4Confirmation { get; set; } = true;

        public string OptionColorStyle { get; set; } = "Linear";

        public string OptionMainWindowOpenState { get; set; } = "Normal";

        public string OptionMainWindowSize { get; set; } = "762, 592";

        public string OptionToolbarLocation { get; set; } = "506, 24";

        public bool OptionShowFreeSpace { get; set; } = false;

        public bool OptionShowSizesInHumanForm { get; set; } = true;

        public bool OptionShowTooltips { get; set; } = true;

        public int OptionTextDensity { get; set; } = 10;

        public bool OptionUseSimpleScanEngine { get; set; } = false;

        public string MyDrawOptions { get; set; }

        public Guid StartupDrawEngine { get; set; } = new Guid("9053b2b2-e6e3-4ee7-bdac-fcca15c9e3be");

        public string[] AvailableLanguages { get; set; } = new string[] { "en-US", "fr-FR" };

        public bool OptionCheckForNewVersionAfterStartup { get; set; } = true;

        public bool IsFirstRun { get; set; } = true;

        public bool OptionIgnoreReparsePoints { get; set; } = true;

        public SettingsService()
            : base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json"))
        {
        }
    }
}
