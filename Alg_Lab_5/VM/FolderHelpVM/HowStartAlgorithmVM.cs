using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowStartAlgorithmVM: BaseVM
    {
        private string _textT = "\tДля запуска алгоритма по текущему графу нужно сперва выбрать категорию алгоритма. Потом нажать на кнопку \"Выбрать алгоритм\". Если алгоритму для работы нужны дополнительные входные данные от пользователя, то введите необходиммые данные в новом окне. После этого не забудьте нажать на все необходимые кнопки \"Применить\". После того, как окно закроется нажмите кнопку \"Запустить алгоритм\". Вам выведется список кнопок, в каждой из которых можно будет узнать всю необходимую информацию по выбранному шагу алгоритма.\n\tДля возврата графа в его первоначальный вид нажмите кнопку \"Востановить граф\".";

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
