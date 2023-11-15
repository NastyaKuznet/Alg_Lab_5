using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Alg_Lab_5.M;
using Alg_Lab_5.V.FolderCreateNewGraph;
using Alg_Lab_5.VM.FolderCreateNewGraphVM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Alg_Lab_5.VM
{
    public class MainVM: BaseVM
    {
        DialogeOpen dialoge = new DialogeOpen();
        string pathFolder = null;

        private string nameGraph = "";
        public string NameGraph
        {
            get { return nameGraph; }
            set
            {
                nameGraph = value;
                OnPropertyChanged();
            }
        }
        public ICommand CreateNewGraph => new CommandDelegate(param =>
        {
            CreateNewGraphW cUW = new CreateNewGraphW();
            CreateNewGraphVM cVM = new CreateNewGraphVM();
            cVM.createGraphW = cUW;
            cUW.DataContext = cVM;
            cUW.Show();
            NameGraph = cVM.NameGraph;
            pathFolder = cVM.PathFolder;
        });

        public ICommand OpenGraph => new CommandDelegate(param =>
        {
            pathFolder = dialoge.CallFolderBrowserDialog();
            int firstIndexName = pathFolder.LastIndexOf("\\") + 1;
            NameGraph = pathFolder.Substring(firstIndexName, pathFolder.Length - firstIndexName);
        });
    }
}
