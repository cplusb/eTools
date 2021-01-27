using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    /// <summary>
    /// 新构建的物品数据
    /// </summary>
    public class NewTypeID
    {
        public int ID { get; set; }
        public int iconID { get; set; }
        public int marketGroupID { get; set; }  // map maket group.
        public string name { get; set; }

        public NewTypeID() { }
        public NewTypeID(int id, TypeID typeID)
        {
            ID = id;
            iconID = typeID.iconID;
            marketGroupID = typeID.marketGroupID;
            name = typeID.name.zh;
        }
    }

    /// <summary>
    /// 物品数据字段
    /// </summary>
    public class TypeID
    {
        public float basePrice { get; set; }
        public float capacity { get; set; }
        public Multilingual description { get; set; }
        public int factionID { get; set; }
        public int graphicID { get; set; }
        public int groupID { get; set; }
        public int iconID { get; set; }
        public int marketGroupID { get; set; }  // map maket group.
        public float mass { get; set; }
        public int metaGroupID { get; set; }
        public Multilingual name { get; set; }  // item name, for mutilanguage.
        public int portionSize { get; set; }
        public bool published { get; set; }
        public int raceID { get; set; }
        public float radius { get; set; }
        public string sofFactionName { get; set; }
        public int variationParentTypeID { get; set; }
        public int sofMaterialSetID { get; set; }
        public int soundID { get; set; }
        public Traits traits { get; set; }
        public float volume { get; set; }
        public Dictionary<int, int[]> masteries { get; set; }
    }
    /// <summary>
    /// 加成类型
    /// </summary>
    public class Traits
    {
        public int iconID { get; set; }
        public Bonus[] miscBonuses { get; set; } // unknow
        public Bonus[] roleBonuses { get; set; }  // map fixed bonus.
        public Dictionary<int, Bonus[]> types { get; set; } // bonus with skill level.
    }
    /// <summary>
    /// Bonus usually used for ships??? or weapon model???
    /// </summary>
    public class Bonus
    {
        public float bonus { get; set; }  // bonus value
        public bool isPositive { get; set; } // unknow
        public Multilingual bonusText { get; set; }  // bouns, mitilanguages
        public int importance { get; set; } // unknow
        public int unitID { get; set; } // unknow
    }
    

}
