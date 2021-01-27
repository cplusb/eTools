using eTools.Datas;
using eTools.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using eTools.Global;
using eTools.Manager.Notice;
using System.Threading;
using eTools.ViewModel.DataSources;

namespace eTools.Manager.Web
{
    class HttpRequestManager
    {
        // 返回XML格式
        private static string _urlGet 
            = "https://api.evemarketer.com/ec/marketstat?typeid=34&regionlimit=10000002&hours=24&usesystem=30000142&minq=1";
        // 返回JSON格式
        private static string _urlPost
            = "https://api.evemarketer.com/ec/marketstat/json?typeid=34&regionlimit=10000002&hours=24&usesystem=30000142&minq=1";
        // 返回JSON格式
        // private static string _urlGet 
        //  = "https://esi.evetech.net/latest/markets/10000002/orders/?datasource=tranquility&order_type=all&page=1&type_id=34";

        /// <summary>
        /// 查询入口
        /// </summary>
        /// <param name="idCollections">字符串格式 允许多个查询，中间需要用,隔开 exp. "34,35,36"  最对支持200个批量查询，需要注意，如果数量多，需要分批次处理</param>
        /// <param name="region">查询星域，默认伏尔戈</param>
        /// <param name="system">查询星系，默认吉他</param>
        public static void SetQueryGet(string idCollections, string region = "10000002", string system = "30000142") 
            => _urlGet = $"https://api.evemarketer.com/ec/marketstat?typeid={idCollections}&regionlimit={region}&hours=24&usesystem={system}&minq=1";
        /// <summary>
        /// 查询入口
        /// </summary>
        /// <param name="idCollections">字符串格式 允许多个查询，中间需要用,隔开 exp. "34,35,36"  最对支持200个批量查询，需要注意，如果数量多，需要分批次处理</param>
        /// <param name="region">查询星域，默认伏尔戈</param>
        /// <param name="system">查询星系，默认吉他</param>
        public static void SetQueryPost(string idCollections, string region = "10000002", string system = "30000142") 
            => _urlPost = $"https://api.evemarketer.com/ec/marketstat/json?typeid={idCollections}&regionlimit={region}&hours=24&usesystem={system}&minq=1";

