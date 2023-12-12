using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.M.Algorithms
{
    public class Dextra
    {
        // -1 = infinity
        //рабочие элементы
        Graph graph;
        Dictionary<NodeGraph, ItemDextra> nodes = new Dictionary<NodeGraph, ItemDextra>();
        NodeGraph startNode;
        NodeGraph endNode;
        ItemDextra currentNode;
        int numberStep = 1;
        bool allNodes;

        //инструменты
        Drawer drawer = new Drawer();

        //результаты
        public int ResultWeight;
        public bool HasResult;
        public List<Canvas> Steps = new List<Canvas>();
        public List<string> Comments = new List<string>();
        public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();

        public Dextra(Graph graph, NodeGraph startNode, NodeGraph endNode, bool allNodes)
        {
            this.graph = graph;
            foreach (NodeGraph node in graph.NodeGraphs)
            {
                if (node.Equals(startNode))
                {
                    nodes.Add(node, new ItemDextra { Node = node, WeightNode = 0, IsPickOut = true });
                }
                else
                {
                    nodes.Add(node, new ItemDextra { Node = node, WeightNode = -1, IsPickOut = false });
                }
            }
            this.startNode = startNode;
            this.endNode = endNode;
            this.allNodes = allNodes;
            currentNode = nodes[startNode];
            Canvas newCanvas = new Canvas();
            DrawGraph(newCanvas);
            Steps.Add(newCanvas);
            Comments.Add($"Всем вершинам за исключением первой присвается вес равный бесконечности, а первой вершине 0. Первая вершина объявляется текущей: currentNode = {currentNode.Node.Name}.");
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
        }

        public void DoDextra()
        {
            if (currentNode.Node.Equals(endNode))
            {
                ReturnIfFinal();
                return;
            }
            if (allNodes && IsAllVisited())
            {
                RerurnAll();
                return;
            }
            SetStartWeight();
            AddCommentType1();

            int min = int.MaxValue;
            NodeGraph minNode = null;

            StringBuilder commentStep = new StringBuilder();
            commentStep.Append("Среди невыделенных вершин ищется вершина с минимальным весом. Если таковая не найдена, то есть вес всех вершин равен бесконечности, то маршрут не существует.");

            if (IsAllInfinity())
            {
                HasResult = false;
                commentStep.Append("Маршрут не найдет. Вес всех вершин равен бесконечности.");
                return;
            }
            foreach (NodeGraph node in nodes.Keys)
            {
                if (nodes[node].WeightNode < min && nodes[node].WeightNode != -1 && !nodes[node].IsPickOut)
                {
                    min = nodes[node].WeightNode;
                    minNode = node;
                }
            }

            nodes[minNode].IsPickOut = true;
            currentNode = nodes[minNode];


            commentStep.Append($"Иначе, текущей вершиной становится найденная вершина. CurrentNode = {currentNode.Node.Name}. Она же выделяется.");
            Comments.Add(commentStep.ToString());
            AddCommentType2();

            DoDextra();
        }

        public void ReturnIfFinal()
        {
            ResultWeight = currentNode.WeightNode;
            HasResult = true;
            Comments.Add($"Если текущей вершиной оказывается конечная, то путь найден, и его вес есть вес конечной вершины. Вес минимального пути от вершины {startNode.Name} до {endNode.Name} равен {ResultWeight}.");
            Canvas newCanvas3 = new Canvas();
            DrawGraph(newCanvas3);
            Steps.Add(newCanvas3);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
        }

        public void RerurnAll()
        {
            HasResult = true;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Если все вершины выделены, то работа алгоритма окончена. Вес минимальных путей от вершины {startNode.Name} до остальных вершин:\n");
            foreach (ItemDextra item in nodes.Values)
            {
                stringBuilder.Append($"{item.Node.Name} : {item.WeightNode}\n");
            }
            if (currentNode.WeightNode == 0)
            {
                stringBuilder.Append("Алгоритм Дейкстры работает с графом, который имеет взвешенные ребра.");
            }
            Comments.Add(stringBuilder.ToString());
            Canvas newCanvas3 = new Canvas();
            DrawGraph(newCanvas3);
            Steps.Add(newCanvas3);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
        }
        public void SetStartWeight()
        {
            foreach (Edge edge in currentNode.Node.Edges)
            {
                ItemDextra workItemDextra = nodes[FindNeededNodeForEdge(edge, currentNode.Node)];
                if (workItemDextra.IsPickOut) continue;
                workItemDextra.WeightNode = MinWeight(workItemDextra.WeightNode, currentNode.WeightNode + edge.Weight);
            }
        }
        public void AddCommentType1()
        {
            Comments.Add("Вес невыделенных вершин расчитывается по формуле: вес невыделенной вершины есть минимальное число из старого веса данной вершнины, суммы веса текущей вершины и веса ребра, соединяющего текущую вершину с невыделенной.");
            Canvas newCanvas = new Canvas();
            DrawGraph(newCanvas);
            Steps.Add(newCanvas);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
        }
        public void AddCommentType2()
        {
            Canvas newCanvas2 = new Canvas();
            DrawGraph(newCanvas2);
            Steps.Add(newCanvas2);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
        }

        private bool IsAllVisited()
        {
            foreach(ItemDextra itemDextra in nodes.Values)
            {
                if(!itemDextra.IsPickOut) return false;
            }
            return true;
        }

        private bool IsAllInfinity()
        {
            foreach (ItemDextra itemDextra in nodes.Values)
            {
                if (itemDextra.WeightNode != -1)
                    return false;
            }
            return true;
        }

        private NodeGraph FindNeededNodeForEdge(Edge edge, NodeGraph node)
        {
            NodeGraph workNode = null;
            NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
            NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
            if (node1.Equals(currentNode.Node))
                workNode = node2;
            else if (node2.Equals(currentNode.Node))
                workNode = node1;
            return workNode;
        }

        private int MinWeight(int oldWeight, int sum)
        {
            if (oldWeight == -1)
                return sum;
            return Math.Min(oldWeight, sum);
        }

        private void DrawGraph(Canvas currentCanvas)
        {
            foreach(ItemDextra itemDextra in nodes.Values)
            {
                foreach(Edge edge in itemDextra.Node.Edges)
                {
                    if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Base))
                    {
                        drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1);
                    }
                    else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Base))
                    {
                        drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                    }
                    else if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Directed))
                    {
                        drawer.DrawDirectedLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1);
                    }
                    else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Directed))
                    {
                        drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                    }

                    NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                    NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);

                    DrawNode(currentCanvas, nodes[node1]);
                    DrawNode(currentCanvas, nodes[node2]);
                    //Drawll(currentCanvas, nodes[node1]);
                    //Drawll(currentCanvas, nodes[node2]);

                }
            }
        }

        private void DrawNode(Canvas currentCanvas, ItemDextra itemDextra)
        {
            if (itemDextra.WeightNode == -1)
            {
                if(itemDextra.IsPickOut)
                    drawer.DrawEllipsWithNameWithWeight(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, itemDextra.Node.PosX, itemDextra.Node.PosY, currentCanvas, itemDextra.Node.Name, "INF");
                else
                    drawer.DrawEllipsWithNameWithWeight(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, itemDextra.Node.PosX, itemDextra.Node.PosY, currentCanvas, itemDextra.Node.Name, "INF");
            }
            else
            {
                if(itemDextra.IsPickOut)
                    drawer.DrawEllipsWithNameWithWeight(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, itemDextra.Node.PosX, itemDextra.Node.PosY, currentCanvas, itemDextra.Node.Name, itemDextra.WeightNode.ToString());
                else
                    drawer.DrawEllipsWithNameWithWeight(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, itemDextra.Node.PosX, itemDextra.Node.PosY, currentCanvas, itemDextra.Node.Name, itemDextra.WeightNode.ToString());
            }
        }

        private void Drawll(Canvas currentCanvas, ItemDextra itemDextra)
        {
            List<int> keys = new List<int>();
            foreach(NodeGraph node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    foreach(Edge edge1 in itemDextra.Node.Edges)
                    {
                        //ItemDextra tempItemD = new ItemDextra();
                        //if (!nodes.TryGetValue(node, out tempItemD))
                        //{
                        //    if(edge.Id == edge1.Id && tempItemD.IsPickOut)
                        //    {
                        //        keys.Add(edge1.Id);
                        //    }
                        //}

                        if (nodes.ContainsKey(node))
                        {
                            ItemDextra tempItemD = nodes[node];
                            if (edge.Id == edge1.Id && tempItemD.IsPickOut)
                            {
                                keys.Add(edge1.Id);
                            }
                        }
                         
                    }
                }
            }
            int temp = 1;
            foreach (Edge edge in itemDextra.Node.Edges)
            {
                if(keys.Contains(edge.Id))
                {
                    temp = 2;
                }
                if (keys.Contains(edge.Id))
                {
                    temp = 1;
                }
                if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Base))
                {
                    drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, temp);
                }
                else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Base))
                {
                    drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, temp, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                }
                else if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Directed))
                {
                    drawer.DrawDirectedLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, temp);
                }
                else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Directed))
                {
                    drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, temp, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                }
            }
        }
    }
}
