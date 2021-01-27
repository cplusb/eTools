using eTools.Manager.Notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace eTools.Manager
{
    public class LayoutSwitchManager
    {
        private static LayoutSwitchManager _mgr;
        private Dictionary<GridLayoutType, GridLayout> _layouts;
        private LayoutSwitchManager() => _layouts = new Dictionary<GridLayoutType, GridLayout>();
        public static LayoutSwitchManager GetManager() => _mgr = _mgr ?? (_mgr = new LayoutSwitchManager());
        /// <summary>
        /// 当前布局界面
        /// </summary>
        public GridLayout Current { get; private set; }

        /// <summary>
        /// 注册完后初始化，调用一次
        /// </summary>
        public void Init()
        {
            foreach (var grid in _layouts)
                grid.Value?.Hide();
            Switch(GridLayoutType.Manufacturing);
        }
        /// <summary>
        /// 注册布局界面
        /// </summary>
        /// <param name="type"></param>
        /// <param name="grid"></param>
        public void Register(GridLayoutType type, GridLayout grid)
        {
            if (!_layouts.ContainsKey(type) && grid != null)
                _layouts.Add(type, grid);
        }
        /// <summary>
        /// 切换显示界面
        /// </summary>
        /// <param name="type"></param>
        public void Switch(GridLayoutType type)
        {
            if (_layouts.TryGetValue(type, out var grid))
            {
                Current?.Hide();
                grid?.Show();
                Current = grid;
            }
        }
        /// <summary>
        /// 销毁实例
        /// </summary>
        public void Destroy()
        {
            _layouts.Clear();
            _mgr = null;
            GC.Collect();
        }
    }
    public class GridLayout
    {
        private Grid _grid;
        public GridLayout(Grid grid) => _grid = grid;

        /// <summary>
        /// 隐藏布局
        /// </summary>
        public void Hide()
        {
            if (_grid != null)
                _grid.Visibility = System.Windows.Visibility.Hidden;
        }
        /// <summary>
        /// 显示布局
        /// </summary>
        public void Show()
        {
            if (_grid != null)
                _grid.Visibility = System.Windows.Visibility.Visible;
            if (!inited)
                Init();
        }

        protected bool inited = false;
        protected virtual void Init()
        {
            inited = true;
        }
    }

    public class GridLayoutMarketGroup : GridLayout
    {
        public GridLayoutMarketGroup(Grid grid) : base(grid) { }
        protected override void Init()
        {
            base.Init();
            var treeView = XamlComponentsManager.GetManager().Get<TreeView>(XamlComponentsType.MarketGroupTree);
            if (treeView != null)
                treeView.ItemsSource = LocalConfigLoadManager.GroupMarketTypeIDs();
            try
            {
                var marketGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.MarketGrid);
                (marketGrid?.Resources["rgVM"] as ViewModel.DataSources.MarketGroupRegionGalaxyVM).Regions = LocalConfigLoadManager.GetConfigClass<RegionGalaxyDBClass>().Regions;
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }
    }

    public class GridLayoutLpGroup : GridLayout
    {
        public GridLayoutLpGroup(Grid grid) : base(grid) { }
        protected override void Init()
        {
            base.Init();
           
            var treeView = XamlComponentsManager.GetManager().Get<TreeView>(XamlComponentsType.LpGroupTree);
            if (treeView != null)
                treeView.ItemsSource = LocalConfigLoadManager.GetConfigClass<NewNpcCorporationsDBClass>().NpcCorpGroupDBs;
            try
            {
                var lpGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.LpGrid);
                (lpGrid?.Resources["rgVM"] as ViewModel.DataSources.LpGroupRegionGalaxyVM).Regions = LocalConfigLoadManager.GetConfigClass<RegionGalaxyDBClass>().Regions;
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }
    }

    public class GridLayoutManufacturingGroup : GridLayout
    {
        public GridLayoutManufacturingGroup(Grid grid) : base(grid) { }
        protected override void Init()
        {
            base.Init();

            var treeView = XamlComponentsManager.GetManager().Get<TreeView>(XamlComponentsType.ManufacturingGroupTree);
            try
            {
                if (treeView != null)
                {
                    // 想用多线程，就要添加VM
                    List<Datas.MarketGroupNode> nodes = new List<Datas.MarketGroupNode>();
                    nodes.Add(LocalConfigLoadManager.GroupMarketTypeIDs()?[0]);
                    treeView.ItemsSource = nodes;
                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }
    }

    public enum GridLayoutType
    {
        Market,
        Lp,
        Contract,
        Mineral,
        Manufacturing
    }
}
