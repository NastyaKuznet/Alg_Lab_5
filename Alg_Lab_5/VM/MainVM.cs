﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using Alg_Lab_5.M;
using Alg_Lab_5.M.Algorithms;
using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.V.FolderCreateNewGraph;
using Alg_Lab_5.VM.FolderCreateNewGraphVM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.VM
{
    public class MainVM: BaseVM
    {
        CreateNewGraphW cUW = new CreateNewGraphW();
        CreateNewGraphVM cVM = new CreateNewGraphVM();

        DialogeOpen dialoge = new DialogeOpen();
        string pathFolder = null;
        int countGraph = 0;
        Graph graph;

        private string _nameGraph = "";
        public string NameGraph
        {
            get { return _nameGraph; }
            set
            {
                _nameGraph = value;
                OnPropertyChanged();
            }
        }
        private int _countNode = 0;
        public int CountNode
        {
            get { return _countNode; }
            set
            {
                _countNode = value;
                OnPropertyChanged();
            }
        }

        private Canvas _mainCanvas = new Canvas();
        public Canvas MainCanvas
        { 
            get { return _mainCanvas; }
            set {
                _mainCanvas = value;
                OnPropertyChanged();
            }
        }

        private double _panelX;
        public double PanelX
        {
            get { return _panelX; }
            set
            {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                OnPropertyChanged();
            }
        }
        private double _panelY;
        public double PanelY
        {
            get { return _panelY; }
            set
            {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                OnPropertyChanged();
            }
        }
        private string _mood = "Стандартный";
        public string Mood
        {
            get { return _mood; }
            set
            {
                _mood = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NodeGraphVM> _namesNode = new ObservableCollection<NodeGraphVM>();
        public ObservableCollection<NodeGraphVM> NamesNode
        {
            get { return _namesNode; }
            set
            {
                _namesNode = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Button> _buttonNode = new ObservableCollection<Button>();
        public ObservableCollection<Button> ButtonNode
        {
            get { return _buttonNode; }
            set
            {
                _buttonNode = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Button> _buttonClose = new ObservableCollection<Button>();
        public ObservableCollection<Button> ButtonClose
        {
            get { return _buttonClose; }
            set
            {
                _buttonClose = value;
                OnPropertyChanged();
            }
        }

        private DataTable _matrixGraph = new DataTable();
        public DataTable MatrixGraph 
        {
            get { return _matrixGraph; }
            set
            {
                _matrixGraph = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _typeEdges = new ObservableCollection<string> { "Неориентированные", "Ориентированные"};
        public ObservableCollection<string> TypeEdges
        {
            get { return _typeEdges; }
            set
            {
                _typeEdges = value;
                OnPropertyChanged();
            }
        }

        private string _selectedType = "Неориентированные";
        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        private bool _isHasWeight = false;
        public bool IsHasWeight
        {
            get { return _isHasWeight; }
            set
            {
                _isHasWeight = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EdgeGraphVM> _infoEdges = new ObservableCollection<EdgeGraphVM>();
        public ObservableCollection<EdgeGraphVM> InfoEdges
        {
            get { return _infoEdges; }
            set
            {
                _infoEdges = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Button> _buttonEdge = new ObservableCollection<Button>();
        public ObservableCollection<Button> ButtonEdge
        {
            get { return _buttonEdge; }
            set
            {
                _buttonEdge = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Button> _buttonCloseEdge = new ObservableCollection<Button>();
        public ObservableCollection<Button> ButtonCloseEdge
        {
            get { return _buttonCloseEdge; }
            set
            {
                _buttonCloseEdge = value;
                OnPropertyChanged();
            }
        }

        //куча флагов по блокировке кнопок и других элементов
        #region
        private bool _isEnableButtonFile = true;
        public bool IsEnableButtonFile 
        {
            get { return _isEnableButtonFile; }
            set
            {
                _isEnableButtonFile = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonCreateNewGraph = true;
        public bool IsEnableButtonCreateNewGraph
        {
            get { return _isEnableButtonCreateNewGraph; }
            set
            {
                _isEnableButtonCreateNewGraph = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonOpenGraph = true;
        public bool IsEnableButtonOpenGraph
        {
            get { return _isEnableButtonOpenGraph; }
            set
            {
                _isEnableButtonOpenGraph = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonMood = false;
        public bool IsEnableButtonMood
        {
            get { return _isEnableButtonMood; }
            set
            {
                _isEnableButtonMood = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonCreatingNodes = false;
        public bool IsEnableButtonCreatingNodes
        {
            get { return _isEnableButtonCreatingNodes; }
            set
            {
                _isEnableButtonCreatingNodes = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonUpdate = false;
        public bool IsEnableButtonUpdate
        {
            get { return _isEnableButtonUpdate; }
            set
            {
                _isEnableButtonUpdate = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonCloseMood = false;
        public bool IsEnableButtonCloseMood
        {
            get { return _isEnableButtonCloseMood; }
            set
            {
                _isEnableButtonCloseMood = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnableButtonSaveGraph = false;
        public bool IsEnableButtonSaveGraph
        {
            get { return _isEnableButtonSaveGraph; }
            set
            {
                _isEnableButtonSaveGraph = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableButtonCreatingEdge = false;
        public bool IsEnableButtonCreatingEdge
        {
            get { return _isEnableButtonCreatingEdge; }
            set
            {
                _isEnableButtonCreatingEdge = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnableTypeEdges = false;
        public bool IsEnableTypeEdges
        {
            get { return _isEnableTypeEdges; }
            set
            {
                _isEnableTypeEdges = value;
                OnPropertyChanged();
            }
        }

        #endregion

        //флаги на то, какие кнопки были нажаты ранее
        #region
        bool wasPressedButtonCreateNewGraph = false;
        bool wasOpenGraph = false;
        bool isCreatindNodesMood = false;
        bool isCreatindEdgeMood = false;
        #endregion
        public MainVM()
        {
            
        }

        //File
        public ICommand CreateNewGraph => new CommandDelegate(param =>
        {
            cVM.createGraphW = cUW;
            cUW.DataContext = cVM;
            cUW.ShowDialog();
            wasPressedButtonCreateNewGraph = true;
            IsEnableButtonUpdate = true;
        });

        public ICommand OpenGraph => new CommandDelegate(param =>
        {
            pathFolder = dialoge.CallFolderBrowserDialog();
            wasOpenGraph = !string.IsNullOrEmpty(pathFolder);
            if (wasOpenGraph)
            {
                graph = new FileProcessing().OpenGraph(pathFolder);
                int firstIndexName = pathFolder.LastIndexOf("\\") + 1;
                NameGraph = pathFolder.Substring(firstIndexName, pathFolder.Length - firstIndexName);

                foreach(NodeGraph node in graph.NodeGraphs)
                {
                    Drawer drawer = new Drawer();
                    drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node.PosX, node.PosY, MainCanvas, node.Name);
                    AddElementsInSettingPanel(node.Id, node.Name);
                }

                IsEnableButtonMood = true; IsEnableButtonCreatingNodes = true; IsEnableButtonSaveGraph = true; IsEnableButtonCreatingEdge = true;
            }
        });

        //Mode
        public ICommand CreatingNodes => new CommandDelegate(param =>
        {
            if (!wasOpenGraph) return;
            IsEnableButtonFile = false; IsEnableButtonCreateNewGraph = false; IsEnableButtonOpenGraph = false; IsEnableButtonUpdate = false; IsEnableButtonCreatingEdge = false;
            isCreatindNodesMood = true; IsEnableButtonCloseMood = true;
            Mood = Moods["Nodes"];
        });

        public ICommand CreateNode => new CommandDelegate(param =>
        {
            if (isCreatindNodesMood) DrawNode();
            if (isCreatindEdgeMood) DrawEdge();
        });

        private void DrawNode()
        {
            Drawer drawer = new Drawer();
            if (!isCreatindNodesMood || !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY) || !drawer.CanDrawNearEdge(PanelX, PanelY)) return;

            NodeGraph node = new NodeGraph(countGraph.ToString(), PanelX, PanelY);
            graph.NodeGraphs.AddLast(node);

            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, PanelX, PanelY, MainCanvas, countGraph.ToString());

            AddElementsInSettingPanel(node.Id, countGraph.ToString());

            countGraph++;
            CountNode++;
        }

        private void AddElementsInSettingPanel(int id, string name)
        {
            AddNamesNodes(id, name); 
            AddButtonNodes(id ); 
            AddButtonCancelNodes(id);
        }

        private void AddNamesNodes(int id, string name)
        {
            NodeGraphVM nodeGrapgVM = new NodeGraphVM(id);
            nodeGrapgVM.Name = name;
            nodeGrapgVM.IsEnable = false;
            NamesNode.Add(nodeGrapgVM);
        }

        private void AddButtonNodes(int id)
        {
            Button but = new Button();
            but.Content = "Изменить";
            but.CommandParameter = id;
            but.Command = ChangeSetting;
            ButtonNode.Add(but);
        }
        private void AddButtonCancelNodes(int id)
        {
            Button buto = new Button();
            buto.Content = "X";
            buto.CommandParameter = id;
            buto.IsEnabled = false;
            buto.Command = DeleteNode;
            ButtonClose.Add(buto);
        }

        private void DrawEdge()
        {
            switch (SelectedType)
            {
                case("Неориентированные"):
                    if (IsHasWeight) { }
                    else DrawBaseEdge();
                    break;
                case ("Ориентированные"):
                    if(IsHasWeight) { }
                    else { }
                    break;
            }
        }

        bool isFirst = true;
        bool isLine = false;
        double X1 = 0;
        double Y1 = 0;
        double X2 = 0;
        double Y2 = 0;
        private void DrawBaseEdge()
        {
            Drawer drawer = new Drawer();
            if (isFirst && !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY))
            {
                X1 = PanelX;
                Y1 = PanelY;
                isFirst = false;
            }
            else if (!isFirst && !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY))
            {
                X2 = PanelX;
                Y2 = PanelY;
                isFirst = true;
                isLine = true;
            }
            if (isLine)
            {
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, X1, Y1);
                NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, X2, Y2);
                drawer.DrawBaseLine(node1.PosX, node1.PosY, node2.PosX, node2.PosY, MainCanvas);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);

                Edge edge = new Edge() { Type = TypeEdge.Base, FirstPosX = node1.PosX, FirstPosY = node1.PosY,
                    SecondPosX = node2.PosX, SecondPosY = node2.PosY };
                node1.Edges.Add(edge);
                node2.Edges.Add(edge);

                InfoEdges.Add(new EdgeGraphVM() { FromTo = $"{node1.Name}->{node2.Name}"});
                ButtonEdge.Add(new Button() {CommandParameter = edge.Id, Content = "Изменить" });
                ButtonCloseEdge.Add(new Button() {CommandParameter = edge.Id, Content = "X", IsEnabled = false });
                isLine = false;
            }
        }


        public ICommand CreatingEdges => new CommandDelegate(param =>
        {
            if (!wasOpenGraph) return;
            IsEnableButtonFile = false; IsEnableButtonCreateNewGraph = false; IsEnableButtonOpenGraph = false; IsEnableButtonUpdate = false; IsEnableButtonCreatingNodes = false;
            isCreatindEdgeMood = true; IsEnableButtonCloseMood = true; IsEnableTypeEdges = true;
            Mood = Moods["Edges"];
        });

        //Right panel
        public ICommand ChangeSetting => new CommandDelegate(param =>
        {
            //учитывать блоки
            if (!(wasPressedButtonCreateNewGraph && cVM.wasFinal || wasOpenGraph)) return;
            foreach (NodeGraphVM nodeGraphVM in NamesNode)
            {
                if (nodeGraphVM.Id.Equals((int)param))
                {
                    nodeGraphVM.IsEnable = true;
                }
            }
            foreach (Button button in ButtonNode)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    button.Content = "Сохранить";
                    button.Command = SaveSettingNode;
                }
            }
            foreach (Button button in ButtonClose)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    button.IsEnabled = true;
                }
            }
        });

        public ICommand SaveSettingNode => new CommandDelegate(param =>
        {
            string newName = null;
            foreach (NodeGraphVM nodeGraphVM in NamesNode)
            {
                if (nodeGraphVM.Id.Equals((int)param))
                {
                    nodeGraphVM.IsEnable = false;
                    newName = nodeGraphVM.Name;
                }
            }
            foreach (NodeGraph nodeGraph in graph.NodeGraphs)
            {
                if (nodeGraph.Id.Equals((int)param))
                {
                    if (newName != null)
                    {
                        nodeGraph.Name = newName;
                        Drawer drawer = new Drawer();
                        drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, nodeGraph.PosX, nodeGraph.PosY, MainCanvas, nodeGraph.Name);
                    }
                }
            }
            foreach (Button button in ButtonNode)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    button.Content = "Изменить";
                    button.Command = ChangeSetting;
                }
            }
            foreach (Button button in ButtonClose)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    button.IsEnabled = false;
                }
            }
        });

        public ICommand DeleteNode => new CommandDelegate(param =>
        {
            LinkedListNode<NodeGraph> currentNode = graph.NodeGraphs.First;
            while (currentNode != null)
            {
                if (currentNode.Value.Id.Equals((int)param))
                {
                    LinkedListNode<NodeGraph> del = currentNode;
                    Drawer drawer = new Drawer();
                    drawer.DrawEllipsWithName(SizeNodeGraph + 1, SizeNodeGraph + 1, (SolidColorBrush)new BrushConverter().ConvertFrom("#f0f8ff"), (SolidColorBrush)new BrushConverter().ConvertFrom("#f0f8ff"), currentNode.Value.PosX, currentNode.Value.PosY, MainCanvas, "");
                    currentNode = currentNode.Next;
                    graph.NodeGraphs.Remove(del);
                    continue;
                }
                currentNode = currentNode.Next;
            }
            NodeGraphVM delNodeVM = new NodeGraphVM(-100); 
            foreach (NodeGraphVM nodeGraphVM in NamesNode)
            {
                if (nodeGraphVM.Id.Equals((int)param))
                {
                    delNodeVM = nodeGraphVM;
                }
            }
            NamesNode.Remove(delNodeVM);

            Button delBut = new Button();
            foreach (Button button in ButtonNode)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    delBut = button;
                }
            }
            ButtonNode.Remove(delBut);

            foreach (Button button in ButtonClose)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    delBut = button;
                }
            }
            ButtonClose.Remove(delBut);
            CountNode--;

        });

        public ICommand UpdateMatrix => new CommandDelegate(param =>
        {
            WorkerMatrix workerMatrix = new WorkerMatrix();
            MatrixGraph = workerMatrix.CreateMatrix(graph.NodeGraphs);
            List<string> list = new List<string>();
            DfsAlgorithm dfsAlgorithm = new DfsAlgorithm();
            //list = dfsAlgorithm.Dfs(graph);
            //int a = 0;
        });

        //Down panel
        public ICommand Update => new CommandDelegate(param =>
        {
            if(wasPressedButtonCreateNewGraph && cVM.wasFinal)
            {
                NameGraph = cVM.NameGraph;
                pathFolder = cVM.PathFolder;

                graph = new Graph();
                graph.Name = NameGraph;
                wasOpenGraph = true;
                IsEnableButtonSaveGraph = true; IsEnableButtonCreatingEdge = true;
            }
            IsEnableButtonUpdate = false;
        });

        public ICommand CloseMood => new CommandDelegate(param =>
        {
            if(isCreatindNodesMood)
            {
                IsEnableButtonFile = true; IsEnableButtonCreateNewGraph = true; IsEnableButtonOpenGraph = true; IsEnableButtonUpdate = true; IsEnableButtonCreatingEdge = true;
                isCreatindNodesMood = false;
                IsEnableButtonCloseMood = false;
                Mood = Moods["Standart"];
            }
            if(isCreatindEdgeMood)
            {
                IsEnableButtonFile = true; IsEnableButtonCreateNewGraph = true; IsEnableButtonOpenGraph = true; IsEnableButtonUpdate = true; IsEnableButtonCreatingNodes = true;
                isCreatindEdgeMood = false; 
                IsEnableButtonCloseMood = false; IsEnableTypeEdges = false;
                Mood = Moods["Standart"];
            }
        });

        public ICommand SaveGraph => new CommandDelegate(param =>
        {
            new FileProcessing().SaveGraph(pathFolder, graph);
        });
    }
}
