using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM
{
    public class AlgorithmLauncher
    {
        public AlgorithmLauncher(string nameAlgorithm)
        {
            switch (nameAlgorithm)
            {
                case ("Обход взвешенного графа в ширину"):
                    BypassWeightedGraphInWidth();
                    break;
                case ("Обход взвешенного графа в глубину"):
                    BypassWeightedGraphInDepth();
                    break;
                case ("Поиск максимального потока через транспортную сеть"):
                    FindMaxThreadAcrossTrasportNet();
                    break;
                case ("Построение минимального остовного дерева"):
                    BuildMinSpanningTree();
                    break;
                case ("Поиск кратчайшего пути между двумя вершинами графа"):
                    FindMinPathBetweenTwoNodes();
                    break;
            }
        }

        private void BypassWeightedGraphInWidth()
        {

        }

        private void BypassWeightedGraphInDepth() 
        {
        
        }

        private void FindMaxThreadAcrossTrasportNet()
        {

        }

        private void BuildMinSpanningTree()
        {

        }

        private void FindMinPathBetweenTwoNodes()
        {
            
        }
    }
}