        /// <summary>
        /// 获取NPC公司信息
        /// </summary>
        /// <param name="corp"></param>
        public static string SetQueryGet(int corpID) 
            => $"https://esi.evetech.net/latest/loyalty/stores/{corpID}/offers/?datasource=tranquility";

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void Get(Action<string> onComplete, string url = "")
        {
            try
            {
                if (string.IsNullOrEmpty(_urlGet))
                    throw new ArgumentNullException("url");
                else
                    _urlGet = url;
                if (_urlGet.StartsWith("https"))
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync(_urlGet).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    onComplete?.Invoke(result);
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="usrData"></param>
        /// <param name="onComplete"></param>
        public static void Post(string usrData, Action<string> onComplete)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(usrData);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_urlPost);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = byteArray.Length;
                Stream newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                onComplete?.Invoke(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"异常:{ex.Source},消息{ex.Message}");
            }
        }
    }
    interface IQueryStart
    {
        void QueryStart(object parameter);
        void QueryStart(string parameter);
    }
    public enum QueryType
    {
        Lp,
        Market
    }
    partial class QueryManager
    {
        private static Dictionary<QueryType, string> _queryClass = new Dictionary<QueryType, string>
        {
            {QueryType.Market, "eTools.Manager.Web.QueryGroupMarket"},
            {QueryType.Lp, "eTools.Manager.Web.QueryGroupLp"}
        };
        private static Dictionary<string, IQueryStart> _querys = new Dictionary<string, IQueryStart>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">对应类型</param>
        /// <param name="key">实例key</param>
        /// <param name="parameter">参数</param>
        public static void QueryStart(QueryType type, string key, object parameter) => QueryClass(type, key)?.QueryStart(parameter);
        
        public static void QueryStart(QueryType type, string key, string parameter) => QueryClass(type, key)?.QueryStart(parameter);
        private static IQueryStart QueryClass(QueryType type, string key)
        {
            if (!_querys.ContainsKey(key))
                _querys.Add(key, ReflectionHelper.CreateInstance<IQueryStart>(_queryClass[type], "eTools"));
            return _querys[key];
        }
    }
    /// <summary>
    /// 查询类基类
    /// </summary>
    public class QueryClass : IQueryStart
    {
        protected bool querying;
        protected int counter;
        protected int times;
        public QueryClass() => ResetSingal();
        public virtual void QueryStart(object parameter) { }
        public virtual void QueryStart(string parameter) { }
        protected void ResetSingal()
        {
            querying = false;
            counter = 0;
            times = 0;
        }
    }
    /// <summary>
    /// 查询市场
    /// </summary>
    public class QueryGroupMarket : QueryClass
    {
        private MarketGroupNode _groupNode;
        private string _mgSearchStr = "";
        private List<MarketGroupTypeShow> _queryMarketGroupResults;
        public override void QueryStart(object parameter)
        {
            _groupNode = parameter as MarketGroupNode;
            if (_groupNode != null && !querying)
            {
                try
                {
                    var ids = GetIDs();
                    StatusManager.GetManager()?.ShowMsg("获取服务器数据中.....");
                    counter = 0;
                    times = ids.Count;
                    querying = true;
                    _queryMarketGroupResults = new List<MarketGroupTypeShow>();
                    foreach (var idStr in ids)
                    {
                        if (string.IsNullOrEmpty(idStr))
                        {
                            times--;
                            if (times <= 0)
                            {
                                StatusManager.GetManager()?.ShowMsg("没有匹配到请求的数据，本次请求取消，检查物品类型.....");
                                querying = false;
                                return;
                            }
                            continue;
                        }
                        new Thread(new ThreadStart(()=> {
                            HttpRequestManager.SetQueryPost(idStr, RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
                            HttpRequestManager.Post("NULL", QueryMarketGroupCompleteMuti);
                        })).Start();
                        Thread.Sleep(100);
                    }
                    
                }
                catch { }
            }
        }
        public override void QueryStart(string parameter)
        {
            SearchQueryIDs(parameter);
        }

        private List<string> GetIDs()
        {
            StringBuilder sb = new StringBuilder();
            List<string> idStr = new List<string>();
            if (_groupNode.IsGroup && _groupNode.Group.hasTypes)
            {
                int counter = 0;
                foreach (var no in _groupNode.ChildrensList)
                {
                    sb.Append($"{no.TypeID.ID},");
                    if(counter >= 199)
                    {
                        counter = 0;
                        idStr.Add(sb.RemoveLast(1).ToString());
                    }
                }
                idStr.Add(sb.RemoveLast(1).ToString());
            }
            else if (_groupNode.IsGroup && _groupNode.TypeID != null)
            {
                sb.Append($"{_groupNode.TypeID.ID},");
                idStr.Add(sb.RemoveLast(1).ToString());
            }

            return idStr;
        }
        /// <summary>
        /// 根据输入搜索
        /// </summary>
        /// <param name="msg"></param>
        private void SearchQueryIDs(string msg)
        {
            if (querying) return;
            if (string.IsNullOrEmpty(msg))
            {
                StatusManager.GetManager()?.ShowMsg("无法查询空对象.....");
                return;
            }
            _mgSearchStr = msg;
            try
            {
                querying = true;
                new Thread(new ThreadStart(ThreadSearchQueryIDs)).Start();
            }
            catch { }
           
        }
        private void ThreadSearchQueryIDs()
        {
            List<string> ids = new List<string>();
            int index = 0;
            int counter = 0;
            StringBuilder sb = new StringBuilder();
            try
            {
                StatusManager.GetManager()?.ShowMsg("解析数据中.....");
                var typeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>().TypeIDs;
                var marketGroupDB = LocalConfigLoadManager.GetConfigClass<MarketGroupDBClass>().MarketGroupDB;
                var itemTypeFilterVM = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.RootGrid).Resources["itemTypeFilterVM"] as ItemTypeFilterVM;
                foreach (var no in typeIDs.Values)
                {
                    // 这里需要处理一下，因为服务端只支持单次处理200个
                    if (no != null && no.name != null && no.name.Contains(_mgSearchStr))
                    {
                        // 获取所在组，找到最上层的组
                        marketGroupDB.TryGetValue(no.marketGroupID, out var group);
                        if (group == null) continue;
                        var resultGroup = GetRoot(group, marketGroupDB);
                        Filter.ItemTypeFilterDict.TryGetValue(itemTypeFilterVM.SelectedItem, out var rootID);
                        if (rootID == resultGroup.ID)
                        {
                            // 如果是所在组类型，则查询
                            sb.Append($"{no.ID},");
                            index++;
                        }
                    }
                    if (index > 199)
                    {
                        counter++;
                        index = 0;
                        ids.Add(sb.RemoveLast(1).ToString());
                        sb.Clear();
                    }
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }

            // 处理最后一段数据
            ids.Add(sb.RemoveLast(1).ToString());

            StatusManager.GetManager()?.ShowMsg("获取远端服务器数据.....");
            // 设置批次查询计数器与查询状态，使用多线程查询时，禁止对同一个功能的后续查询
            // 等待批次处理完毕再进行下面的
            counter = 0;
            times = ids.Count;
            // 清空数据缓存
            _queryMarketGroupResults = new List<MarketGroupTypeShow>();
            foreach (var str in ids)
            {
                if (string.IsNullOrEmpty(str))
                {
                    times--;
                    if (times <= 0)
                    {
                        StatusManager.GetManager()?.ShowMsg("没有匹配到请求的数据，本次请求取消，检查物品类型.....");
                        querying = false;
                        return;
                    }
                    continue;
                }
                

                new Thread(new ThreadStart(() => {
                    HttpRequestManager.SetQueryPost(str, RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
                    HttpRequestManager.Post("NULL", QueryMarketGroupCompleteMuti);
                })).Start();
                Thread.Sleep(100);
            }
        }
        private NewMarketGroup GetRoot(NewMarketGroup group, Dictionary<int, NewMarketGroup> groups)
        {
            if (groups.TryGetValue(group.parentGroupID, out var newGroup))
                return GetRoot(newGroup, groups);
            else
                return group;
        }
        private void QueryMarketGroupCompleteMuti(string result)
        {

            counter++;
            var rList = JsonTools.ParseTo<List<JsonFormatType>>(result);
            // 更新UI
            var listView = XamlComponentsManager.GetManager().Get<ListView>(XamlComponentsType.MarketGroupListView);
            var newTypeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();
            try
            {
                if (listView != null)
                {
                    lock (_queryMarketGroupResults)
                    {
                        foreach (var node in rList)
                        {
                            newTypeIDs.TryGet(node.buy.forQuery.types[0], out string name);
                            _queryMarketGroupResults.Add(new MarketGroupTypeShow(name, node.sell.min, node.buy.max));
                        }
                    }
                }
                StatusManager.GetManager()?.ShowMsg($"正在查询第[{counter}/{times}]批次数据.....");
            }
            catch { }

            try
            {
                if (counter >= times)
                {
                    querying = false;
                    var marketGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.MarketGrid);
                    var queryMarketGroupVM = marketGrid.Resources["queryMarketGroupVM"] as ViewModel.Query.QueryMarketGroupVM;
                    queryMarketGroupVM.TypeShowSources = _queryMarketGroupResults;
                    StatusManager.GetManager()?.ShowMsg("查询完毕....");
                }
            }
            catch { }
        }
    }
    /// <summary>
    /// 查询LP
    /// </summary>
    public class QueryGroupLp : QueryClass
    {
        private NewNpcCorporations _corp;
        private List<LpItem> _lpItemsBuffer;
        private Dictionary<int, JsonFormatType> _dictLpItemRequestData;
        private List<int> _ids = new List<int>(); // 缓存从esi拿到问NPC公司ID
        private bool _qeuryCorp = false;
        private bool _corpQueryed = false;

        public QueryGroupLp()
        {
            _lpItemsBuffer = new List<LpItem>();
            _dictLpItemRequestData = new Dictionary<int, JsonFormatType>();
        }

        public override void QueryStart(object parameter)
        {
            // 这个线程正在被使用，禁止再继续调用这个线程
            if (_qeuryCorp)
                return;

            if (_corpQueryed)
            {
                QueryLpGroup();
                return;
            }

            _qeuryCorp = true;
            var threadQueryCorpTab = new Thread(new ThreadStart(ThreadQueryCorpTab));
            _corp = parameter as NewNpcCorporations;
            if (_corp != null && !querying)
                threadQueryCorpTab.Start();
            else
                _qeuryCorp = false;
        }
        private void ThreadQueryCorpTab()
        {
            StatusManager.GetManager()?.ShowMsg("获取兑换列表中.....");
            var url = HttpRequestManager.SetQueryGet(_corp.id);
            HttpRequestManager.Get(QueryCorpTabComplete, url);
        }
        private void QueryCorpTabComplete(string result)
        {
            _lpItemsBuffer.Clear();
            var buffer = JsonTools.ParseTo<List<LpItem>>(result);
            if (buffer == null) 
                return;
            _lpItemsBuffer.AddRange(buffer);
            // 这个id包含了前置物品，所有的都查询
            AnalysisQueryIDs(_lpItemsBuffer, ref _ids);
            StatusManager.GetManager()?.ShowMsg($"获取兑换列表完成, 物品数量:[{_lpItemsBuffer.Count}],开始查询价格.....");
            _corpQueryed = true;
            QueryLpGroup();
        }
        private void AnalysisQueryIDs(List<LpItem> items, ref List<int> tmpList)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (!tmpList.Contains(item.type_id)) tmpList.Add(item.type_id);
                    if (item.required_items != null && item.required_items.Count > 0)
                        foreach (var required in item.required_items)
                            if (!tmpList.Contains(required.type_id))
                                tmpList.Add(required.type_id);
                }
            }
        }
        private void QueryLpGroup()
        {
            StringBuilder sb = new StringBuilder();
            List<string> idStrs = new List<string>();

            for (int i = 0; i < _ids.Count; i++)
            {
                sb.Append($"{_ids[i]},");
                if (i != 0 && i % 199 == 0)
                {
                    sb.RemoveLast(1);
                    idStrs.Add(sb.ToString());
                    sb.Clear();
                }
            }
            sb.RemoveLast(1);
            idStrs.Add(sb.ToString());
            // 设置批次查询计数器与查询状态，使用多线程查询时，禁止对同一个功能的后续查询
            // 等待批次处理完毕再进行下面的
            counter = 0;
            times = idStrs.Count;
            querying = true;
            _dictLpItemRequestData.Clear();
            foreach (var str in idStrs)
            {
                if (string.IsNullOrEmpty(str))
                {
                    times--;
                    if (times <= 0)
                    {
                        querying = false;
                        StatusManager.GetManager()?.ShowMsg("没有匹配到请求的数据，本次请求取消，检查物品类型.....");
                        return;
                    }
                    continue;
                }
                
                new Thread(new ThreadStart(() => {
                    // HttpRequestManager.SetQueryPost(str, RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
                    HttpRequestManager.SetQueryPost(str, RegionGalaxyID.LpQueryRegionID, RegionGalaxyID.LpQueryGalaxyID);
                    HttpRequestManager.Post("NULL", QueryLpGroupCompleteMuti);
                })).Start();
                // 休眠0.1s，否则会有线程不安全，线程准备需要时间，这里赋值可能造成引用错乱
                Thread.Sleep(100);
            }

        }
        private void QueryLpGroupCompleteMuti(string result)
        {
            counter++;
            var items = JsonTools.ParseTo<List<JsonFormatType>>(result);
            foreach (var node in items)
                _dictLpItemRequestData.Add(node.buy.forQuery.types[0], node);
            StatusManager.GetManager()?.ShowMsg($"正在查询第[{counter}/{times}]批次数据.....");
            try
            {
                if (counter >= times)
                {
                    querying = false;
                    _qeuryCorp = false;
                    FilterShowResult();
                }
            }
            catch { }
        }

        private void FilterShowResult()
        {
            var lpGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.LpGrid);

            // 获取数据库
            var newTypeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();
            List<LpGroupTypeShow> groups = new List<LpGroupTypeShow>();
            foreach (var item in _lpItemsBuffer)
            {
                var iShow = new LpGroupTypeShow();
                // 获取物品名称和数量
                newTypeIDs.TryGet(item.type_id, out string name);
                iShow.Name = name;
                iShow.Count = item.quantity;
                iShow.IskCost = item.isk_cost;
                iShow.LpCpost = item.lp_cost;
                if (item.required_items != null)
                {
                    // 前置名和前置数量,以及前置总价
                    StringBuilder preNames = new StringBuilder();
                    StringBuilder preCounts = new StringBuilder();
                    foreach (var preItem in item.required_items)
                    {
                        _dictLpItemRequestData.TryGetValue(preItem.type_id, out var jsonfpi);
                        if (jsonfpi != null)
                        {
                            // 这里需要有个变量来控制前置消耗是按照卖单还是买单
                            if (!Signal.LpPreSale)
                                iShow.PreCost += jsonfpi.buy.max * preItem.quantity;
                            else
                                iShow.PreCost += jsonfpi.sell.min * preItem.quantity;
                        }
                        newTypeIDs.TryGet(preItem.type_id, out string preN);
                        preNames.Append($"{preN}&");
                        preCounts.Append($"{preItem.quantity}&");
                    }
                    preNames.RemoveLast(1);
                    iShow.PreName = preNames.ToString();
                    preCounts.RemoveLast(1);
                    iShow.PreCount = preCounts.ToString();
                }
                _dictLpItemRequestData.TryGetValue(item.type_id, out var jsonfi);
                if (jsonfi != null)
                {
                    iShow.Buy = jsonfi.buy.max * item.quantity;
                    iShow.Sale = jsonfi.sell.min * item.quantity;
                }
                groups.Add(iShow);
            }
            
            var queryLpGroupVM = lpGrid.Resources["queryLpGroupVM"] as ViewModel.Query.QueryLpGroupVM;
            queryLpGroupVM.TypeShowSources = groups;
            StatusManager.GetManager()?.ShowMsg($"请求数据完毕.....");
        }

    }
}

