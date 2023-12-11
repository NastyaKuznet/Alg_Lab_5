using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Alg_Lab_5.M.FolderGraph;

namespace Alg_Lab_5.M.Algorithms;

public class FordFalkersonAlgorithm
{
    private Graph graph = new Graph();
    public List<Graph> graphs = new List<Graph>();
    NodeGraph startNode;
    NodeGraph endNode;
    
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
        
    }
    
    //первое лист всех путей вершин, второе лист всех шагов
    public (List<List<string>>, List<string>) GetSteps(Graph graph, int vertices, int source, int sink)
    {
        _steps.Clear();
        var result = FordFulkerson(graph, vertices, source, sink);
        _steps.Add($"Максимальный поток найден: {result.Item2}. ");
        return (result.Item1, _steps);
    }
    public (List<List<string>>,int) FordFulkerson(Graph graph, int vertices, int source, int sink)
    {
        //Копируем исходный граф во временную матрицу смежности. Ее мы и будет изменять
        List<List<string>> allPaths = new List<List<string>>();
        
        int[][] tempGraph = new int[vertices][];
        for (int i = 0; i < vertices; i++)
        {
            tempGraph[i] = new int[vertices];
            for (int v = 0; v < vertices; v++)
            {
                tempGraph[i][v] = graph.AdjacencyMatrix[i][v];
            }
        }
        _steps.Add($"Копируем исходный граф во временную матрицу смежности. Она будет служить сетью для изменений. ");
        //Создаем массив "родителей" вершин.
        int[] parents = new int[vertices]; //в парентс лежат последние вершины-родители при поиске в глубину, который нашли.
        //т.е. информация о путях в графе
        int maxFlow = 0;

        //Пока путь ЕСТЬ, мы выполняем алгоритм. (путь ищется BFS-ом)
        while (Bfs(tempGraph,  vertices, source, sink, parents))
        {
            _steps.Add($"Путь был найден, продолжаем алгоритм. ");
            int pathFlow = int.MaxValue;
            //Ищем минимальный поток в пути образовавшемся. Путь мы достаем из parents,
            //который изменился после проходки BFS.
            List<string> currentPath = new List<string>(); //ищется минимальный поток в найденном пути и путь записывается в <- 
            for (int i = sink; i != source; i = parents[i])
            {
                int iVertexParent = parents[i];
                currentPath.Add(graph.VerticesNames[iVertexParent]);
                pathFlow = Math.Min(pathFlow, tempGraph[iVertexParent][i]);
            }
            allPaths.Add(currentPath);
            _steps.Add($"Минимальный поток в найденном пути: {pathFlow}");
            //Меняем исходный граф. Мы делаем что-то вроде ребра "обратного потока" (на хабре есть статья).
            /* Это своеобразные ребра, по которым можно вернуть жидкость обратно из одной точки в другую.
             * Этто нужно для оптимального решения, так как иногда путь, который мы нашли, может быть не оптимальным и
             * просто перекроет другие пути. Для этого нужны обратные ребра, по которым можно "вернуться".
             */
            for (int i = sink; i != source; i = parents[i])
            {
                int currentVertexParent = parents[i]; //индекс родительской вершины до текущего элемента
                _steps.Add($"Устанавливаем следующее: во временном графе по индексу {currentVertexParent},{i} к этому элементу и " +
                           $" {tempGraph[currentVertexParent][i]} - {pathFlow}. Уменьшаем на мин.поток текущий поток по пути " +
                           $"найденному. ");
                tempGraph[currentVertexParent][i] -= pathFlow; //уменьшение потока по текущему пути
                _steps.Add($"Устанавливаем следующее: во временном графе по индексу {i},{currentVertexParent} к этому элементу и " +
                           $" {tempGraph[i][currentVertexParent]} + {pathFlow}. Увеличиваем на мин.поток текущий поток по пути " +
                           $"найденному. ");
                tempGraph[i][currentVertexParent] += pathFlow; //увеличение потока по текущему пути
            }
            _steps.Add($"Прибавляем к счетчику максимального потока наш минимальный поток: {maxFlow} += {pathFlow}");
            maxFlow += pathFlow; //увеличение максимального потока
            //Исходный граф модифицируется для учета найденного пути, и максимальный поток увеличивается.
        }

        return (allPaths, maxFlow);
    }
    
    private bool Bfs(int[][] residualGraph, int vertices, int source, int sink, int[] parents)
    {
        _steps.Add($"Начинаем обход графа в ширину, чтобы найти путь от истока до стока. ");
        bool[] visitedVertices = new bool[vertices];
        Queue<int> queue = new Queue<int>();
        _steps.Add($"Добавили в очередь вершину по номеру {source}, пометили ее как пройденную. ");
        queue.Enqueue(source);
        visitedVertices[source] = true;
        parents[source] = -1;
        
        while (queue.Count > 0)
        {
            int currentItem = queue.Dequeue();

            for (int vert = 0; vert < vertices; vert++)
            {
                if (!visitedVertices[vert] && residualGraph[currentItem][vert] > 0)
                {
                    _steps.Add($"Добавили в очередь вершину по номеру {vert}, пометили ее как пройденную. ");
                    queue.Enqueue(vert);
                    parents[vert] = currentItem;
                    visitedVertices[vert] = true;
                }
            }
        }

        return visitedVertices[sink];
    }
}