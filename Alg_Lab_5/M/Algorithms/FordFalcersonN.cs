using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.M.Algorithms
{
    public class FordFalcersonN
    {
        //рабочие элементы
        Graph graph;
        NodeGraph source;
        NodeGraph stock;
        int pastFlow = 0;
        int flow = 0;
        Dictionary<int, ItemEdgeFord> itemsEdge = new Dictionary<int, ItemEdgeFord>();
        Dictionary<NodeGraph, ItemNodeFord> itemsNode = new Dictionary<NodeGraph, ItemNodeFord>();
        Stack<ItemNodeFord> stackNode = new Stack<ItemNodeFord>();
        Stack<ItemEdgeFord> stackEdge = new Stack<ItemEdgeFord>();
        List<int> results = new List<int>();
        int numberStep = 0;

        //инструменты
        Drawer drawer = new Drawer();

        //результат
        public List<bool> hasResult = new List<bool>();
        public int Result;
        public List<Canvas> Steps = new List<Canvas>();
        public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();
        public List<string> Comments = new List<string>();

        public FordFalcersonN(Graph graph)
        {
            this.graph = graph;
            hasResult.Add(FindSource());
            hasResult.Add(FindStock());
            AddItems();
            DoFordFalcerson(source);
            Result = CheckSum();

            CommentFinal();
        }

        public void DoFordFalcerson(NodeGraph currentNode)
        {
            if(currentNode.Equals(stock))
            {
                SetFlow();

                CommentSetFlow();

                return;
            }
            DesignationsElements(currentNode);
        }

        private bool FindSource()
        {
            bool isThat = true;
            foreach(NodeGraph node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                    if (node2.Equals(node))
                    {
                        isThat = false;
                    }
                }
                if(isThat)
                {
                    source = node;
                    return true;
                }
                isThat = true;
            }
            return false;
        }

        private bool FindStock()
        {
            bool isThat = true;
            foreach (NodeGraph node in graph.NodeGraphs)
            {
                foreach (Edge edge in node.Edges)
                {
                    NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                    if (node1.Equals(node))
                    {
                        isThat = false;
                        break;
                    }
                }
                if (isThat)
                {
                    stock = node;
                    return true;
                }
                isThat = true;
            }
            return false;
        }

        private void AddItems()
        {
            foreach(NodeGraph node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    if(!itemsEdge.ContainsKey(edge.Id))
                        itemsEdge.Add(edge.Id, new ItemEdgeFord(edge));
                }
                itemsNode.Add(node, new ItemNodeFord(node));
            }
        }

        private void DesignationsElements(NodeGraph currentNode)
        {
            foreach(Edge edge in currentNode.Edges)
            {
                NodeGraph secondNode = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                if (secondNode.Equals(currentNode))
                    continue;
                CommentNonFlow(edge, currentNode, secondNode);
                if (flow < edge.Weight - itemsEdge[edge.Id].flow && !itemsNode[secondNode].isVisited)
                {
                    itemsEdge[edge.Id].isVisited = true;
                    stackEdge.Push(itemsEdge[edge.Id]);
                    stackNode.Push(itemsNode[secondNode]);
                    itemsNode[secondNode].isVisited = true;

                    CommentFlow(currentNode, secondNode);

                    DoFordFalcerson(secondNode);

                    itemsEdge[edge.Id].isVisited = false;
                    itemsNode[secondNode].isVisited = false;
                    stackEdge.Pop();

                    CommetnBack(currentNode);
                }
            }
            CommentNonEdgeEven(currentNode);
        }

        private int MinFlow()
        {
            int min = int.MaxValue;
            foreach(ItemEdgeFord edge in stackEdge)
            {
                if(edge.edge.Weight - itemsEdge[edge.edge.Id].flow < min)
                    min = edge.edge.Weight - itemsEdge[edge.edge.Id].flow;
            }
            return min;
        }

        private void SetFlow()
        {
            int currentFlow = MinFlow();
            pastFlow = currentFlow;
            foreach (ItemEdgeFord itemEdge in  stackEdge)
            {
                
                itemEdge.flow += currentFlow;
            }
            results.Add(currentFlow);
        }

        private int CheckSum()
        {
            int sum = 0;
            foreach(int result in results)
            {
                sum += result;
            }
            return sum;
        }

        private void CommentNonFlow(Edge edge, NodeGraph currentNode, NodeGraph secondNode)
        {
            if (0 >= edge.Weight - itemsEdge[edge.Id].flow)
            {
                Canvas newCanvas2 = new Canvas();
                DrawGraph(newCanvas2);
                Steps.Add(newCanvas2);
                ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
                Comments.Add($"Нельзя проложить поток, потому что поток у ребра {itemsEdge[edge.Id].flow} " +
                    $"полностью закрывает его вес {edge.Weight} с вершинами {currentNode.Name} и {secondNode.Name}.");
            }
        }

        private void CommentFlow(NodeGraph currentNode, NodeGraph secondNode)
        {
            Canvas newCanvas = new Canvas();
            DrawGraph(newCanvas);
            Steps.Add(newCanvas);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
            Comments.Add($"Пойдем по пути от вершины {currentNode.Name} до вершины {secondNode.Name}, " +
                $"так как по этому пути можно проложить " +
                $"поток.");
        }

        private void CommetnBack(NodeGraph currentNode)
        {
            Canvas newCanvas2 = new Canvas();
            DrawGraph(newCanvas2);
            Steps.Add(newCanvas2);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
            Comments.Add($"Вернемся назад на одно ребро назад и проверим остальные пути вершины {currentNode.Name}.");
        }

        private void CommentNonEdgeEven(NodeGraph currentNode)
        {
            Canvas newCanvas3 = new Canvas();
            DrawGraph(newCanvas3);
            Steps.Add(newCanvas3);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
            Comments.Add($"У вершины {currentNode.Name} больше нет ребер.");
        }

        private void CommentSetFlow()
        {
            Canvas newCanvas2 = new Canvas();
            DrawGraph(newCanvas2);
            Steps.Add(newCanvas2);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
            Comments.Add($"Устанавливаем проложенному пути поток {pastFlow}.");
        }

        private void CommentFinal()
        {
            Canvas newCanvas = new Canvas();
            DrawGraph(newCanvas);
            Steps.Add(newCanvas);
            ButtonSteps.Add(new Button() { CommandParameter = Steps.Count - 1, Content = $"Шаг{numberStep++}" });
            Comments.Add($"Расчет максимального потока равен {Result}.");
        }

        private void DrawGraph(Canvas currentCanvas)
        {
            foreach (ItemNodeFord itemNode in itemsNode.Values)
            {
                foreach (Edge edge in itemNode.node.Edges)
                {
                    if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Directed) && !itemsEdge[edge.Id].isVisited)
                    {
                        drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, 
                            currentCanvas, ColorForeGroundTextGraph, 1, itemsEdge[edge.Id].flow + "/" + edge.Weight, 
                            ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                    }
                    else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Directed) && itemsEdge[edge.Id].isVisited)
                    {
                        drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY,
                            currentCanvas, ColorStrokeSelectedNodeGraph, 3, itemsEdge[edge.Id].flow + "/" + edge.Weight, 
                            ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                    }

                    NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                    NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);

                    DrawNode(currentCanvas, itemsNode[node1]);
                    DrawNode(currentCanvas, itemsNode[node2]);
                }
            }
        }

        private void DrawNode(Canvas currentCanvas, ItemNodeFord itemNode)
        {
            if (itemNode.isVisited)
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph,
                    itemNode.node.PosX, itemNode.node.PosY, currentCanvas, itemNode.node.Name);
            else
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, 
                    itemNode.node.PosX, itemNode.node.PosY, currentCanvas, itemNode.node.Name);
        
        }
    }
}
