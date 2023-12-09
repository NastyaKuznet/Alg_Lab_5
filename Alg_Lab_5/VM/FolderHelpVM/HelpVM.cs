using Alg_Lab_5.V.FolderHelpWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HelpVM: BaseVM
    {
        HowCreateNewGraphVM howCreateNewGVM = new HowCreateNewGraphVM();
        HowCreateNewGraphP howCreateNewGP = new HowCreateNewGraphP();
        HowOpenGraphVM howOpenGVM = new HowOpenGraphVM();
        HowOpenGraphP howOpenGP = new HowOpenGraphP();
        HowCreateNewNodeVM howCreateNewNodeVM = new HowCreateNewNodeVM();
        HowCreateNewNodeP howCreateNewNodeP = new HowCreateNewNodeP();
        HowChangeNodeVM howChangeNodeVM = new HowChangeNodeVM();
        HowChangeNodeP howChangeNodeP = new HowChangeNodeP();
        HowCreateNewEdgeVM howCreateNewEdgeVM = new HowCreateNewEdgeVM();
        HowCreateNewEdgeP howCreateNewEdgeP = new HowCreateNewEdgeP();
        HowChangeEdgeVM howChangeEdgeVM = new HowChangeEdgeVM();
        HowChangeEdgeP howChangeEdgeP = new HowChangeEdgeP();
        HowSaveGraphVM howSaveGraphVM = new HowSaveGraphVM();
        HowSaveGraphP howSaveGraphP = new HowSaveGraphP();
        HowStartAlgorithmVM howStartAlgorithmVM = new HowStartAlgorithmVM();
        HowStartAlgorithmP howStartAlgorithmP = new HowStartAlgorithmP();

        private ObservableCollection<Button> _pointHelpButtons = new ObservableCollection<Button>();

        public ObservableCollection<Button> PointHelpButtons
        { 
            get { return _pointHelpButtons;}
            set 
            {
                _pointHelpButtons = value;
                OnPropertyChanged();
            }
        }

        private Page currentPage = new Page();
        public Page CurrentPage 
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged(); }
        }

        public HelpVM()
        {
            howCreateNewGP.DataContext = howCreateNewGVM;
            howOpenGP.DataContext = howOpenGVM;
            howCreateNewNodeP.DataContext = howCreateNewNodeVM;
            howChangeNodeP.DataContext = howChangeNodeVM;
            howCreateNewEdgeP.DataContext = howCreateNewEdgeVM;
            howChangeEdgeP.DataContext = howChangeEdgeVM;
            howSaveGraphP.DataContext = howSaveGraphVM;
            howStartAlgorithmP.DataContext = howStartAlgorithmVM;
            PointHelpButtons.Add(new Button { Content = "Как создать новый граф", Command = HowCreateNewGraph});
            PointHelpButtons.Add(new Button { Content = "Как открыть существующий граф", Command = HowOpenGraph });
            PointHelpButtons.Add(new Button { Content = "Как создать новый узел (вершину) графа", Command = HowCreateNewNode });
            PointHelpButtons.Add(new Button { Content = "Как редактировать узлы (вершины) графа", Command = HowChangeNode });
            PointHelpButtons.Add(new Button { Content = "Как создать новое ребро", Command = HowCreateNewEdge });
            PointHelpButtons.Add(new Button { Content = "Как редактировать ребра графа", Command = HowChangeEdge });
            PointHelpButtons.Add(new Button { Content = "Как сохранить граф", Command = HowSaveGraph });
            PointHelpButtons.Add(new Button { Content = "Как запустить алгоритм", Command = HowStartAlgorithm });
        }

        public ICommand HowCreateNewGraph => new CommandDelegate(param =>
        {
            CurrentPage = howCreateNewGP;
        });

        public ICommand HowOpenGraph => new CommandDelegate(param =>
        {
            CurrentPage = howOpenGP;
        });

        public ICommand HowCreateNewNode => new CommandDelegate(param =>
        {
            CurrentPage = howCreateNewNodeP;
        });

        public ICommand HowChangeNode => new CommandDelegate(param =>
        {
            CurrentPage = howChangeNodeP;
        });

        public ICommand HowCreateNewEdge => new CommandDelegate(param =>
        {
            CurrentPage = howCreateNewEdgeP;
        });

        public ICommand HowChangeEdge => new CommandDelegate(param =>
        {
            CurrentPage = howChangeEdgeP;
        });
        public ICommand HowSaveGraph => new CommandDelegate(param =>
        {
            CurrentPage = howSaveGraphP;
        });

        public ICommand HowStartAlgorithm => new CommandDelegate(param =>
        {
            CurrentPage = howStartAlgorithmP;
        });
    }
}
