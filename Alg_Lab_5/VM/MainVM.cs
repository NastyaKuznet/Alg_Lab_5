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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using Alg_Lab_5.M;
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
            IsEnableButtonFile = false; IsEnableButtonCreateNewGraph = false; IsEnableButtonOpenGraph = false; IsEnableButtonUpdate = false;
            isCreatindNodesMood = true; IsEnableButtonCloseMood = true;
            Mood = Moods["Nodes"];
        });

        public ICommand CreateNode => new CommandDelegate(param =>
        {
            Drawer drawer = new Drawer();
            if (!isCreatindNodesMood || !drawer.CanDrawEllipsWithoutOverlay(graph.NodeGraphs, PanelX, PanelY) || !drawer.CanDrawNearEdge(PanelX, PanelY)) return;

            NodeGraph node = new NodeGraph(countGraph.ToString(), PanelX, PanelY);
            graph.NodeGraphs.AddLast(node);

            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, PanelX, PanelY, MainCanvas, countGraph.ToString());
            
            AddElementsInSettingPanel(node.Id, countGraph.ToString());

            countGraph++;
            CountNode++;
        });


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

        public ICommand CreatingEdges => new CommandDelegate(param =>
        {
            if (!wasOpenGraph) return;
            IsEnableButtonFile = false; IsEnableButtonCreateNewGraph = false; IsEnableButtonOpenGraph = false; IsEnableButtonUpdate = false; IsEnableButtonCreatingNodes = false;
            isCreatindEdgeMood = true; IsEnableButtonCloseMood = true;
            Mood = Moods["Nodes"];
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
            foreach(Button button in ButtonNode)
            {
                if (button.CommandParameter.Equals((int)param))
                {
                    button.Content = "Сохранить";
                    button.Command = SaveSettingNode;
                }
            }
            foreach(Button button in ButtonClose) 
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
                IsEnableButtonFile = true; IsEnableButtonCreateNewGraph = true; IsEnableButtonOpenGraph = true; IsEnableButtonUpdate = true;
                isCreatindNodesMood = false;
                IsEnableButtonCloseMood = false;
                Mood = Moods["Standart"];
            }
        });

        public ICommand SaveGraph => new CommandDelegate(param =>
        {
            new FileProcessing().SaveGraph(pathFolder, graph);
        });
    }
}
