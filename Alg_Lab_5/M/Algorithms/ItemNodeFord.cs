using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.Algorithms
{
    public class ItemNodeFord
    {
        public NodeGraph node;

        public bool isVisited;

        public ItemNodeFord(NodeGraph node)
        {
            this.node = node;
        }
    }
}
