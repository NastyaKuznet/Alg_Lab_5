using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Alg_Lab_5.M.Algorithms
{
    public class KruskalAlgorithm
    {
        private List<string> _steps = new List<string>();
        private Graph minSpanningTree = new Graph();
        public List<Graph> graphs = new List<Graph>(); 
        public Graph graphBefore = new Graph();
        Drawer drawer = new Drawer();
        public (List<Edge>, List<string>) GetSteps(Graph graph)
        {
            _steps.Clear();
            graphs.Clear();
            graphBefore = graph;
            List<Edge> edges = Kruskal(graph);
            GetGraph(edges);
            return (edges, _steps);
        }

        private Graph GetGraph(List<Edge> edges)
        {
            Graph newGraph = new Graph();
            foreach (var edge in edges)
            {
                foreach (var node in graphBefore.NodeGraphs)
                {
                    NodeGraph node1 = drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                    NodeGraph node2 = drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
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

            _steps.Add("Сортируем в порядке возрастания грани графа.");
            Edge[] edges = edgess.ToArray();
            edges = edges.OrderBy(x => x.Weight).ToArray();
            StringBuilder builder = new StringBuilder();
            foreach (var edge in edges)
            {
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                builder.Append($"{node1.Id} -> {node2.Id} : Вес - {edge.Weight} ");
            }
                

            builder.Append("\n");
            _steps.Add(builder.ToString());

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
                //Ищем множества, к которым принадлежат вершины начальная и конечная.
                _steps.Add("Проверяем к каким множества относятся начальная и конечная точка ребра: ");
                int setX = Find(parentsSets, node1.Id);

                int setY = Find(parentsSets, node2.Id);
                _steps.Add($"Начальная вершина, множество: {setX}. Конечная вершина, множество {setY}. ");
                //если вершины не из одного множества, то добавляем их в МОД и объединяем их множества.
                if (setX != setY)
                {
                    _steps.Add($"Вершины не из одного множества, объединяем множества {setX} и {setY}. ");
                    result.Add(nextEdge);
                    graphs.Add(GetGraph(result));
                    Union(parentsSets, setX, setY);
                    edgeCount++;
                }
            }
            return result;
        }
        //К какому множеству относится вершина i. (вплоть до vertices - 1 множеств)
        private int Find(int[] parentsSets, int i)
        {
            _steps.Add($"Ищем к какому множетсву относится {i} вершина. ");
            if (parentsSets[i] != i)
            {
                _steps.Add($"В массиве родителей по индексу самой {i} вершины не находится равное {i} значение. Рекурсивно погружаемся" +
                           $" в этот поиск снова, толкьо вместо {i} передаем элемент из массива родителей по {i} индексу ");
                parentsSets[i] = Find(parentsSets, parentsSets[i]);
            }

            return parentsSets[i];
        }

        //Объединение множеств. просто обычный поиск множеств и присваивание первому номер последнего.
        private void Union(int[] parentsSets, int x, int y)
        {
            _steps.Add($"Объединение: Находим множества X и Y. После присваиваем множеству X тот же номер, что и у Y. ");
            int setX = Find(parentsSets, x);
            int setY = Find(parentsSets, y);
            parentsSets[setX] = setY;
        }
    }
}
