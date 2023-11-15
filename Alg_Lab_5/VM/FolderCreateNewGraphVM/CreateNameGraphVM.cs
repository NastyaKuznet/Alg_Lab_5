using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderCreateNewGraphVM
{
    public class CreateNameGraphVM: BaseVM
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
    }
}
