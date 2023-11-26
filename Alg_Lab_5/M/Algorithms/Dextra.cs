using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alg_Lab_5.M.Algorithms
{
    public class Dextra
    {
        // -1 = infinity
        //рабочие элементы
        Graph graph;
        Dictionary<NodeGraph,ItemDextra> nodes = new Dictionary<NodeGraph, ItemDextra>();
        NodeGraph startNode;
        NodeGraph endNode;
        ItemDextra currentNode;

        //инструменты
        Drawer drawer = new Drawer();

        //результаты
        public int ResultWeight;
        public bool HasResult;

        public Dextra(Graph graph, NodeGraph startNode, NodeGraph endNode) 
        {
            this.graph = graph;
            foreach(NodeGraph node in graph.NodeGraphs)
            {
                if(node.Equals(startNode))
                {
                    nodes.Add(node, new ItemDextra { Node = node, WeightNode = 0, IsPickOut = true});
                }
                else
                {
                    nodes.Add(node, new ItemDextra { Node = node, WeightNode = -1, IsPickOut = false });
                }
            }
            this.startNode = startNode;
            this.endNode = endNode;
            currentNode = nodes[startNode];

        }

        public void DoDextra()
        {
            if (IsAllVisited()) { HasResult = false; return; }//!
            if (currentNode.Node.Equals(endNode))
            {
                ResultWeight = currentNode.WeightNode;
                HasResult = true;
                //!
            }
            foreach (Edge edge in currentNode.Node.Edges)
            {
                ItemDextra workItemDextra = nodes[FindNeededNodeForEdge(edge, currentNode.Node)];
                if (workItemDextra.IsPickOut) continue;
                workItemDextra.WeightNode = MinWeight(workItemDextra.WeightNode, currentNode.WeightNode + edge.Weight);
            }
            int min = int.MaxValue;
            NodeGraph minNode = null;
            if (IsAllInfinity()) { HasResult = false; return; } //!
            foreach (NodeGraph node in nodes.Keys)
            {
                if (nodes[node].WeightNode < min && nodes[node].WeightNode != -1)
                {
                    min = nodes[node].WeightNode;
                    minNode = node;
                }
            }
            nodes[minNode].IsPickOut = true;
            currentNode = nodes[minNode];
            
            DoDextra();
        }

        private bool IsAllVisited()
        {
            foreach(ItemDextra itemDextra in nodes.Values)
            {
                if(!itemDextra.IsPickOut) return false;
            }
            return true;
        }

        private bool IsAllInfinity()
        {
            foreach (ItemDextra itemDextra in nodes.Values)
            {
                if (itemDextra.WeightNode != -1)
                    return false;
            }
            return true;
        }

        private NodeGraph FindNeededNodeForEdge(Edge edge, NodeGraph node)
        {
            NodeGraph workNode = null;
            NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
            NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
            if (node1.Equals(currentNode.Node))
                workNode = node2;
            else if (node2.Equals(currentNode.Node))
                workNode = node1;
            return workNode;
        }

        private int MinWeight(int oldWeight, int sum)
        {
            if (oldWeight == -1)
                return sum;
            return Math.Min(oldWeight, sum);
        }
    }
}
