using eTools.Datas;
using eTools.Global;
using eTools.Manager.Web;
using eTools.ViewModel.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace eTools.Command.Query
{
    class QueryLpCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var corp = parameter as NewNpcCorporations;
            if (corp != null && !corp.isGroup)
                QueryManager.QueryStart(QueryType.Lp, $"{QueryType.Lp}_{corp.id}", corp);
        }
    }

    class QueryLpGroupResultSortCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Signal.LpGroupSortWay = (SortWay)Enum.Parse(typeof(SortWay), parameter as string);
           
        }
    }
}
