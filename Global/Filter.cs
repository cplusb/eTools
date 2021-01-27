using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Global
{
    class Filter
    {
        public static Dictionary<string, int> ItemTypeFilterDict = new Dictionary<string, int>
            {
                {"蓝图和反应",2 },
                {"舰船",4 },
                {"舰船装备",9 },
                {"军火和弹药",11 },
                {"贸易货物",19 },
                {"植入体和增效剂",24 },
                {"技能",150 },
                {"无人机",157 },
                {"制造和研究",475 },
                {"建筑",477 },
                {"舰船和装备改装件",955 },
                {"行星基础设施",1320 },
                {"服饰",1396 },
                {"特别版用品",1659 },
                {"飞行员服务",1922 },
                {"舰船涂装",1954 },
                {"建筑装备",2202 },
                {"建筑改装件",2203 },
            };
    }
}
