using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.M
{
    public class Drawer
    {
        public void DrawEllipsWithName(double width, double height, SolidColorBrush colorFill, SolidColorBrush colorStroke, double posX, double posY, Canvas canvas, string name, SolidColorBrush colorText = null)
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
            if (colorText != null) { textBlock.Foreground = colorText; }
            textBlock.FontSize = 12;
            Canvas.SetBottom(textBlock, (-1) * posY - 4);
            Canvas.SetLeft(textBlock, posX - 4);
            canvas.Children.Add(textBlock);
        }

        public void DrawEllipsWithNameWithWeight(double width, double height, SolidColorBrush colorFill, SolidColorBrush colorStroke, double posX, double posY, Canvas canvas, string name, string weitght)
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

            TextBlock weightNode = new TextBlock { Foreground = ColorWeightNode, Text = weitght, FontSize = 12 };
            Canvas.SetBottom(weightNode, (-1) * posY + 5);
            Canvas.SetLeft(weightNode, posX - 4);
            canvas.Children.Add(weightNode);
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

        public void DrawCircleLine(double posX, double posY, Canvas canvas, SolidColorBrush colorStroke, double size)
        {
            Ellipse ellipse = new Ellipse { Width = SizeNodeGraph / 2, Height = SizeNodeGraph / 2, Stroke = colorStroke, StrokeThickness = size };
            Canvas.SetBottom(ellipse, (-1) * posY + SizeNodeGraph / 4);
            Canvas.SetLeft(ellipse, posX - SizeNodeGraph / 2);
            canvas.Children.Add(ellipse);
        }


        public void DrawCircleLineWeight(double posX, double posY, Canvas canvas, SolidColorBrush colorStroke, double size, double weight, SolidColorBrush recColorStroke, SolidColorBrush recColorFill)
        {
            Ellipse ellipse = new Ellipse { Width = SizeNodeGraph / 2, Height = SizeNodeGraph / 2, Stroke = colorStroke, StrokeThickness = size };
            Canvas.SetBottom(ellipse, (-1) * posY + SizeNodeGraph / 4);
            Canvas.SetLeft(ellipse, posX - SizeNodeGraph / 2);
            canvas.Children.Add(ellipse);

            Rectangle rectangle = new Rectangle { Width = weight.ToString().Length * 10, Height = 15, Stroke = recColorStroke, Fill = recColorFill };
            Canvas.SetBottom(rectangle, (-1) * posY + SizeNodeGraph / 2);
            Canvas.SetLeft(rectangle, posX - SizeNodeGraph / 4);
            canvas.Children.Add(rectangle);

            if (weight != -1)
            {
                TextBlock weightNode = new TextBlock { Foreground = ColorForeGroundTextGraph, Text = weight.ToString(), FontSize = 12 };
                Canvas.SetBottom(weightNode, (-1) * posY + SizeNodeGraph / 2);
                Canvas.SetLeft(weightNode, posX - SizeNodeGraph / 4);
                canvas.Children.Add(weightNode);
            }
        }

        public void DrawBaseLine(double firstPosX, double firstPosY, double secondPosX, double secondPosY, Canvas canvas, SolidColorBrush colorStroke, double size)
        {
            if (firstPosX == secondPosX && firstPosY == secondPosY)
            {
                DrawCircleLine(firstPosX, secondPosY, canvas, colorStroke, size);
            }
            else
            {
                Line edge = new Line
                {
                    X1 = firstPosX,
                    Y1 = firstPosY,
                    X2 = secondPosX,
                    Y2 = secondPosY,
                    StrokeThickness = size,
                    Stroke = colorStroke
                };
                canvas.Children.Add(edge);
            }
        }

        public void DrawBaseLineWeight(double firstPosX, double firstPosY, double secondPosX, double secondPosY, Canvas canvas, SolidColorBrush colorStroke, double size, int weight, SolidColorBrush recColorStroke, SolidColorBrush recColorFill)
        {
            if (firstPosX == secondPosX && firstPosY == secondPosY)
            {
                DrawCircleLineWeight(firstPosX, secondPosY, canvas, colorStroke, size, weight, recColorStroke, recColorFill);
            }
            else
            {
                Line edge = new Line
                {
                    X1 = firstPosX,
                    Y1 = firstPosY,
                    X2 = secondPosX,
                    Y2 = secondPosY,
                    StrokeThickness = size,
                    Stroke = colorStroke
                };
                canvas.Children.Add(edge);
                Rectangle rectangle = new Rectangle { Width = weight.ToString().Length * 10, Height = 15, Stroke = recColorStroke, Fill = recColorFill };
                Canvas.SetBottom(rectangle, (-1) * (firstPosY - (firstPosY - secondPosY) / 2) - 5);
                Canvas.SetLeft(rectangle, firstPosX - (firstPosX - secondPosX) / 2);
                canvas.Children.Add(rectangle);
                if (weight != -1)
                {
                    TextBlock textBlock = new TextBlock { Foreground = colorStroke, Text = weight.ToString(), FontSize = 10 };
                    Canvas.SetBottom(textBlock, (-1) * (firstPosY - (firstPosY - secondPosY) / 2 + 2));
                    Canvas.SetLeft(textBlock, firstPosX - (firstPosX - secondPosX) / 2 + 2);
                    canvas.Children.Add(textBlock);
                }
            }
        }

        public void DrawDirectedLine(double firstPosX, double firstPosY, double secondPosX, double secondPosY, Canvas canvas, SolidColorBrush colorStroke, double size)
        {

            if (firstPosX == secondPosX && firstPosY == secondPosY)
            {
                DrawCircleLine(firstPosX, secondPosY, canvas, colorStroke, size);
            }
            else
            {
                ArrowLine aline1 = new ArrowLine();
                aline1.Stroke = colorStroke;
                aline1.StrokeThickness = size;
                aline1.X1 = firstPosX;
                aline1.Y1 = firstPosY;

                double dy = secondPosY - firstPosY;
                double dx = secondPosX - firstPosX;
                double r = SizeNodeGraph / 2;
                double l = Math.Sqrt(dx * dx + dy * dy);
                dx /= l;
                dy /= l;

                aline1.X2 = secondPosX - dx * r;
                aline1.Y2 = secondPosY - dy * r;
                canvas.Children.Add(aline1);
            }
        }

        public void DrawDirectedLineWeight(double firstPosX, double firstPosY, double secondPosX, double secondPosY, Canvas canvas, SolidColorBrush colorStroke, double size, int weight, SolidColorBrush recColorStroke, SolidColorBrush recColorFill)
        {
            if (firstPosX == secondPosX && firstPosY == secondPosY)
            {
                DrawCircleLineWeight(firstPosX, secondPosY, canvas, colorStroke, size, weight, recColorStroke, recColorFill);
            }
            else
            {
                ArrowLine aline1 = new ArrowLine();
                aline1.Stroke = colorStroke;
                aline1.StrokeThickness = size;
                aline1.X1 = firstPosX;
                aline1.Y1 = firstPosY;

                double dy = secondPosY - firstPosY;
                double dx = secondPosX - firstPosX;
                double r = SizeNodeGraph / 2;
                double l = Math.Sqrt(dx * dx + dy * dy);
                dx /= l;
                dy /= l;

                aline1.X2 = secondPosX - dx * r;
                aline1.Y2 = secondPosY - dy * r;
                canvas.Children.Add(aline1);
                Rectangle rectangle = new Rectangle { Width = weight.ToString().Length * 10, Height = 15, Stroke = recColorStroke, Fill = recColorFill };
                Canvas.SetBottom(rectangle, (-1) * (firstPosY - (firstPosY - secondPosY) / 2) - 5);
                Canvas.SetLeft(rectangle, firstPosX - (firstPosX - secondPosX) / 2);
                canvas.Children.Add(rectangle);
                if (weight != -1)
                {
                    TextBlock textBlock = new TextBlock { Foreground = colorStroke, Text = weight.ToString(), FontSize = 10 };
                    Canvas.SetBottom(textBlock, (-1) * (firstPosY - (firstPosY - secondPosY) / 2 + 2));
                    Canvas.SetLeft(textBlock, firstPosX - (firstPosX - secondPosX) / 2 + 2);
                    canvas.Children.Add(textBlock);
                }
            }
        }

        public void DrawDirectedLineWeight(double firstPosX, double firstPosY, double secondPosX, double secondPosY, Canvas canvas, SolidColorBrush colorStroke, double size, string weight, SolidColorBrush recColorStroke, SolidColorBrush recColorFill)
        {
            //if (firstPosX == secondPosX && firstPosY == secondPosY)
            //{
            //    DrawCircleLineWeight(firstPosX, secondPosY, canvas, colorStroke, size, weight, recColorStroke, recColorFill);
            //}
            //else
            //{
                ArrowLine aline1 = new ArrowLine();
                aline1.Stroke = colorStroke;
                aline1.StrokeThickness = size;
                aline1.X1 = firstPosX;
                aline1.Y1 = firstPosY;

                double dy = secondPosY - firstPosY;
                double dx = secondPosX - firstPosX;
                double r = SizeNodeGraph / 2;
                double l = Math.Sqrt(dx * dx + dy * dy);
                dx /= l;
                dy /= l;

                aline1.X2 = secondPosX - dx * r;
                aline1.Y2 = secondPosY - dy * r;
                canvas.Children.Add(aline1);
                Rectangle rectangle = new Rectangle { Width = weight.ToString().Length * 10, Height = 15, Stroke = recColorStroke, Fill = recColorFill };
                Canvas.SetBottom(rectangle, (-1) * (firstPosY - (firstPosY - secondPosY) / 2) - 5);
                Canvas.SetLeft(rectangle, firstPosX - (firstPosX - secondPosX) / 2);
                canvas.Children.Add(rectangle);
            TextBlock textBlock = new TextBlock { Foreground = ColorForeGroundTextGraph, Text = weight.ToString(), FontSize = 10 };
            Canvas.SetBottom(textBlock, (-1) * (firstPosY - (firstPosY - secondPosY) / 2 + 2));
            Canvas.SetLeft(textBlock, firstPosX - (firstPosX - secondPosX) / 2 + 2);
            canvas.Children.Add(textBlock);
            //if (weight != -1)
            //{
            //    TextBlock textBlock = new TextBlock { Foreground = colorStroke, Text = weight.ToString(), FontSize = 10 };
            //    Canvas.SetBottom(textBlock, (-1) * (firstPosY - (firstPosY - secondPosY) / 2 + 2));
            //    Canvas.SetLeft(textBlock, firstPosX - (firstPosX - secondPosX) / 2 + 2);
            //    canvas.Children.Add(textBlock);
            //}
            //}
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
