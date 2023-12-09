using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.FolderGraph
{
    public class NodeGraph
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        public List<Edge> Edges { get; set; } = new List<Edge>();

        public NodeGraph(int id, string name, double posX, double posY)
        {
            Id = id;
            Name = name;
            PosX = posX;
            PosY = posY;
        }
    }
}
