using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM.FolderHelpVM
{
    public class HowCreateNewGraphVM:BaseVM
    {
        private string _textT = "\tВ верхнем меню нажмите на кнопку «Файл», а потом на кнопку «Создать новый граф». Введите имя графа и выберете папку, в которой будут сохранятся данные графа. После в нижнем меню нажмите кнопку «Обновить», чтобы начать работу с созданным графом. ";

        public string TextT
        {
            get { return _textT; } set
            {
                _textT = value;
                OnPropertyChanged();
            }
        }
    }
}
