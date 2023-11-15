using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alg_Lab_5.VM
{
    public class CommandDelegate : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;
        private Predicate<object> canExecutePredicate;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CommandDelegate(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object param)
        {
            return this.canExecute == null || this.canExecute(param);
        }

        public void Execute(object param)
        {
            this.execute(param);
        }
    }
}
