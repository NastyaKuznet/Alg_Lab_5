using System;
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
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using Alg_Lab_5.M;
using Alg_Lab_5.M.Algorithms;
using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.V.FolderAlgorithms;
using Alg_Lab_5.V.FolderCreateNewGraph;
using Alg_Lab_5.V.FolderHelpWindow;
using Alg_Lab_5.VM.FolderAlgorithmsVM;
using Alg_Lab_5.VM.FolderCreateNewGraphVM;
using Alg_Lab_5.VM.FolderHelpVM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static Alg_Lab_5.M.ImportData;

namespace Alg_Lab_5.VM
{
    public class MainVM : BaseVM
    {
        CreateNewGraphW cUW = new CreateNewGraphW();
        CreateNewGraphVM cVM = new CreateNewGraphVM();
        HelpW helpW = new HelpW();
        HelpVM helpVM = new HelpVM();
        BaseDextraW bDW = new BaseDextraW();
        BaseDextraVM bDVM;

        DialogeOpen dialoge = new DialogeOpen();
        string pathFolder = null;
        Graph graph;
        int idEdge = 0;

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

        private ObservableCollection<string> _typeEdges = new ObservableCollection<string> { "Неориентированные", "Ориентированные" };
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

        private string[] _nameAlgorithms = {"Обход взвешенного графа в ширину", "Обход взвешенного графа в глубину", "Поиск максимального потока через транспортную сеть", "Построение минимального остовного дерева", "Поиск кратчайшего пути между двумя вершинами графа" };
        public string[] NameAlgorithms
        {
            get { return _nameAlgorithms; }
            set { _nameAlgorithms= value; OnPropertyChanged(); }
        }

        private string _selectedNameAlgorithm = "";
        public string SelectedNameAlgorithm
        {
            get { return _selectedNameAlgorithm; }
            set { _selectedNameAlgorithm = value; OnPropertyChanged(); }
        }
        
        private StackPanel _baseDataForAlgortithm = new StackPanel();
        public StackPanel BaseDataForAlgortithm
        {
            get { return _baseDataForAlgortithm; }
            set { _baseDataForAlgortithm = value; OnPropertyChanged(); }
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

        private bool _isEnableNamesAlgorithm = false;
        public bool IsEnableNamesAlgorithm
        {
            get { return _isEnableNamesAlgorithm;}
            set { _isEnableNamesAlgorithm = value; OnPropertyChanged(); }
        }

        private bool _isEnableButtonStartAlgorithm = false;
        
        public bool IsEnableButtonStartAlgorithm 
        {
            get { return _isEnableButtonStartAlgorithm; }
            set { _isEnableButtonStartAlgorithm = value; OnPropertyChanged(); }
        }

        #endregion

        //флаги на то, какие кнопки были нажаты ранее
        #region
        bool wasPressedButtonCreateNewGraph = false;
        bool wasOpenGraph = false;
        bool isCreatindNodesMood = false;
        bool isCreatindEdgeMood = false;
        bool wasChoiceTypeEdgesGraph = false;
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
            ClearAll();
            pathFolder = dialoge.CallFolderBrowserDialog();
            wasOpenGraph = !string.IsNullOrEmpty(pathFolder);
            if (wasOpenGraph)
            {
                graph = new FileProcessing().OpenGraph(pathFolder);
                int firstIndexName = pathFolder.LastIndexOf("\\") + 1;
                NameGraph = pathFolder.Substring(firstIndexName, pathFolder.Length - firstIndexName);

                List<int> unikIdEdge = new List<int>();
                foreach(NodeGraph node in graph.NodeGraphs)
                {
                    Drawer drawer = new Drawer();
                    foreach (Edge edge in node.Edges)
                    {
                        if (!unikIdEdge.Contains(edge.Id))
                        {
                            unikIdEdge.Add(edge.Id);
                            if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Base))
                            {
                                drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1);
                            }
                            else if (edge.Weight != 0 && edge.Type.Equals(TypeEdge.Base))
                            {
                                drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                            }
                            else if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Directed))
                            {
                                drawer.DrawDirectedLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1);
                            }
                            else if(edge.Weight == 0 && edge.Type.Equals(TypeEdge.Directed))
                            {
                                drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1, edge.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                            }
                            if(idEdge < edge.Id) idEdge = edge.Id;

