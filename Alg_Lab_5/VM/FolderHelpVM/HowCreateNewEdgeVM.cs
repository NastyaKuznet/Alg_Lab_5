using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowCreateNewEdgeVM: BaseVM
    {
        private string _textT = "\tПосле того как вы открыли новый или существующий граф, в верхнем меню нажмите на кнопку «Режимы», а потом на кнопку «Создание ребер». В режиме создания ребер нажимайте на рабочую область для создания ребра. Важно! Нажимать на вершины, иначе ничего не произойдет. Также граф имеет только определённый тип ребер. Выбрать тип ребер можно в нижней панели при создании первого ребра. Изменить режим у текущего графа после установки какого-то определенного можно после удаления всех ребер.\n\tДля выхода из режима в нижнем меню нажмите кнопку «Закрыть режим». Во время режимов блокируются некоторые функции программы.";
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
