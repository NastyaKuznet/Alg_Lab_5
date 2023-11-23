using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM
{
    public class EdgeGraphVM: BaseVM
    {
        public int Id { get; set; }
        public string FromTo { get; set; }
        public int Weight { get; set; }

        public bool IsEnableFromTo { get; set; }

        private bool _isEnableFromTo = false;

        public bool IsEnableWeight 
        {
            get { return _isEnableFromTo; }
            set { _isEnableFromTo = value;
                OnPropertyChanged();
            }
        }

        public bool IsWeight { get; set; } = false;
       
    }
}
