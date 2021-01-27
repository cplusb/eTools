using eTools.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using eTools.Datas;
using eTools.Model.DataSources;
using System.Linq;
using System.Windows.Forms;

namespace eTools.Manager
{
    /// <summary>
    /// JSON配置文件加载
    /// </summary>
    public partial class LocalConfigLoadManager
    {
        private static Dictionary<string, ConfigClass> _dataCollectionDict = new Dictionary<string, ConfigClass>();
        /// <summary>
        /// 读取本地yaml配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="onComplete">读取完毕后存入数据库</param>
        public static void ReadYaml<T>(string path, Action<T> onComplete)
        {
            try
            {
                onComplete?.Invoke(YamlTools.Read<T>(path));
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 存放newTypeID
        /// </summary>
        /// <param name="result"></param>
        public static void OnReadYamlFinished_NewTypeID(Dictionary<int, NewTypeID> result) => AddConfigClass(new TypeIDDBClass(result));
        /// <summary>
        /// 存放newMarketGroup
        /// </summary>
        /// <param name="result"></param>
        public static void OnReadYamlFinished_NewMarketGroup(Dictionary<int, NewMarketGroup> result) => AddConfigClass(new MarketGroupDBClass(result));
        /// <summary>
        /// 存放星域星域映射数据
        /// </summary>
        /// <param name="regions"></param>
        public static void OnReadYamlFinished_RegionGalaxy(List<Region> regions) => AddConfigClass(new RegionGalaxyDBClass(regions));
        /// <summary>
        /// 存放市场组的映射
        /// </summary>
        /// <param name="corps"></param>
        public static void OnReadYamlFinished_NewNpcCorpGroup(Dictionary<int, NewNpcCorporations> corps) => AddConfigClass(new NewNpcCorporationsDBClass(corps));
        /// <summary>
        /// 存放蓝图数据的映射
        /// </summary>
        /// <param name="blueprints"></param>
        public static void OnReadYamlFinished_NewBlueprintsGroup(Dictionary<int, NewBuleprints> blueprints) => AddConfigClass(new NewBuleprintsDBClass(blueprints));

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        private static void AddConfigClass<T>(T t) where T : ConfigClass
        {
            if (!_dataCollectionDict.ContainsKey(typeof(T).Name))
                _dataCollectionDict.Add(typeof(T).Name, t);
        }
        /// <summary>
        /// 返回对应类型的数据实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <returns></returns>
        public static T GetConfigClass<T>() where T : class => _dataCollectionDict[typeof(T).Name] as T;

        /// <summary>
        ///  这个方法用来将市场组与物品数据绑定起来
        /// </summary>
        /// <returns></returns>
        public static List<MarketGroupNode> GroupMarketTypeIDs()
        {
            // 将物品分组，绑定对应组内ID
            Dictionary<int, List<NewTypeID>> groupTypeIDs = new Dictionary<int, List<NewTypeID>>();
            // 将组与组之间进行归类，设置父子关系
            Dictionary<int, MarketGroupNode> groupRootFolder = new Dictionary<int, MarketGroupNode>();

            // 获取预选数据集
            var typeIDs = GetConfigClass<TypeIDDBClass>().TypeIDs;
            var marketGroups = GetConfigClass<MarketGroupDBClass>().MarketGroupDB;
            // item分组
            try
            {
                foreach (var kvt in typeIDs)
                {
                    // 拿到ID与所在组的ID，将其加入属于他的组内
                    var id = kvt.Key;
                    var groupId = kvt.Value.marketGroupID;
                    if (marketGroups.ContainsKey(groupId)) // 判断这个组内包不包含这个物体
                    {
                        groupTypeIDs.TryGetValue(groupId, out var lst); // 看组内是否已经有成员了，没有则新建组
                        if (lst == null)
                        {
                            lst = new List<NewTypeID>();
                            groupTypeIDs.Add(groupId, lst);
                        }
                        lst.Add(kvt.Value); // 组内添加成员
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            // 组内层级关系确立
            GenerateNode(marketGroups, groupTypeIDs, groupRootFolder);

            // dict转成list
            List<MarketGroupNode> nodes = new List<MarketGroupNode>();
            foreach (var node in groupRootFolder.Values)
                nodes.Add(node);
            // 写进本地配置文件
            //YamlTools.Write(nodes, "Resources/marketGroupTreeNodes.yaml");
            // 返回结果
            return nodes;
        }
        /// <summary>
        /// 创建市场组结点
        /// </summary>
        /// <param name="marketGroups"></param>
        /// <param name="groupTypeIDs"></param>
        /// <param name="groupRootFolder"></param>
        private static void GenerateNode(Dictionary<int, NewMarketGroup> marketGroups, Dictionary<int, List<NewTypeID>> groupTypeIDs,
            Dictionary<int, MarketGroupNode> groupRootFolder)
        {
            // 组内层级关系确立
            var tmpMarketGroups = CopyTools.DeepByBinaryFormatter(marketGroups);
            try
            {
                foreach (var kvg in marketGroups)
                {
                    // 拿到ID与组实例
                    var id = kvg.Key;
                    var group = kvg.Value;
                    // 创建树状图节点
                    var node = new MarketGroupNode(group, true);
                    // 如果这个组内有item成员，则创建特殊组，成员为item
                    if (group.hasTypes)
                    {
                        groupTypeIDs.TryGetValue(id, out var typeLst);
                        node.AddMarketGroup(typeLst);
                    }
                    // 根结点并非一个，
                    if (group.parentGroupID == 0)
                    {
                        groupRootFolder.Add(id, node); // 处理最上层的根节点
                        tmpMarketGroups.Remove(kvg.Value.ID);
                    }
                    else // 否则，寻找父组添加到其内
                    {
                        foreach (var leaf in groupRootFolder)
                        {
                            if (Search(leaf.Value, node, tmpMarketGroups))
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            //MessageBox.Show(marketGroups.Count.ToString());
            if (marketGroups.Count > 0)
                GenerateNode(tmpMarketGroups, groupTypeIDs, groupRootFolder);
        }

        /// <summary>
        /// 找寻市场组子节点
        /// </summary>
        /// <param name="find"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        private static bool Search(MarketGroupNode find, MarketGroupNode add, Dictionary<int, NewMarketGroup> tmpMarketGroups)
        {
            if (find.IsGroup)  // 只有包含子物体的组允许访问
            {
                if (find.Group.ID == add.Group.parentGroupID)  // 找到了则添加进去
                {
                    if (add != null)
                        find.AddMarketGroup(add);
                    tmpMarketGroups.Remove(add.Group.ID);
                    return true;
                }
                else
                {
                    foreach (var findChild in find.ChildrensDcit.Values) // 否则继续找
                    {
                        Search(findChild, add, tmpMarketGroups);
                    }

                }
            }
            return false;
        }
    }

    public partial class LocalConfigLoadManager
    {

        /// <summary>
        /// 这里开始是构造层级映射，再委托给HZ_插件管理
        /// </summary>
        /// <param name="key"></param>
        public static void SS(int key)
        {
            var db = GetConfigClass<NewBuleprintsDBClass>().Blueprints;
            NewBuleprints blue = default;
            db.TryGetValue(key, out blue);
            var node = Se(blue, db, true);
            var panel = XamlComponentsManager.GetManager().Get<Panel>(XamlComponentsType.ManufactPanel);
            HZ_Blueprints.BlueprintsShowManager.Show(panel, node);
        }
        public static BlueprintsTreeNode Se(NewBuleprints blue, Dictionary<int, NewBuleprints> db, bool first = false)
        {
            BlueprintsTreeNode node = new BlueprintsTreeNode();
            node.name = blue.product.name;
            
            if (first)
            {
                node.id = blue.product.id;
                node.count = blue.product.count;
            }
                
            
            foreach(var minearal in blue.costmineral)
            {
                node.Nodes.Add(new BlueprintsTreeNode(minearal.id, minearal.name, minearal.count));
            }
            for (var i =0; i< blue.costProductMap.Count; i++)
            {
                var product = blue.costProduct[i];
                var costkey = blue.costProductMap[i];
                db.TryGetValue(costkey, out var @newblue);
                var n = Se(@newblue, db);
                n.count = product.count;
                n.id = product.id;
                node.Nodes.Add(n);
            }
            return node;
        }


    }
    public class LocalConfigLoadManagerAssist
    {
        /*下面是配置工具，开发用*/
        /// <summary>
        /// 本地配置文件修改翻译,市场组
        /// </summary>
        public static void ReadYaml_FilterToNewMarketGroup()
        {
            try
            {
                var @default = YamlTools.Read<Dictionary<int, MarketGroup>>("Resources/marketGroups.yaml");
                System.Windows.MessageBox.Show(@default.Count.ToString());
                YamlFilter.MarketGroups(@default, out var @new);
                YamlTools.Write(@new, "Resources/newMarketGroups.yaml");
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 本地配置文件修改翻译,typeIDs
        /// </summary>
        public static void ReadYaml_FilterToNewTypeID()
        {
            try
            {
                var @default = YamlTools.Read<Dictionary<int, TypeID>>("Resources/typeIDs.yaml");
                System.Windows.MessageBox.Show(@default.Count.ToString());
                YamlFilter.TypeIDs(@default, out var @new);
                YamlTools.Write(@new, "Resources/newTypeIDs.yaml");
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 本地配置文件修改翻译,蓝图消耗
        /// </summary>
        public static void ReadYaml_FilterToNewBlueprints()
        {
            // 读取原始数据
            // 一级原始数据解析，反向产物存储，由蓝图ID key->实例 ，构造二级字典，produce-.实例，这个过程中实例引用不变，最佳新字典存储
            // 由正向字典数据，consumer追加蓝图实例，作为子节点实例，构造新树，保存新的实例
            // 读取解析
            try
            {
                var db = LocalConfigLoadManager.GetConfigClass<TypeIDDBClass>();
                var @default = YamlTools.Read<Dictionary<int, Buleprints>>("Resources/blueprints.yaml");
                Dictionary<int, Buleprints> productsR = new Dictionary<int, Buleprints>(); // 反转产出与蓝图
                foreach (var blue in @default)
                    if (blue.Value.activities.manufacturing != null)
                    {
                        if (db.TypeIDs.ContainsKey(blue.Key))
                        {
                            var id = blue.Value.activities.manufacturing.products[0].typeID;
                            if (db.TryGet(id, out NewTypeID value))
                                if (!productsR.ContainsKey(id))
                                    productsR.Add(id, blue.Value);
                        }
                    }

                Dictionary<int, NewBuleprints> @new = new Dictionary<int, NewBuleprints>();
                foreach (var blue in productsR.Values)
                {
                    var manufact = blue.activities.manufacturing;

                    if (manufact == null) continue;
                    NewBuleprints nb = new NewBuleprints();
                    nb.costProduct = new List<Products>();
                    nb.costProductMap = new List<int>();
                    nb.costmineral = new List<Products>();
                    nb.product = new Products();
                    nb.product.id = manufact.products[0].typeID;
                    nb.product.count = manufact.products[0].quantity;
                    nb.blueprintTypeID = blue.blueprintTypeID;
                    db.TryGet(nb.product.id, out string pname);
                    nb.product.name = pname;
                    db.TryGet(nb.blueprintTypeID, out string bname);
                    nb.name = bname;
                    if (manufact.materials == null)
                    {
                        @new.Add(blue.blueprintTypeID, nb);
                        continue;
                    }

                    foreach (var co in manufact.materials)
                    {
                        db.TryGet(co.typeID, out string cname);

                        productsR.TryGetValue(co.typeID, out var bp);
                        // 这里需要注意一点，有些原材料是矿石，需要单独处理
                        if (bp == null)
                            nb.costmineral.Add(new Products(co.typeID, cname, co.quantity));
                        else
                        {
                            nb.costProduct.Add(new Products(co.typeID, cname, co.quantity));
                            nb.costProductMap.Add(bp.blueprintTypeID);
                        }

                    }
                    @new.Add(blue.blueprintTypeID, nb);
                }

                System.Windows.MessageBox.Show(@new.Count.ToString());

                YamlTools.Write(@new, "Resources/newBlueprints.yaml");
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 写星域星系，只负责主要的
        /// </summary>
        public static void WriteYaml_RegionGalaxy()
        {

            Dictionary<int, Galaxy> gxyFuerge = new Dictionary<int, Galaxy>
            {
                {30000142, new Galaxy(30000142, "吉他") },
                {30000144, new Galaxy(30000144, "皮尔米特") }
            };

            Dictionary<int, Galaxy> gxyDelve = new Dictionary<int, Galaxy>
            {
                {30004759, new Galaxy(30004759, "1DQ-A") }
            };


            List<Region> regions = new List<Region>()
            {
                {new Region(10000002, "伏尔戈", gxyFuerge)},
                {new Region(10000060, "绝地之域", gxyDelve)}
            };

            YamlTools.Write(regions, "Resources/regionGalaxy.yaml");
        }

        /// <summary>
        ///根据本地文件解析出能使用的NPC公司数据
        /// </summary>
        public static void ReadAndParseNpcCorpGroup()
        {
            var @default = YamlTools.Read<Dictionary<int, NpcCorporations>>("Resources/npcCorporations.yaml");
            //YamlFilter.NpcCorporations(@default, out var @new);
            FPS.FileReader.Read("Resources/npcCorporations.txt", out string contents);

            // 反转字典，用于反向构造映射关系
            Dictionary<string, int> @neww = new Dictionary<string, int>();
            foreach (var n in @default)
                if (!@neww.ContainsKey(n.Value.nameID.zh))
                    @neww.Add(n.Value.nameID.zh, n.Key);
            
            var strs = contents.Split('\n');
            Dictionary<int, NewNpcCorporations> tmncc = new Dictionary<int, NewNpcCorporations>();
            List<TmpNc> tmncs = new List<TmpNc>();
            // nc用来存txt解析出来的数据
            // ncc用来构造新模板实例
            bool title = true;
            int tmncsIndex = 0;
            for(int i =0; i< strs.Length; i++)
            {
                var name = strs[i].Replace("\r", "");
                if (title)
                {
                    title = false;
                    tmncs.Add(new TmpNc(name));
                    continue;
                }
                
                if (name.Contains("//"))
                {
                    title = true;
                    tmncsIndex++;
                    continue;
                }
                tmncs[tmncsIndex].AddSub(name, 0);
            }
            int tmnccIndex = 0;
            foreach(var n in tmncs)
            {
                var tmnc = new NewNpcCorporations(tmnccIndex, n.name);
                tmncc.Add(tmnccIndex, tmnc);
                foreach(var n1 in n.subs)
                {
                    @neww.TryGetValue(n1.Key, out int id);
                    tmnc.AddSub(new NewNpcCorporations(id, n1.Key));
                }
                tmnccIndex++;
            }

            YamlTools.Write(tmncc, "Resources/newNpcCorporations.yaml");
        }
        /// <summary>
        /// 辅助类，解析npcCorpGroup
        /// </summary>
        public class TmpNc
        {
            public string name;
            public Dictionary<string, int> subs;
            public TmpNc(string s)
            {
                name = s;
                subs = new Dictionary<string, int>();
            }
            public void AddSub(string key, int value)
            {
                if (!subs.ContainsKey(key))
                    subs.Add(key, value);
            }
        }
    }

    #region DBClass
    /// <summary>
    /// 数据基类，做字典缓存类型
    /// </summary>
    public class ConfigClass { }

    public class TypeIDDBClass : ConfigClass
    {
        public Dictionary<int, NewTypeID> TypeIDs { get; set; }
        public TypeIDDBClass(Dictionary<int, NewTypeID> typeIDs) => TypeIDs = typeIDs;

        public bool TryGet(int id, out NewTypeID value) => TypeIDs.TryGetValue(id, out value);

        public bool TryGet(int id, out string name)
        {
            bool result = TryGet(id, out NewTypeID value);
            // 针对空值，返回一个ID
            if (value == null)
                name = "N: "+id.ToString();
            else
                name = value.name;
            return result;
        }
    }

    public class MarketGroupDBClass : ConfigClass
    {
        public Dictionary<int, NewMarketGroup> MarketGroupDB { get; set; }
        public MarketGroupDBClass(Dictionary<int, NewMarketGroup> marketGroupDB) => MarketGroupDB = marketGroupDB;
    }

    public class NewNpcCorporationsDBClass : ConfigClass
    {
        public Dictionary<int, NewNpcCorporations> NpcCorpGroupDB { get; set; }
        public List<NewNpcCorporations> NpcCorpGroupDBs => NpcCorpGroupDB.Values.ToList();
        public NewNpcCorporationsDBClass(Dictionary<int, NewNpcCorporations> npcCorpGroupDB) => NpcCorpGroupDB = npcCorpGroupDB;
    }

    public class NewBuleprintsDBClass : ConfigClass
    {
        public Dictionary<int, NewBuleprints> Blueprints { get; set; }
        public NewBuleprintsDBClass(Dictionary<int, NewBuleprints> blueprints) => Blueprints = blueprints;
    }

    public class RegionGalaxyDBClass : ConfigClass
    {
        private Dictionary<int, Region> _regionsDict;
        public List<Region> Regions { get; set; }
        public Dictionary<int, Region> RegionsDict { get
            {
                if(_regionsDict == null)
                {
                    foreach (var region in Regions)
                        if (!_regionsDict.ContainsKey(region.Id))
                            _regionsDict.Add(region.Id, region);
                }
                return _regionsDict;
            } }
        public RegionGalaxyDBClass(List<Region> regions) => Regions = regions;
    }
    #endregion
}


/*
 
 
 */

