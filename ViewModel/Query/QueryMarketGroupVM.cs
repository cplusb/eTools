using eTools.Command.Query;
using eTools.Datas;
using eTools.Model.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace eTools.ViewModel.Query
{
    class QueryMarketGroupVM : INotifyPropertyChanged
    {

        private QueryMarketGroup _queryMarketGroup;
        public QueryMarketGroupVM()
        {
            _queryMarketGroup = new QueryMarketGroup();
            Search = new QueryMarketSearchCmd();
            SortWay = new QueryMarketGroupResultSortCmd();
        }

        public ICommand Search { get; private set; }//
        public ICommand SortWay { get; private set; }

        public List<MarketGroupTypeShow> TypeShowSources
        {
            get => _queryMarketGroup.TypeShowSources;
            set
            {
                if (value == null) return;
                _queryMarketGroup.TypeShowSources = value;
                OnPropertyChanged("TypeShowSources");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
