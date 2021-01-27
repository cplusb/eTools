using eTools.Manager;
using System;
using System.Windows;
using System.Windows.Input;

namespace eTools.Comand.Manager
{
    class SwitchLayoutCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                GridLayoutType layout = (GridLayoutType)System.Enum.Parse(typeof(GridLayoutType), parameter as string);
                LayoutSwitchManager.GetManager().Switch(layout);
            }
            catch { }
        }
    }
}
