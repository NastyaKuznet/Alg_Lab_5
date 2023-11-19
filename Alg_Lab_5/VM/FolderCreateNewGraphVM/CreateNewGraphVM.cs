using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Alg_Lab_5.V.FolderCreateNewGraph;
using Alg_Lab_5.M;

namespace Alg_Lab_5.VM.FolderCreateNewGraphVM
{
    public class CreateNewGraphVM: BaseVM
    {
        private ManagerPages managerPages;

        private CreateNameGraphP nameGraphP = new CreateNameGraphP();
        private CreateNameGraphVM nameGraphPVM = new CreateNameGraphVM();
        private CreatePathGraphP pathGraphP = new CreatePathGraphP();
        private CreatePathGraphVM pathPathPVM = new CreatePathGraphVM();
        private CreateFinalyGraphP finalyGraphPage = new CreateFinalyGraphP();
        private CreateFinalyGraphVM finalyGraphVM = new CreateFinalyGraphVM();
        private Page currentPage;
        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged();
            }
        }


        public CreateNewGraphW createGraphW { get; set; }


        private bool CanNext;

        private string pathFolder = "";
        public string PathFolder
        {
            get { return pathFolder; }
            set
            {
                pathFolder = value;
                OnPropertyChanged();
            }
        }

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

        private bool isEnabledUndo;
        public bool IsEnabledUndo
        {
            get { return isEnabledUndo; }
            set
            {
                isEnabledUndo = value;
                OnPropertyChanged();
            }
        }

        public bool wasFinal = false;

        public CreateNewGraphVM()
        {
            AddDataContext();
            managerPages = new ManagerPages(CreateListPages());
            CurrentPage = managerPages.GetCurrentPage();
        }

        private void AddDataContext()
        {
            nameGraphP.DataContext = nameGraphPVM;
            pathGraphP.DataContext = pathPathPVM;
            finalyGraphPage.DataContext = finalyGraphVM;
        }
        private List<Page> CreateListPages()
        {
            List<Page> pagesList = new List<Page>
            {
                nameGraphP,
                pathGraphP,
                finalyGraphPage
            };
            return pagesList;
        }


        public ICommand Apply => new CommandDelegate(param =>
        {
            if (CurrentPage is CreateNameGraphP)
            {
                NameGraph = nameGraphPVM.NameGraph;
                CanNext = !(string.IsNullOrEmpty(NameGraph) || string.IsNullOrWhiteSpace(NameGraph));

            }
            else if (CurrentPage is CreatePathGraphP)
            {
                PathFolder = pathPathPVM.PathFolder;
                CanNext = !(string.IsNullOrEmpty(PathFolder) || string.IsNullOrWhiteSpace(PathFolder));
                PathFolder += "\\" + NameGraph;
                finalyGraphVM.NameGraph = NameGraph;
                finalyGraphVM.PathFolder = PathFolder;
            }
            else if (CurrentPage is CreateFinalyGraphP)
            {
                Directory.CreateDirectory(PathFolder);
                createGraphW.Close();
                CanNext = false;
                wasFinal = true;
            }
            if (CanNext)
            {
                managerPages.NextPage();
                CurrentPage = managerPages.GetCurrentPage();
                IsEnabledUndo = managerPages.CanUndo();
            }

        });

        public ICommand Undo => new CommandDelegate(param =>
        {
            managerPages.LastPage();
            CurrentPage = managerPages.GetCurrentPage();
            IsEnabledUndo = managerPages.CanUndo();
        });
    }
}
