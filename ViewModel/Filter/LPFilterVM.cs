using eTools.Command;
using eTools.Model.Filter;
using System.ComponentModel;
using System.Windows.Input;

namespace eTools.ViewModel.Filter
{
    public class LPFilterVM : INotifyPropertyChanged
    {
        private LPFilter _lpFilter;

        public LPFilterVM() {
            Switch = new LpFilterCmd();
            _lpFilter = new LPFilter();
        }

        public ICommand Switch { get; set; }

        public float TaxSale { get => _lpFilter.TaxSale; set {
                _lpFilter.TaxSale = value;
                
                OnPropertyChanged("TaxSale");
            } }
        public float TaxIntermediary { get => _lpFilter.TaxIntermediary; set {
                _lpFilter.TaxIntermediary = value;
                OnPropertyChanged("TaxIntermediary");
            } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