/*
        /// <summary>
    /// 查询管理器
    /// </summary>
    partial class QueryManager
    {
       

        private static MarketGroupNode _mgNode;
        private static string _mgSearchStr = "";
        private static int _mgSearchID = 34;
        private static bool _isMutiQuerying = false;
        private static int _mutiQueryTimer = 0;
        private static int _mutiQueryTimes = 0;
        private static List<MarketGroupTypeShow> _queryMarketGroupResults = new List<MarketGroupTypeShow>();
        /// <summary>
        /// 查询组
        /// </summary>
        /// <param name="node"></param>
        public static void QueryMarketGroup(MarketGroupNode node)
        {
            if (_isMutiQuerying) return;
            if (node == null) return;
            _mgNode = node;
            try{
                new Thread(new ThreadStart(ThreadQueryMarketGroup)).Start();
            }
            catch { }
        }
        /// <summary>
        /// 开启查询的线程
        /// </summary>
        private static void ThreadQueryMarketGroup()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var no in _mgNode.ChildrensList)
                sb.Append($"{no.TypeID.ID},");

            sb.RemoveLast(1);
            StatusManager.GetManager()?.ShowMsg("获取服务器数据中.....");
            HttpRequestManager.SetQueryPost(sb.ToString(), RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
            HttpRequestManager.Post("NULL", QueryMarketGroupComplete);
        }

        /// <summary>
        /// 查询单个物品
        /// </summary>
        /// <param name="id"></param>
        public static void QueryMarketTypeID(int id)
        {
            if (_isMutiQuerying) return;
            if (string.IsNullOrEmpty(id.ToString())) return;
            _mgSearchID = id;
            try
            {
                var newTypeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();
                if (!newTypeIDs.TypeIDs.ContainsKey(id)) return;
                new Thread(new ThreadStart(ThreadQueryMarketTypeID)).Start();
            }
            catch { }
        }
        /// <summary>
        /// 开启查询单个物品的线程
        /// </summary>
        private static void ThreadQueryMarketTypeID()
        {
            // 清空数据缓存
            _queryMarketGroupResults.Clear();
            StatusManager.GetManager()?.ShowMsg("获取服务器数据中.....");
            HttpRequestManager.SetQueryPost(_mgSearchID.ToString(), RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
            HttpRequestManager.Post("NULL", QueryMarketGroupComplete);
        }

        /// <summary>
        /// 根据输入搜索
        /// </summary>
        /// <param name="msg"></param>
        public static void QueryMarketTypeIDSearch(string msg)
        {
            if (_isMutiQuerying) return;
            if (string.IsNullOrEmpty(msg))
            {
                StatusManager.GetManager()?.ShowMsg("无法查询空对象.....");
                return;
            }
            _mgSearchStr = msg;
            try {
                new Thread(new ThreadStart(ThreadQueryMarketTypeIDSearch)).Start();
            }
            catch { }
            #region
            //StringBuilder sb = new StringBuilder();
            //try
            //{
            //    int index = 0;
            //    var typeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>().TypeIDs;
            //    var marketGroupDB = LocalConfigLoadManager.GetConfigClass<MarketGroupDBClass>().MarketGroupDB;
            //    var itemTypeFilterVM = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.RootGrid).Resources["itemTypeFilterVM"] as ItemTypeFilterVM;
            //    foreach (var no in typeIDs.Values)
            //    {
            //        // 这里需要处理一下，因为服务端只支持单次处理200个
            //        if (no != null && no.name != null && no.name.Contains(msg))
            //        {
            //            // 获取所在组，找到最上层的组
            //            marketGroupDB.TryGetValue(no.marketGroupID, out var group);
            //            if (group == null) continue;
            //            var resultGroup = GetRoot(group, marketGroupDB);
            //            Filter.ItemTypeFilterDict.TryGetValue(itemTypeFilterVM.SelectedItem, out var rootID);
            //            if(rootID == resultGroup.ID)
            //            {
            //                // 如果是所在组类型，则查询
            //                sb.Append($"{no.ID},");
            //                index++;
            //            }
            //        }
            //        if (index > 199) break;
            //    }
            //}
            //catch { }
            //if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
            //else
            //{
            //    StatusManager.GetManager()?.ShowMsg("类型不匹配无法查询.....");
            //    return;
            //}
            //StatusManager.GetManager()?.ShowMsg("获取服务器数据中.....");
            //HttpRequestManager.SetQueryPost(sb.ToString(), RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
            //HttpRequestManager.Post("NULL", QueryMarketGroupComplete);
            #endregion
        }
        /// <summary>
        /// 开启搜索与查询的线程
        /// </summary>
        //private static void ThreadQueryMarketTypeIDSearch()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    try
        //    {
        //        StatusManager.GetManager()?.ShowMsg("解析数据中.....");
        //        int index = 0;
        //        var typeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>().TypeIDs;
        //        var marketGroupDB = LocalConfigLoadManager.GetConfigClass<MarketGroupDBClass>().MarketGroupDB;
        //        var itemTypeFilterVM = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.RootGrid).Resources["itemTypeFilterVM"] as ItemTypeFilterVM;
        //        foreach (var no in typeIDs.Values)
        //        {
        //            // 这里需要处理一下，因为服务端只支持单次处理200个
        //            if (no != null && no.name != null && no.name.Contains(_mgSearchStr))
        //            {
        //                // 获取所在组，找到最上层的组
        //                marketGroupDB.TryGetValue(no.marketGroupID, out var group);
        //                if (group == null) continue;
        //                var resultGroup = GetRoot(group, marketGroupDB);
        //                Filter.ItemTypeFilterDict.TryGetValue(itemTypeFilterVM.SelectedItem, out var rootID);
        //                if (rootID == resultGroup.ID)
        //                {
        //                    // 如果是所在组类型，则查询
        //                    sb.Append($"{no.ID},");
        //                    index++;
        //                }
        //            }
        //            if (index > 199) break;
        //        }
        //    }
        //    catch { }
        //    if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
        //    else
        //    {
        //        StatusManager.GetManager()?.ShowMsg("类型不匹配无法查询.....");
        //        return;
        //    }
        //    StatusManager.GetManager()?.ShowMsg("获取服务器数据中.....");
        //    HttpRequestManager.SetQueryPost(sb.ToString(), RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
        //    HttpRequestManager.Post("NULL", QueryMarketGroupComplete);
        //}
        private static void ThreadQueryMarketTypeIDSearch()
        {
            List<string> ids = new List<string>();
            int index = 0;
            int counter = 0;
            StringBuilder sb = new StringBuilder();
            try
            {
                StatusManager.GetManager()?.ShowMsg("解析数据中.....");
                var typeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>().TypeIDs;
                var marketGroupDB = LocalConfigLoadManager.GetConfigClass<MarketGroupDBClass>().MarketGroupDB;
                var itemTypeFilterVM = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.RootGrid).Resources["itemTypeFilterVM"] as ItemTypeFilterVM;
                foreach (var no in typeIDs.Values)
                {
                    // 这里需要处理一下，因为服务端只支持单次处理200个
                    if (no != null && no.name != null && no.name.Contains(_mgSearchStr))
                    {
                        // 获取所在组，找到最上层的组
                        marketGroupDB.TryGetValue(no.marketGroupID, out var group);
                        if (group == null) continue;
                        var resultGroup = GetRoot(group, marketGroupDB);
                        Filter.ItemTypeFilterDict.TryGetValue(itemTypeFilterVM.SelectedItem, out var rootID);
                        if (rootID == resultGroup.ID)
                        {
                            // 如果是所在组类型，则查询
                            sb.Append($"{no.ID},");
                            index++;
                        }
                    }
                    if (index > 199)
                    {
                        counter++;
                        index = 0;
                        ids.Add(sb.RemoveLast(1).ToString());
                        sb.Clear();
                    }
                }
            }
            catch(Exception e) { MessageBox.Show(e.Message); }

            // 处理最后一段数据
            ids.Add(sb.RemoveLast(1).ToString());

            StatusManager.GetManager()?.ShowMsg("获取远端服务器数据.....");
            // 设置批次查询计数器与查询状态，使用多线程查询时，禁止对同一个功能的后续查询
            // 等待批次处理完毕再进行下面的
            _mutiQueryTimer = 0;
            _mutiQueryTimes = ids.Count;
            _isMutiQuerying = true;
            // 清空数据缓存
            _queryMarketGroupResults = new List<MarketGroupTypeShow>();
            foreach (var str in ids)
            {
                if (string.IsNullOrEmpty(str))
                {
                    StatusManager.GetManager()?.ShowMsg("没有匹配到请求的数据，本次请求取消，检查物品类型.....");
                    _isMutiQuerying = false;
                    return;
                }
                new Thread(new ThreadStart(() => {
                    HttpRequestManager.SetQueryPost(str, RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
                    HttpRequestManager.Post("NULL", QueryMarketGroupCompleteMuti);
                })).Start();
            }
        }

        private static NewMarketGroup GetRoot(NewMarketGroup group, Dictionary<int, NewMarketGroup> groups)
        {
            if (groups.TryGetValue(group.parentGroupID, out var newGroup))
                return GetRoot(newGroup, groups);
            else
                return group;
        }


        // 特殊符号
        private const string _riseSymbol = "\u25b2"; //上三角
        private const string _dropSymbol = "\u25bc"; //下三角

        private static void QueryMarketGroupComplete(string result)
        {
            var rList = JsonTools.ParseTo<List<JsonFormatType>>(result);
            // 更新UI
            var listView = XamlComponentsManager.GetManager().Get<ListView>(XamlComponentsType.MarketGroupListView);
            var newTypeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();

            var marketGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.MarketGrid);
            var queryMarketGroupVM = marketGrid.Resources["queryMarketGroupVM"] as ViewModel.Query.QueryMarketGroupVM;
            try
            {
                List<MarketGroupTypeShow> lst = new List<MarketGroupTypeShow>();
                if (listView != null)
                {
                 //   listView.Items.Clear();
                    foreach (var node in rList)
                    {
                        newTypeIDs.TryGet(node.buy.forQuery.types[0], out string name);
                        // listView.Items.Add(new MarketGroupTypeShow(name, node.sell.min, node.buy.max));
                         lst.Add(new MarketGroupTypeShow(name, node.sell.min, node.buy.max));
                        //_queryMarketGroupResults.Add(new MarketGroupTypeShow(name, node.sell.min, node.buy.max));
                    }
                }

                queryMarketGroupVM.TypeShowSources = lst;
                // 切换排序器
                //ListViewSortManager.GetManager().SwitchSorter(ListViewType.MarketGroup, ListViewSortType.MarketGroupBuy);
                StatusManager.GetManager()?.Clear();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private static void QueryMarketGroupCompleteMuti(string result)
        {
            
            _mutiQueryTimer++;
            var rList = JsonTools.ParseTo<List<JsonFormatType>>(result);
            // 更新UI
            var listView = XamlComponentsManager.GetManager().Get<ListView>(XamlComponentsType.MarketGroupListView);
            var newTypeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();
            try
            {
                if (listView != null)
                {
                    lock (_queryMarketGroupResults)
                    {
                        //   listView.Items.Clear();
                        foreach (var node in rList)
                        {
                            newTypeIDs.TryGet(node.buy.forQuery.types[0], out string name);
                            // listView.Items.Add(new MarketGroupTypeShow(name, node.sell.min, node.buy.max));
                            _queryMarketGroupResults.Add(new MarketGroupTypeShow(name, node.sell.min, node.buy.max));
                        }
                    }
                }
                StatusManager.GetManager()?.ShowMsg($"正在查询第[{_mutiQueryTimer}/{_mutiQueryTimes}]批次数据.....");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                if (_mutiQueryTimer >= _mutiQueryTimes)
                {
                    _isMutiQuerying = false;
                    // show reslut
                    var marketGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.MarketGrid);
                    var queryMarketGroupVM = marketGrid.Resources["queryMarketGroupVM"] as ViewModel.Query.QueryMarketGroupVM;
                    queryMarketGroupVM.TypeShowSources = _queryMarketGroupResults;
                    StatusManager.GetManager()?.Clear();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
    } 
     partial class QueryManager
    {
        private static NewNpcCorporations _corp;
        private static bool _queryCorp = false;
        private static List<LpItem> _lpItemsBuffer = new List<LpItem>();
        /// <summary>
        /// 查询组
        /// </summary>
        /// <param name="node"></param>
        public static void QueryLpTable(NewNpcCorporations corp)
        {
            if (_isMutiQuerying || _queryCorp) return;
            if (corp == null) return;
            _corp = corp;
            try
            {
                _queryCorp = true;
               // ThreadQueryLpTable();
                 new Thread(new ThreadStart(ThreadQueryLpTable)).Start();
            }
            catch { }
        }
        /// <summary>
        /// 开启查询的线程
        /// </summary>
        private static void ThreadQueryLpTable()
        {
            StatusManager.GetManager()?.ShowMsg("获取NPC公司兑换数据列表中.....");
            var url = HttpRequestManager.SetQueryGet(_corp.id);
            HttpRequestManager.Get(QueryNpcCorpComplete, url);
        }

        private static void QueryNpcCorpComplete(string result)
        {
            var buffer = JsonTools.ParseTo<List<LpItem>>(result);

            _lpItemsBuffer.AddRange(buffer);
            var ids = new List<int>();
            _queryCorp = false;
            GetNpcCorpItemIDs(_lpItemsBuffer, ref ids);

            StatusManager.GetManager()?.ShowMsg($"获取NPC公司兑换数据列表完成, 物品数量{_lpItemsBuffer.Count},开始查询价格.....");
            QueryLpItems(ids);

        }

        private static void GetNpcCorpItemIDs(List<LpItem> items, ref List<int> tmpList)
        {
            tmpList.Clear();
            if(items != null)
            {
                foreach(var item in items)
                {
                    if (!tmpList.Contains(item.type_id)) tmpList.Add(item.type_id);
                    if (item.required_items != null && item.required_items.Count > 0)
                        foreach (var required in item.required_items)
                            if (!tmpList.Contains(required.type_id))
                                tmpList.Add(required.type_id);
                }
            }
        }

        public static void QueryLpItems(List<int> ids)
        {
            StringBuilder sb = new StringBuilder();
            List<string> idStrs = new List<string>();
            
            for(int i =0;i< ids.Count; i++)
            {
                sb.Append($"{ids[i]},");
                if (i != 0 && i % 198 == 0)
                {
                    sb.RemoveLast(1);
                    idStrs.Add(sb.ToString());
                    sb.Clear();
                }
            }
            sb.RemoveLast(1);
            idStrs.Add(sb.ToString());
            StatusManager.GetManager()?.ShowMsg($"{ids.Count}..{idStrs.Count}");
            // 设置批次查询计数器与查询状态，使用多线程查询时，禁止对同一个功能的后续查询
            // 等待批次处理完毕再进行下面的
            _mutiQueryTimer = 0;
            _mutiQueryTimes = idStrs.Count;
            _isMutiQuerying = true;
            StatusManager.GetManager()?.ShowMsg($"{idStrs.Count}");
            foreach (var str in idStrs)
            {
                if (string.IsNullOrEmpty(str))
                {
                    StatusManager.GetManager()?.ShowMsg("没有匹配到请求的数据，本次请求取消，检查物品类型.....");
                    _isMutiQuerying = false;
                    return;
                }
                new Thread(new ThreadStart(() => {
                    HttpRequestManager.SetQueryPost(str, RegionGalaxyID.MarketGroupRegionID, RegionGalaxyID.MarketGroupGalaxyID);
                    HttpRequestManager.Post("NULL", QueryLpGroupCompleteMuti);
                })).Start();
                Thread.Sleep(100);
            }

        }

        private static Dictionary<int, JsonFormatType> _dictLpItemRequestData = new Dictionary<int, JsonFormatType>();
        private static void QueryLpGroupCompleteMuti(string result)
        {
            _mutiQueryTimer++;
            var items = JsonTools.ParseTo<List<JsonFormatType>>(result);
            foreach (var node in items)
                _dictLpItemRequestData.Add(node.buy.forQuery.types[0], node);
            StatusManager.GetManager()?.ShowMsg($"正在查询第[{_mutiQueryTimer}/{_mutiQueryTimes}]批次数据.....");
            try
            {
                if (_mutiQueryTimer >= _mutiQueryTimes)
                {
                    _isMutiQuerying = false;
                    // show reslut
                    FilterLpShowResult();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static void FilterLpShowResult()
        {
            var newTypeIDs = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();
            List<LpGroupTypeShow> groups = new List<LpGroupTypeShow>();
            //StatusManager.GetManager()?.ShowMsg($"筛选出来的数据个数{_dictLpItemRequestData.Count}");
            foreach (var item in _lpItemsBuffer)
            {
                var iShow = new LpGroupTypeShow();
                // 获取物品名称和数量
                newTypeIDs.TryGet(item.type_id, out string name);
                iShow.Name = name;
                iShow.Count = item.quantity;
                iShow.IskCost = item.isk_cost;
                iShow.LpCpost = item.lp_cost;
                // 前置名和前置数量,以及前置总价
                StringBuilder preNames = new StringBuilder();
                StringBuilder preCounts = new StringBuilder();
                if (item.required_items != null)
                {
                    foreach (var preItem in item.required_items)
                    {
                        _dictLpItemRequestData.TryGetValue(preItem.type_id, out var jsonfpi);
                        if(jsonfpi != null)
                        {
                            iShow.PreCost += jsonfpi.buy.max * preItem.quantity;
                            // iShow.PreCost += jsonfpi.sell.min;
                        }
                        newTypeIDs.TryGet(preItem.type_id, out string preN);
                        preNames.Append($"{preN},");
                        preCounts.Append($"{preItem.quantity},");
                    }
                    preNames.RemoveLast(1);
                    iShow.PreName = preNames.ToString();
                    preCounts.RemoveLast(1);
                    iShow.PreCount = preCounts.ToString();
                }
                _dictLpItemRequestData.TryGetValue(item.type_id, out var jsonfi);
                if(jsonfi!= null)
                {
                    iShow.Buy = jsonfi.buy.max * item.quantity;
                    iShow.Sale = jsonfi.sell.min * item.quantity;
                }
                groups.Add(iShow);
            }

            var lpGrid = XamlComponentsManager.GetManager().Get<Grid>(XamlComponentsType.LpGrid);
            var queryLpGroupVM = lpGrid.Resources["queryLpGroupVM"] as ViewModel.Query.QueryLpGroupVM;
            queryLpGroupVM.TypeShowSources = groups;
        }


    }
 */
