using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Alg_Lab_5.M.FolderGraph;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.M.Algorithms;

public class BfsAlgorithm
{
    public Graph graph = new Graph();
    public List<string> comments = new List<string>();
    public List<Graph> graphs = new List<Graph>();
    private Drawer _drawer = new Drawer();
    public List<Canvas> StepsOfCanvases = new List<Canvas>();
    Drawer drawer = new Drawer();
    public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();

    public BfsAlgorithm(Graph graphBFS)
    {
        graph = graphBFS;
    }

    public void DoBFS()
    {
        comments.Clear();
        StepsOfCanvases.Clear();
        ButtonSteps.Clear();
        bool[] visitedVertices = new bool[graph.NodeGraphs.Count];
        Queue<NodeGraph> queue = new Queue<NodeGraph>();
        queue.Enqueue(graph.NodeGraphs.First());
        BFS(queue, visitedVertices);
        GetCanvases();
    }

    private List<NodeGraph> nodess = new List<NodeGraph>();
    private void BFS(Queue<NodeGraph> queue, bool[] visitedVertices)
    {
        NodeGraph tempcurrent = queue.Peek();
        List<NodeGraph> visited = new List<NodeGraph>();
        visited.Clear();
        visited.Add(queue.Peek());
        while (queue.Count != 0)
        {
            var currentNode = queue.Dequeue();
            int count = 0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"\nТекущая вершина: {currentNode.Name}\nИщем смежные непройденные вершины\n");
            visitedVertices[0] = true;
            nodess.Clear();

            foreach (NodeGraph node in graph.NodeGraphs)
            {
                stringBuilder.Append($"\nРассматриваем вершину:{node.Name}\n");
                if (visited.Contains(node))
                {
                    stringBuilder.Append($"Состояние-пройдена\n");
                }

                foreach (Edge edge in node.Edges)
                {
                    if (!visitedVertices[count] && IsContainEdge(edge, currentNode))
                    {
                        graphs.Add(GetGraph(edge, graph));
                        stringBuilder.Append($"\nВершина {node.Name}-смежная\nЕщё не пройдена,\nпомечаем эту вершину пройденной - туда больше пути нет.");
                        comments.Add(stringBuilder.ToString());
                        visitedVertices[count] = true;
                        queue.Enqueue(node);
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append($"\nТекущая вершина: {currentNode.Name}\nИщем смежные непройденные вершины\n");
                        visited.Add(node);
                    }
                }
                count++;
            }
        }
    }

    private bool IsContainEdge(Edge edge, NodeGraph node)
    {
        foreach (Edge tempEdge in node.Edges)
        {
            if (tempEdge.Id == edge.Id)
            {
                return true;
            }
        }
        return false;
    }

    private Graph GetGraph(Edge edge, Graph graphBefore)
    {
        Graph graph = new Graph();
        NodeGraph tempNode1 = _drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
        NodeGraph node1 = new NodeGraph(tempNode1.Id, tempNode1.Name, tempNode1.PosX, tempNode1.PosY);
        node1.Edges.Add(edge);
        NodeGraph tempNode2 = _drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
        NodeGraph node2 = new NodeGraph(tempNode2.Id, tempNode2.Name, tempNode2.PosX, tempNode2.PosY);
        node2.Edges.Add(edge);
        graph.NodeGraphs.AddLast(node1);
        graph.NodeGraphs.AddLast(node2);
        foreach (Graph graph1 in graphs)
        {
            foreach (NodeGraph node in graph1.NodeGraphs)
            {
                graph.NodeGraphs.AddLast(node);
            }
        }
        return graph;
    }

    private void GetCanvases()
    {
        int count = 0;
        foreach (Graph tempGraph in graphs)
        {
            Canvas canvas = new Canvas();
            AddDefaultGraph(canvas);
            List<Edge> edges = GetUnicEdges(tempGraph);
            foreach (Edge edge in edges)
            {
                drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorFillForBfs, 2);
            }
            foreach (NodeGraph node in tempGraph.NodeGraphs)
            {
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillForBfs, ColorStrokeForBfs, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraphW);
            }
            StepsOfCanvases.Add(canvas);
            ButtonSteps.Add(new Button() { CommandParameter = count - 1, Content = $"Шаг{++count}" });
        }
    }

    private List<Edge> GetUnicEdges(Graph graph)
    {
        List<Edge> edges = new List<Edge>();
        foreach (NodeGraph node in graph.NodeGraphs)
        {
            foreach (Edge edge in node.Edges)
            {
                if (!edges.Contains(edge)) edges.Add(edge);
            }
        }
        return edges;
    }

    private void AddDefaultGraph(Canvas canvas)
    {
        List<Edge> edges = GetUnicEdges(graph);
        foreach (Edge edge in edges)
        {
            drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorForeGroundTextGraph, 1);
        }
        foreach (NodeGraph node in graph.NodeGraphs)
        {
            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraph);
        }
    }
}