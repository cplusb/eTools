using System.Windows.Input;
using eTools.Comand.Manager;

namespace eTools.ViewModel.Manager
{
    public class SwitchLayoutVM
    {
        public SwitchLayoutVM() => SwitchLayout = new SwitchLayoutCmd();
        public ICommand SwitchLayout { get; private set; }
    }
}
