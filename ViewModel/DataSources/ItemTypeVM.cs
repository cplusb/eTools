using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace eTools.ViewModel.DataSources
{
    public class ItemTypeVM : INotifyPropertyChanged
    {
        private Model.DataSources.ItemType _itemType;

        public ItemTypeVM() => _itemType = new Model.DataSources.ItemType();

        public List<string> Collection { get => _itemType.Collections; set
            {
                _itemType.Collections = value;
                OnPropertyChanged("Collection");
            } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
