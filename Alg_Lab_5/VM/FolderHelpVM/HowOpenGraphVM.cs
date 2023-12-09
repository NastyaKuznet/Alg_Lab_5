using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowOpenGraphVM: BaseVM
    {
        private string _textT = "\tВ верхнем меню нажмите на кнопку «Файл», а потом на кнопку «Открыть граф». Выберете папку, в которой хранится данные о графе. Важно! Программа не работает только с таблицей смежности, ей также необходим и специальный программный файл.";

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
