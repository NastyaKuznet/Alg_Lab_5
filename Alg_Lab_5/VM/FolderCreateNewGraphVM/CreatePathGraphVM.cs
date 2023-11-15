using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Alg_Lab_5.M;

namespace Alg_Lab_5.VM.FolderCreateNewGraphVM
{
    public class CreatePathGraphVM: BaseVM
    {
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

        public ICommand ChoiceFolder => new CommandDelegate(param =>
        {
            DialogeOpen dialoge = new DialogeOpen();
            PathFolder = dialoge.CallFolderBrowserDialog();
        });
    }
}
