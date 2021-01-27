using eTools.Model.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.ViewModel.DataSources
{
    class ItemTypeFilterVM : INotifyPropertyChanged
    {
        private ItemTypeFilter _filter;

        private string _selectedItem;
        public string SelectedItem { get => _selectedItem; set=> _selectedItem = value; }

        public ItemTypeFilterVM() => _filter = new ItemTypeFilter();

        public List<string> Filters { get => _filter.Filters; set
            {
                _filter.Filters = value;
                OnPropertyChanged("Filters");
            } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
