using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Alg_Lab_5.M.FolderGraph;
using static Alg_Lab_5.M.ImportData;
namespace Alg_Lab_5.M.Algorithms;

public class FordFalkersonAlgorithm
{
    private Graph graph = new Graph();
    public List<Graph> graphs = new List<Graph>();
    NodeGraph startNode;
    NodeGraph endNode;
    Dictionary<int, ItemFord> fordEdges = new Dictionary<int, ItemFord>();
    
    private Drawer _drawer = new Drawer();


    public List<string> comments = new List<string>();
    public List<Canvas> StepsCanvases = new List<Canvas>();
    private List<string> _steps = new List<string>();
    public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();

    public FordFalkersonAlgorithm(Graph graphFordFalkerson, NodeGraph startNode, NodeGraph endNode)
    {
        graph = graphFordFalkerson;
        this.startNode = startNode;
        this.endNode = endNode;
    }

    public void DoFordFalkerson()
    {
        MakeFordEdges();
        FordFulkerson(graph, startNode, endNode);
    }

    private void MakeFordEdges()
    {
        foreach(NodeGraph node in graph.NodeGraphs)
        {
            foreach(Edge edge in node.Edges)
            {
                if (!fordEdges.ContainsKey(edge.Id))
                {
                    fordEdges.Add(edge.Id, new ItemFord(edge, edge.Weight, 0));
                }
            }
        }
    }

    //первое лист всех путей вершин, второе лист всех шагов
    //public (List<List<string>>, List<string>) GetSteps(Graph graph, int vertices, int source, int sink)
    //{
    //    _steps.Clear();
    //    FordFulkerson(graph, vertices, source, sink);
    //    //_steps.Add($"Максимальный поток найден: {result.Item2}. ");
    //    //return (result.Item1, _steps);
    //}

    private Graph CopyCraph(Graph graph)
    {
        Graph copy = new Graph();
        copy.Name = graph.Name;
        foreach (var item in graph.NodeGraphs) 
        { 
            NodeGraph nodeGraph = new NodeGraph(item.Id, item.Name, item.PosX, item.PosY);
            foreach(var edge in item.Edges)
            {
                nodeGraph.Edges.Add(edge);
            }
            copy.NodeGraphs.AddLast(nodeGraph);
        }
        return copy;
    }


    // 1 - найти путь в ширину c учетом направлений
    // 2 - найти минимальный вес в этом пути
    // 3 - отнять от remaind и присвоить к pass
    // 4 - 

