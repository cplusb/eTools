using eTools.Manager;
using eTools.ViewModel.DataSources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using eTools.Datas;
using System.ComponentModel;
using eTools.Manager.SortManager;
using System.Windows.Data;
using System.Threading;
using eTools.Manager.Notice;
using eTools.Manager.Web;

namespace eTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeTitleBar();
            InitializeXamlComponents();
            InitializeListViewSorter();
        }

        /// <summary>
        /// 初始化最上方的主模块布局切换
        /// </summary>
        private void InitializeTitleBar()
        {
            var layoutSwitchManager = LayoutSwitchManager.GetManager();
            layoutSwitchManager?.Register(GridLayoutType.Market, new GridLayoutMarketGroup(this.GridMarket));
            layoutSwitchManager?.Register(GridLayoutType.Lp, new GridLayoutLpGroup(this.GridLp));
            layoutSwitchManager?.Register(GridLayoutType.Contract, new GridLayout(this.GridContract));
            layoutSwitchManager?.Register(GridLayoutType.Mineral, new GridLayout(this.GridMineral));
            layoutSwitchManager?.Register(GridLayoutType.Manufacturing, new GridLayoutManufacturingGroup(this.GridManufacturing));
        }

        /// <summary>
        /// 初始化，注册组件到管理器中，提供给三方管理器访问
        /// </summary>
        private void InitializeXamlComponents()
        {
            var xamlComponentsManager = XamlComponentsManager.GetManager();
            xamlComponentsManager?.Register(XamlComponentsType.MarketGroupTree, this.MarketGroupTree);
            xamlComponentsManager?.Register(XamlComponentsType.LpGroupTree, this.LpGroupTree);
            xamlComponentsManager?.Register(XamlComponentsType.ManufacturingGroupTree, this.ManufacturingGroupTree);
          //  xamlComponentsManager?.Register(XamlComponentsType.BlueprintsTree, this.BlueprintsTree);
            xamlComponentsManager?.Register(XamlComponentsType.RootGrid, this.RootGrid);
            xamlComponentsManager?.Register(XamlComponentsType.MarketGrid, this.GridMarket);
            xamlComponentsManager?.Register(XamlComponentsType.LpGrid, this.GridLp);
            xamlComponentsManager?.Register(XamlComponentsType.ContractGrid, this.GridContract);
            xamlComponentsManager?.Register(XamlComponentsType.MineralGrid, this.GridMineral);
            xamlComponentsManager?.Register(XamlComponentsType.MarketGroupListView, this.MarketGroupListView);
            xamlComponentsManager?.Register(XamlComponentsType.MarketGroupRegion, this.MarketGroupRegion);
            xamlComponentsManager?.Register(XamlComponentsType.MarketGroupGalaxy, this.MarketGroupGalaxy);
            xamlComponentsManager?.Register(XamlComponentsType.LpGroupRegion, this.LpGroupRegion);
            xamlComponentsManager?.Register(XamlComponentsType.LpGroupGalaxy, this.LpGroupGalaxy);


            xamlComponentsManager?.Register(XamlComponentsType.ManufactPanel, this.ManufactPanel);
        }
        /// <summary>
        /// 初始化报表的排序管理器
        /// </summary>
        private void InitializeListViewSorter()
        {
            ListViewSortManager.GetManager().Register(ListViewType.MarketGroup, this.MarketGroupListView);
            ListViewSortManager.GetManager().Register(ListViewType.LpGroup, this.LpGroupListView);
        }

        private void BtnClick_Test(object sender, RoutedEventArgs e)
        {
            var co = LocalConfigLoadManager.GetConfigClass<TypeMaterialsDBClass>().TypeMaterials.Count;
            MessageBox.Show(co.ToString());
        }

        /// <summary>
        /// 市场组被选中,物品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketGroupTree_Selected(object sender, RoutedEventArgs e)
        {
            // 获取节点实例
            var node = (e.OriginalSource as TreeViewItem).Header as MarketGroupNode;
            if (node != null && node.IsGroup && node.Group.hasTypes)
                QueryManager.QueryStart(QueryType.Market, QueryType.Market.ToString(), node);
            else if (node != null && !node.IsGroup && node.TypeID != null)
                QueryManager.QueryStart(QueryType.Market, QueryType.Market.ToString(), node.TypeID.ID.ToString());
        }

        /// <summary>
        /// 蓝图制造组被选中,物品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManufacturingGroupTree_Selected(object sender, RoutedEventArgs e)
        {
            var node = (e.OriginalSource as TreeViewItem).Header as MarketGroupNode;
            if (node != null && !node.IsGroup && node.TypeID != null)
            {
              //  MessageBox.Show(node.TypeID.ID.ToString());
                LocalConfigLoadManager.SS(node.TypeID.ID);
            }
            //if (node != null && node.IsGroup && node.Group.hasTypes)
            //    QueryManager.QueryStart(QueryType.Market, QueryType.Market.ToString(), node);
            //else if (node != null && !node.IsGroup && node.TypeID != null)
            //    QueryManager.QueryStart(QueryType.Market, QueryType.Market.ToString(), node.TypeID.ID.ToString());
        }


        /// <summary>
        /// 窗口绘制完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // 加载预选数据
            LocalConfigLoadManager.ReadYaml<Dictionary<int, NewTypeID>>("Resources/newTypeIDs.yaml", LocalConfigLoadManager.OnReadYamlFinished_NewTypeID);
            LocalConfigLoadManager.ReadYaml<Dictionary<int, NewMarketGroup>>("Resources/newMarketGroups.yaml", LocalConfigLoadManager.OnReadYamlFinished_NewMarketGroup);
            LocalConfigLoadManager.ReadYaml<List<Model.DataSources.Region>>("Resources/regionGalaxy.yaml", LocalConfigLoadManager.OnReadYamlFinished_RegionGalaxy);
            LocalConfigLoadManager.ReadYaml<Dictionary<int, NewNpcCorporations>>("Resources/newNpcCorporations.yaml", LocalConfigLoadManager.OnReadYamlFinished_NewNpcCorpGroup);
            LocalConfigLoadManager.ReadYaml<Dictionary<int, NewBuleprints>>("Resources/newBlueprints.yaml", LocalConfigLoadManager.OnReadYamlFinished_NewBlueprintsGroup);
            LocalConfigLoadManager.ReadYaml<Dictionary<int, TypeMaterials>>("Resources/typeMaterials.yaml", LocalConfigLoadManager.OnReadYamlFinished_TypeMaterials);
           
            LayoutSwitchManager.GetManager()?.Init();

           

        }
        /// <summary>
        /// 选中组市场报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketGroupListView_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //Get clicked column
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column; //得到单击的列
                if (clickedColumn != null)
                {
                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path; //得到单击列所绑定的属性
                    ListViewSortManagerAssist.Sort(ListViewType.MarketGroup, bindingProperty);
                }
            }
        }
        /// <summary>
        /// 选中组LP报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LpGroupListView_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //Get clicked column
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column; //得到单击的列
                if (clickedColumn != null)
                {
                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path; //得到单击列所绑定的属性
                    ListViewSortManagerAssist.Sort(ListViewType.LpGroup, bindingProperty);
                }
            }
        }

       
    }
}
/*
 あ/ア 行  あアa   いイi   うウu   えエe   おオo
 か/カ 行  かカka  きキki  くクku  けケke  こコko
 さ/サ 行  さサsa  しシshi すスsu  せセse  そソso
 た/タ 行  たタta  ちチchi つツtsu てテte  とトto
 な/ナ 行  なナna  にニni  ぬヌnu  ねネne  のノno
 は/ハ 行  はハha  ひヒhi  ふフfu  へヘhe  ほホho
 ま/マ 行  まマma  みミmi  むムmu  めメme  もモmo
 や/ヤ 行  やヤya          ゆユyu          よヨyo
 ら/ラ 行  らラra  りリri  るルru  れレre  ろロro
 わ/ワ 行  わワwa                          をヲwo
 拨音      んンn
 */
