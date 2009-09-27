using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.DrawEngines;
using System.Windows.Forms;

namespace HDGraph.DrawEngine
{
    public class SimpleDrawEngine : IDrawEngine
    {
        #region IDrawEngine Members

        public Control GenerateControlFromNode(HDGraph.Interfaces.ScanEngines.IDirectoryNode node, DrawOptions options, IActionExecutor actionExecutor)
        {
            // 
            // treeGraph1
            // 
            TreeGraph treeGraph1 = new TreeGraph(actionExecutor);
            treeGraph1.BackColor = System.Drawing.Color.White;
            treeGraph1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            treeGraph1.DrawOptions = options;
            treeGraph1.DrawType = HDGraph.DrawEngine.DrawType.Circular;
            treeGraph1.MinimumSize = new System.Drawing.Size(50, 50);
            treeGraph1.Name = "treeGraph1";
            treeGraph1.Root = node;
            return treeGraph1;
        }

        #endregion

        #region IDrawEngine Members

        public HDGraph.Interfaces.ScanEngines.IDirectoryNode GetRootNodeOfControl(Control control)
        {
            if (control == null
               || !(control is TreeGraph))
                throw new ArgumentException("The given control is null or is not a TreeGraph.", "control");

            return ((TreeGraph)control).Root;
        }

        public void SetRootNodeOfControl(Control control, HDGraph.Interfaces.ScanEngines.IDirectoryNode newRoot)
        {
            if (control == null
               || !(control is TreeGraph))
                throw new ArgumentException("The given control is null or is not a TreeGraph.", "control");

            ((TreeGraph)control).Root = newRoot;
        }

        #endregion

        #region IDrawEngine Members


        public void SaveAsImageToFile(Control control, string filePath)
        {
            if (control == null
               || !(control is TreeGraph))
                throw new ArgumentException("The given control is null or is not a TreeGraph.", "control");

            ((TreeGraph)control).ImageBuffer.Save(filePath);
        }

        #endregion
    }
}