                            NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                            NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);
                            InfoEdges.Add(new EdgeGraphVM() { Id = edge.Id, FromTo = $"{node1.Name}->{node2.Name}", IsDirected = edge.Type.Equals(TypeEdge.Directed), IsWeight = edge.Weight != 0});
                            ButtonEdge.Add(new Button() { CommandParameter = edge.Id, Content = "Изменить", Command = ChangeEdges });
                            ButtonCloseEdge.Add(new Button() { CommandParameter = edge.Id, Content = "X", IsEnabled = false, Command = DeleteEdges });
                        }
                    }
                    drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node.PosX, node.PosY, MainCanvas, node.Name);

                    NamesNode.Add(new NodeGraphVM(node.Id) { Name = node.Name, IsEnable = false });
                    ButtonNode.Add(new Button { Content = "Изменить", CommandParameter = node.Id, Command = ChangeSetting });
                    ButtonClose.Add(new Button { Content = "X", CommandParameter = node.Id, Command = DeleteNode });

                }
                CountNode = graph.NodeGraphs.Count;
                IsEnableButtonMood = true; IsEnableButtonCreatingNodes = true; IsEnableButtonSaveGraph = true; IsEnableButtonCreatingEdge = true; IsEnableTypeEdges = false; IsEnableNamesAlgorithm = true;
                wasChoiceTypeEdgesGraph = InfoEdges.Count != 0;
                if(wasChoiceTypeEdgesGraph)
                {
                    if (InfoEdges[0].IsDirected)
                        SelectedType = "Ориентированные";
                    else SelectedType = "Неориентированные";
                    IsHasWeight = InfoEdges[0].IsWeight;
                }
                idEdge++;
            }
        });

        private void ClearAll()
        {
            pathFolder = null;
            graph = null;
            idEdge = 0;
            NameGraph = "";
            CountNode = 0;
            MainCanvas = new Canvas();
            Mood = "Стандартный";
            NamesNode = new ObservableCollection<NodeGraphVM>();
            ButtonNode = new ObservableCollection<Button>();
            ButtonClose = new ObservableCollection<Button>();
            MatrixGraph = new DataTable();
            SelectedType = "Неориентированные";
            IsHasWeight = false;
            InfoEdges = new ObservableCollection<EdgeGraphVM>();
            ButtonEdge = new ObservableCollection<Button>();
            ButtonCloseEdge = new ObservableCollection<Button>();
            IsEnableButtonFile = true;
            IsEnableButtonCreateNewGraph = true;
            IsEnableButtonOpenGraph = true;
            IsEnableButtonMood = true;
            IsEnableButtonCreatingNodes = true;
            IsEnableButtonUpdate = false;
            IsEnableButtonCloseMood = false;
            IsEnableButtonSaveGraph = true;
            IsEnableButtonCreatingEdge = true;
            IsEnableTypeEdges = false;
        }

        //Mode
        public ICommand CreatingNodes => new CommandDelegate(param =>
        {
            if (!wasOpenGraph) return;
            IsEnableButtonFile = false; IsEnableButtonCreateNewGraph = false; IsEnableButtonOpenGraph = false; IsEnableButtonUpdate = false; IsEnableButtonCreatingEdge = false;
            isCreatindNodesMood = true; IsEnableButtonCloseMood = true; IsEnableNamesAlgorithm = false;
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

            NodeGraph node = new NodeGraph(CountNode,CountNode.ToString(), PanelX, PanelY);
            graph.NodeGraphs.AddLast(node);

            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, PanelX, PanelY, MainCanvas, CountNode.ToString());

            NamesNode.Add(new NodeGraphVM(node.Id) { Name = CountNode.ToString(), IsEnable = false });
            ButtonNode.Add(new Button {Content = "Изменить", CommandParameter = node.Id, Command = ChangeSetting });
            ButtonClose.Add(new Button { Content = "X", CommandParameter = node.Id, Command = DeleteNode });

            CountNode++;
        }

        private void DrawEdge()
        {
            wasChoiceTypeEdgesGraph = true;
            if (wasChoiceTypeEdgesGraph)
            {
                IsEnableTypeEdges = false;
            }

            switch (SelectedType)
            {
                case("Неориентированные"):
                    if (IsHasWeight) DrawBaseEdgeWeigh();
                    else DrawBaseEdge();
                    break;
                case ("Ориентированные"):
                    if (IsHasWeight) DrawDirectedEdgeWeihg();
                    else DrawDirectedEdge(); 
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
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, X1, Y1);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
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
                drawer.DrawBaseLine(node1.PosX, node1.PosY, node2.PosX, node2.PosY, MainCanvas, ColorForeGroundTextGraph, 1);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);

                Edge edge = new Edge(idEdge++) { Type = TypeEdge.Base, FirstPosX = node1.PosX, FirstPosY = node1.PosY,
                    SecondPosX = node2.PosX, SecondPosY = node2.PosY };
                node1.Edges.Add(edge);
                node2.Edges.Add(edge);

                InfoEdges.Add(new EdgeGraphVM() { Id = edge.Id, FromTo = $"{node1.Name}->{node2.Name}"});
                ButtonEdge.Add(new Button() {CommandParameter = edge.Id, Content = "Изменить", Command = ChangeEdges });
                ButtonCloseEdge.Add(new Button() {CommandParameter = edge.Id, Content = "X", IsEnabled = false, Command = DeleteEdges});
                isLine = false;
            }
        }
        
        private void DrawBaseEdgeWeigh()
        {
            Drawer drawer = new Drawer();
            if (isFirst && !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY))
            {
                X1 = PanelX;
                Y1 = PanelY;
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, X1, Y1);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
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
                drawer.DrawBaseLineWeight(node1.PosX, node1.PosY, node2.PosX, node2.PosY, MainCanvas, ColorForeGroundTextGraph, 1, 1, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);

                Edge edge = new Edge(idEdge++)
                {
                    Type = TypeEdge.Base,
                    FirstPosX = node1.PosX,
                    FirstPosY = node1.PosY,
                    SecondPosX = node2.PosX,
                    SecondPosY = node2.PosY,
                    Weight = 1
                };
                node1.Edges.Add(edge);
                node2.Edges.Add(edge);

                InfoEdges.Add(new EdgeGraphVM() { Id = edge.Id, FromTo = $"{node1.Name}->{node2.Name}", IsWeight = true, Weight = 1 });
                ButtonEdge.Add(new Button() { CommandParameter = edge.Id, Content = "Изменить", Command = ChangeEdges });
                ButtonCloseEdge.Add(new Button() { CommandParameter = edge.Id, Content = "X", IsEnabled = false, Command = DeleteEdges });
                isLine = false;
            }
        }

        private void DrawDirectedEdge()
        {
            Drawer drawer = new Drawer();
            if (isFirst && !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY))
            {
                X1 = PanelX;
                Y1 = PanelY;
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, X1, Y1);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
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
                drawer.DrawDirectedLine(node1.PosX, node1.PosY, node2.PosX, node2.PosY, MainCanvas, ColorForeGroundTextGraph, 1);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);

                Edge edge = new Edge(idEdge++)
                {
                    Type = TypeEdge.Directed,
                    FirstPosX = node1.PosX,
                    FirstPosY = node1.PosY,
                    SecondPosX = node2.PosX,
                    SecondPosY = node2.PosY
                };
                node1.Edges.Add(edge);
                node2.Edges.Add(edge);

                InfoEdges.Add(new EdgeGraphVM() { Id = edge.Id, FromTo = $"{node1.Name}->{node2.Name}", IsDirected = true });
                ButtonEdge.Add(new Button() { CommandParameter = edge.Id, Content = "Изменить", Command = ChangeEdges });
                ButtonCloseEdge.Add(new Button() { CommandParameter = edge.Id, Content = "X", IsEnabled = false, Command = DeleteEdges });
                isLine = false;
            }
        }

        private void DrawDirectedEdgeWeihg()
        {
            Drawer drawer = new Drawer();
            if (isFirst && !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY))
            {
                X1 = PanelX;
                Y1 = PanelY;
                NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, X1, Y1);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeSelectedNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
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
                drawer.DrawDirectedLineWeight(node1.PosX, node1.PosY, node2.PosX, node2.PosY, MainCanvas, ColorForeGroundTextGraph, 1, 1, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);

                Edge edge = new Edge(idEdge++)
                {
                    Type = TypeEdge.Directed,
                    FirstPosX = node1.PosX,
                    FirstPosY = node1.PosY,
                    SecondPosX = node2.PosX,
                    SecondPosY = node2.PosY,
                    Weight = 1
                };
                node1.Edges.Add(edge);
                node2.Edges.Add(edge);

                InfoEdges.Add(new EdgeGraphVM() { Id = edge.Id, FromTo = $"{node1.Name}->{node2.Name}", IsDirected = true, IsWeight = true, Weight = 1 });
                ButtonEdge.Add(new Button() { CommandParameter = edge.Id, Content = "Изменить", Command = ChangeEdges });
                ButtonCloseEdge.Add(new Button() { CommandParameter = edge.Id, Content = "X", IsEnabled = false, Command = DeleteEdges });
                isLine = false;
            }
        }

        public ICommand CreatingEdges => new CommandDelegate(param =>
        {
            if (!wasOpenGraph) return;
            IsEnableButtonFile = false; IsEnableButtonCreateNewGraph = false; IsEnableButtonOpenGraph = false; IsEnableButtonUpdate = false; IsEnableButtonCreatingNodes = false;
            isCreatindEdgeMood = true; IsEnableButtonCloseMood = true; IsEnableTypeEdges = true; IsEnableNamesAlgorithm = false;
            if (wasChoiceTypeEdgesGraph) IsEnableTypeEdges = false;
            Mood = Moods["Edges"];
        });

        //Help
        public ICommand OpenHelp => new CommandDelegate(param =>
        {
            helpW.DataContext = helpVM;
            helpW.Show();
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
                    
                    List<object> ids = new List<object>();
                    foreach(Edge edge in currentNode.Value.Edges)
                    {
                        ids.Add(edge.Id);   
                    }
                    foreach(object id in ids)
                    {
                        DeleteEdgeM(id);
                    }
                    drawer.DrawEllipsWithName(SizeNodeGraph + 1, SizeNodeGraph + 1, ColorBackground, ColorBackground, currentNode.Value.PosX, currentNode.Value.PosY, MainCanvas, "");
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

        public ICommand ChangeEdges => new CommandDelegate(param =>
        {
            foreach (EdgeGraphVM edgeVM in InfoEdges)
            {
                if(edgeVM.Id.Equals((int)param))
                {
                    if(edgeVM.IsWeight)
                        edgeVM.IsEnableWeight = true;
                    break;
                }
            }
            foreach(Button but in ButtonEdge)
            {
                if(but.CommandParameter.Equals((int)param))
                {
                    but.Content = "Сохранить";
                    but.Command = SaveEdges;
                    break;
                }
            }
            foreach(Button but in ButtonCloseEdge)
            {
                if(but.CommandParameter.Equals((int)param))
                {
                    but.IsEnabled = true;
                    but.Command = DeleteEdges;
                    break;
                }
            }
        });

        public ICommand SaveEdges => new CommandDelegate(param =>
        {
            int weigh = 0;
            foreach (EdgeGraphVM edgeVM in InfoEdges)
            {
                if (edgeVM.Id.Equals((int)param))
                {
                    if (edgeVM.IsWeight)
                    {
                        weigh = edgeVM.Weight;
                        edgeVM.IsEnableWeight = false;
                        break;
                    }
                }
            }
            bool canbreak = false;
            foreach (NodeGraph node in graph.NodeGraphs)
            {
                foreach(Edge edge in node.Edges)
                {
                    if(edge.Id.Equals((int)param) && edge.Weight != 0)
                    {
                        edge.Weight = weigh;
                        canbreak = true;
                        Drawer drawer = new Drawer();
                        NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                        NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                        drawer.DrawBaseLineWeight(node1.PosX, node1.PosY, node2.PosX, node2.PosY, MainCanvas, ColorForeGroundTextGraph, 1, weigh, ColorStrokeRectangleOnEdgeGraph, ColorFillRectangleOnEndeGraph);
                        drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                        drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);
                        break;
                    }
                }
                if (canbreak) break;
            }
            foreach (Button but in ButtonEdge)
            {
                if (but.CommandParameter.Equals((int)param))
                {
                    but.Content = "Изменить";
                    but.Command = ChangeEdges;
                    break;
                }
            }
            foreach (Button but in ButtonCloseEdge)
            {
                if (but.CommandParameter.Equals((int)param))
                {
                    but.IsEnabled = false;
                    break;
                }
            }
        });

        public ICommand DeleteEdges => new CommandDelegate(param =>
        {
            DeleteEdgeM(param);
            if(InfoEdges.Count == 0)
            {
                wasChoiceTypeEdgesGraph = false;
                IsEnableTypeEdges = true;
            }
        });

        private void DeleteEdgeM(object param)
        {
            Edge remov = new Edge(-1);
            bool canbreak = false;
            foreach (NodeGraph node in graph.NodeGraphs)
            {
                foreach (Edge edge in node.Edges)
                {
                    if (edge.Id.Equals((int)param))
                    {
                        remov = edge;
                        canbreak = true;
                        Drawer drawer = new Drawer();
                        NodeGraph node1 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.FirstPosX, edge.FirstPosY);
                        NodeGraph node2 = drawer.FindNodeInTouch(graph.NodeGraphs, edge.SecondPosX, edge.SecondPosY);
                        if (edge.Weight == 0 && !edge.Type.Equals(TypeEdge.Directed))
                        {
                            drawer.DrawBaseLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorBackground, 2);
                            foreach (Edge edge1 in node1.Edges)
                            {
                                if (node2.Edges.Contains(edge1) && !edge.Equals(edge1))
                                {
                                    drawer.DrawBaseLine(edge1.FirstPosX, edge1.FirstPosY, edge1.SecondPosX, edge1.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1);
                                }
                            }
                        }
                        else if (edge.Weight != 0 && !edge.Type.Equals(TypeEdge.Directed))
                        {
                            drawer.DrawBaseLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorBackground, 2, -1, ColorBackground, ColorBackground);
                            foreach (Edge edge1 in node1.Edges)
                            {
                                if (node2.Edges.Contains(edge1) && !edge.Equals(edge1))
                                {
                                    drawer.DrawBaseLineWeight(edge1.FirstPosX, edge1.FirstPosY, edge1.SecondPosX, edge1.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1, edge1.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph);
                                }
                            }
                        }
                        else if (edge.Weight == 0 && edge.Type.Equals(TypeEdge.Directed))
                        {
                            drawer.DrawDirectedLine(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorBackground, 2);
                            foreach (Edge edge1 in node1.Edges)
                            {
                                if (node2.Edges.Contains(edge1) && !edge.Equals(edge1))
                                {
                                    drawer.DrawDirectedLine(edge1.FirstPosX, edge1.FirstPosY, edge1.SecondPosX, edge1.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1);
                                }
                            }
                        }
                        else if(edge.Weight != 0 && edge.Type.Equals(TypeEdge.Directed))
                        {
                            drawer.DrawDirectedLineWeight(edge.FirstPosX, edge.FirstPosY, edge.SecondPosX, edge.SecondPosY, MainCanvas, ColorBackground, 2, -1, ColorBackground, ColorBackground);
                            foreach (Edge edge1 in node1.Edges)
                            {
                                if (node2.Edges.Contains(edge1) && !edge.Equals(edge1))
                                {
                                    drawer.DrawDirectedLineWeight(edge1.FirstPosX, edge1.FirstPosY, edge1.SecondPosX, edge1.SecondPosY, MainCanvas, ColorForeGroundTextGraph, 1, edge1.Weight, ColorStrokeRectangleOnEdgeGraph, ColorFillNodeGraph ) ;
                                }
                            }
                        }

                        drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node1.PosX, node1.PosY, MainCanvas, node1.Name);
                        drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, node2.PosX, node2.PosY, MainCanvas, node2.Name);
                        break;
                    }
                }
                if (canbreak)
                {
                    node.Edges.Remove(remov);
                    break;
                };
            }
            EdgeGraphVM remove = new EdgeGraphVM();
            foreach (EdgeGraphVM edgeVM in InfoEdges)
            {
                if (edgeVM.Id.Equals((int)param))
                {
                    remove = edgeVM;
                    break;
                }
            }
            InfoEdges.Remove(remove);
            Button remo = new Button();
            foreach (Button but in ButtonEdge)
            {
                if (but.CommandParameter.Equals((int)param))
                {
                    remo = but;
                    break;
                }
            }
            ButtonEdge.Remove(remo);
            foreach (Button but in ButtonCloseEdge)
            {
                if (but.CommandParameter.Equals((int)param))
                {
                    remo = but;
                    break;
                }
            }
            ButtonCloseEdge.Remove(remo);
        }

        public ICommand StartBaseAlogorithm => new CommandDelegate(param =>
        {
            switch (SelectedNameAlgorithm)
            {
                case ("Обход взвешенного графа в ширину"):

                    break;
                case ("Обход взвешенного графа в глубину"):

                    break;
                case ("Поиск максимального потока через транспортную сеть"):

                    break;
                case ("Построение минимального остовного дерева"):

                    break;
                case ("Поиск кратчайшего пути между двумя вершинами графа"):
                    bDVM = new BaseDextraVM(graph, bDW, this);
                    bDW.DataContext = bDVM;
                    bDW.ShowDialog();
                    IsEnableNamesAlgorithm = false;
                    IsEnableButtonStartAlgorithm = true;
                    break;
            }
            
        });

        public ICommand StartAlgorithm => new CommandDelegate(param =>
        {
            AlgorithmLauncher algorithmLauncher = new AlgorithmLauncher();
            switch(SelectedNameAlgorithm)
            {
                case ("Обход взвешенного графа в ширину"):
                    algorithmLauncher.BypassWeightedGraphInWidth();
                break;
                case ("Обход взвешенного графа в глубину"):
                    algorithmLauncher.BypassWeightedGraphInDepth();
                break;
                case ("Поиск максимального потока через транспортную сеть"):
                    algorithmLauncher.FindMaxThreadAcrossTrasportNet();
                break;
                case ("Построение минимального остовного дерева"):
                    algorithmLauncher.BuildMinSpanningTree();
                break;
                case ("Поиск кратчайшего пути между двумя вершинами графа"):
                    algorithmLauncher.FindMinPathBetweenTwoNodes(graph, bDVM.node1, bDVM.node2);
                break;
            }
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
                IsEnableButtonMood = true; IsEnableButtonCreatingNodes = true; IsEnableButtonSaveGraph = true; IsEnableButtonCreatingEdge = true; IsEnableNamesAlgorithm = true;
            }
            IsEnableButtonUpdate = false;
        });

        public ICommand CloseMood => new CommandDelegate(param =>
        {
            if(isCreatindNodesMood)
            {
                IsEnableButtonFile = true; IsEnableButtonCreateNewGraph = true; IsEnableButtonOpenGraph = true; IsEnableButtonUpdate = true; IsEnableButtonCreatingEdge = true;
                isCreatindNodesMood = false; IsEnableNamesAlgorithm = true;
                IsEnableButtonCloseMood = false;
                Mood = Moods["Standart"];
            }
            if(isCreatindEdgeMood)
            {
                IsEnableButtonFile = true; IsEnableButtonCreateNewGraph = true; IsEnableButtonOpenGraph = true; IsEnableButtonUpdate = true; IsEnableButtonCreatingNodes = true;
                isCreatindEdgeMood = false; IsEnableNamesAlgorithm = true;
                IsEnableButtonCloseMood = false; IsEnableTypeEdges = false;
                Mood = Moods["Standart"];
            }
        });

        public ICommand SaveGraph => new CommandDelegate(param =>
        {
            FileProcessing fileProcessing = new FileProcessing();
            fileProcessing.SaveGraph(pathFolder, graph);
            fileProcessing.SaveMatrix(pathFolder, new WorkerMatrix().CreateMatrix(graph.NodeGraphs));
        });
    }
}
