using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.M.FolderGraph
{
    public class Edge
    {
        public int Id { get; set; }

        public TypeEdge Type { get; set; }
        public int Weight { get; set; }

        public double FirstPosX { get; set; }
        public double FirstPosY { get; set; }

        public double SecondPosX { get; set; }
        public double SecondPosY { get; set; }

        public Edge(int id)
        {
            Id = id;
        }
    }

    public enum TypeEdge
    {
        Base,
        Directed,
    }

}
