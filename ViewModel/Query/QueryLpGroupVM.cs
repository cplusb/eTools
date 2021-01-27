using eTools.Command.Query;
using eTools.Datas;
using eTools.Model.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace eTools.ViewModel.Query
{
    class QueryLpGroupVM : INotifyPropertyChanged
    {
        private QueryLpGroup _queryLpGroup;
        public QueryLpGroupVM()
        {
            QueryLP = new QueryLpCmd();
            _queryLpGroup = new QueryLpGroup();
            SortWay = new QueryLpGroupResultSortCmd();
        }
        public ICommand QueryLP { get; private set; }
        public ICommand SortWay { get; private set; }

        public List<LpGroupTypeShow> TypeShowSources
        {
            get => _queryLpGroup.TypeShowSources;
            set
            {
                if (value == null) return;
                _queryLpGroup.TypeShowSources = value;
                OnPropertyChanged("TypeShowSources");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