    public void FordFulkerson(Graph graph, NodeGraph sourceNode, NodeGraph sinkNode)
    {
        //Копируем исходный граф во временную матрицу смежности. Ее мы и будет изменять
        //List<List<string>> allPaths = new List<List<string>>();

        Graph tempGraph = CopyCraph(graph);

        _steps.Add($"Копируем исходный граф во временную матрицу смежности. Она будет служить сетью для изменений. ");
        //Создаем массив "родителей" вершин.
        List<NodeGraph> parents = new List<NodeGraph>(); //в парентс лежат последние вершины-родители при поиске в глубину, который нашли.
        //т.е. информация о путях в графе
        List<Edge> parentsedges = new List<Edge>();
        int maxFlow = 0;

        //Пока путь ЕСТЬ, мы выполняем алгоритм. (путь ищется BFS-ом)
        while (true)
        {
            List<NodeGraph> nodeG = Bfs(tempGraph, graph.NodeGraphs.Count, sourceNode, sinkNode, parents, parentsedges);
            if(nodeG.Count == 0 || nodeG.Count == 1)
            {
                break;
            }
            _steps.Add($"Путь был найден, продолжаем алгоритм. ");
            int pathFlow = int.MaxValue;
            //Ищем минимальный поток в пути образовавшемся. Путь мы достаем из parents,
            //который изменился после проходки BFS.
            List<string> currentPath = new List<string>(); //ищется минимальный поток в найденном пути и путь записывается в <- 

            Graph canvasGraph = new Graph();


            NodeGraph prevNode = nodeG[0];
            bool flag = true;
            Edge tempEdge1 = new Edge(-1);
            foreach (Edge edge in nodeG[1].Edges)
            {
                foreach (Edge edge1 in prevNode.Edges)
                {
                    if (edge.Id == edge1.Id)
                    {
                        pathFlow = Math.Min(pathFlow, fordEdges[edge.Id].Remained);
                        tempEdge1.Id = edge1.Id;
                        tempEdge1.FirstPosX = edge1.FirstPosX;
                        tempEdge1.FirstPosY = edge1.FirstPosY;
                        tempEdge1.SecondPosX = edge1.SecondPosX;
                        tempEdge1.SecondPosY = edge1.SecondPosY;
                        tempEdge1.Weight = edge1.Weight;
                    }
                }
            }
            NodeGraph tempNode1 = new NodeGraph(prevNode.Id, prevNode.Name, prevNode.PosX, prevNode.PosY);
            tempNode1.Edges.Add(tempEdge1);
            canvasGraph.NodeGraphs.AddLast(tempNode1);
            foreach (NodeGraph node in nodeG)
            {
                if (flag)
                {
                    flag = false;
                    continue;
                }
                Edge tempEdge = new Edge(-1);
                foreach(Edge edge in node.Edges)
                {
                    foreach(Edge edge1 in prevNode.Edges)
                    {
                        if(edge.Id == edge1.Id)
                        {
                            pathFlow = Math.Min(pathFlow, fordEdges[edge.Id].Remained);
                            tempEdge.Id = edge1.Id;
                            tempEdge.FirstPosX = edge1.FirstPosX;
                            tempEdge.FirstPosY = edge1.FirstPosY;   
                            tempEdge.SecondPosX = edge1.SecondPosX;
                            tempEdge.SecondPosY = edge1.SecondPosY;
                            tempEdge.Weight = edge1.Weight;
                        }
                    }
                }
                NodeGraph tempNode = new NodeGraph(node.Id, node.Name, node.PosX, node.PosY);
                tempNode.Edges.Add(tempEdge);
                canvasGraph.NodeGraphs.AddLast(tempNode);
            }

            _steps.Add($"Минимальный поток в найденном пути: {pathFlow}");
            //Меняем исходный граф. Мы делаем что-то вроде ребра "обратного потока" (на хабре есть статья).
            /* Это своеобразные ребра, по которым можно вернуть жидкость обратно из одной точки в другую.
             * Этто нужно для оптимального решения, так как иногда путь, который мы нашли, может быть не оптимальным и
             * просто перекроет другие пути. Для этого нужны обратные ребра, по которым можно "вернуться".
             */

            List<int> ids = new List<int>();
            foreach(NodeGraph node in canvasGraph.NodeGraphs)
            {
                foreach (Edge edge in node.Edges)
                {
                    if (!ids.Contains(edge.Id))
                    {
                        int rem = fordEdges[edge.Id].Remained;
                        int pass = fordEdges[edge.Id].Pass;
                        rem = rem - pathFlow;
                        pass = pass + pathFlow;
                        fordEdges[edge.Id].Remained = rem;
                        fordEdges[edge.Id].Pass = pass;
                        ids.Add(edge.Id);
                    }
                }
            }
           

            //for (int i = sinkNode; i != sourceNode; i = parents[i])
            //{
            //    int currentVertexParent = parents[i]; //индекс родительской вершины до текущего элемента
            //    _steps.Add($"Устанавливаем следующее: во временном графе по индексу {currentVertexParent},{i} к этому элементу и " +
            //               $" {tempGraph[currentVertexParent][i]} - {pathFlow}. Уменьшаем на мин.поток текущий поток по пути " +
            //               $"найденному. ");
            //    tempGraph[currentVertexParent][i] -= pathFlow; //уменьшение потока по текущему пути
            //    _steps.Add($"Устанавливаем следующее: во временном графе по индексу {i},{currentVertexParent} к этому элементу и " +
            //               $" {tempGraph[i][currentVertexParent]} + {pathFlow}. Увеличиваем на мин.поток текущий поток по пути " +
            //               $"найденному. ");
            //    tempGraph[i][currentVertexParent] += pathFlow; //увеличение потока по текущему пути
            //}
            _steps.Add($"Прибавляем к счетчику максимального потока наш минимальный поток: {maxFlow} += {pathFlow}");
            maxFlow += pathFlow; //увеличение максимального потока
                                 //Исходный граф модифицируется для учета найденного пути, и максимальный поток увеличивается.
            SaveStep(canvasGraph, maxFlow.ToString());
        }

        //return (allPaths, maxFlow);
    }


