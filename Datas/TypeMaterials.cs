using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Datas
{
    public class TypeMaterials
    {
        public List<Materials> materials { get; set; }
    }

    public class Materials
    {
        public int materialTypeID { get; set; }
        public int quantity { get; set; }
    }
}
