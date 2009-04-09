using System;
using System.Collections.Generic;
using System.Text;

namespace HDGraph.Interop.Unknown
{
    public class UnknownEnvToolProvider : ToolProviderBase
    {
        public override List<PathWithIcon> ListFavoritPath()
        {
            List<PathWithIcon> res = new List<PathWithIcon>();
            res.Add(new PathWithIcon()
            {
                Name = "Home", // TODO : localize.
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Icon = this.GetFolderIcon(IconSize.Small, FolderType.Open)
            });
            return res;
        }

        public override System.Drawing.Icon GetFileIcon(string name, IconSize size, bool linkOverlay)
        {
            return System.Windows.Forms.Application.OpenForms[0].Icon; // TODO
        }

        public override System.Drawing.Icon GetFolderIcon(IconSize size, FolderType folderType)
        {
            return System.Windows.Forms.Application.OpenForms[0].Icon; // TODO
        }
    }
}
