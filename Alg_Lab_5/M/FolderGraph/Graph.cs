using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.FolderGraph
{
    public class Graph
    {
        public List<NodeGraph> NodeGraphs { get; set; } 

        public Graph() 
        {
            NodeGraphs = new List<NodeGraph>();
        }
    }
}
