using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowSaveGraphVM: BaseVM
    {
        private string _textT = "\tЧтобы сохранить граф, на нижней панели нажмите кнопку «Сохранить».";

        public string TextT
        {
            get { return _textT; }
            set
            {
                _textT = value;
                OnPropertyChanged();
            }
        }
    }
}
