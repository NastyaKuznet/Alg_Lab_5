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

        private List<string> _steps = new List<string>();
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
            int k = 0;
            foreach(NodeGraph node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    if(!visitedVertices[k] && currentNode.Edges.Contains(edge))
                    {
                        DfsUtil(k, graph, visitedVertices, node);
                    }
                }
                k++;
            }
        }
    }
}