    private void SaveStep(Graph saveGraph, string comm)
    {
        Canvas canvas = new Canvas();
        AddDefaultGraph(canvas);
        int count = 0;
        StepsCanvases.Add(canvas);
        ButtonSteps.Add(new Button() { CommandParameter = count - 1, Content = $"Шаг{++count}" });
        List<Edge> edges = GetUnicEdges(saveGraph);
        comments.Add(comm);
        foreach (Edge edge in edges)
        {
            ItemFord item = fordEdges[edge.Id];
            StringBuilder sb = new StringBuilder();
            sb.Append($"{item.Remained.ToString()}/{item.Pass.ToString()}");
            _drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorFillForLine, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph, sb.ToString());
        }
        foreach (NodeGraph node in saveGraph.NodeGraphs)
        {
            _drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillForDfs, ColorStrokeForDfs, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraphW);
        }
    }

    private void AddDefaultGraph(Canvas canvas)
    {
        List<Edge> edges = GetUnicEdges(graph);
        foreach (Edge edge in edges)
        {
            _drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, canvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph);
        }
        foreach (NodeGraph node in graph.NodeGraphs)
        {
            _drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node.PosX, node.PosY, canvas, node.Name, ColorForeGroundTextGraph);
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

    private List<NodeGraph> Bfs(Graph graph, int vertices, NodeGraph source, NodeGraph sink, List<NodeGraph> parents, List<Edge> parentsE)
    {
        _steps.Add($"Начинаем обход графа в ширину, чтобы найти путь от истока до стока. ");
        parents.Clear();
        bool[] visitedVertices = new bool[vertices];
        Queue<NodeGraph> queue = new Queue<NodeGraph>();
        _steps.Add($"Добавили в очередь вершину по номеру {source}, пометили ее как пройденную. ");
        queue.Enqueue(source);
        visitedVertices[source.Id] = true;
        //parents[source.Id] = null;

        //List<List<NodeGraph>> paths = new List<List<NodeGraph>>();
        List<NodeGraph> path = new List<NodeGraph>();
        bool flag = false;
        while (queue.Count > 0)
        {
            NodeGraph currentItem = queue.Dequeue();
            path.Add(currentItem);
            int k = 0;
            foreach (NodeGraph node in graph.NodeGraphs)
            {
                foreach (Edge edge in node.Edges)
                {
                    ItemFord item = fordEdges[edge.Id];
                    if (!visitedVertices[k] && IsContainEdge(edge, currentItem) && item.Remained > 0 && edge.SecondPosX == node.PosX && edge.SecondPosY == node.PosY)
                    {
                        queue.Enqueue(node);
                        parents.Add(node);
                        parentsE.Add(edge);
                        visitedVertices[k] = true;
                        flag = true;
                        if (node.Id == sink.Id)
                        {
                            path.Add(sink);
                            return path;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                k++;
            }
            //if (!flag)
            //{
            //    //paths.Add(path);
            //    path.Clear();
            //}
        }
        return path;
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
}