using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.FolderGraph
{
    public class NodeGraph
    {
        public string Name { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        public NodeGraph(string name, double posX, double posY)
        {
            Name = name;
            PosX = posX;
            PosY = posY;
        }
    }
}
