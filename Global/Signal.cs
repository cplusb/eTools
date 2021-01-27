using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Global
{
    class Signal
    {
        public static bool LpPreSale = true;

        public static SortWay SortWay = SortWay.Dec;
        public static SortWay MarketGroupSortWay = SortWay.Dec;
        public static SortWay LpGroupSortWay = SortWay.Dec;
    }

    public enum SortWay
    {
        Default,
        Dec,
        Inc
    }
}
