using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowChangeEdgeVM: BaseVM
    {
        private string _textT = "\tРедактирование ребер графа можно осуществить в панели справа. Есть возможность изменить вес ребра (если граф взвешенный) или удалить его вовсе. Для этого нажмите на кнопку «Изменить» нужного ребра и выполните необходимое действие. После этого нажмите на ту же кнопку, но с изменившимся названием «Сохранить».";

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
