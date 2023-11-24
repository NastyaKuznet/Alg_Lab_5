using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowCreateNewNodeVM: BaseVM
    {
        private string _textT = "\tПосле того как вы открыли новый или существующий граф, в верхнем меню нажмите на кнопку «Режимы», а потом на кнопку «Создание узлов». В режиме создания узлов нажимайте на рабочую область для создания узла (вершины). \n\tДля выхода из режима в нижнем меню нажмите кнопку «Закрыть режим». Во время режимов блокируются некоторые функции программы.";
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
