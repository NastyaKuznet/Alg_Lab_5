using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.M
{
    public class Drawer
    {
        public void DrawEllipsWithName(double width, double height, SolidColorBrush colorFill, SolidColorBrush colorStroke, double posX, double posY, Canvas canvas, string name)
        {
            Ellipse node = new Ellipse();
            node.Width = width;
            node.Height = height;
            node.Fill = colorFill;
            node.Stroke = colorStroke;
            Canvas.SetBottom(node, (-1) * posY - height / 2);
            Canvas.SetLeft(node, posX - width / 2);
            canvas.Children.Add(node);
            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = ColorForeGroundTextGraph;
            textBlock.Text = name;
            textBlock.FontSize = 12;
            Canvas.SetBottom(textBlock, (-1) * posY - 4);
            Canvas.SetLeft(textBlock, posX - 4);
            canvas.Children.Add(textBlock);
        }

        public bool CanDrawEllipsWithoutOverlay(double posX, double posY)
        {
            return false; 
        }
    }
}
