using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Model.DataSources
{
    public class ItemType
    {
        private List<string> _collections;
        public List<string> Collections { get => _collections; set => _collections = value; }
    }
}
