using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.Interfaces.DrawEngines
{
    public interface IActionExecutor
    {
        void ExecuteTreeFullRefresh(IDirectoryNode node);

        void ExecuteTreeFillUpToLevel(IDirectoryNode node, int targetLevel);

        void NavigateForward();

        void NavigateBackward();

        /// <summary>
        /// Notify the main windows that the cursor's position has changed and a new new node is hovered.
        /// </summary>
        /// <param name="node"></param>
        void Notify4NewHoveredNode(IDirectoryNode node);

        /// <summary>
        /// Notify the main windows that the root node of the graph has changed.
        /// </summary>
        /// <param name="node"></param>
        void Notify4NewRootNode(IDirectoryNode node);

        /// <summary>
        /// Open a new window showing in a grid the details of the given node.
        /// </summary>
        /// <param name="node"></param>
        void ShowNodeDetails(IDirectoryNode node);

    }
}
