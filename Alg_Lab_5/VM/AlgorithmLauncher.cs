using Alg_Lab_5.M.Algorithms;
using Alg_Lab_5.M.FolderGraph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Alg_Lab_5.VM
{
    public class AlgorithmLauncher
    {
        public int Result;
        public bool HasResult;
        public List<Canvas> Steps = new List<Canvas>();
        public List<string> Comments = new List<string>();
        public ObservableCollection<Button> ButtonSteps = new ObservableCollection<Button>();

        public void BypassWeightedGraphInWidth()
        {

        }

        public void BypassWeightedGraphInDepth() 
        {
        
        }

        public void FindMaxThreadAcrossTrasportNet()
        {

        }

        public void BuildMinSpanningTree()
        {

        }

        public void FindMinPathBetweenTwoNodes(Graph graph,NodeGraph startNode, NodeGraph endNode, bool allNodes)
        {
            Dextra dextra = new Dextra(graph, startNode, endNode, allNodes);
            dextra.DoDextra();
            Result = dextra.ResultWeight;
            HasResult = dextra.HasResult;
            Steps = dextra.Steps;
            Comments = dextra.Comments;
            ButtonSteps = dextra.ButtonSteps;
        }
    }
}
