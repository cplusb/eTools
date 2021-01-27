using eTools.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    /// <summary>
    /// 市场组结点
    /// </summary>
    public class MarketGroupNode
    {
        public Dictionary<int, MarketGroupNode> ChildrensDcit { get; set; } // 负责构造索引查找
        public bool IsGroup { get; set; } // 判断是组还是物品成员
        public NewMarketGroup Group { get; set; }  // 组本身

        public NewTypeID TypeID { get; set; } // 最外层叶子结点

        public List<MarketGroupNode> ChildrensList { get => ListChidren(); } // 组的childrens

        /// <summary>
        /// 树状图显示的内容
        /// </summary>
        public string ShowContent
        {
            get
            {
                if (!IsGroup && TypeID != null) return TypeID.name;
                else if (IsGroup && Group != null) return Group.nameID;
                else return string.Empty;
            }
        }

        public MarketGroupNode(NewMarketGroup group, bool isGroup)
        {
            Group = group;
            IsGroup = isGroup;
            ChildrensDcit = new Dictionary<int, MarketGroupNode>();
        }
        /// <summary>
        /// 构造最外层叶子结点
        /// </summary>
        /// <param name="typeID"></param>
        public MarketGroupNode(NewTypeID typeID)
        {
            TypeID = typeID;
            IsGroup = false;
            ChildrensDcit = new Dictionary<int, MarketGroupNode>();
        }

        /// <summary>
        /// 添加子组
        /// </summary>
        /// <param name="group"></param>
        public void AddMarketGroup(MarketGroupNode group)
        {
            if (!ChildrensDcit.ContainsKey(group.Group.ID))
                ChildrensDcit.Add(group.Group.ID, group);
        }
        /// <summary>
        /// 添加叶子子组，负责更新itemIDs
        /// </summary>
        /// <param name="typeIDs"></param>
        public void AddMarketGroup(List<NewTypeID> typeIDs)
        {
            if (typeIDs == null) return;
            foreach (var typeID in typeIDs)
                if (!ChildrensDcit.ContainsKey(typeID.ID))
                    ChildrensDcit.Add(typeID.ID, new MarketGroupNode(typeID));
        }

        /// <summary>
        /// 字典转list显示更新
        /// </summary>
        /// <returns></returns>
        public List<MarketGroupNode> ListChidren()
        {
            List<MarketGroupNode> lst = new List<MarketGroupNode>();
            foreach (var child in ChildrensDcit.Values)
                lst.Add(child);
            return lst;
        }
    }
}
