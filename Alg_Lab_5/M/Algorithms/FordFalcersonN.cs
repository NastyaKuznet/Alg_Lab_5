using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        int startFlow = 0;
        int flow = 0;
        //NodeGraph currentNode;
        Dictionary<Edge, ItemEdgeFord> itemsEdge = new Dictionary<Edge, ItemEdgeFord>();
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

        public FordFalcersonN(Graph graph)
        {
            this.graph = graph;
            hasResult.Add(FindSource());
            hasResult.Add(FindStock());
            //currentNode = source;
            AddItems();
            DoFordFalcerson(source);
            Result = CheckSum();
        }

        public void DoFordFalcerson(NodeGraph currentNode)
        {
            if(currentNode.Equals(stock))
            {
                SetFlow();
                return;
            }
            DesignationsElements(currentNode);
                    
            BaskVisited();
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
            int b;
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
                    if(!itemsEdge.ContainsKey(edge))
                        itemsEdge.Add(edge, new ItemEdgeFord(edge));
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
                if (flow < edge.Weight - itemsEdge[edge].flow && !itemsNode[secondNode].isVisited)
                {
                    itemsEdge[edge].isVisited = true;
                    stackEdge.Push(itemsEdge[edge]);
                    stackNode.Push(itemsNode[secondNode]);
                    itemsNode[secondNode].isVisited = true;
                    DoFordFalcerson(secondNode);
                }
            }
        }

        private int MinFlow()
        {
            int min = int.MaxValue;
            foreach(ItemEdgeFord edge in stackEdge)
            {
                if(edge.edge.Weight - itemsEdge[edge.edge].flow < min)
                    min = edge.edge.Weight - itemsEdge[edge.edge].flow;
            }
            return min;
        }

        private void SetFlow()
        {
            int currentFlow = 0;
            currentFlow = MinFlow();
            while (stackEdge.Count > 0)
            {   
                ItemEdgeFord itemEdge = stackEdge.Pop();
                itemEdge.flow = currentFlow;
            }
            results.Add(currentFlow);
        }

        private void BaskVisited()
        {
            foreach(ItemEdgeFord edgeFord in itemsEdge.Values)
            {
                edgeFord.isVisited = false;
            }

            foreach(ItemNodeFord nodeFord in itemsNode.Values)
            {
                nodeFord.isVisited = false;
            }
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

        private void DrawGraph(Canvas currentCanvas)
        {
            foreach (ItemNodeFord itemNode in itemsNode.Values)
            {
                foreach (Edge edge in itemNode.node.Edges)
                {

                    //if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Base))
                    //{
                    //    drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1);
                    //}
                    //else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Base))
                    //{
                    //    drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                    //}
                    //else if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Directed))
                    //{
                    //    drawer.DrawDirectedLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1);
                    //}
                    /*else */if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Directed))
                    {
                        drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, currentCanvas, ColorForeGroundTextGraph, 1, itemsEdge[edge].flow + "/" + edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
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
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, itemNode.node.PosX, itemNode.node.PosY, currentCanvas, itemNode.node.Name);
            else
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, itemNode.node.PosX, itemNode.node.PosY, currentCanvas, itemNode.node.Name);
        
        }
    }
}
