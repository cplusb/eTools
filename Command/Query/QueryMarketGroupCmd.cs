using eTools.Global;
using eTools.Manager;
using eTools.Manager.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eTools.Command.Query
{
    class QueryMarketSearchCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            QueryManager.QueryStart(QueryType.Market, QueryType.Market.ToString(), parameter as string);
        }
    }

    class QueryMarketGroupResultSortCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Signal.MarketGroupSortWay = (SortWay)Enum.Parse(typeof(SortWay), parameter as string);
        }
    }
}
