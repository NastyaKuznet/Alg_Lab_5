using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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
        #endregion

        //флаги на то, какие кнопки были нажаты ранее
        #region
        bool wasPressedButtonCreateNewGraph = false;

        #endregion

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
            int firstIndexName = pathFolder.LastIndexOf("\\") + 1;
            NameGraph = pathFolder.Substring(firstIndexName, pathFolder.Length - firstIndexName);
        });

        //Mode
        public ICommand CreatingNodes => new CommandDelegate(param =>
        {
            //написать блокировку остальных кнопок
            MessageBox.Show("ep");
        });

        public ICommand CreateNode => new CommandDelegate(param =>
        {
            //написать блокировку остальных кнопок
            //CanDrawEllipsWithoutOverlay написать чтобы не было возможности что круги перекрывали друг друга
            //если ставить узлы сверху то они выходят за пределы канваса. Нужна проверка, что нельзя ставить узел туда

            graph.NodeGraphs.Add(new NodeGraph(countGraph.ToString(), PanelX, PanelY));

            Drawer drawer = new Drawer();
            drawer.DrawEllipsWithName(SizeNodeGraph, SizeNodeGraph, ColorFillNodeGraph, ColorStrokeNodeGraph, PanelX, PanelY, MainCanvas, countGraph.ToString());
            countGraph++;
        });

        //Down panel
        public ICommand Update => new CommandDelegate(param =>
        {
            if(wasPressedButtonCreateNewGraph && cVM.wasFinal)
            {
                NameGraph = cVM.NameGraph;
                pathFolder = cVM.PathFolder;

                graph = new Graph();
            }
            IsEnableButtonUpdate = false;
        });
    }
}
