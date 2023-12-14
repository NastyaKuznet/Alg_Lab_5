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

        public MainWindow MainWindow
        {
            get => default;
            set
            {
            }
        }

        public KruskalAlgorithm KruskalAlgorithm
        {
            get => default;
            set
            {
            }
        }

        public void BypassWeightedGraphInWidth(Graph graph)
        {
            try
            {
                BfsAlgorithm bfsAlgorithm = new BfsAlgorithm(graph);
                bfsAlgorithm.DoBFS();
                HasResult = true;
                Steps = bfsAlgorithm.StepsOfCanvases;
                Comments = bfsAlgorithm.comments;
                ButtonSteps = bfsAlgorithm.ButtonSteps;
            }
            catch
            {
                MessageBox.Show("Некорректный тип графа для данного алгоритма");
            }
        }

        public void BypassWeightedGraphInDepth(Graph graph) 
        {
            try
            {
                DfsAlgorithm dfsAlgorithm = new DfsAlgorithm(graph);
                dfsAlgorithm.DoDfs();
                HasResult = true;
                Steps = dfsAlgorithm.StepsOfCanvases;
                Comments = dfsAlgorithm.comments;
                ButtonSteps = dfsAlgorithm.ButtonSteps;
            }
            catch
            {
                MessageBox.Show("Некорректный тип графа для данного алгоритма");
            }
            
        }

        public void FindMaxThreadAcrossTrasportNet(Graph graph)
        {
            try
            {
                FordFalcersonN fordFalcerson = new FordFalcersonN(graph);
                Steps = fordFalcerson.Steps;
                ButtonSteps = fordFalcerson.ButtonSteps;
                Comments = fordFalcerson.Comments;
            }
            catch
            {
                MessageBox.Show("Некорректный тип графа для данного алгоритма");
            }
        }

        public void BuildMinSpanningTree(Graph graph)
        {
            try
            {
                KruskalAlgorithm kruskalAlgorithm = new KruskalAlgorithm(graph);
                kruskalAlgorithm.GetSteps();
                HasResult = true;
                Steps = kruskalAlgorithm.StepsOfCanvases;
                Comments = kruskalAlgorithm.comments;
                ButtonSteps = kruskalAlgorithm.ButtonSteps;
            }
            catch
            {
                MessageBox.Show("Некорректный тип графа для данного алгоритма");
            }
        }


        public void FindMinPathBetweenTwoNodes(Graph graph,NodeGraph startNode, NodeGraph endNode, bool allNodes)
        {
            try
            {
                Dextra dextra = new Dextra(graph, startNode, endNode, allNodes);
                dextra.DoDextra();
                Result = dextra.ResultWeight;
                HasResult = dextra.HasResult;
                Steps = dextra.Steps;
                Comments = dextra.Comments;
                ButtonSteps = dextra.ButtonSteps;
            }
            catch
            {
                MessageBox.Show("Некорректный тип графа для данного алгоритма");
            }
            
        }
    }
}
