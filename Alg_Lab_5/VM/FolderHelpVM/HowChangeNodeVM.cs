using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowChangeNodeVM: BaseVM
    {
        private string _textT = "\tРедактирование узлов (вершин) графа можно осуществить в панели справа. Есть возможность изменить имя узла (вершины) или удалить его вовсе. Для этого нажмите на кнопку «Изменить» нужного узла (вершины) и выполните необходимое действие. После этого нажмите на ту же кнопку, но с изменившимся названием «Сохранить».";
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
