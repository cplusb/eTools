using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace eTools.Manager.SortManager
{
    public class ListViewSortManager
    {
        private Dictionary<ListViewType, ListView> _listViews;
        private static ListViewSortManager _mgr;
        private ListViewSortDec _sortDec;
        private ListViewSortInc _sortInc;
        private ListViewSortManager()
        {
            _listViews = new Dictionary<ListViewType, ListView>();
            _sortDec = new ListViewSortDec();
            _sortInc = new ListViewSortInc();
        }
        public static ListViewSortManager GetManager() => _mgr = _mgr ?? (_mgr = new ListViewSortManager());
        public void Register(ListViewType type, ListView lv)
        {
            if (!_listViews.ContainsKey(type))
                _listViews.Add(type, lv);
        }
        public ListView Get(ListViewType type)
        {
            _listViews.TryGetValue(type, out var lv);
            return lv;
        }

        public void SwitchSorter(ListViewType type, SortDescription sorter)
        {
            var lv = Get(type);
            if(lv != default && lv.ItemsSource != null && sorter != null)
            {
                CollectionViewSource.GetDefaultView(lv.ItemsSource).SortDescriptions.Clear();
                CollectionViewSource.GetDefaultView(lv.ItemsSource).SortDescriptions.Add(sorter);
            }
        }
        public void SwitchSorter(ListViewType listViewType, ListViewSortType sortType)
        {
            var sortWay = GetSortWay(listViewType);
            if (sortWay == Global.SortWay.Default) return;
            var sorter = GetSorter(sortWay, sortType);
            SwitchSorter(listViewType, sorter);
        }
        /// <summary>
        /// 根据排序方式以及要排序的对象类型获取排序器
        /// </summary>
        /// <param name="sortWay"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        private SortDescription GetSorter(Global.SortWay sortWay, ListViewSortType sortType)
        {
            SortDescription sorter = default;
            switch (sortWay)
            {
                case Global.SortWay.Dec:
                    sorter = _sortDec.Get(sortType);
                    break;
                case Global.SortWay.Inc:
                    sorter = _sortInc.Get(sortType);
                    break;
            }
            return sorter;
        }
        /// <summary>
        /// 因为涉及多个界面，所以需要将不同界面的数据转换成统一调用，
        /// 如果界面过多，需要配置hash表
        /// </summary>
        /// <param name="listViewType"></param>
        /// <returns></returns>
        private Global.SortWay GetSortWay(ListViewType listViewType)
        {
            switch (listViewType)
            {
                case ListViewType.MarketGroup:
                    if (Global.Signal.MarketGroupSortWay == Global.SortWay.Dec)
                        return Global.SortWay.Dec;
                    else if (Global.Signal.MarketGroupSortWay == Global.SortWay.Inc)
                        return Global.SortWay.Inc;
                    break;
                case ListViewType.LpGroup:
                    if (Global.Signal.LpGroupSortWay == Global.SortWay.Dec)
                        return Global.SortWay.Dec;
                    else if (Global.Signal.LpGroupSortWay == Global.SortWay.Inc)
                        return Global.SortWay.Inc;
                    break;
            }
            return Global.SortWay.Default;
        }
    }
    public class ListViewSortDec
    {
        private Dictionary<ListViewSortType, SortDescription> _sorts;
        public ListViewSortDec()
        {
            _sorts = new Dictionary<ListViewSortType, SortDescription>();
            Init();
        }
        private void Init()
        {
            Register(ListViewSortType.MarketGroupName, new SortDescription("Name", ListSortDirection.Descending));
            Register(ListViewSortType.MarketGroupSale, new SortDescription("Sale", ListSortDirection.Descending));
            Register(ListViewSortType.MarketGroupBuy, new SortDescription("Buy", ListSortDirection.Descending));
            Register(ListViewSortType.MarketGroupSub, new SortDescription("Sub", ListSortDirection.Descending));

            Register(ListViewSortType.LpGroupName, new SortDescription("Name", ListSortDirection.Descending));
            Register(ListViewSortType.LpGroupSale, new SortDescription("Sale", ListSortDirection.Descending));
            Register(ListViewSortType.LpGroupSaleRate, new SortDescription("SaleRate", ListSortDirection.Descending));
            Register(ListViewSortType.LpGroupBuy, new SortDescription("Buy", ListSortDirection.Descending));
            Register(ListViewSortType.LpGroupBuyRate, new SortDescription("BuyRate", ListSortDirection.Descending));
        }

        public void Register(ListViewSortType type, SortDescription sorter)
        {
            if (!_sorts.ContainsKey(type))
                _sorts.Add(type, sorter);
        }

        public SortDescription Get(ListViewSortType type)
        {
            _sorts.TryGetValue(type, out var sort);
            return sort;
        }
    }
    public class ListViewSortInc
    {
        private Dictionary<ListViewSortType, SortDescription> _sorts;
        public ListViewSortInc()
        {
            _sorts = new Dictionary<ListViewSortType, SortDescription>();
            Init();
        }
        private void Init()
        {
            Register(ListViewSortType.MarketGroupName, new SortDescription("Name", ListSortDirection.Ascending));
            Register(ListViewSortType.MarketGroupSale, new SortDescription("Sale", ListSortDirection.Ascending));
            Register(ListViewSortType.MarketGroupBuy, new SortDescription("Buy", ListSortDirection.Ascending));
            Register(ListViewSortType.MarketGroupSub, new SortDescription("Sub", ListSortDirection.Ascending));

            Register(ListViewSortType.LpGroupName, new SortDescription("Name", ListSortDirection.Ascending));
            Register(ListViewSortType.LpGroupSale, new SortDescription("Sale", ListSortDirection.Ascending));
            Register(ListViewSortType.LpGroupSaleRate, new SortDescription("SaleRate", ListSortDirection.Ascending));
            Register(ListViewSortType.LpGroupBuy, new SortDescription("Buy", ListSortDirection.Ascending));
            Register(ListViewSortType.LpGroupBuyRate, new SortDescription("BuyRate", ListSortDirection.Ascending));
        }

        public void Register(ListViewSortType type, SortDescription sorter)
        {
            if (!_sorts.ContainsKey(type))
                _sorts.Add(type, sorter);
        }

        public SortDescription Get(ListViewSortType type)
        {
            _sorts.TryGetValue(type, out var sort);
            return sort;
        }
    }
    /// <summary>
    /// 这个类将查询进一步封装，传递参数，直接切换
    /// </summary>
    public class ListViewSortManagerAssist
    {
        private static Dictionary<ListViewType, Delegate> _events = new Dictionary<ListViewType, Delegate>();

        static ListViewSortManagerAssist()
        {
            Init();
        }

        private static void Init()
        {
            Action<string> @delegateMarketGroup = new
                 Action<string>((paramerter) =>
                 {
                     switch (paramerter)
                     {
                         case "Name":
                             ListViewSortManager.GetManager().SwitchSorter(ListViewType.MarketGroup, ListViewSortType.MarketGroupName);
                             break;
                         case "PriceBuy":
                             ListViewSortManager.GetManager().SwitchSorter(ListViewType.MarketGroup, ListViewSortType.MarketGroupBuy);
                             break;
                         case "PriceSale":
                             ListViewSortManager.GetManager().SwitchSorter(ListViewType.MarketGroup, ListViewSortType.MarketGroupSale);
                             break;
                         case "PriceSub":
                             ListViewSortManager.GetManager().SwitchSorter(ListViewType.MarketGroup, ListViewSortType.MarketGroupSub);
                             break;
                     }
                 });
            _events.Add(ListViewType.MarketGroup, @delegateMarketGroup);

            Action<string> @delegateLpGroup = new
                Action<string>((paramerter) =>
                {
                    
                    switch (paramerter)
                    {
                        case "Name":
                            ListViewSortManager.GetManager().SwitchSorter(ListViewType.LpGroup, ListViewSortType.LpGroupName);
                            break;
                        case "SaleShow":
                            ListViewSortManager.GetManager().SwitchSorter(ListViewType.LpGroup, ListViewSortType.LpGroupSale);
                            break;
                        case "BuyShow":
                            ListViewSortManager.GetManager().SwitchSorter(ListViewType.LpGroup, ListViewSortType.LpGroupBuy);
                            break;
                        case "SaleRateShow":
                            ListViewSortManager.GetManager().SwitchSorter(ListViewType.LpGroup, ListViewSortType.LpGroupSaleRate);
                            break;
                        case "BuyRateShow":
                            ListViewSortManager.GetManager().SwitchSorter(ListViewType.LpGroup, ListViewSortType.LpGroupBuyRate);
                            break;
                    }
                });
            _events.Add(ListViewType.LpGroup, @delegateLpGroup);
        }

        public static void Sort(ListViewType listViewType, string paramerter)
        {
            _events.TryGetValue(listViewType, out var @delegate);
            @delegate.DynamicInvoke(paramerter);
        }
    }

    public enum ListViewType
    {
        MarketGroup,
        LpGroup
    }

    public enum ListViewSortType
    {
        MarketGroupName,
        MarketGroupSale,
        MarketGroupBuy,
        MarketGroupSub,


        LpGroupName,
        LpGroupSale,
        LpGroupSaleRate,
        LpGroupBuy,
        LpGroupBuyRate,
    }
}
