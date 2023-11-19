using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Lab_5.VM
{
    public class NodeGraphVM: BaseVM
    {
        public int Id { get; set; }
        private string _name;
        public string Name 
        { get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private bool _enabled;
        public bool IsEnable
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }
        
        public NodeGraphVM(int id)
        {
            Id = id;
        }
    }
}
