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

        public void BypassWeightedGraphInWidth(Graph graph)
        {
            BfsAlgorithm bfsAlgorithm = new BfsAlgorithm(graph);
            bfsAlgorithm.DoBFS();
            HasResult = true;
            Steps = bfsAlgorithm.StepsOfCanvases;
            Comments = bfsAlgorithm.comments;
            ButtonSteps = bfsAlgorithm.ButtonSteps;
        }

        public void BypassWeightedGraphInDepth(Graph graph) 
        {
            DfsAlgorithm dfsAlgorithm = new DfsAlgorithm(graph);
            dfsAlgorithm.DoDfs();
            HasResult = true;
            Steps = dfsAlgorithm.StepsOfCanvases;
            Comments = dfsAlgorithm.comments;
            ButtonSteps = dfsAlgorithm.ButtonSteps;
        }

        public void FindMaxThreadAcrossTransportNet(Graph graph)
        {
            FordFalkersonAlgorithm fordFalkersonAlgorithm = new FordFalkersonAlgorithm(graph);
            fordFalkersonAlgorithm.DoFordFalkerson();
            HasResult = true;
            Steps = fordFalkersonAlgorithm.StepsCanvases;
            Comments = fordFalkersonAlgorithm.comments;
            ButtonSteps = fordFalkersonAlgorithm.ButtonSteps;
        }

        public void BuildMinSpanningTree(Graph graph)
        {
            KruskalAlgorithm kruskalAlgorithm = new KruskalAlgorithm(graph);
            kruskalAlgorithm.GetSteps();
            HasResult = true;
            Steps = kruskalAlgorithm.StepsOfCanvases;
            Comments = kruskalAlgorithm.comments;
            ButtonSteps = kruskalAlgorithm.ButtonSteps;
        }

        public void FindMinPathBetweenTwoNodes(Graph graph, NodeGraph startNode, NodeGraph endNode)
        {
            Dextra dextra = new Dextra(graph, startNode, endNode);
            dextra.DoDextra();
            Result = dextra.ResultWeight;
            HasResult = dextra.HasResult;
            Steps = dextra.Steps;
            Comments = dextra.Comments;
            ButtonSteps = dextra.ButtonSteps;
        }
    }
}
