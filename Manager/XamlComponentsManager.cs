using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace eTools.Manager
{
    /// <summary>
    /// 负责treeviw控件的管理
    /// </summary>
    public class XamlComponentsManager
    {
        private Dictionary<XamlComponentsType, object> _collections;

        private static XamlComponentsManager _manager;
        public static XamlComponentsManager GetManager() => _manager = _manager ?? (_manager = new XamlComponentsManager());
        private XamlComponentsManager() => _collections = new Dictionary<XamlComponentsType, object>();

        public void Register<T>(XamlComponentsType type, T t)
        {
            if (!_collections.ContainsKey(type)) _collections.Add(type, t);
        }

        public T Get<T>(XamlComponentsType type) where T : class
        {
            _collections.TryGetValue(type, out var t);
            return t as T;
        }
    }

    public enum XamlComponentsType
    {
        MarketGroupTree,
        LpGroupTree,
        ManufacturingGroupTree,
        BlueprintsTree,
        MarketGroupListView,
        MarketGroupRegion,
        MarketGroupGalaxy,
        LpGroupRegion,
        LpGroupGalaxy,
        RootGrid,
        MarketGrid,
        LpGrid,
        ContractGrid,
        MineralGrid,

        ManufactPanel,
        
    }
}
