using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.V.FolderAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Alg_Lab_5.VM.FolderAlgorithmsVM
{
    public class BaseDextraVM : BaseVM
    {
        //для передачи
        public BaseDextraW baseDextraW;
        public MainVM mainVM;

        //возвращаемые значения
        public NodeGraph node1;
        public NodeGraph node2;
        public bool Done = false;
        public bool AllNodes = false;

        //инстументы
        Dictionary<string, NodeGraph> nameNodes = new Dictionary<string, NodeGraph>();

        //для mvvm
        private List<string> _listNodes = new List<string>();
        public List<string> ListNodes
        {
            get { return _listNodes; }
            set { _listNodes = value; OnPropertyChanged(); }
        }

        private string _selectedFirstNode;
        public string SelectedFirstNode
        {
            get { return _selectedFirstNode; }
            set
            {
                _selectedFirstNode = value;
                OnPropertyChanged();
            }
        }

        private List<string> _listNodesWithOutFirst = new List<string>();
        public List<string> ListNodesWithOutFirst
        {
            get { return _listNodesWithOutFirst; }
            set { _listNodesWithOutFirst = value; OnPropertyChanged(); }
        }

        private string _selectedSecondNode;
        public string SelectedSecondNode
        {
            get { return _selectedSecondNode; }
            set
            {
                _selectedSecondNode = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnableSecondComboBox = false;
        public bool IsEnableSecondComboBox
        {
            get { return _isEnableSecondComboBox; }
            set { _isEnableSecondComboBox = value; OnPropertyChanged(); }
        }

        private bool _isEnableButtonAccept = false;
        public bool IsEnableButtonAccept
        {
            get { return _isEnableButtonAccept; }
            set { _isEnableButtonAccept = value; OnPropertyChanged(); }
        }


        public BaseDextraVM(Graph graph, BaseDextraW baseDextraW, MainVM mainVM)
        {
            foreach (NodeGraph node in graph.NodeGraphs)
            {
                ListNodes.Add(node.Name);
                nameNodes.Add(node.Name, node);
            }
            this.baseDextraW = baseDextraW;
            this.mainVM = mainVM;
        }

        public ICommand AcceptBaseFirst => new CommandDelegate(param =>
        {
            if (string.IsNullOrEmpty(SelectedFirstNode) || string.IsNullOrWhiteSpace(SelectedFirstNode)) return;
            node1 = nameNodes[SelectedFirstNode];
            ListNodesWithOutFirst = new List<string>(ListNodes);
            ListNodesWithOutFirst.Remove(SelectedFirstNode);
            ListNodesWithOutFirst.Add("Все вершины");
            IsEnableSecondComboBox = true;
        });

        public ICommand AcceptBase => new CommandDelegate(param =>
        {
            if (string.IsNullOrEmpty(SelectedSecondNode) || string.IsNullOrWhiteSpace(SelectedSecondNode)) return;
            AllNodes = SelectedSecondNode.Equals("Все вершины");
            if(!AllNodes)
            {
                node2 = nameNodes[SelectedSecondNode];
            }
            IsEnableButtonAccept = true;
        });

        public ICommand Accept => new CommandDelegate(param =>
        {
            TextBlock nameAlgorithm = new TextBlock() { Text = "Название алгоритма: Алгоритм Дейкстры" };
            TextBlock textFirstNode = new TextBlock() { Text = "Начальная вершина: " + node1.Name };
            TextBlock textSecondNode = new TextBlock();
            if (AllNodes)
            {
                textSecondNode.Text = "Обход всех вершин";
            }
            else 
            {
                textSecondNode.Text = "Конечная вершина: " + node2.Name;
            }
            
            mainVM.BaseDataForAlgortithm.Children.Add(nameAlgorithm);
            mainVM.BaseDataForAlgortithm.Children.Add(textFirstNode);
            mainVM.BaseDataForAlgortithm.Children.Add(textSecondNode);
            baseDextraW.Close();
        });
    }
}
