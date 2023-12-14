using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.M.Algorithms
{
    public class KruskalAlgorithm
    {
        public List<string> comments = new List<string>();
        public List<Graph> graphs = new List<Graph>(); 
        public Graph graphBefore = new Graph();
        Drawer drawer = new Drawer();
        public List<Canvas> StepsOfCanvases = new List<Canvas>();
        public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();


        public KruskalAlgorithm(Graph graph)
        {
            graphBefore = graph;
        }

        public VM.MainVM MainVM
        {
            get => default;
            set
            {
            }
        }

        public VM.AlgorithmLauncher AlgorithmLauncher
        {
            get => default;
            set
            {
            }
        }

        public void GetSteps()
        {
            comments.Clear();
            graphs.Clear();
            List<Edge> edges = Kruskal(graphBefore);
            GetGraph(edges);
            GetCanvases();
        }

        private Graph GetGraph(List<Edge> edges)
        {
            Graph newGraph = new Graph();
            foreach (var edge in edges)
            {
                foreach (var node in graphBefore.NodeGraphs)
                {
                    NodeGraph tempNode1 = drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                    NodeGraph node1 = new NodeGraph(tempNode1.Id, tempNode1.Name, tempNode1.PosX, tempNode1.PosY);
                    node1.Edges.Add(edge);
                    NodeGraph tempNode2 = drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                    NodeGraph node2 = new NodeGraph(tempNode2.Id, tempNode2.Name, tempNode2.PosX, tempNode2.PosY);
                    if (node.Id == node1.Id)
                    {
                        NodeGraph newNode = new NodeGraph(node.Id, node.Name, node.PosX, node.PosY);
                        newNode.Edges.Add(edge);
                        newGraph.NodeGraphs.AddLast(newNode);
                    }
                    else if (node.Id == node2.Id)
                    {
                        NodeGraph newNode = new NodeGraph(node.Id, node.Name, node.PosX, node.PosY);
                        newNode.Edges.Add(edge);
                        newGraph.NodeGraphs.AddLast(newNode);
                    }
                }
            }
            return newGraph;
        }

       

        public List<Edge> Kruskal(Graph graph)
        {
            List<int> ids = new List<int>();
            List<Edge> edgess = new List<Edge>();
            foreach (var node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    if (!ids.Contains(edge.Id))
                    {
                        ids.Add(edge.Id);
                        edgess.Add(edge);
                    }
                }
            }
            List<Edge> result = new List<Edge>();

            StringBuilder stringBuilder = new StringBuilder();
            int count = 1;
            stringBuilder.Append($"Шаг {count}\n Сортируем в порядке возрастания грани графа");
            Edge[] edges = edgess.ToArray();
            edges = edges.OrderBy(x => x.Weight).ToArray();
            foreach (var edge in edges)
            {
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                stringBuilder.Append($"\n{node1.Name} -> {node2.Name} : Вес - {edge.Weight}");
            }

            stringBuilder.Append("\nБудем проверять пары узлов в этом отсортированном порядке,\nне образуют цикл - добавляем\n" +
                "образуют - пропускаем\n");
            comments.Add(stringBuilder.ToString());

            int[] parentsSets = new int[graph.NodeGraphs.Count];

            for (int i = 0; i < graph.NodeGraphs.Count; i++)
            {
                parentsSets[i] = i;
            }
            count++;
            int edgeCount = 0;
            int edgeIndex = 0;
            while (edgeCount < graph.NodeGraphs.Count - 1)
            {
                Edge nextEdge = edges[edgeIndex++];
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, nextEdge.FirstPosX, nextEdge.FirstPosY);
                NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, nextEdge.SecondPosX, nextEdge.SecondPosY);

                StringBuilder builder = new StringBuilder();
                builder.Append($"Шаг {count}\nПроверяем образуют ли вершины {node1.Name} и {node2.Name} цикл");
                int setX = Find(parentsSets, node1.Id);

                int setY = Find(parentsSets, node2.Id);
                builder.Append($"\n\nВершины:    ");
                for (int i = 0; i < parentsSets.Length; i++)
                {
                    builder.Append($"{i} ");
                }
                builder.Append("\nМножества: ");
                foreach (int temp in parentsSets)
                {
                    builder.Append($"{temp} ");
                }
                builder.Append($"\n\nВершина {node1.Name}, множество: {setX}\nВершина {node2.Name}, множество: {setY}");
                if (setX != setY)
                {
                    builder.Append($"\n\nВершины {node1.Name} и {node2.Name} не образуют цикл, добавляем");
                    result.Add(nextEdge);
                    graphs.Add(GetGraph(result));
                    Union(parentsSets, setX, setY);
                    edgeCount++;
                    builder.Append($"\n\nВершины:   ");
                    for (int i = 0; i < parentsSets.Length; i++)
                    {
                        builder.Append($"{i} ");
                    }
                    builder.Append("\nМножества: ");
                    foreach (int temp in parentsSets)
                    {
                        builder.Append($"{temp} ");
                    }
                }
                else
                {
                    graphs.Add(graphs[graphs.Count - 1]);
                    builder.Append($"\n\nВершины {node1.Name} и {node2.Name} образуют цикл, не добавляем");
                }
                comments.Add(builder.ToString());
                count++;
            }
            StringBuilder builderr = new StringBuilder();
            graphs.Add(graphs[graphs.Count - 1]);
            builderr.Append($"Шаг {count}\nВсе вершины пройдены\nПолучили минимальное остовное дерево");
            comments.Add(builderr.ToString());
            return result;
        }

        private int Find(int[] parentsSets, int i)
        {
            if (parentsSets[i] != i)
            {
                parentsSets[i] = Find(parentsSets, parentsSets[i]);
            }

            return parentsSets[i];
        }

        private void Union(int[] parentsSets, int x, int y)
        {
            int setX = Find(parentsSets, x);
            int setY = Find(parentsSets, y);
            parentsSets[setX] = setY;
        }


        private void GetCanvases()
        {
            int count = 0;
            Canvas canvass = new Canvas();
            AddDefaultGraph(canvass);
            StepsOfCanvases.Add(canvass);
            ButtonSteps.Add(new Button() { CommandParameter = count - 1, Content = $"Шаг{++count}" });
            foreach (Graph tempGraph in graphs)
            {
                Canvas canvas = new Canvas();
                AddDefaultGraph(canvas);
                List<Edge> edges = GetUnicEdges(tempGraph);
                foreach (Edge edge in edges)
                {
                    drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorFillForLine, 2, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph);
                }
                foreach (NodeGraph node in tempGraph.NodeGraphs)
                {
                    drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillForDfs, ColorFillForDfs, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraphW);
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
            List<Edge> edges = GetUnicEdges(graphBefore);
            foreach (Edge edge in edges)
            {
                drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph);
            }
            foreach (NodeGraph node in graphBefore.NodeGraphs)
            {
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraph);
            }
        }
    }
}
