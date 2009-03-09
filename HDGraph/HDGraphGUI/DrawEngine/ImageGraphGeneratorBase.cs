using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HDGraph.DrawEngine
{
    public abstract class ImageGraphGeneratorBase
    {
        public abstract BiResult<Bitmap, DrawOptions> Draw(bool drawImage, bool drawText, DrawOptions options);

        public abstract DirectoryNode FindNodeByCursorPosition(Point curseurPos);
    }
}