/*
 浊音
 が/ガ 行  がガga  ぎギgi  ぐグgu  げゲge  ごゴgo
 ざ/ザ 行  ざザza  じジji  ずズzu  ぜゼze  ぞゾzo
 だ/ダ 行  だダda  ぢヂji  づヅzu  でデde  どドdo
 ば/バ 行  ばバba  びビbi  ぶブbu  べベbe  ぼボbo
 半浊音
 ぱ/パ 行  ぱパpa  ぴピpi  ぷプpu  ぺペpe  ぽポpo
 拗音
 や / ヤ 段
 ゆ / ユ 段
 よ / ヨ 段
 き/キ 行  きゃキャkya  きゅキュkyu  きょキョkyo
 ぎ/ギ 行  ぎゃギャgya  ぎゅギュgyu  ぎょギョgyo
 し/シ 行  しゃシャsha  しゅシュshu  しょショsho
 じ/ジ 行  じゃジャjya  じゅジュjyu  じょジョjyo
 ち/チ 行  ちゃチャcha  ちゅチュchu  ちょチョcho
 に/ニ 行  にゃニャnya  にゅニュnyu  にょニョnyo
 ひ/ヒ 行  ひゃヒャhya  ひゅヒュhyu  ひょヒョhyo
 び/ビ 行  びゃビャbya  びゅビュbyu  びょビョbyo
 ぴ/ピ 行  ぴゃピャpya  ぴゅピュpyu  ぴょピョpyo
 み/ミ 行  みゃミャmya  みゅミュmyu  みょミョmyo
 り/リ 行  りゃリャrya  りゅリュryu  りょリョryo
 */



/*
    我们从未遇见，却从此与你们相识。
        おおの あきら & ひだか こはる & やぐち はるお
*/

/*
    你并不只属于你自己。这世上没有只属于自己的人。 
大家总会和别人结识，共有着某些事物。所以，无法自由。 
但也正因为如此，这世界才会有快乐，才会有悲伤，才会，惹人怜爱。
 */

/*
 “上天赐予人类短暂生命的理由” 
“就是让我们在有限的人生里，活得更有价值” 
“所以我们在临终前，为了某种意义而努力拼搏” 
“虽然失去某个东西的时候，会深痛欲绝” 
“但越是如此，才越能证实其价值所在” 
“这个价值是战胜长久以来的艰难才得以造就” 
“一瞬间构筑的虚像，是无法体现其真谛的”
 */


/*
    我不过是个存放记忆的容器，每一次转移都会丢失掉曾拥有的情感，
但即使如此，即使如此，与你一起所诞生的感情，唯独属于我的感情，也绝不是任意一次克隆复制所能取代的。
    我的愿望是什么？现在、此刻、未来，我唯一的愿望，唯一的请求，下次遇见时，我还是我，和你经历一切的我。
            --末夜
 */