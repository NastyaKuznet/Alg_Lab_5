using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        public bool CanDrawEllipsWithoutOverlay(LinkedList<NodeGraph> nodesGraph, double posX, double posY)
        {
            foreach(NodeGraph node in nodesGraph) 
            {
                double radius = Math.Sqrt(Math.Pow(posX - node.PosX, 2) + Math.Pow(posY - node.PosY,2));
                if(radius < SizeNodeGraph)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanDrawNearEdge(double posX, double posY)
        {
            return posX > SizeNodeGraph / 2 && posY > SizeNodeGraph / 2;
        }

        public void DrawBaseLine(double firstPosX, double firstPosY, double secondPosX, double secondPosY, Canvas canvas)
        {
            Line edge = new Line
            {
                X1 = firstPosX,
                Y1 = firstPosY,
                X2 = secondPosX,
                Y2 = secondPosY,
                Stroke = ColorForeGroundTextGraph
            };
            canvas.Children.Add(edge);
        }

        public NodeGraph FindNodeInTouch(LinkedList<NodeGraph> nodesGraph, double posX, double posY)
        {
            foreach(NodeGraph node in nodesGraph)
            {
                double radius = Math.Sqrt(Math.Pow(posX - node.PosX, 2) + Math.Pow(posY - node.PosY, 2));
                if (radius < SizeNodeGraph)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
