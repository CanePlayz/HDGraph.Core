using HDGraph.Interfaces.ScanEngines;
using System.Windows.Forms;

namespace HDGraph.Interfaces.DrawEngines
{

    public interface IDrawEngine
    {
        Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options, IActionExecutor actionExecutor);

        IDirectoryNode GetRootNodeOfControl(Control control);
        void SetRootNodeOfControl(Control control, IDirectoryNode newRoot);

        void SaveAsImageToFile(System.Windows.Forms.Control control, string filePath);
        void Print(System.Windows.Forms.Control control);
        void PrintWithPreview(System.Windows.Forms.Control control);
        /// <summary>
        /// Redraw the whole graphic
        /// </summary>
        void UpdateVisual(System.Windows.Forms.Control control);

        #region Options management

        Control GetOptionsControl(DrawOptions options);
        void SaveCurrentSpecificOptions();

        #endregion


    }
}
