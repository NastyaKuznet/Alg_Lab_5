using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.Algorithms
{
    public class ItemEdgeFord
    {
        public Edge edge;

        public int flow;

        public bool isPositive;
        public bool isVisited;

        public ItemEdgeFord(Edge edge)
        {
            this.edge = edge;
        }
    }
}
