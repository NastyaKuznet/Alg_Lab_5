using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.Algorithms
{
    public class DfsAlgorithm
    {
        public Graph graph = new Graph();
        private List<string> _steps = new List<string>();
        public List<Graph> graphs = new List<Graph>();
        private Drawer _drawer = new Drawer();
        public List<string> Dfs(Graph graph)
        {
            _steps.Clear();
            bool[] visitedVertices = new bool[graph.NodeGraphs.Count];
            NodeGraph node = graph.NodeGraphs.First();
            DfsUtil(0, graph, visitedVertices, node);
            return _steps;
        }

        private void DfsUtil(int currentVertex, Graph graph, bool[] visitedVertices, NodeGraph currentNode)
        {
            visitedVertices[currentVertex] = true;
            _steps.Add($"Название вершины: {currentNode.Name}, состояние : пройдена");
            List<NodeGraph> nodes = new List<NodeGraph>();
            int k = 0;
            foreach(NodeGraph node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    if(!visitedVertices[k] && currentNode.Edges.Contains(edge))
                    {
                        graphs.Add(GetGraph(edge, graph));
                        DfsUtil(k, graph, visitedVertices, node);
                    }
                }
                k++;
            }
        }

        private Graph GetGraph(Edge edge, Graph graphBefore)
        {
            Graph graph = new Graph();
            NodeGraph node1 = _drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
            NodeGraph node2 = _drawer.FindNodeInTouch(graphBefore.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
            graph.NodeGraphs.AddLast(node1);
            graph.NodeGraphs.AddLast(node2);
            return graph;
        }
    }
}
