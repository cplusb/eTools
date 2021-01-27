using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    public class BlueprintsTreeNode
    {
        public BlueprintsTreeNode()
        {
            Nodes = new List<BlueprintsTreeNode>();
        }
        public BlueprintsTreeNode(int id, string name, int count)
        {
            this.id = id;
            this.name = name;
            this.count = count;
            Nodes = new List<BlueprintsTreeNode>();
        }

        public string name { get; set; }
        public int count { get; set; }
        public int id { get; set; }
        public List<BlueprintsTreeNode> Nodes { get; set; }
    }

    public class NewBuleprints
    {
        public int blueprintTypeID { get; set; }
        public string name { get; set; }
        public Products product { get; set; }
        public List<Products> costProduct { get; set; }
        public List<int> costProductMap { get; set; }
        public List<Products> costmineral { get; set; }
    }

    public class Products
    {
        public int id { get; set; }
        public string name { get; set; }

        public int count { get; set; }
        public Products() { }
        public Products(int id, string name, int count = 1)
        {
            this.id = id;
            this.name = name;
            this.count = count;
        }
    }

    public class Buleprints
    {
        public BuleprintsManufacturing activities { get; set; }
        public int blueprintTypeID { get; set; }
        public int maxProductionLimit { get; set; }
    }

    public class BuleprintsManufacturing
    {
        public Manufacturing copying { get; set; }
        public Manufacturing reaction { get; set; }
        public Manufacturing invention { get; set; }
        public Manufacturing manufacturing { get; set; }
        public Manufacturing research_material { get; set; }
        public Manufacturing research_time { get; set; }
    }

    public class Manufacturing
    {
        public List<TypeRequire> materials { get; set; }
        public List<TypeRequire> products { get; set; }
        public List<SkillRequire> skills { get; set; }
        public int time { get; set; }
    }

    public class TypeRequire
    {
        public int typeID { get; set; }
        public int quantity { get; set; }
        public float probability { get; set; }
    }

    public class SkillRequire
    {
        public int typeID { get; set; }
        public int level { get; set; }
    }
}
