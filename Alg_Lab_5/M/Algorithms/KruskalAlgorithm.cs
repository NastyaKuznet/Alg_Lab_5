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
        private Graph minSpanningTree = new Graph();
        public List<Graph> graphs = new List<Graph>(); 
        public Graph graphBefore = new Graph();
        Drawer drawer = new Drawer();
        public List<Canvas> StepsOfCanvases = new List<Canvas>();
        public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();


        public KruskalAlgorithm(Graph graph)
        {
            graphBefore = graph;
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
            stringBuilder.Append("Сортируем в порядке возрастания грани графа");
            Edge[] edges = edgess.ToArray();
            edges = edges.OrderBy(x => x.Weight).ToArray();
            foreach (var edge in edges)
            {
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                stringBuilder.Append($"\n{node1.Id} -> {node2.Id} : Вес - {edge.Weight}");
            }

            stringBuilder.Append("\n");
            comments.Add(stringBuilder.ToString());

            int[] parentsSets = new int[graph.NodeGraphs.Count];

            for (int i = 0; i < graph.NodeGraphs.Count; i++)
            {
                parentsSets[i] = i;
            }

            int edgeCount = 0;
            int edgeIndex = 0;

            while (edgeCount < graph.NodeGraphs.Count - 1)
            {
                Edge nextEdge = edges[edgeIndex++];
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, nextEdge.FirstPosX, nextEdge.FirstPosY);
                NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, nextEdge.SecondPosX, nextEdge.SecondPosY);

                StringBuilder builder = new StringBuilder();
                //Ищем множества, к которым принадлежат вершины начальная и конечная.
                //builder.Append("\nПроверяем к каким множества относятся начальная и конечная точка ребра: ");

                builder.Append($"\nПроверяем образуют ли вершины {node1.Name} и {node2.Name} цикл");
                int setX = Find(parentsSets, node1.Id);

                int setY = Find(parentsSets, node2.Id);
                builder.Append($"\nНачальная вершина, множество: {setX}. Конечная вершина, множество {setY}. ");
                //если вершины не из одного множества, то добавляем их в МОД и объединяем их множества.
                if (setX != setY)
                {
                    //builder.Append($"\nВершины не из одного множества, объединяем множества {setX} и {setY}. ");
                    builder.Append($"\nВершины {node1.Name} и {node2.Name} не образуют цикл, добавляем");
                    result.Add(nextEdge);
                    graphs.Add(GetGraph(result));
                    Union(parentsSets, setX, setY);
                    edgeCount++;
                }
                else
                {
                    graphs.Add(graphs[graphs.Count - 1]);
                    builder.Append($"\nВершины {node1.Name} и {node2.Name} из одного множества, образуют цикл, проверяем следующую пару");
                }
                comments.Add(builder.ToString());
            }
            return result;
        }
        //К какому множеству относится вершина i. (вплоть до vertices - 1 множеств)
        private int Find(int[] parentsSets, int i)
        {
            //comments.Add($"Ищем к какому множетсву относится {i} вершина. ");
            if (parentsSets[i] != i)
            {
                //comments.Add($"В массиве родителей по индексу самой {i} вершины не находится равное {i} значение. Рекурсивно погружаемся" +
                //           $" в этот поиск снова, толкьо вместо {i} передаем элемент из массива родителей по {i} индексу ");
                parentsSets[i] = Find(parentsSets, parentsSets[i]);
            }

            return parentsSets[i];
        }

        //Объединение множеств. просто обычный поиск множеств и присваивание первому номер последнего.
        private void Union(int[] parentsSets, int x, int y)
        {
            //comments.Add($"Объединение: Находим множества X и Y. После присваиваем множеству X тот же номер, что и у Y. ");
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
                    drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorFillForLine, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph);
                }
                foreach (NodeGraph node in tempGraph.NodeGraphs)
                {
                    drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillForDfs, ColorStrokeForDfs, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraphW);
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
