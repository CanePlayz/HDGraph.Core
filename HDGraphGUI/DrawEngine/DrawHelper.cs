using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HDGraph.DrawEngine
{
    public static class DrawHelper
    {
        public static void PrintTextInTheMiddle(Graphics graph, Size graphSize, string text, Font font, Brush brush, bool encadrer)
        {
            float x = graphSize.Width / 2f;
            float y = graphSize.Height / 2f;

            SizeF sizeTextName = graph.MeasureString(text, font, graphSize.Width);
            x -= sizeTextName.Width / 2f;
            y -= sizeTextName.Height / 2f;

            int padding = 5; // 5 pixels
            // background Rectangle
            Rectangle rectangle = new Rectangle(Convert.ToInt32(x) - padding,
                              Convert.ToInt32(y) - padding,
                              Convert.ToInt32(sizeTextName.Width) + 2 * padding,
                              Convert.ToInt32(sizeTextName.Height) + 2 * padding);
            if (encadrer)
            {
                // Create a new pen.
                Pen pen = new Pen(Brushes.Gray);

                // Set the pen's width.
                pen.Width = 8;

                // Set the LineJoin property.
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;


                // Draw a rectangle.
                graph.FillRectangle(new SolidBrush(Color.FromArgb(150, 255, 255, 255)), rectangle);
                graph.DrawRectangle(pen, rectangle);

                //Dispose of the pen.
                pen.Dispose();

            }
            rectangle.Inflate(-padding + 1, -padding + 1);
            graph.DrawString(text, font, brush, rectangle);
        }

    }
}
