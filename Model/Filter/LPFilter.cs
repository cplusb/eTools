using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Model.Filter
{
    public class LPFilter
    {
        public float TaxSale { get => Global.Tax.LpSale; set => Global.Tax.LpSale = value; }
        public float TaxIntermediary { get => Global.Tax.LpMedian; set => Global.Tax.LpMedian = value; }
        public bool IsPreSale { get => Global.Signal.LpPreSale; set => Global.Signal.LpPreSale = true; }
        public bool IsPreBuy { get => Global.Signal.LpPreSale; set => Global.Signal.LpPreSale = false; }
    }
}
