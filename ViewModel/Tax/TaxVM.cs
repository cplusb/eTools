using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.ViewModel.Tax
{
    public class TaxVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public float TaxMarketSale
        {
            get => Global.Tax.MarketSale; set
            {
                Global.Tax.MarketSale = value;
                OnPropertyChanged("TaxMarketSale");
            }
        }
        public float TaxMarketMedian
        {
            get => Global.Tax.MarketMedian; set
            {
                Global.Tax.MarketMedian = value;
                OnPropertyChanged("TaxMarketMedian");
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
