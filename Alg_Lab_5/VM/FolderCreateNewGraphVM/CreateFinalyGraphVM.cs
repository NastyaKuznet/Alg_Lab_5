using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderCreateNewGraphVM
{
    public class CreateFinalyGraphVM: BaseVM
    {
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
    }
}
