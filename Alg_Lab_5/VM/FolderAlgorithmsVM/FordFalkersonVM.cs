using Alg_Lab_5.M.FolderGraph;
using Alg_Lab_5.V.FolderAlgorithms;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Alg_Lab_5.VM.FolderAlgorithmsVM;

public class FordFalkersonVM : BaseVM
{
    public NodesForFordFalkersonW FordFalkersonW;
    public MainVM mainVM;

    public NodeGraph StartNode;
    public NodeGraph EndNode;
    
    Dictionary<string, NodeGraph> nameNodes = new Dictionary<string, NodeGraph>();
    
    private List<string> _listNodes = new List<string>();
    public List<string> ListNodes
    {
        get { return _listNodes; }
        set 
        {   _listNodes = value;
            OnPropertyChanged(); 
        }
    }
    
    public FordFalkersonVM(Graph graph, NodesForFordFalkersonW nodesForFordFalkersonW, MainVM mainVM)
    {
        foreach (NodeGraph node in graph.NodeGraphs)
        {
            ListNodes.Add(node.Name);
            nameNodes.Add(node.Name, node);
        }
        FordFalkersonW = nodesForFordFalkersonW;
        this.mainVM = mainVM;
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
    
    public ICommand AcceptBaseFirst => new CommandDelegate(param =>
    {
        if (string.IsNullOrEmpty(SelectedFirstNode) || string.IsNullOrWhiteSpace(SelectedFirstNode)) return;
        StartNode = nameNodes[SelectedFirstNode];
        ListNodesWithOutFirst = new List<string>(ListNodes);
        ListNodesWithOutFirst.Remove(SelectedFirstNode);
        IsEnableSecondComboBox = true;
    });

    public ICommand AcceptBase => new CommandDelegate(param =>
    {
        if (string.IsNullOrEmpty(SelectedSecondNode) || string.IsNullOrWhiteSpace(SelectedSecondNode)) return;
        EndNode = nameNodes[SelectedSecondNode];
        IsEnableButtonAccept = true;
    });

    public ICommand Accept => new CommandDelegate(param =>
    {
        TextBlock nameAlgorithm = new TextBlock() { Text = "Алгоритм Форда Фалкерсона" };
        TextBlock textFirstNode = new TextBlock() { Text = "Начальная вершина: " + StartNode.Name };
        TextBlock textSecondNode = new TextBlock() { Text = "Конечная вершина: " + EndNode.Name };
        mainVM.BaseDataForAlgortithm.Children.Add(nameAlgorithm);
        mainVM.BaseDataForAlgortithm.Children.Add(textFirstNode);
        mainVM.BaseDataForAlgortithm.Children.Add(textSecondNode);
        FordFalkersonW.Close();
    });


}