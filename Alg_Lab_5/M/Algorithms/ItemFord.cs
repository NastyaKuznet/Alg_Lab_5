using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.Algorithms
{
     class ItemFord
    {
        public Edge Edge { get; set; }
        public int Remained { get; set; }
        public int Pass { get; set; }

        public ItemFord(Edge edge, int remained, int pass) 
        {
            Edge = edge;
            Remained = remained;
            Pass = pass;
        }
    }
}
