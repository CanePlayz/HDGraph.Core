using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HDGraph.WpfDrawEngine
{
    public class GraphCommands
    {
        public static RoutedUICommand ArcCommand = new RoutedUICommand();

        public const string ActionOpenExternal = "openExternal";
        public const string ActionShowDetails = "showDetails";
        public const string ActionRefresh = "refresh";
        public const string ActionCenterOnDir = "centerOnDir";
        public const string ActionCenterOnParent = "centerOnParent";
        public const string ActionCanDelete = "canDelete";
        public const string ActionDelete = "delete";

    }
}
