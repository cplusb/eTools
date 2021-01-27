using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace eTools.Command
{
    class LpFilterCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var msg = parameter as string;
            if (msg == "Sale")
                Global.Signal.LpPreSale = true;
            else if(msg == "Buy")
                Global.Signal.LpPreSale = false;
        }
    }
}
