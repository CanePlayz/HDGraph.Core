using System;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.DrawEngine
{
    public enum DrawType
    {
        Circular,
        Rectangular,
    }

    public class ImageGraphGeneratorFactory
    {
        public static ImageGraphGeneratorBase CreateGenerator(DrawType type, IDirectoryNode node)
        {
            switch (type)
            {
                case DrawType.Circular:
                    return new CircularImageGraphGenerator(node);
                case DrawType.Rectangular:
                    return new RectangularImageGraphGenerator(node);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
