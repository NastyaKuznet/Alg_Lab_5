using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.FolderGraph
{
    public class Graph
    {
        public string Name { get; set; }
        public LinkedList<NodeGraph> NodeGraphs { get; set; } 

        public Graph() 
        {
            NodeGraphs = new LinkedList<NodeGraph>();
        }
    }
}
